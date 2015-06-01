
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using DorkWeb.Models;
using DorkWeb.DorkServiceReference;

namespace DorkWeb.Controllers
{
    public class GoogleDorkController : Controller
    {
        public ActionResult Index()
        {
            var dorkMaster = new GoogleDorkMasterViewModel();
            return View("Index", dorkMaster);
        }

        [HttpPost]
        public ActionResult Index(GoogleDorkMasterViewModel dorkMaster)
        {
            using (var proxy = new DorkServiceClient(AppSettings.Config.DorkServiceActiveEndpoint))
            {
                dorkMaster.GoogleDorkParentViewModels = Mapper.Map<List<GoogleDorkParentViewModel>>(proxy.SearchGoogleDorks(dorkMaster.SiteToSearch, dorkMaster.Keywords, dorkMaster.GoogleDorkParentIds));
                foreach (var parent in dorkMaster.GoogleDorkParentViewModels)
                {
                    foreach (var dork in parent.GoogleDorks)
                    {
                        dork.GoogleDorkParentId = parent.Id;
                    }
                }
            }
            
            return View("Index", dorkMaster);
        }
    }
}
