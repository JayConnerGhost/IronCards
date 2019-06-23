using System;
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
            this.ControlBox = false;
            this.Width = formInfo.FormWidth;
            this.Height = formInfo.FormHeight;
            this.Text = formInfo.FormCaption;
        }
    }
}