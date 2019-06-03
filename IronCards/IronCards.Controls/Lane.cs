using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Drawing;

namespace IronCards.Controls
{
    public class Lane:UserControl
    {
        public Lane(string laneLabel)
        {
            BorderStyle = BorderStyle.FixedSingle;
            
            Width = 250;
            
            Controls.Add(BuildLabel(laneLabel));
        }

        private Control BuildLabel(string laneLabel)
        {
            var label = new MetroLabel() {Text = laneLabel,Width = 200,TextAlign =ContentAlignment.BottomLeft,Dock = DockStyle.Top};
            return label;
        }
    }
}