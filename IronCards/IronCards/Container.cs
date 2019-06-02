using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronCards.Controls;

namespace IronCards
{
    public partial class Container : BaseForm, IApplicationContainer
    {
        public Container(ILanesContainer lanes):base()
        {
            ((UserControl) lanes).Dock = DockStyle.Fill;
            InitializeComponent();
        }
    }

    public interface IApplicationContainer
    {
    }
}
