using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using VA.Repositories;

namespace VA
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();   
            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IAdministratorRepository, AdministratorRepository>();
            container.RegisterType<IMemberRepository, MemberRepository>();
            container.RegisterType<IPetRepository, PetRepository>();
            container.RegisterType<IPetSpecieRepository, PetSpecieRepository>();
            container.RegisterType<IVCRepository, VCRepository>();

            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}