using Autofac;
using Framework.FrameworkContext;
using Framework.Repository;
using Framework.Repository.RepositorySpace;
using Microsoft.Owin;
using Owin;
using System.Reflection;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Web.Http;
using Framework.Service;
using System.Web.Mvc;
using Framework.Repository.Infrastructure;
using Framework.Service.Admin;
using System.Globalization;
using System.Threading;
using Framework.Infrastructure;
using Microsoft.AspNet.SignalR;



[assembly: OwinStartupAttribute(typeof(Framework.Startup))]
namespace Framework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigAutofac(app);
            ConfigureAuth(app);



            var idProvider = new CustomUserIdProvider();

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();


            ConfigureAuth(app);

        }

        public void ConfigAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            // Register your Web API controllers. 
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()); //Register WebApi Controllers

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            builder.RegisterType<FrameworkDbContext>().AsSelf().InstancePerRequest();

            //////Asp.net Identity

            //builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            //builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            //builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            //builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            //builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(ApplicationUserRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            //// Services
            builder.RegisterAssemblyTypes(typeof(ApplicationUserService).Assembly)
               .Where(t => t.Name.EndsWith("Service"))
               .AsImplementedInterfaces().InstancePerRequest();

            ////// Validate
            //builder.RegisterAssemblyTypes(typeof(TopicFieldValidate).Assembly)
            //    .Where(t => t.Name.EndsWith("Validate"))
            //    .AsImplementedInterfaces().InstancePerRequest();


            ////// CheckLink
            //builder.RegisterAssemblyTypes(typeof(ICategoryListCheckLink).Assembly)
            //    .Where(t => t.Name.EndsWith("CheckLink"))
            //    .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container); //Set the WebApi DependencyResolver
        }



    }
}
