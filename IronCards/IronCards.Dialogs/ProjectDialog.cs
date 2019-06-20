using System;
using System.Drawing;
using System.Windows.Forms;

namespace IronCards.Dialogs
{
    public class CreateProjectDialog : BaseDialogForm
    {
        public Tuple<ProjectResult, DialogResult> ShowDialog()
        {


        }
    }

    public enum ProjectResult
    {
        Simple,
        Complex,
        Empty
    }

    public class ProjectDialog : BaseDialogForm
    {
        public Tuple<int, bool, DialogResult, string> ShowDialog()
        {
            int projectId=0;
            bool IsNewProject=true;
            DialogResult result=DialogResult.None;
            var newProjectTextBox = new TextBox() { Height = 20, Width = 150, Font = DefaultFont };
            using (var form = new DialogForm(new FormInfo("Projects", 485, 600)))
            {
                var layout=new FlowLayoutPanel(){Dock=DockStyle.Fill, FlowDirection = FlowDirection.TopDown};
                var newProjectLayout=new FlowLayoutPanel(){FlowDirection = FlowDirection.LeftToRight,WrapContents = false,Width = 330};
                var newProjectLabel=new Label(){Text = "Project Name", Height=20,Width=80, Font = DefaultFont,TextAlign = ContentAlignment.MiddleCenter};
            
                var newProjectButton=new Button(){Text="Go",Height = 20, Font=DefaultFont};

                newProjectLayout.Controls.Add(newProjectLabel);
                newProjectLayout.Controls.Add(newProjectTextBox);
                newProjectLayout.Controls.Add(newProjectButton);

                layout.Controls.Add(newProjectLayout);

                form.Controls.Add(layout);
                form.ShowDialog();
            }

            return new Tuple<int, bool, DialogResult,string>(projectId,IsNewProject,result,newProjectTextBox.Text);
        }
    }
}