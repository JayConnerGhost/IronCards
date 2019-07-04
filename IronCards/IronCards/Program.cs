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
            var mainForm = (Form) container.Resolve<IProjectContainer>();
            Application.Run(mainForm);
        }

        private static UnityContainer BuildUpContainer()
        {
            //TODO - now go build up the ProjectMDI with the ILanesContainer etc.
            var container = new UnityContainer();
            container.RegisterType<IProjectContainer, ProjectMDI>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILanesContainer, LanesContainer>(new ContainerControlledLifetimeManager());
            container.RegisterType<IApplicationContainer, Container>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILanesDatabaseService, LanesDatabaseService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICardDatabaseService, CardDatabaseService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProjectDatabaseService, ProjectDatabaseService>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}
