using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using IronCards.Objects;
using MetroFramework.Drawing;

namespace IronCards.Dialogs
{
    public class ProjectDialog : BaseDialogForm
    {
        public DialogForm form;
        private int ProjectId;
        public Tuple<int, bool, DialogResult, string> ShowDialog(List<ProjectDocument> projects)
        {
            
            bool IsNewProject=true;
            DialogResult result=DialogResult.None;
            var newProjectTextBox = new TextBox() { Height = 20, Width = 150, Font = DefaultFont };

            form = new DialogForm(new FormInfo("Projects", 485, 600));
            using (form)
            {
                var layout=new FlowLayoutPanel(){Dock=DockStyle.Fill, FlowDirection = FlowDirection.TopDown,WrapContents = false};
                var newProjectLayout=new FlowLayoutPanel(){FlowDirection = FlowDirection.LeftToRight,WrapContents = false,Width = 330};
                var newProjectLabel=new Label(){Text = "Project Name", Height=20,Width=80, Font = DefaultFont,TextAlign = ContentAlignment.MiddleCenter};
            
                var newProjectButton=new Button(){Text="Go",Height = 20, Font=DefaultFont};
                newProjectButton.Click += (sender, e) =>
                {
                    if (newProjectTextBox.Text != string.Empty)
                    {
                        form.DialogResult = DialogResult.OK;
                        form.Close();
                    }
                    else
                    {
                        MessageBox.Show("please enter a name for the project");
                    }
                };

                newProjectLayout.Controls.Add(newProjectLabel);
                newProjectLayout.Controls.Add(newProjectTextBox);
                newProjectLayout.Controls.Add(newProjectButton);
                //TODO add a list view showing all the projects 
                //TODO: make each project selectable
                layout.Controls.Add(newProjectLayout);
               layout.Controls.Add(BuildExistingProjectLayout(projects));

                form.Controls.Add(layout);
                result= form.ShowDialog();
            }

            return new Tuple<int, bool, DialogResult,string>(ProjectId,IsNewProject,result,newProjectTextBox.Text);
        }

        private FlowLayoutPanel BuildExistingProjectLayout(List<ProjectDocument> projects)
        {
            var existingProjectLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown, Size = new Size(430, 400),WrapContents = false
            };
            Label label=new Label(){Text="Choose existing project"};
            existingProjectLayout.Controls.Add(label);
            ListView projectListView=new ListView(){Size = new Size(425,340),BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle};
            projectListView.View = View.List;
            foreach (var project in projects)
            {
                projectListView.Items.Add(project.Id.ToString(), project.Name,null);
            }

            projectListView.Activation = ItemActivation.OneClick;
            projectListView.ItemActivate += ProjectListView_ItemActivate;

            existingProjectLayout.Controls.Add(projectListView);
            return existingProjectLayout;
        }

        private void ProjectListView_ItemActivate(object sender, EventArgs e)
        {
            var selectedItem = ((ListView) sender).SelectedItems[0];
            //TODO: code in here to load project an pass it back to the calling form , or somehting like that
            ProjectId = int.Parse(selectedItem.Name.Trim());
            form.DialogResult = DialogResult.OK;
            form.Close();
        }
    }
}