using Autofac;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.Concreate;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.GenericManager;
using NLayer.Service.Mapping;
using System.Reflection;
using Module = Autofac.Module;

namespace NLayer.WebUI.Modules
{
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GenericRepositoy<>)).As(typeof(GenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
            var serviceAssmbly = Assembly.GetAssembly(typeof(MapProfile));

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssmbly).Where(x => x.Name.EndsWith
            ("Repository")).AsImplementedInterfaces().InstancePerDependency();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssmbly).Where(x => x.Name.EndsWith
            ("Service")).AsImplementedInterfaces().InstancePerDependency();

            base.Load(builder);
        }
    }
}
