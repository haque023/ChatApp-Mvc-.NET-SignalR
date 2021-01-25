using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyChat.DataModels;
using MyChat.Models;

namespace MyChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public ActionResult SendMessage(tbl_messages messages)
        {
            using (ChatAppEntities obj = new ChatAppEntities())
            {
                try
                {
                    messages.sender = Session["UserCode"].ToString();
                    messages.senderName = Session["UserName"].ToString();
                    messages.chatText = messages.chatText == null ? "empty" : messages.chatText;
                    obj.tbl_messages.Add(messages);
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
            }



            return Content("ok");
        }



        public string DeleteReceiverMessage(tbl_messages messages)
        {
            if (messages == null)
            {
                return "Try Again";
            }
            else
            {
                using (ChatAppEntities obj = new ChatAppEntities())
                {
                    var message = obj.Entry(messages);

                    tbl_messages myMessage = obj.tbl_messages.Find(message.Entity.id);
                    myMessage.isDeleteReceiver = "Y";
                    obj.SaveChanges();
                    return "Deleted";
                }
            }

        }


        public string DeleteSenderMessage(tbl_messages messages)
        {
            if (messages == null)
            {
                return "Try Again";
            }
            else
            {
                using (ChatAppEntities obj = new ChatAppEntities())
                {
                    var message = obj.Entry(messages);

                    tbl_messages myMessage = obj.tbl_messages.Find(message.Entity.id);
                    myMessage.isDeleteSender = "Y";
                    obj.SaveChanges();
                    return "Deleted";
                }
            }

        }



        public JsonResult GetAllMessage(string receiver)
        {
            Repository repository = new Repository();
            List<tbl_messages> messages = new List<tbl_messages>();
            if (Session["UserCode"] != null)
            {
                messages = repository.GetMessages(Session["UserCode"].ToString(), receiver);
            }
            else
                return Json("not", JsonRequestBehavior.AllowGet);



            using (ChatAppEntities obj = new ChatAppEntities())
            {
                try
                {
                    string userCode = Session["UserCode"].ToString();
                    messages = obj.tbl_messages.Where(x => (x.receiver == receiver && x.sender == userCode) || (x.receiver == userCode && x.sender == receiver)).ToList();
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
            }




            return Json(messages, JsonRequestBehavior.AllowGet);
        }




    }
}