using System;
using System.Windows.Forms;
using IronCards.Services;
using MetroFramework.Controls;

namespace IronCards.Dialogs
{
    public class AddFeatureDialog : BaseDialogForm
    {
        public Tuple<string, int,int, DialogResult> ShowDialog(int projectId, IFeatureDatabaseService service )
        {
            MetroTextBox name = new MetroTextBox() { Width = 460, Height = 20, TabIndex = 0, TabStop = true, Multiline = false, Text = "" };
            var result = DialogResult;
            int featureId=0;
            using (var form = new DialogForm(new FormInfo("Add Feature", 485, 220)))
            {
                MetroLabel nameLabel=new MetroLabel(){Text="Feature Name"};
                var nameLayout=new FlowLayoutPanel(){Height=60,Width=470,Top = 100, Left=10};
                nameLayout.Controls.Add(nameLabel);
                nameLayout.Controls.Add(name);
                form.Controls.Add(nameLayout);

                MetroButton confirmation = new MetroButton() { Text = "Insert", TabIndex = 1, TabStop = true };
                MetroButton close = new MetroButton() { Text = "close", TabIndex = 1, TabStop = true };
                confirmation.Click += (sender, e) =>
                {
                    form.DialogResult = DialogResult.OK;
                    featureId=service.Insert(new FeatureDocument() {Name = name.Text, ProjectId = projectId});
                    form.Close();
                    
                };
                close.Click += (sender, e) =>
                {
                    form.DialogResult = DialogResult.Cancel;
                    form.Close();
                };

                var controlsLayout = new FlowLayoutPanel {FlowDirection = FlowDirection.RightToLeft, Height = 60, Width = 470, Top = 185, Left = 10 };
                controlsLayout.Controls.Add(confirmation);
                controlsLayout.Controls.Add(close);

                form.Controls.Add(controlsLayout);
                result =form.ShowDialog();
            }



            return new Tuple<string, int,int, DialogResult>(name.Text.Trim(), featureId, projectId, result);
        }
    }
}