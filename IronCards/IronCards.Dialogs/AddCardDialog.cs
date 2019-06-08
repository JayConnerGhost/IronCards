using System;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace IronCards.Dialogs
{
    //Add points text box and label
    public class AddCardDialog : BaseDialogForm
    {
        public Tuple<string, string> ShowDialog()
        {
            MetroTextBox name = new MetroTextBox() {Top=64, Width = 460, Height = 20, TabIndex = 0, TabStop = true, Multiline = false, Text = "" };
            MetroTextBox description = new MetroTextBox() {Top=144, Width = 460, Height = 350, TabIndex = 0, TabStop = true, Multiline = true, Text = "" };

            using (var form = new DialogForm(new FormInfo("Add Card", 485, 700)))
            {
               MetroLabel nameLabel = new MetroLabel() { Top = 80, Left = 4, Height = 15, Text = "Card Name" };
                name.Top = 120;
                name.Left = 4;
                description.Top = 68;
                description.Left = 4;

                MetroLabel descriptionLabel = new MetroLabel() { Top = 144, Left = 4, Text = "Card Description" };
                MetroButton confirmation = new MetroButton() { Text = "Save", TabIndex = 1, TabStop = true };
                MetroButton close = new MetroButton() { Text = "close", TabIndex = 1, TabStop = true };
                confirmation.Click += (sender, e) => { form.Close(); };
                close.Click += (sender, e) => { form.Close(); };


                form.Controls.Add(nameLabel);
                form.Controls.Add(name);
                form.Controls.Add(descriptionLabel);
                form.Controls.Add(description);
                var buttonLayoutPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.RightToLeft,
                    Top = 420,
                    Left = 4,
                    Width = 460
                };
                buttonLayoutPanel.Controls.Add(close);
                buttonLayoutPanel.Controls.Add(confirmation);
                form.Controls.Add(buttonLayoutPanel);
                //TODO build form
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ControlBox = false;
                form.ShowDialog();
            }

            return new Tuple<string, string>(name.Text, description.Text);
        }
    }
}