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
using IronCards.Services.Controls;
using LiteDB;

namespace IronCards.Services.Controls
{
}

namespace IronCards.Controls
{
    public partial class LanesContainer: UserControl,ILanesContainer
    {
        private readonly IDatabaseService _databaseService;
        public List<Lane> LanesCollection { get; set; }
        private FlowLayoutPanel _layoutPanel;
        public LanesContainer(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            InitializeComponent();
            LanesCollection = new List<Lane>();
            _layoutPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Fill, WrapContents = false,
                HorizontalScroll = {Enabled = true},AutoScroll = true
            };
            this.Controls.Add(_layoutPanel);
            this.Resize += LanesContainer_Resize;
        }

        private void LanesContainer_Resize(object sender, EventArgs e)
        {
            foreach (var lane in LanesCollection)
            {
                ((UserControl) lane).Height = this.Height - 25;
            }
        }

        public void AddLane(string laneLabel)
        {
            //Code to add to database needed 

            var lane = new Lane(laneLabel) {Height = this.Height - 20};
            lane.TitleChanged += Lane_TitleChanged;
            lane.Id = Guid.NewGuid();
            using (var db = new LiteDatabase(@"requirements.db"))
            {
                var lanes = db.GetCollection<LaneDocument>();
                lanes.Insert(new LaneDocument(){Title = laneLabel});
                lanes.EnsureIndex("Title");
                var docs=lanes.FindAll().First();
            }

            LanesCollection.Add(lane);
           _layoutPanel.Controls.Add(lane);
        }

        private void Lane_TitleChanged(object sender, LaneTitleEditedArgs e)
        {
            //code to edit lane title in the database
        }
    }

    public class LaneDocument 
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
