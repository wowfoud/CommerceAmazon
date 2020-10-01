
//using Autofac;
//using System.Linq;
//using System.Web.Mvc;

//namespace Commerce.Amazon.Web.Modules
//{
//    public class MvcModule
//        : global::Autofac.Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            var currentAssembly = typeof(MvcModule).Assembly;

//            builder.RegisterAssemblyTypes(currentAssembly)
//                .Where(t => typeof(IController).IsAssignableFrom(t))
//                .AsImplementedInterfaces()
//                .AsSelf()
//                .InstancePerDependency();
//        }
//    }
//}