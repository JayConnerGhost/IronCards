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


namespace IronCards.Controls
{
    public partial class LanesContainer: UserControl,ILanesContainer
    {
        private readonly ILanesDatabaseService _lanesDatabaseService;
        private readonly ICardDatabaseService _cardDatabaseService;
        public List<Lane> LanesCollection { get; set; }
        private FlowLayoutPanel _layoutPanel;
        public LanesContainer(ILanesDatabaseService lanesDatabaseService, ICardDatabaseService cardDatabaseService)
        {
            InitializeComponent();
            _lanesDatabaseService = lanesDatabaseService;
            _cardDatabaseService = cardDatabaseService;
            LanesCollection = new List<Lane>();
            _layoutPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Fill, WrapContents = false,
                HorizontalScroll = {Enabled = true},AutoScroll = true
            };
            this.Controls.Add(_layoutPanel);
            this.Resize += LanesContainer_Resize;
            LoadLanes();
        }

        

        private void LoadLanes()
        {
            var lanesCollection = _lanesDatabaseService.GetAll();

            foreach (var laneDocument in lanesCollection)
            {
                var lane = new Lane(laneDocument.Title, _cardDatabaseService) { Height = this.Height - 20 ,Id = laneDocument.Id};
                lane.LaneRequestingTitleChanged += LaneLaneRequestingTitleChanged;
                lane.LaneRequestingDelete += Lane_LaneRequestingDelete;
                lane.LaneRequestingAddLane += Lane_LaneRequestingAddLane;
                lane.LaneRequestingAddCard += Lane_LaneRequestingAddCard;
                lane.LaneRequestingEditCardLane += Lane_LaneRequestingEditCardLane;
                LanesCollection.Add(lane);
                LoadCards(lane);
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
            var card = new Card(parentLaneId,cardName,cardDescription,cardPoints,cardId);
            
            parentLane.AddCard(card);
        }

        private void LoadCards(Lane lane)
        {
           var cardDocuments= _cardDatabaseService.Get(lane.Id);
           //TODO: Build cards and add them to lane
           foreach (var cardDocument in cardDocuments)
           {
                var card = new Card(cardDocument.ParentLaneId,cardDocument.CardName,cardDocument.CardDescription,cardDocument.CardPoints,cardDocument.Id);
                lane.AddCard(card);
           }
        }

        private void Lane_LaneRequestingAddLane(object sender, LaneAddArgs e)
        {
            AddLane("new Lane");
        }

        private void Lane_LaneRequestingDelete(object sender, LaneDeleteArgs e)
        {
            DeleteLane(e.LaneId,(UserControl) sender);
        }

        private void DeleteLane(int Id, UserControl lane)
        {
            _lanesDatabaseService.Delete(Id);
            LanesCollection.Remove(LanesCollection.Find(x => x.Id == Id));
            _layoutPanel.Controls.Remove((UserControl) lane);
        }

        private void LanesContainer_Resize(object sender, EventArgs e)
        {
            foreach (var lane in LanesCollection)
            {
                ((UserControl) lane).Height = this.Height - 20;
            }
        }

        public void AddLane(string laneLabel)
        {
            var lane = new Lane(laneLabel, _cardDatabaseService) {Height = this.Height - 20};
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
            if (!_cardDatabaseService.Update(cardDocument))
            {
                throw new KeyNotFoundException("Card could not be updated ");
            }
        }

        private void LaneLaneRequestingTitleChanged(object sender, LaneTitleEditedArgs e)
        {
            _lanesDatabaseService.Update(e.LaneId, e.NewTitle);
        }
    }
}
