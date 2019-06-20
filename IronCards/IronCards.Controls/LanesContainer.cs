using System;
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
using IronCards.Services;
using MaterialSkin.Controls;
using MetroFramework;
using MetroFramework.Components;


namespace IronCards.Controls
{
    public partial class LanesContainer: UserControl,ILanesContainer
    {
        private readonly ILanesDatabaseService _lanesDatabaseService;
        private readonly ICardDatabaseService _cardDatabaseService;
        public List<Lane> LanesCollection { get; set; }
        public ToolTip GlobalToolTip =new ToolTip();
        private FlowLayoutPanel _layoutPanel;
        public LanesContainer(ILanesDatabaseService lanesDatabaseService, ICardDatabaseService cardDatabaseService)
        {
            SetupToolTip();
            InitializeComponent();
            _lanesDatabaseService = lanesDatabaseService;
            _cardDatabaseService = cardDatabaseService;
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
            mainLayoutPanel.Controls.Add(BuildMainMenu(),0,0);


            LanesCollection = new List<Lane>();
            _layoutPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Fill, WrapContents = false,
                HorizontalScroll = {Enabled = true},AutoScroll = true,
                Size = new Size(this.Width, 800)
            };
            this.Controls.Add(_layoutPanel);
            this.Resize += LanesContainer_Resize;
            LoadLanes();
            mainLayoutPanel.Controls.Add(_layoutPanel,0,1);
            this.Controls.Add(mainLayoutPanel);
        }

        private MenuStrip BuildMainMenu()
        {
            var mainMenu = new MenuStrip();
            mainMenu.Width = this.Width;
            var projectMenuItem = new ToolStripMenuItem("Project"); 
            var projectDropDownNew = new ToolStripMenuItem("New",null,NewProjectOnClick);
            projectMenuItem.DropDownItems.Add(projectDropDownNew);
            mainMenu.Items.Add(projectMenuItem);
            this.Controls.Add(mainMenu);
            return mainMenu;
        }

        private void NewProjectOnClick(object sender, EventArgs e)
        {
            EventHandler<EventArgs> handler = LaneContainerRequestingNewProject;
            handler?.Invoke(this, new LaneTitleEditedArgs());

        }

        private void SetupToolTip()
        {
            GlobalToolTip.ToolTipIcon = ToolTipIcon.Info;
            GlobalToolTip.ShowAlways = true;
        }


        private void LoadLanes()
        {
            var lanesCollection = _lanesDatabaseService.GetAll();

            foreach (var laneDocument in lanesCollection)
            {
                //TODO refactor duplicated code .
                var lane = new Lane(laneDocument.Title,_cardDatabaseService, GlobalToolTip) { Height = this.Height - 60 ,Id = laneDocument.Id};
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
            if (dialogResult==DialogResult.Cancel || cardName==string.Empty)
            {
                return;
            }
            //Insert card to UserControl Lane passed in Args
            var cardId=_cardDatabaseService.Insert(parentLaneId, cardName, cardDescription, cardPoints);
            var card = new Card(parentLaneId,cardName,cardDescription,cardPoints,cardId, GlobalToolTip);
            parentLane.AddCard(card);
        }

        private void LoadCards(Lane lane, ToolTip globalToolTip)
        {
           var cardDocuments= _cardDatabaseService.Get(lane.Id);
           //TODO: Build cards and add them to lane
           foreach (var cardDocument in cardDocuments)
           {
                var card = new Card(cardDocument.ParentLaneId,cardDocument.CardName,cardDocument.CardDescription,cardDocument.CardPoints,cardDocument.Id, GlobalToolTip);

                lane.AddCard(card);
           }
        }

        private void Lane_LaneRequestingAddLane(object sender, LaneAddArgs e)
        {
            AddLane("new Lane");
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

        public void AddLane(string laneLabel)
        {
            var lane = new Lane(laneLabel,_cardDatabaseService, GlobalToolTip) {Height = this.Height - 20};
            lane.LaneRequestingTitleChanged += LaneLaneRequestingTitleChanged;
            lane.LaneRequestingDelete += Lane_LaneRequestingDelete;
            lane.LaneRequestingAddLane += Lane_LaneRequestingAddLane;
            lane.LaneRequestingAddCard += Lane_LaneRequestingAddCard;
            lane.LaneRequestingEditCardLane += Lane_LaneRequestingEditCardLane;
            lane.Id=_lanesDatabaseService.Insert(laneLabel);
            LanesCollection.Add(lane);
           _layoutPanel.Controls.Add(lane);
           lane.Focus();
        }
      
        private void Lane_LaneRequestingEditCardLane(object sender, EditCardLaneArgs e)
        {
            e.target.ParentLaneId = e.NewLaneId;
            var cardDocument = new CardDocument
            {
                Id = e.target.CardId,
                CardDescription = e.target.CardDescription,
                CardName = e.target.CardName,
                CardPoints = e.target.CardPoints,
                ParentLaneId = e.target.ParentLaneId
            };
            _cardDatabaseService.Update(cardDocument);

        }

        private void LaneLaneRequestingTitleChanged(object sender, LaneTitleEditedArgs e)
        {
            _lanesDatabaseService.Update(e.LaneId, e.NewTitle);
        }

        public event EventHandler<EventArgs> LaneContainerRequestingNewProject;
    }
}
