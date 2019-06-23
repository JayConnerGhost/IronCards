using MetroFramework;
using MetroFramework.Forms;

namespace IronCards.Dialogs
{
    public class BaseDialogForm:MetroForm
    {
        public BaseDialogForm()
        {
            base.Theme = MetroThemeStyle.Light;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseDialogForm
            // 
            this.BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Name = "BaseDialogForm";
            this.ResumeLayout(false);

        }
    }
}