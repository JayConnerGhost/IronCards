using System;
using MetroFramework.Forms;

namespace IronCards.Dialogs
{
    public class DialogForm :BaseDialogForm, IDisposable
    {
        public DialogForm(FormInfo formInfo)
        {
            this.Width = formInfo.FormWidth;
            this.Height = formInfo.FormHeight;
            this.Text = formInfo.FormCaption;
        }
    }
}