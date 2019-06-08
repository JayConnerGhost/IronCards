using System;

namespace IronCards.Dialogs
{
    public class FormInfo
    {
        public string FormCaption;
        public int FormWidth;
        public int FormHeight;

        public FormInfo(string formCaption, int formWidth, int formHeight)
        {
            {
                FormCaption = formCaption.Trim();
                FormWidth = formWidth;
                FormHeight = formHeight;
            }
        }
    }
}