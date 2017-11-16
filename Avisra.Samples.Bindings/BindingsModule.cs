using Ninject.Modules;
using Ninject.Extensions.Conventions;
using Telerik.Sitefinity.DynamicModules;

namespace Avisra.Samples.Bindings
{
    public class BindingsModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<DynamicModuleManager>().ToMethod(p => DynamicModuleManager.GetManager());

            this.Bind(x => x
               .FromAssemblyContaining<Avisra.Samples.Hogwarts.HogwartsConstants>()
               .SelectAllClasses().BindDefaultInterface());
        }
    }
}
