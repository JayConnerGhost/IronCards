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
            var layout = new TableLayoutPanel {RowCount = 2, ColumnCount = 1, Dock = DockStyle.Fill};
            var menu=new MenuStrip();
            BuildMenu(lanes,menu);

            layout.Controls.Add(menu,0,0);
            ((UserControl) lanes).Dock = DockStyle.Fill;
            layout.Controls.Add((UserControl)lanes,0,1);
            Controls.Add(layout);
            //Add menustrip to container 
            //Add lanes to container 
            //  BuildInitialLanes(lanes);//commented out use for dev purposes
        }

        private void BuildMenu(ILanesContainer lanes, MenuStrip menu)
        {
            //build menu 
            var mnuLanes = new ToolStripMenuItem("Lane");
            var mnuFile = new ToolStripMenuItem("File");
            var mnuCards = new ToolStripMenuItem("Cards");
            //Add menu items 
            BuildUpFileMenu(mnuFile);
            BuildUpLanesMenu(mnuLanes);
            BuildUpCardsMenu(mnuCards);


            menu.Items.Add(mnuFile);
            menu.Items.Add(mnuLanes);
            menu.Items.Add(mnuCards);
        }

        private void BuildUpCardsMenu(ToolStripMenuItem menu)
        {
        }

        private void BuildUpLanesMenu(ToolStripMenuItem menu)
        {
            var lanesNewMenuItem = new ToolStripMenuItem("New Lane", null, NewLaneLanesOnClick);
            menu.DropDownItems.Add(lanesNewMenuItem);
         }

       

        private void NewLaneLanesOnClick(object sender, EventArgs e)
        {
            _lanes.AddLane("new Lane");
        }

        private void BuildUpFileMenu(ToolStripMenuItem menu)
        {
            var fileCloseMenuItem=new ToolStripMenuItem("Quit", null,CloseFileOnClick);
            menu.DropDownItems.Add(fileCloseMenuItem);
        }

        private void CloseFileOnClick(object sender, EventArgs e)
        {
           this.Close();
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
