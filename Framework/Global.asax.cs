using Framework.FrameworkContext;
using Framework.ViewModels;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Framework.Models;
using Framework.Schedule;

namespace Framework
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            IKernel kernel = new StandardKernel();
            //WebApiConfig.Register(GlobalConfiguration.Configuration);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<FrameworkDbContext>(null);
            Database.SetInitializer<ApplicationDbContext>(null);
            JobScheduler.Start();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //HttpCookie langCookie = HttpContext.Current.Request.Cookies["Language"];
            //if (langCookie != null && langCookie.Value != null)
            //{
            //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(langCookie.Value);
            //    Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCookie.Value);

            //}
            //else
            //{
            //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");
            //    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            //}

            CultureInfo vi = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = vi;

        }

    }
}
