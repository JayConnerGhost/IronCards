using System.Windows.Forms;
using MetroFramework.Controls;

namespace IronCards.Dialogs
{
    public class ViewCardDialog : BaseDialogForm
    {
        public void ShowDialog(string cardName, string cardDescription, int cardPoints, int cardId)
        {
            MetroLabel name = new MetroLabel() { Width = 460, Height = 20, TabIndex = 0, TabStop = true,  Text = cardName };
            MetroTextBox description = new MetroTextBox() { Width = 460, Height = 350, TabIndex = 0, TabStop = true, Text = cardDescription,ReadOnly = true,Multiline = true};
            var numericUpDown = new MetroLabel() { Width = 50, Height = 20, TabIndex = 0, TabStop = true, Text = cardPoints.ToString() };
            var result = DialogResult;
            using (var form = new DialogForm(new FormInfo("View Card", 485, 600)))
            {
                MetroLabel nameLabel = new MetroLabel() { Height = 20, Text = "Card Name" };

                name.Left = 4;

                description.Left = 4;

                MetroLabel descriptionLabel = new MetroLabel() { Text = "Card Description", Width = 110 };
            
                MetroButton close = new MetroButton() { Text = "close", TabIndex = 1, TabStop = true };
                ;
                close.Click += (sender, e) =>
                {
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };

                var pointsLayoutPanel = new FlowLayoutPanel();
                pointsLayoutPanel.Size = new System.Drawing.Size(485, 30);
                pointsLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
                var pointsLabel = new MetroLabel() { Text = "Points" };

                pointsLayoutPanel.Controls.Add(pointsLabel);
                pointsLayoutPanel.Controls.Add(numericUpDown);

                var flowLayoutVertical = new FlowLayoutPanel();

                flowLayoutVertical.FlowDirection = FlowDirection.TopDown;
                flowLayoutVertical.Location = new System.Drawing.Point(0, 60);
                flowLayoutVertical.Size = new System.Drawing.Size(485, 600);
                flowLayoutVertical.Controls.Add(nameLabel);
                flowLayoutVertical.Controls.Add(name);
                flowLayoutVertical.Controls.Add(descriptionLabel);
                flowLayoutVertical.Controls.Add(description);
                flowLayoutVertical.Controls.Add(pointsLayoutPanel);
                var buttonLayoutPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Top = 420,
                    Left = 4,
                    Width = 460
                };
                buttonLayoutPanel.Controls.Add(close);
                flowLayoutVertical.Controls.Add(buttonLayoutPanel);
                form.Controls.Add(flowLayoutVertical);
                //TODO build form
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ControlBox = false;
                result = form.ShowDialog();
            }
        }


       
    }
}