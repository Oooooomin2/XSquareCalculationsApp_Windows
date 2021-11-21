using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using XSquareCalculationsApi.Models;
using XSquareCalculationsApi.Persistance;
using XSquareCalculationsApi.Repositories;
using XSquareCalculationsApi.Services.Templates;

namespace XSquareCalculationsApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // DI Container
            var container = new Container();

            container.Register<ISystemDate, SystemDate>(Lifestyle.Singleton);
            container.Register<IResolveAthenticateRepository, ResolveAuthenticateRepository>(Lifestyle.Singleton);
            container.Register<IResolveTemplatesRepository, ResolveTemplatesRepository>(Lifestyle.Singleton);
            container.Register<IResolveUsersRepository, ResolveUsersRepository>(Lifestyle.Singleton);
            container.Register<IDownloadTemplateService, DownloadTemplateService>(Lifestyle.Singleton);
            container.Register<IResolveJwtAuthenticate, ResolveJwtAuthenticate>(Lifestyle.Singleton);
            container.Register<IPassword, Password>(Lifestyle.Singleton);
            container.Register<XSquareCalculationContext>(Lifestyle.Singleton);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
