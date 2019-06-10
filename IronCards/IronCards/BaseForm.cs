using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using MetroFramework;

namespace IronCards
{
    public class BaseForm: MetroFramework.Forms.MetroForm
    {
        public BaseForm()
        {
          /**  base.FormBorderStyle = FormBorderStyle.Sizable;
            
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo400, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        **/
          base.Theme = MetroThemeStyle.Light;


        }

    }
}