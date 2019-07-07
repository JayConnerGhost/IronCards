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
using WeifenLuo.WinFormsUI.Docking;

namespace IronCards
{
    public partial class Container : DockContent, IApplicationContainer
    {
        private readonly ILanesContainer _lanes;
        private readonly ICardDatabaseService _cardDatabaseService;
        private readonly ILanesDatabaseService _lanesDatabaseService;
        private readonly IProjectDatabaseService _projectDatabaseService;
        private int projectId;
        private string projectName;

        public Container(ILanesContainer lanes, ICardDatabaseService cardDatabaseService,
            ILanesDatabaseService lanesDatabaseService, IProjectDatabaseService projectDatabaseService) : base()
        {
            _lanes = lanes;
            _cardDatabaseService = cardDatabaseService;
            _lanesDatabaseService = lanesDatabaseService;
            _projectDatabaseService = projectDatabaseService;
            InitializeComponent();
            //Build a container 
            this.Text = "Card Wall";
            var container = BuildTabPageContainer();
            ((UserControl) lanes).Dock = DockStyle.Fill;
            ((LanesContainer) lanes).LaneContainerRequestingNewProject += Container_LaneContainerRequestingNewProject;
           // Controls.Add((UserControl) lanes);
           CreateTabPages(container);
           container.TabPages[0].Controls.Add((UserControl)lanes);
           BuildAttachments(container.TabPages[2]);
            //Insert context menu  to container 
            var contextMenu = BuildContextMenu();
            contextMenu.Show();
        }

        private void BuildAttachments(TabPage containerTabPage)
        {
            var attachmentControl = new Attachments {Dock = DockStyle.Fill};
            containerTabPage.Controls.Add(attachmentControl);
        }

        private void CreateTabPages(TabControl container)
        {
            container.BackColor=Color.Snow;
            
            container.TabPages.Add("lanes", "Card Wall");
            container.TabPages[0].BorderStyle = BorderStyle.FixedSingle;
            container.TabPages[0].BackColor=Color.Snow;
            container.TabPages.Add("planningwall", "Planning Wall");
            container.TabPages[1].BorderStyle = BorderStyle.FixedSingle;
            container.TabPages[1].BackColor = Color.Snow;
            container.TabPages.Add("attachments", "Attachments");
            container.TabPages[2].BorderStyle = BorderStyle.FixedSingle;
            container.TabPages[2].BackColor = Color.Snow;
            container.TabPages.Add("moodwall", "Mood Wall");
            container.TabPages[3].BorderStyle = BorderStyle.FixedSingle;
            container.TabPages[3].BackColor = Color.Snow;
            container.TabPages.Add("notes", "Notes");
            container.TabPages[4].BorderStyle = BorderStyle.FixedSingle;
            container.TabPages[4].BackColor = Color.Snow;

        }

        private TabControl BuildTabPageContainer()
        {
            var tabControl = new TabControl
            {
                Dock = DockStyle.Fill, Alignment = TabAlignment.Left, Appearance = TabAppearance.Normal
            };
            this.Controls.Add(tabControl);
            return tabControl;
        }

        private void Container_LaneContainerRequestingNewProject(object sender, EventArgs e)
        {
            OpenProjectDialog();
        }

        public void OpenProjectDialog()
        {

            bool IsNewProject;
            DialogResult result;

            var returnResult = new ProjectDialog().ShowDialog(_projectDatabaseService.GetAll());

            projectId = returnResult.Item1;
            DialogResult = returnResult.Item3;
            projectName = returnResult.Item4;
            if (projectId < 1 && DialogResult == DialogResult.OK)
            {
                ShowCreateProject(projectName);
            }

            if (projectId > 0 && DialogResult == DialogResult.OK)
            {
                LoadProjectFromDatabase(projectId);
            }
        }

        private void ShowCreateProject(string projectName)
        {
            this.Text = projectName;
            var result = new CreateProjectDialog().ShowDialog();
            int projectId = SaveProject(projectName);
            if (result.Item1 == ProjectResult.Simple)
            {
                SetUpSimpleProject(projectId, projectName);
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
            CleanDownWall();
            this.Text = projectName;
        }

        private void SetupComplexProject(int projectId, string projectName)
        {
            CleanDownWall();
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
            CleanDownWall();
            this.Text = projectName;
            _lanes.AddLane(projectId, projectName, "TODO");
            _lanes.AddLane(projectId, projectName, "Doing");
            _lanes.AddLane(projectId, projectName, "Done");
        }

        private void CleanDownWall()
        {
            _lanes.RemoveLanesDatabase();
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
            _lanes.AddLane(projectId, projectName, "New Lane");
        }

        private void LoadProjectFromDatabase(int projectId)
        {
          
            CleanDownWall();
            var project = _projectDatabaseService.Get(projectId);

            this.Text = project.Name;
            this.Invalidate();
            this.Update();
            var lanes=_lanesDatabaseService.GetAll(projectId);
            foreach (var lane in lanes)
            {
                _lanes.LoadLane(lane);
            }
        }
    }
}
