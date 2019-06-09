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
using DataGridViewAutoSizeColumnModeEventArgs = System.Windows.Forms.DataGridViewAutoSizeColumnModeEventArgs;


namespace IronCards.Controls
{
    public partial class LanesContainer: UserControl,ILanesContainer
    {
        private readonly IDatabaseService _databaseService;
        public List<Lane> LanesCollection { get; set; }
        private FlowLayoutPanel _layoutPanel;
        public LanesContainer(IDatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
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
            var lanesCollection = _databaseService.GetAll();

            foreach (var laneDocument in lanesCollection)
            {
                var lane = new Lane(laneDocument.Title) { Height = this.Height - 20 ,Id = laneDocument.Id};
                lane.TitleChanged += Lane_TitleChanged;
                lane.LaneRequestingDelete += Lane_LaneRequestingDelete;
                lane.LaneRequestingAddLane += Lane_LaneRequestingAddLane;
                lane.LaneRequestingAddCard += Lane_LaneRequestingAddCard;
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

            //Add card to UserControl Lane passed in Args
            var card = new Card(parentLaneId,cardName,cardDescription,cardPoints);
            //Add CardDocument, store Card, name, points, description ,lane_Id in the database as a card Document 
        }

        private void LoadCards(Lane lane)
        {
            //TODO: implement load card collection per lane form the database
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
            _databaseService.Delete(Id);
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
            var lane = new Lane(laneLabel) {Height = this.Height - 20};
            lane.TitleChanged += Lane_TitleChanged;
            lane.LaneRequestingDelete += Lane_LaneRequestingDelete;
            lane.LaneRequestingAddLane += Lane_LaneRequestingAddLane;
            lane.Id=_databaseService.Insert(laneLabel);
            LanesCollection.Add(lane);
           _layoutPanel.Controls.Add(lane);
           lane.Focus();
        }

        private void Lane_TitleChanged(object sender, LaneTitleEditedArgs e)
        {
            _databaseService.Update(e.LaneId, e.NewTitle);
        }
    }
}
