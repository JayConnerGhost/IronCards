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
        private readonly IFeatureDatabaseService _featureDatabaseService;
        private readonly int _projectId;
        private SplitterPanel _features;
        private SplitterPanel _content;

        public Project(IFeatureDatabaseService featureDatabaseService, int projectId):base()
        {
            InitializeComponent();
            _featureDatabaseService = featureDatabaseService;
            _projectId = projectId;
            BuildFrame();
            BuildFeatureList(_features);
        }

        private void BuildFeatureList(SplitterPanel features)
        {
            var featureList=new ListView();
            featureList.Dock = DockStyle.Fill;
            var featureDataSource=LoadFeatures();
            foreach (var feature in featureDataSource)
            {
                featureList.Controls.Add(feature);
            }
            features.Controls.Add(featureList);
        }

        private List<Feature> LoadFeatures()
        {
            var results = new List<Feature>();
            var featureDocuments = _featureDatabaseService.GetAllByProjectId(_projectId);
            //TODO add fetureDocuments and build out Feature
            foreach (var featureDocument in featureDocuments)
            {
                results.Add(new Feature()
                {
                    ProjectId=_projectId,
                    FeatureName=featureDocument.Name
                });
            }

            return results;
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
