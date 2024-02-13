using AuthenticationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AuthenticationMVC.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(UserModel model)
        {
            using (AuthDBEntities _db = new AuthDBEntities())
            {
                bool isValidUser =
                    _db.Users.Any(user => user.UserName.ToLower() ==
                    model.UserName.ToLower() && user.UserPassword == model.UserPassword);

                if (isValidUser)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Employees");
                }

                ModelState.AddModelError("", "Invalid username and/or password");
                return View();
            }
        }


        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserModel model)
        {
            using (AuthDBEntities _db = new AuthDBEntities())
            {
                if (_db.Users.Any(us => us.UserName.ToLower() == model.UserName.ToLower()))
                {
                    ModelState.AddModelError("", "Username already exists");
                    return View();
                }

                User user = new User()
                {
                    UserName = model.UserName,
                    UserPassword = model.UserPassword
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                return RedirectToAction("Login");
            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}