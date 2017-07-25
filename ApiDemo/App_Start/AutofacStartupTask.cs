using Autofac;
using Autofac.Integration.WebApi;
using DapperExtensions.Extend;
using System.Reflection;
using System.Web.Http;

namespace ApiDemo.App_Start
{
    public class AutofacStartupTask
    {
        public static void Execute(HttpConfiguration config)
        {
            ContainerBuilder builder = new ContainerBuilder();
            RegisterInterface(builder);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired().InstancePerRequest();//注册API
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        static void RegisterInterface(ContainerBuilder builder)
        {
            //sql
            builder.RegisterAssemblyTypes(Assembly.Load("DapperExtensions.Extend")).AsImplementedInterfaces().InstancePerRequest();
          
            builder.Register<IDapperContext>(c => new DapperContext("db")).InstancePerRequest();

            builder.RegisterGeneric(typeof(RespositoryBase<>)).As(typeof(IRespositoryBase<>));
        }
    }
}