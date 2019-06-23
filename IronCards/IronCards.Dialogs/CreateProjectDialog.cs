using System;
using System.Windows.Forms;

namespace IronCards.Dialogs
{
    public class CreateProjectDialog : BaseDialogForm
    {
        public Tuple<ProjectResult, DialogResult> ShowDialog()
        {
            DialogResult result;
            ProjectResult projectTypeSelected=ProjectResult.Simple;
            var projectTypeGroupBox = new GroupBox {Text = "Project Type",Top=100,Dock=DockStyle.Fill};
            var layoutContainer = new FlowLayoutPanel {Dock = DockStyle.Fill};
            var radioButtonSimple = new RadioButton() {Text = "Simple",Checked = true};
            var radioButtonComplex = new RadioButton() {Text = "Complex"};
            var radioButtonEmpty = new RadioButton() {Text = "Empty"};
            layoutContainer.Controls.Add(radioButtonSimple);
            layoutContainer.Controls.Add(radioButtonComplex);
            layoutContainer.Controls.Add(radioButtonEmpty);
            var continueButton = new Button(){Text = "Create",Anchor = AnchorStyles.Right | AnchorStyles.Bottom};
            layoutContainer.Controls.Add(continueButton);
            projectTypeGroupBox.Controls.Add(layoutContainer);
            using (var form = new DialogForm(new FormInfo("Create New Project", 485, 600)))
            {
                form.Controls.Add(projectTypeGroupBox);

                continueButton.Click += (sender, e) =>
                {
               
                    projectTypeSelected=ReturnSelectedProjectType(radioButtonSimple, radioButtonComplex, radioButtonEmpty);
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };
                result = form.ShowDialog();
            }
            return new Tuple<ProjectResult, DialogResult>(projectTypeSelected, result);
        }

        private ProjectResult ReturnSelectedProjectType(RadioButton radioButtonSimple, RadioButton radioButtonComplex, RadioButton radioButtonEmpty)
        {
            if (radioButtonSimple.Checked)
            {
                return ProjectResult.Simple;
            }

            if (radioButtonComplex.Checked)
            {
                return ProjectResult.Complex;
            }

            if (radioButtonEmpty.Checked)
            {
                return ProjectResult.Empty;
            }

            return ProjectResult.Simple;
        }
    }
}