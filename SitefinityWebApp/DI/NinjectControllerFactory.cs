using Ninject;
using System;
using System.Web.Mvc;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Frontend;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;

namespace SitefinityWebApp.DI
{
    public class NinjectControllerFactory : FrontendControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = FrontendModule.Current.DependencyResolver;
            ninjectKernel.Rebind<DynamicModuleManager>().ToMethod(p => DynamicModuleManager.GetManager());
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return null;
            }
            var resolvedController = this.ninjectKernel.Get(controllerType);

            IController controller = resolvedController as IController;
            return controller;
        }
    }
}
