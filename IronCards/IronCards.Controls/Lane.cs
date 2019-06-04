using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Drawing;

namespace IronCards.Controls
{
    public class Lane:UserControl
    {
        enum TextChangedValue
        {
            Changed,
            Unchanged
        }
        public Guid Id { get; set; }

        public Lane(string laneLabel)
        {
            BorderStyle = BorderStyle.FixedSingle;
            
            Width = 250;
            
            Controls.Add(BuildLabel(laneLabel));
        }

        private Control BuildLabel(string laneLabel)
        {
            var label = new MetroTextBox() {Text = laneLabel,Width = 200,ReadOnly = true,Dock = DockStyle.Top};
            label.Click += Label_Click;
            label.Leave += Label_Leave;
            label.TextChanged += Label_TextChanged;
            return label;
        }

        private void Label_TextChanged(object sender, System.EventArgs e)
        {
            ((MetroTextBox) (sender)).Tag = TextChangedValue.Changed;
        }

        private void Label_Leave(object sender, System.EventArgs e)
        {
            try
            {
                var textBox = ((MetroTextBox) sender);
            if ((TextChangedValue)textBox.Tag == TextChangedValue.Changed)
            {
               
                    EventHandler<LaneTitleEditedArgs> handler = TitleChanged;
                    handler?.Invoke(this, new LaneTitleEditedArgs() { LaneId = Id, NewTitle = textBox.Text.Trim() });
                    //raise event passing out new Args 
               
            }
            textBox.Tag = TextChangedValue.Unchanged;

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                //Ignore exception 
            }

        }

        private void Label_Click(object sender, System.EventArgs e)
        {
            ((MetroTextBox) (sender)).ReadOnly = false;
        }

        public event EventHandler<LaneTitleEditedArgs> TitleChanged;
    }

    public class LaneTitleEditedArgs : EventArgs
    {
        public Guid LaneId { get; set; }
        public string NewTitle { get; set; }
    }
}

