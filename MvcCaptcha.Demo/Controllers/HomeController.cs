using ImageDraw;
using System.Web.Mvc;

namespace MvcCaptcha.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, MvcCaptcha]
        public ActionResult Index(string name, string email)
        {
            return View();
        }
    }
}
