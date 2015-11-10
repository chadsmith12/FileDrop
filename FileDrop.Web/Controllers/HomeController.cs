using System.Web.Mvc;

namespace FileDrop.Web.Controllers
{
    public class HomeController : FileDropControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}