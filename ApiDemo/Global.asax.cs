using ApiDemo.App_Start;
using System.Web.Http;

namespace ApiDemo
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutofacStartupTask.Execute(GlobalConfiguration.Configuration);
        }
    }
}
