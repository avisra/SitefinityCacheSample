using SitefinityWebApp.DI;
using System;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;
using Telerik.Microsoft.Practices.Unity;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Mvc;

namespace SitefinityWebApp
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Bootstrapper.Initialized += new EventHandler<ExecutedEventArgs>(this.Bootstrapper_Initialized);
            Bootstrapper.Bootstrapped += new EventHandler<EventArgs>(this.Bootstrapper_Bootstrapped);
        }

        private void Bootstrapper_Bootstrapped(object sender, EventArgs e)
        {
            ObjectFactory.Container.RegisterType<ISitefinityControllerFactory, NinjectControllerFactory>(new ContainerControlledLifetimeManager());

            var factory = ObjectFactory.Resolve<ISitefinityControllerFactory>();

            ControllerBuilder.Current.SetControllerFactory(factory);
        }

        private void Bootstrapper_Initialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped")
            {
                if (Bootstrapper.IsDataInitialized)
                {
                    GlobalConfiguration.Configure(Register);
                }
            }
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.EnsureInitialized();

            config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "h-api/{controller}/{id}/{action}",
                    defaults: new { id = RouteParameter.Optional, action = RouteParameter.Optional }
                );
        }
    }
}