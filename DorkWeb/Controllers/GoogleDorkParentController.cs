using System.Linq;
using System.Web.Mvc;
using DorkWeb.DorkServiceReference;
using DorkWeb.Models;

namespace DorkWeb.Controllers
{
    public class GoogleDorkParentController : Controller
    {
        public ActionResult DisplayGoogleDorkParents()
        {
            var proxy = new DorkServiceClient(AppSettings.Config.DorkServiceActiveEndpoint);
            var googleDorkParents = proxy.GetGoogleDorkParents(GoogleDorkParentSort.Name);

            var parent = new GoogleDorkParentViewModel();
            if (googleDorkParents == null || !googleDorkParents.Any())
            {
                return View("DisplayGoogleDorkParents", parent);
            }
            parent.Id = googleDorkParents[0].Id;
            parent.Name = googleDorkParents[0].Name;

            return View("DisplayGoogleDorkParents", parent);
        }
    }
}
