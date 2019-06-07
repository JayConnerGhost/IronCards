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
using IronCards.Services;


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
                LanesCollection.Add(lane);
                _layoutPanel.Controls.Add(lane);
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
