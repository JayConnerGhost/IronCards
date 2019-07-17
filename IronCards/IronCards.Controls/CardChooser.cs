using System.Windows.Forms;

namespace IronCards.Controls
{
    public class CardChooser:BaseControl
    {
        private readonly int _selectedFeatureId;
        private FlowLayoutPanel selectedFlowLayoutPanel=new FlowLayoutPanel();
        private FlowLayoutPanel unselectedFlowLayoutPanel=new FlowLayoutPanel();
        private FlowLayoutPanel masterFlowLayoutPanel =new FlowLayoutPanel();
        public CardChooser(int selectedFeatureId):base()
        {
            InitializeComponent();
            _selectedFeatureId = selectedFeatureId;
          SetupPanels();

        }

        private void SetupPanels()
        {
            selectedFlowLayoutPanel.Dock = DockStyle.Left;
            unselectedFlowLayoutPanel.Dock = DockStyle.Left;
            masterFlowLayoutPanel.Dock = DockStyle.Fill;
            masterFlowLayoutPanel.Controls.Add(selectedFlowLayoutPanel);
            masterFlowLayoutPanel.Controls.Add(unselectedFlowLayoutPanel);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CardChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Name = "CardChooser";
            this.ResumeLayout(false);

        }
    }
}