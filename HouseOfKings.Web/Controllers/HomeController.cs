using System.Web.Mvc;

namespace HouseOfKings.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}