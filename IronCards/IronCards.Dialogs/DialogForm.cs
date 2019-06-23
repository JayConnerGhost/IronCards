using System;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Drawing;
using MetroFramework.Forms;

namespace IronCards.Dialogs
{
    public class DialogForm :BaseDialogForm, IDisposable
    {
        public DialogForm(FormInfo formInfo)
        {
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.BorderStyle = MetroBorderStyle.FixedSingle;
            this.ControlBox = true;
            this.Width = formInfo.FormWidth;
            this.Height = formInfo.FormHeight;
            this.Text = formInfo.FormCaption;
            this.MaximumSize=new Size(formInfo.FormWidth,formInfo.FormHeight);
            this.MinimumSize=new Size(formInfo.FormWidth,formInfo.FormHeight);
        }
    }
}