using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace IronCards.Controls
{
    public class Lane:UserControl
    {
        public Lane(string laneLabel)
        {
            Controls.Add(BuildLabel(laneLabel));
        }

        private Control BuildLabel(string laneLabel)
        {
            var label = new MetroLabel() {Text = laneLabel,Width = 200,TextAlign =ContentAlignment.BottomLeft };
            return label;
        }
    }
}