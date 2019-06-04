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
using IronCards.Services.Controls;

namespace IronCards
{
    public partial class Container : BaseForm, IApplicationContainer
    {

        public Container(ILanesContainer lanes):base()
        {
            InitializeComponent();
            
            Controls.Add((UserControl)lanes);
            ((UserControl) lanes).Dock = DockStyle.Fill;
            BuildInitialLanes(lanes);
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
