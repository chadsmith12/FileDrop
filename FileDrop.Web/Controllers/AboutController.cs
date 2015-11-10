using System.Web.Mvc;

namespace FileDrop.Web.Controllers
{
    public class AboutController : FileDropControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}