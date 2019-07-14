using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronCards.Services;

namespace IronCards.Controls
{
    public partial class Project : BaseControl
    {
        private readonly IProjectDatabaseService _projectDatabaseService;
        private readonly int _projectId;
        private SplitterPanel _features;
        private SplitterPanel _content;

        public Project(IProjectDatabaseService projectDatabaseService, int projectId):base()
        {
            InitializeComponent();
            _projectDatabaseService = projectDatabaseService;
            _projectId = projectId;
            BuildFrame();
        }

        private void BuildFrame()
        {
            var splitContainer = new SplitContainer { Orientation = Orientation.Vertical, Dock = DockStyle.Fill };
            splitContainer.Panel1.Name = "featureList";
            splitContainer.Panel2.Name = "featureContent";
            splitContainer.VerticalScroll.Enabled = true;
            _features = splitContainer.Panel1;
            _content = splitContainer.Panel2;
            this.Controls.Add(splitContainer);

        }
    }
}
