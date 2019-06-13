using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronCards.Controls;

namespace IronCards
{
    public partial class Container : BaseForm, IApplicationContainer
    {
        private readonly ILanesContainer _lanes;

        public Container(ILanesContainer lanes):base()
        {
            _lanes = lanes;
            InitializeComponent();
            //Build a container 

            ((UserControl) lanes).Dock = DockStyle.Fill;
            Controls.Add((UserControl)lanes);
            //Insert context menu  to container 
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
           _lanes.AddLane("New Lane");
        }


        private static void BuildInitialLanes(ILanesContainer lanes)
        {
            //Replace when menu in place and database 
            ((LanesContainer) lanes).AddLane("TODO");
            ((LanesContainer) lanes).AddLane("Doing");
            ((LanesContainer) lanes).AddLane("Done");
        }
    }
}
