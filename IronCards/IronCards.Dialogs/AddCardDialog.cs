﻿using System;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace IronCards.Dialogs
{
    //Add points text box and label
    public class AddCardDialog : BaseDialogForm
    {
        public Tuple<string, string> ShowDialog()
        {
            MetroTextBox name = new MetroTextBox() { Width = 460, Height = 20, TabIndex = 0, TabStop = true, Multiline = false, Text = "" };
            MetroTextBox description = new MetroTextBox() { Width = 460, Height = 350, TabIndex = 0, TabStop = true, Multiline = true, Text = "" };

            using (var form = new DialogForm(new FormInfo("Add Card", 485, 570)))
            {
               MetroLabel nameLabel = new MetroLabel() { Height = 20, Text = "Card Name" };
          
                name.Left = 4;
              
                description.Left = 4;

                MetroLabel descriptionLabel = new MetroLabel() {  Text = "Card Description",Width = 110};
                MetroButton confirmation = new MetroButton() { Text = "Save", TabIndex = 1, TabStop = true };
                MetroButton close = new MetroButton() { Text = "close", TabIndex = 1, TabStop = true };
                confirmation.Click += (sender, e) => { form.Close(); };
                close.Click += (sender, e) => { form.Close(); };

                var flowLayoutVertical=new FlowLayoutPanel();
                
                flowLayoutVertical.FlowDirection = FlowDirection.TopDown;
                flowLayoutVertical.Location = new System.Drawing.Point(0, 60);
                flowLayoutVertical.Size = new System.Drawing.Size(485, 550);
                flowLayoutVertical.Controls.Add(nameLabel);
                flowLayoutVertical.Controls.Add(name);
                flowLayoutVertical.Controls.Add(descriptionLabel);
                flowLayoutVertical.Controls.Add(description);
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
                form.ShowDialog();
            }

            return new Tuple<string, string>(name.Text, description.Text);
        }
    }
}