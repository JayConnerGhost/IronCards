using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace IronCards
{
    public class BaseForm: MaterialForm
    {
        public BaseForm()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

        }

    }
}