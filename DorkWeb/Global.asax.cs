using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using DorkWeb.DorkServiceReference;
using DorkWeb.Models;

namespace DorkWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Mapper.CreateMap<GoogleDorkViewModel, GoogleDork>();
            Mapper.CreateMap<GoogleDork, GoogleDorkViewModel>();

            Mapper.CreateMap<GoogleDorkParentViewModel, GoogleDorkParent>();
            Mapper.CreateMap<GoogleDorkParent, GoogleDorkParentViewModel>();

            //Mapper.CreateMap<GoogleDorkVulnerableSiteViewModel, GoogleDorkVulnerableSite>();
            //Mapper.CreateMap<GoogleDorkVulnerableSite, GoogleDorkVulnerableSiteViewModel>();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}