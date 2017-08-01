#define DEBUG

using Autofac;
using DapperExtensions.Extend;
using Autofac.Extras.DynamicProxy;

namespace ApiDemo
{
    public class DapperContextModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlLogInterceptor>().AsSelf();

            builder.Register<IDapperContext>(c => new DapperContext("db"))
                .InstancePerLifetimeScope()
#if DEBUG
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(SqlLogInterceptor));
#endif

            builder.RegisterGeneric(typeof(RespositoryBase<>)).As(typeof(IRespositoryBase<>));
        }
    }
}