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
using IronCards.Dialogs;
using IronCards.Services;

namespace IronCards
{
    public partial class Container : BaseForm, IApplicationContainer
    {
        private readonly ILanesContainer _lanes;
        private readonly ICardDatabaseService _cardDatabaseService;
        private readonly ILanesDatabaseService _lanesDatabaseService;
        private readonly IProjectDatabaseService _projectDatabaseService;

        public Container(ILanesContainer lanes,ICardDatabaseService cardDatabaseService, ILanesDatabaseService lanesDatabaseService, IProjectDatabaseService projectDatabaseService):base()
        {
            _lanes = lanes;
            _cardDatabaseService = cardDatabaseService;
            _lanesDatabaseService = lanesDatabaseService;
            _projectDatabaseService = projectDatabaseService;
            InitializeComponent();
            //Build a container 
            this.Text = "Wall";
            OpenProjectDialog();
            ((UserControl) lanes).Dock = DockStyle.Fill;
            ((LanesContainer)lanes).LaneContainerRequestingNewProject += Container_LaneContainerRequestingNewProject;
            Controls.Add((UserControl)lanes);
            //Insert context menu  to container 
            var contextMenu = BuildContextMenu();
            contextMenu.Show();
        }

        private void Container_LaneContainerRequestingNewProject(object sender, EventArgs e)
        {
            OpenProjectDialog();
        }

        private void OpenProjectDialog()
        {
            int projectId;
            bool IsNewProject;
            DialogResult result;
            string projectName;
            var returnResult = new ProjectDialog().ShowDialog();

            projectId = returnResult.Item1;
            IsNewProject = returnResult.Item2;
            DialogResult = returnResult.Item3;
            projectName = returnResult.Item4;
            if (IsNewProject && DialogResult == DialogResult.OK)
            {
                ShowCreateProject(projectName);
            }

        }

        private void ShowCreateProject(string projectName)
        {
          var result=  new CreateProjectDialog().ShowDialog();
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
