using Autofac;
using AutoMapper;
using EDKMO.BusinessLogic.DTO;
using EDKMO.Domain.Entities;
using System.Collections.Generic;

namespace EDKMO.BusinessLogic.Infrastructure
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //register all profile classes in the calling assembly
            builder.RegisterAssemblyTypes(typeof(AutoMapperModule).Assembly).As<Profile>();

            MapperConfiguration cfg = new MapperConfiguration(c =>
              {
                  c.AddProfile(new DGisAutoMapperProfile());
              });

            builder.Register(context => cfg)
            .AsSelf()
            .SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }

    public class DGisAutoMapperProfile : Profile
    {
        public DGisAutoMapperProfile()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<Territory, TerritoryDTO>();
                cfg.CreateMap<EventType, EventTypeDTO>();
            });
        }
    }
}
