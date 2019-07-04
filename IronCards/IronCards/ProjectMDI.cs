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
using WeifenLuo.WinFormsUI.Docking;

namespace IronCards
{
    public partial class ProjectMDI : Form, IProjectContainer
    {
        private readonly IApplicationContainer _container;

        public ProjectMDI(IApplicationContainer container)
        {
            _container = container;
            InitializeComponent();
            this.dockPanel.Theme=new VS2015BlueTheme();
            ((DockContent)_container).Show(this.dockPanel, DockState.Document);
        }
    }
}
