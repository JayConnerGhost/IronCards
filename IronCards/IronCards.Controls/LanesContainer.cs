using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronCards.Controls
{
    public partial class LanesContainer: UserControl,ILanesContainer
    {
        public List<Lane> LanesCollection { get; set; }
        private FlowLayoutPanel _layoutPanel;
        public LanesContainer()
        {
            InitializeComponent();
            LanesCollection = new List<Lane>();
            _layoutPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Fill };
            this.Controls.Add(_layoutPanel);
        }


        public void AddLane(string laneLabel)
        {
           var lane= new Lane(laneLabel);
           LanesCollection.Add(lane);
           _layoutPanel.Controls.Add(lane);
        }

    }
}
