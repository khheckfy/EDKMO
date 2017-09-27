using Autofac;
using EDKMO.Data.EntityFramework;
using EDKMO.Domain;

namespace EDKMO.BusinessLogic.Infrastructure
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().AsSelf().InstancePerRequest();
        }
    }
}
