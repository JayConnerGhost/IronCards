using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronCards.Dialogs;
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
            BuildContextMenu();
        }

        private void BuildContextMenu()
        {
           var featureList=(ListView) _features.Controls[0];
           var featureListContextMenuStrip = new ContextMenuStrip();
           featureListContextMenuStrip.Items.Add("Add", null, OnClickCreateFeature);
           featureList.ContextMenuStrip=featureListContextMenuStrip;
        }

        private void OnClickCreateFeature(object sender, EventArgs e)
        {
            var result = new AddFeatureDialog().ShowDialog(_projectId,_featureDatabaseService);
            if (result.Item4 != DialogResult.OK)
            {
                return;
            }

            var featureName = result.Item1;
            var featureId = result.Item2;
            var featureList = (ListView)_features.Controls[0];
            featureList.Items.Add(new ListViewItem() {Text = featureName, Tag = featureId});
        }

        private void BuildFeatureList(SplitterPanel features)
        {
            var featureList = new ListView {View = View.List, Dock = DockStyle.Fill};
            var featureDataSource=LoadFeatures();
            foreach (var feature in featureDataSource)
            {
                var listViewItem = new ListViewItem(feature.FeatureName){Tag = feature.Id};
                featureList.Items.Add(listViewItem);
            }
            featureList.ItemSelectionChanged += FeatureList_ItemSelectionChanged;
            features.Controls.Add(featureList);
        }

        private void FeatureList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected)
            {
                return;
            }
            //TODO: load cards for feature
        }

        private List<Feature> LoadFeatures()
        {
            var results = new List<Feature>();
            var featureDocuments = _featureDatabaseService.GetAllByProjectId(_projectId);
         
            foreach (var featureDocument in featureDocuments)
            {
                results.Add(new Feature()
                {
                    ProjectId=_projectId,
                    FeatureName=featureDocument.Name,
                    Id=featureDocument.Id
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
