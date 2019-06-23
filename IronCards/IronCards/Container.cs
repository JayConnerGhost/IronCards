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
using IronCards.Dialogs;
using IronCards.Services;
using IronCards.Objects;
namespace IronCards
{
    public partial class Container : BaseForm, IApplicationContainer
    {
        private readonly ILanesContainer _lanes;
        private readonly ICardDatabaseService _cardDatabaseService;
        private readonly ILanesDatabaseService _lanesDatabaseService;
        private readonly IProjectDatabaseService _projectDatabaseService;
        private int projectId;
        private string projectName;
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
          
            bool IsNewProject;
            DialogResult result;
   
            var returnResult = new ProjectDialog().ShowDialog(_projectDatabaseService.GetAll());

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
          int projectId = SaveProject(projectName);
          if (result.Item1 == ProjectResult.Simple)
          {
              SetUpSimpleProject(projectId,projectName);
              return;
          }

          if (result.Item1 == ProjectResult.Complex)
          {
              SetupComplexProject(projectId, projectName);
              return;
          }

          if (result.Item1 == ProjectResult.Empty)
          {
              SetUpEmptyProject(projectId, projectName);
          }

        }

        private void SetUpEmptyProject(int projectId, string projectName)
        {
            this.Text = projectName;
        }

        private void SetupComplexProject(int projectId, string projectName)
        {
            this.Text = projectName;
            _lanes.AddLane(projectId, projectName, "TODO");
            _lanes.AddLane(projectId, projectName, "Doing");
            _lanes.AddLane(projectId, projectName, "Code Complete");
            _lanes.AddLane(projectId, projectName, "Testing");
            _lanes.AddLane(projectId, projectName, "Deploying");
            _lanes.AddLane(projectId, projectName, "Finished");
        }

        private void SetUpSimpleProject(int projectId, string projectName)
        {
            this.Text = projectName;
            _lanes.AddLane(projectId, projectName, "TODO");
            _lanes.AddLane(projectId, projectName, "Doing");
            _lanes.AddLane(projectId, projectName, "Done");
        }

        private int SaveProject(string projectName)
        {
           return _projectDatabaseService.New(projectName);
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
           _lanes.AddLane(projectId,projectName,"New Lane");
        }


      
    }
}
