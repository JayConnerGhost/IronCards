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
        private readonly INotesDatabaseService _notesDatabaseService;
        private int projectId;
        private string projectName;
        private TabControl container;
        public Container(ILanesContainer lanes, ICardDatabaseService cardDatabaseService,
            ILanesDatabaseService lanesDatabaseService, IProjectDatabaseService projectDatabaseService, INotesDatabaseService notesDatabaseService) : base()
        {
            _lanes = lanes;
            _cardDatabaseService = cardDatabaseService;
            _lanesDatabaseService = lanesDatabaseService;
            _projectDatabaseService = projectDatabaseService;
            _notesDatabaseService = notesDatabaseService;
            InitializeComponent();
            //Build a container 
            this.Text = "Card Wall";
   
            container = BuildTabPageContainer();

        }

        private void BuildAttachments(TabPage containerTabPage)
        {
            //TODO: pass in project Id for appending to path. 
            var attachmentControl = new Attachments (projectId){Dock = DockStyle.Fill};
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

            BuildUpChildEnritchmentTabs();
        }

        private void BuildUpChildEnritchmentTabs()
        {
           
            CreateTabPages(container);
            ((UserControl)_lanes).Dock = DockStyle.Fill;
            ((LanesContainer)_lanes).LaneContainerRequestingNewProject += Container_LaneContainerRequestingNewProject;
            container.TabPages[0].Controls.Add((UserControl)_lanes);
            BuildAttachments(container.TabPages[2]);
            BuildNotes(container.TabPages[4]);
            BuildPlanningWall(container.TabPages[1]);
        }

        private void BuildPlanningWall(TabPage containerTabPage)
        {
            var projectControl=new Project(new FeatureDatabaseService(), projectId){Dock=DockStyle.Fill};
            containerTabPage.Controls.Add(projectControl);
        }

        private void BuildNotes(TabPage containerTabPage)
        {
            var notesControl = new Notes(_notesDatabaseService, projectId) {Dock = DockStyle.Fill};
            containerTabPage.Controls.Add(notesControl);
        }

        private void ShowCreateProject(string projectName)
        {
            this.Text = projectName;
            var result = new CreateProjectDialog().ShowDialog();
             projectId = SaveProject(projectName);
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
            _lanes.ProjectId = projectId;
            this.Text = projectName;
        }

        private void SetupComplexProject(int projectId, string projectName)
        {
            CleanDownWall();
            this.Text = projectName;
            _lanes.ProjectId = projectId;
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
            _lanes.ProjectId = projectId;
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

       

        private void LoadProjectFromDatabase(int projectId)
        {
          
            CleanDownWall();
            var project = _projectDatabaseService.Get(projectId);

            this.Text = project.Name;
            this.Invalidate();
            this.Update();
            var lanes=_lanesDatabaseService.GetAll(projectId);
            _lanes.ProjectId = projectId;
            foreach (var lane in lanes)
            {
                _lanes.LoadLane(lane);
            }
        }
    }
}
