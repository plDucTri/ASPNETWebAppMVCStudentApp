using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class AccountController : Controller
    {
        // Using your existing Entity Framework context
        private SchoolDBEntities db = new SchoolDBEntities();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            // Entity Framework query to the tblUsers table in SchoolDB
            var user = db.tblUsers.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                Session["CurrentUser"] = user.Username;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Login failed. Please check your username and password.";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}