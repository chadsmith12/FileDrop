using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace FileDrop.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : FileDropControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}