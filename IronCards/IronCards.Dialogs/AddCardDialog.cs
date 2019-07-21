using System;
using System.Windows.Forms;
using IronCards.Services;
using MetroFramework.Controls;

namespace IronCards.Dialogs
{
    //Staring Feature Refactor
    public class AddCardDialog : BaseDialogForm
    {
        private IFeatureDatabaseService _featureDatabaseService;
        private int _projectId;

        public Tuple<string, string, int,int,string, DialogResult, string> ShowDialog(IFeatureDatabaseService featureDatabaseService,int projectId)
        {
            _featureDatabaseService = featureDatabaseService;
            _projectId = projectId;
            string type = "Idea";
            string FeatureName = string.Empty;
            int featureId=0;
            MetroTextBox name = new MetroTextBox() { Width = 460, Height = 20, TabIndex = 0, TabStop = true, Multiline = false, Text = "" };
            MetroTextBox description = new MetroTextBox() { Width = 460, Height = 350, TabIndex = 0, TabStop = true, Multiline = true, Text = "" };
            var numericUpDown = new NumericUpDown() { Width = 50, Height = 20, TabIndex = 0, TabStop = true };
            var result = DialogResult;
            using (var form = new DialogForm(new FormInfo("Add Card", 485, 600)))
            {
                MetroLabel nameLabel = new MetroLabel() { Height = 20, Text = "Card Name" };

                name.Left = 4;

                description.Left = 4;

                MetroLabel descriptionLabel = new MetroLabel() { Text = "Card Description", Width = 110 };
                MetroButton confirmation = new MetroButton() { Text = "Insert", TabIndex = 1, TabStop = true };
                MetroButton close = new MetroButton() { Text = "close", TabIndex = 1, TabStop = true };
                confirmation.Click += (sender, e) =>
                {
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };
                close.Click += (sender, e) =>
                {
                    form.DialogResult = DialogResult.Cancel;
                    form.Close();
                };

                var propertiesLayout = new FlowLayoutPanel();
                propertiesLayout.Size = new System.Drawing.Size(485, 30);
                propertiesLayout.FlowDirection = FlowDirection.LeftToRight;
                var pointsLabel = new MetroLabel() { Text = "Points" };

                propertiesLayout.Controls.Add(pointsLabel);
                propertiesLayout.Controls.Add(numericUpDown);
                //TODO - > add card type controls 
                var typesLabel = new Label() { Text = "Card Type" };
                var typeDropDown = new ComboBox();

                propertiesLayout.Controls.Add(typesLabel);
                propertiesLayout.Controls.Add(typeDropDown);
                typeDropDown.Items.Add("Idea");
                typeDropDown.Items.Add("Requirement");
                typeDropDown.Items.Add("Bug");
                typeDropDown.Items.Add("External Requirement");
                typeDropDown.FlatStyle = FlatStyle.Flat;
                typeDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
                typeDropDown.SelectedIndexChanged += (sender, e) =>
                {
                    type = (string)((ComboBox)(sender)).SelectedItem;
                };
                var featureDropDown = new ComboBox();
                propertiesLayout.Controls.Add(BuildFeaturesList(featureDropDown));


                var flowLayoutVertical = new FlowLayoutPanel();

                flowLayoutVertical.FlowDirection = FlowDirection.TopDown;
                flowLayoutVertical.Location = new System.Drawing.Point(0, 60);
                flowLayoutVertical.Size = new System.Drawing.Size(485, 600);
                flowLayoutVertical.Controls.Add(nameLabel);
                flowLayoutVertical.Controls.Add(name);
                flowLayoutVertical.Controls.Add(descriptionLabel);
                flowLayoutVertical.Controls.Add(description);
                flowLayoutVertical.Controls.Add(propertiesLayout);
                var buttonLayoutPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Top = 420,
                    Left = 4,
                    Width = 460
                };
                buttonLayoutPanel.Controls.Add(close);
                buttonLayoutPanel.Controls.Add(confirmation);
                flowLayoutVertical.Controls.Add(buttonLayoutPanel);
                form.Controls.Add(flowLayoutVertical);
                //TODO build form
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ControlBox = false;
                result = form.ShowDialog();
            }

            return new Tuple<string, string, int,int,string, DialogResult,string>(name.Text, description.Text, Decimal.ToInt32(d: numericUpDown.Value),featureId,FeatureName ,result, type);
        }

        private ComboBox BuildFeaturesList(ComboBox featureDropDown)
        {
            //TODO: build combo box 
            //featureDropDown
            //TODO rquest feature list for this dropdown
            //_featureDatabaseService

            var features = _featureDatabaseService.GetAllByProjectId(_projectId);

            foreach (var feature in features)
            {
                featureDropDown.Items.Add(feature);
            }

            return featureDropDown;
        }
    }
}