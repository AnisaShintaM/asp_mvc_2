using System.Web.Mvc;
using Asp_mvc_2.Security;
namespace asp_mvc_2.Controllers 
{ 
    public class HomeController : Controller 
    { public ActionResult Index()
    {

        return View();
    }
    [Authorize]
    public ActionResult Welcome() { return View(); }


    [AuthorizeRoles("Admin")]
    public ActionResult AdminOnly()
    {
        return View();
    }
    public ActionResult UnAuthorized()
    {
        return View();
    }

    }
}


