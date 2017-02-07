using System.Web.Mvc;
using Asp_mvc_2.Security;
using System.Web.Security; 
using asp_mvc_2.Models.ViewModel; 
using asp_mvc_2.Models.EntityManager;
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

    [AuthorizeRoles("Admin")]
    public ActionResult ManageUserPartial()
    {
        if (User.Identity.IsAuthenticated)
        {
            string loginName = User.Identity.Name;
            UserManager UM = new UserManager();
            UserLoginView.UserDataView UDV = UM.GetUserDataView(loginName);
            return PartialView(UDV);
        }
        return View();
    }

    [AuthorizeRoles("Admin")]
    public ActionResult ManageUserPartial(string status = "")
    {
        if (User.Identity.IsAuthenticated)
        {
            string loginName = User.Identity.Name;
            UserManager UM = new UserManager();
            UserLoginView.UserDataView UDV = UM.GetUserDataView(loginName);
            string message = string.Empty;
            if (status.Equals("update"))
                message = "Update Successful";
            else if (status.Equals("delete"))
                message = "Delete Successful";
            ViewBag.Message = message;
            return PartialView(UDV);
        }
        return RedirectToAction("Index", "Home");
    }




    }
}


