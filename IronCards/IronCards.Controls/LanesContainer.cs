﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronCards.Dialogs;
using IronCards.Objects;
using IronCards.Services;
using MaterialSkin.Controls;
using MetroFramework;
using MetroFramework.Components;


namespace IronCards.Controls
{
    public partial class LanesContainer: BaseControl, ILanesContainer
    {
        private readonly ILanesDatabaseService _lanesDatabaseService;
        private readonly ICardDatabaseService _cardDatabaseService;
        public List<Lane> LanesCollection { get; set; }
        public ToolTip GlobalToolTip =new ToolTip();
        private FlowLayoutPanel _layoutPanel;
        public int ProjectId { get; set; }
        public LanesContainer(ILanesDatabaseService lanesDatabaseService, ICardDatabaseService cardDatabaseService):base()
        {
            SetupToolTip();
            InitializeComponent();
            _lanesDatabaseService = lanesDatabaseService;
            _cardDatabaseService = cardDatabaseService;
            BackColor=Color.Snow;
            this.Dock = DockStyle.Fill;
            var mainLayoutPanel = new TableLayoutPanel()
            {
                
                Dock=DockStyle.Fill,
                Size=new Size(400,800)
                
            };
            var row1=new RowStyle(SizeType.Absolute,50);
            var row2=new RowStyle(SizeType.Percent,100);
            var columnStyle=new ColumnStyle(SizeType.Percent,100);
            mainLayoutPanel.RowStyles.Add(row1);
            mainLayoutPanel.RowStyles.Add(row2);
            mainLayoutPanel.ColumnStyles.Add(columnStyle);
           
            LanesCollection = new List<Lane>();
            _layoutPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Fill, WrapContents = false,
                HorizontalScroll = {Enabled = true},AutoScroll = true,
                Size = new Size(this.Width, 800)
            };
            this.Controls.Add(_layoutPanel);
            this.Resize += LanesContainer_Resize;
    
            mainLayoutPanel.Controls.Add(_layoutPanel,0,1);
            this.Controls.Add(mainLayoutPanel);
            var contextMenu = BuildContextMenu();
            contextMenu.Show();

        }


        private ContextMenuStrip BuildContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            this.ContextMenuStrip = contextMenu;
            ContextMenuStrip.Items.Add("Insert Lane", null, AddLaneOnClick);
            return contextMenu;
        }

        private void AddLaneOnClick(object sender, EventArgs e)
        {
            this.AddLane(ProjectId, null, "New Lane");
        }

  

        private void SetupToolTip()
        {
            GlobalToolTip.ToolTipIcon = ToolTipIcon.Info;
            GlobalToolTip.ShowAlways = true;
        }


        public void LoadLanes(int projectId,string projectName)
        {
            var lanesCollection = _lanesDatabaseService.GetAll(projectId);

            foreach (var laneDocument in lanesCollection)
            {
                //TODO refactor duplicated code .
                var lane = new Lane(laneDocument.Title,_cardDatabaseService, GlobalToolTip,projectId) { Height = this.Height - 60 ,Id = laneDocument.Id};
                lane.LaneRequestingTitleChanged += LaneLaneRequestingTitleChanged;
                lane.LaneRequestingDelete += Lane_LaneRequestingDelete;
                lane.LaneRequestingAddLane += Lane_LaneRequestingAddLane;
                lane.LaneRequestingAddCard += Lane_LaneRequestingAddCard;
                lane.LaneRequestingEditCardLane += Lane_LaneRequestingEditCardLane;
         
                LanesCollection.Add(lane);
                LoadCards(lane, GlobalToolTip);
                _layoutPanel.Controls.Add(lane);
            }   
        }

        private void Lane_LaneRequestingAddCard(object sender, AddCardArgs e)
        {
            
            //Open dialog to capture card detials 
            var result = new AddCardDialog().ShowDialog();
            //Create card 
            var parentLaneId = e.LaneId;
            var parentLane = e.Target;
            var cardName = result.Item1;
            var cardDescription = result.Item2;
            var cardPoints = result.Item3;
            var dialogResult = result.Item4;
            var cardType = result.Item5;
            var featureId = 0;
            var featureName = "Coming soon";
            if (dialogResult==DialogResult.Cancel || cardName==string.Empty)
            {
                return;
            }

            CardTypes parsedCardType;
            CardTypes.TryParse(cardType, true, out parsedCardType);
            //Insert card to UserControl Lane passed in Args
            var cardId =
                _cardDatabaseService.Insert(parentLaneId, cardName, cardDescription, cardPoints, parsedCardType, featureId, featureName);
            var card = new Card(parentLaneId,cardName,cardDescription,cardPoints,cardId, GlobalToolTip, parsedCardType, featureId, featureName);
            parentLane.AddCard(card);
        }

        private void LoadCards(Lane lane, ToolTip globalToolTip)
        {
           var cardDocuments= _cardDatabaseService.Get(lane.Id);
           //TODO: Build cards and add them to lane
           foreach (var cardDocument in cardDocuments)
           { 

               CardTypes.TryParse(cardDocument.CardType, true, out CardTypes resultingCardType);
               //Next line needed to reflect true card type.
                var card = new Card(cardDocument.ParentLaneId,cardDocument.CardName,cardDocument.CardDescription,cardDocument.CardPoints,cardDocument.Id, GlobalToolTip, resultingCardType,0,"test");

                lane.AddCard(card);
           }
        }

        private void Lane_LaneRequestingAddLane(object sender, LaneAddArgs e)
        {
            AddLane(e.ProjectId,e.ProjectName,e.LaneName);
        }

        private void Lane_LaneRequestingDelete(object sender, LaneDeleteArgs e)
        {
            e.Target.DeleteAllCardsInLane();
            DeleteLane(e.LaneId,(UserControl) sender);
        }

        private void DeleteLane(int Id, UserControl lane)
        {
            _lanesDatabaseService.Delete(Id);
            LanesCollection.Remove(LanesCollection.Find(x => x.Id == Id));
            _layoutPanel.Controls.Remove((UserControl) lane);
            lane.Dispose();
        }

        private void LanesContainer_Resize(object sender, EventArgs e)
        {
            foreach (var lane in LanesCollection)
            {
                ((UserControl) lane).Height = this.Height - 60;
            }
        }
        public void AddLane(int projectId, string projectName, string laneLabel)
        {
            var lane = new Lane(laneLabel, _cardDatabaseService, GlobalToolTip, projectId) { Height = this.Height - 20 };
            lane.LaneRequestingTitleChanged += LaneLaneRequestingTitleChanged;
            lane.LaneRequestingDelete += Lane_LaneRequestingDelete;
            lane.LaneRequestingAddLane += Lane_LaneRequestingAddLane;
            lane.LaneRequestingAddCard += Lane_LaneRequestingAddCard;
            lane.LaneRequestingEditCardLane += Lane_LaneRequestingEditCardLane;
            lane.Id = _lanesDatabaseService.Insert(laneLabel,projectId);
            LanesCollection.Add(lane);
            _layoutPanel.Controls.Add(lane);
            lane.Focus();
        }

        public void LoadLane(LaneDocument laneDocument)
        {
            var lane = new Lane(laneDocument.Title, _cardDatabaseService, GlobalToolTip, laneDocument.ProjectId) { Height = this.Height - 20,Id = laneDocument.Id};
            lane.LaneRequestingTitleChanged += LaneLaneRequestingTitleChanged;
            lane.LaneRequestingDelete += Lane_LaneRequestingDelete;
            lane.LaneRequestingAddLane += Lane_LaneRequestingAddLane;
            lane.LaneRequestingAddCard += Lane_LaneRequestingAddCard;
            lane.LaneRequestingEditCardLane += Lane_LaneRequestingEditCardLane;
            LanesCollection.Add(lane);
            _layoutPanel.Controls.Add(lane);
            LoadCards(lane, GlobalToolTip);
            lane.Focus();
        }

        public void RemoveLanesDatabase()
        {
            foreach (Lane lane in LanesCollection)
            {
                lane.Dispose();
            }
            
        }

    

        private void Lane_LaneRequestingEditCardLane(object sender, EditCardLaneArgs e)
        {
            e.target.ParentLaneId = e.NewLaneId;
            var cardDocument = new CardDocument
            {
                Id = e.target.Id,
                CardDescription = e.target.CardDescription,
                CardName = e.target.CardName,
                CardPoints = e.target.CardPoints,
                ParentLaneId = e.target.ParentLaneId
            };
            _cardDatabaseService.Update(cardDocument);

        }

        private void LaneLaneRequestingTitleChanged(object sender, LaneTitleEditedArgs e)
        {
            _lanesDatabaseService.Update(e.LaneId, e.NewTitle,e.ProjectId);
        }

        public event EventHandler<EventArgs> LaneContainerRequestingNewProject;
    }
}
