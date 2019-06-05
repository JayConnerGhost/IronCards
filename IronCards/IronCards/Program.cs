using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronCards.Controls;
using IronCards.Services;
using Unity;
using Unity.Lifetime;

namespace IronCards
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
           var container= BuildUpContainer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var applicationContainer = container.Resolve<IApplicationContainer>();
            Application.Run((Form)applicationContainer);
        }

        private static UnityContainer BuildUpContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<ILanesContainer, LanesContainer>(new ContainerControlledLifetimeManager());
            container.RegisterType<IApplicationContainer, Container>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDatabaseService, DatabaseService>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}
