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
        private int _numberOfLanes;

        public LanesContainer()
        {
            InitializeComponent();
        }

        public int NumberOfLanes
        {
            get => _numberOfLanes;
            set
            {
                _numberOfLanes = value;
                BuildLanes(_numberOfLanes);
            }
        }

        private void BuildLanes(int numberOfLanes)
        {
            var layoutPanel = new FlowLayoutPanel {FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Fill};
            for (int i = 0; i< numberOfLanes; i++)
            {
                //Spike code replace with full lanes code 
                //create Lane component 
                layoutPanel.Controls.Add(new TextBox(){Width = 100, Dock = DockStyle.Left, BorderStyle = BorderStyle.Fixed3D,BackColor = Color.Wheat});
            }
            this.Controls.Add(layoutPanel);


        }
    }
}
