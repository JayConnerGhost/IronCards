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
            var textBox = ((MetroTextBox) sender);
            if ((TextChangedValue)textBox.Tag == TextChangedValue.Changed)
            {
                //update DB with new Column Name
            }
            textBox.Tag = TextChangedValue.Unchanged;
        }

        private void Label_Click(object sender, System.EventArgs e)
        {
            ((MetroTextBox) (sender)).ReadOnly = false;
        }
    }
}