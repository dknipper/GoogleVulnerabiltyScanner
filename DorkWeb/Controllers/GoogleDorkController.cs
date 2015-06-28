using System;
using System.Collections.Generic;
using System.Linq;
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

            using (var proxy = new DorkServiceClient(AppSettings.Config.DorkServiceActiveEndpoint))
            {
                var googleDorkParents = Mapper.Map<List<GoogleDorkParentViewModel>>(proxy.GetGoogleDorkParents(GoogleDorkParentSort.Name));
                dorkMaster.GoogleDorkParentList = new MultiSelectList(googleDorkParents, "ID", "Name", null);
            }

            return View("Index", dorkMaster);
        }

        [HttpPost]
        public ActionResult Index(GoogleDorkMasterViewModel dorkMaster)
        {
            using (var proxy = new DorkServiceClient(AppSettings.Config.DorkServiceActiveEndpoint))
            {
                var googleDorkParentIds = dorkMaster.GoogleDorkParentValues.Select(parent => Convert.ToInt32(parent)).ToList();
                dorkMaster.GoogleDorkParentViewModels = Mapper.Map<List<GoogleDorkParentViewModel>>(proxy.SearchGoogleDorks(dorkMaster.SiteToSearch, dorkMaster.Keywords, googleDorkParentIds));
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
