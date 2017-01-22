using System.Web.Mvc; 
namespace Asp_mvc_2.Controllers 
{ 
    public class HomeController : Controller 
    { public ActionResult Index()
    {

        return View();
    }
    [Authorize]
    public ActionResult Welcome() { return View(); }
    }
}