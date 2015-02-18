using HouseOfKings.Web.ViewModels;
using System.Web.Mvc;

namespace HouseOfKings.Web.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Index(string groupName)
        {
            return View(new GameViewModel() { GroupName = groupName });
        }
    }
}