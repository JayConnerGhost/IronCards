using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronCards.Controls;
using Unity;
using WeifenLuo.WinFormsUI.Docking;

namespace IronCards
{
    public partial class ProjectMdi : Form, IProjectContainer
    {
        public  UnityContainer DependencyContainer { get; set; }

        public ProjectMdi()
        {
       
            InitializeComponent();
            this.dockPanel.Theme=new VS2015LightTheme();
            this.dockPanel.DockLeftPortion = 500;

        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newProject = DependencyContainer.Resolve<IApplicationContainer>();

            ((DockContent)newProject).Show(this.dockPanel, DockState.Document);
            ((Container)newProject).OpenProjectDialog();
        }

  
    }
}
