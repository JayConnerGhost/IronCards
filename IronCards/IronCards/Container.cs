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
            BuildMenu(lanes);
            //Build a container 
            var layout = new TableLayoutPanel {RowCount = 2, ColumnCount = 1, Dock = DockStyle.Fill};
            var menu=new MenuStrip();

            layout.Controls.Add(menu,0,0);
            ((UserControl) lanes).Dock = DockStyle.Fill;
            layout.Controls.Add((UserControl)lanes,0,1);
            Controls.Add(layout);
            //Add menustrip to container 
            //Add lanes to container 
            //  BuildInitialLanes(lanes);//commented out use for dev purposes
        }

        private void BuildMenu(ILanesContainer lanes)
        {
            //build menu 
            //Add menu items 
            
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
