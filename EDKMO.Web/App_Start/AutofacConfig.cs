using Autofac;
using Autofac.Integration.Mvc;
using EDKMO.BusinessLogic.Infrastructure;
using EDKMO.BusinessLogic.Interfaces;
using EDKMO.BusinessLogic.Services;
using System.Web.Mvc;

namespace EDKMO.Web
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            // получаем экземпляр контейнера
            var builder = new ContainerBuilder();

            // регистрируем контроллер в текущей сборке
            builder.RegisterControllers(typeof(Global).Assembly);

            // регистрируем споставление типов
            builder.RegisterModule(new ServiceModule());
            //builder.RegisterModule(new AutoMapperModule());
            builder.RegisterType<TerritoriesService>().As<ITerritories>().AsSelf().InstancePerRequest();
            builder.RegisterType<UsersService>().As<IUsers>().AsSelf().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}

