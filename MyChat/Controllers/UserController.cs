using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyChat.DataModels;

namespace MyChat.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getAllUser()
        {
            using (UserEntities obj = new UserEntities())
            {
                List<tbl_users> users = obj.tbl_users.ToList();
                return Json(users, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SignOut()
        {
            Session.RemoveAll();
            return RedirectToAction("", "user");
        }
        public ActionResult Home()
        {
            tbl_users user = new tbl_users();
            if (Session["UserCode"] == null)
            {
                return RedirectToAction("", "user");
            }
            else
            {
                string userCode = Session["UserCode"].ToString();
                using (UserEntities obj = new UserEntities())
                {
                    user = obj.tbl_users.Where(key => key.userCode == userCode).FirstOrDefault();
                    return View(user);
                }

            }
        }

        [HttpPost]
        public ActionResult SignIn(string email)
        {
            tbl_users user;
            if (Session["UserCode"] != null)
            {
                return RedirectToAction("Home", "User");
            }

            else
            {
                using (UserEntities obj = new UserEntities())
                {
                    user = obj.tbl_users.Where(key => key.email == email).FirstOrDefault();

                }
                if (user != null)
                {
                    Session["UserCode"] = user.userCode;
                    Session["UserName"] = user.firstName;
                    
                    return RedirectToAction("Home", "User");
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }

        }


        public string insertNewUser(tbl_users user)
        {
            if (user != null)
            {
                using (UserEntities obj = new UserEntities())
                {
                    try
                    {
                        obj.tbl_users.Add(user);
                        obj.SaveChanges();
                    }
                    catch (DbEntityValidationException ee)
                    {
                        foreach (var error in ee.EntityValidationErrors)
                        {
                            foreach (var thisError in error.ValidationErrors)
                            {
                                var errorMessage = thisError.ErrorMessage;
                            }
                        }
                    }
                    return "User Sign-Up successfully";
                }
            }
            else
            {
                return "Try Again";
            }
        }
    }
}