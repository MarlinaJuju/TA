using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Showvey.Models;
using Showvey.ViewModels;

namespace Showvey.Controllers
{
    public class NotificationsController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<NotificationViewModel> listNotification = new List<NotificationViewModel>();
        private List<UserViewModel> listUser = new List<UserViewModel>();
        private void UpdateList()
        {
            foreach (var item in db.Notifications)
            {
                if (item.IsDeleted == false)
                {
                    listNotification.Add(new NotificationViewModel(item));
                }
            }
            foreach (var item in db.Users)
            {
                if (item.IsDeleted == false)
                {
                    listUser.Add(new UserViewModel(item));
                }
            }
        }
        private void ReceiveEmail(List<NotificationViewModel> n)
        {
            UpdateList();
            foreach (var item in listNotification)
            {
                if (AccountController.CheckUser(item.ToId) && item.ReceiverDeleted==false)
                {
                    n.Add(item);
                }
            }
        }
        private void SentEmail(List<NotificationViewModel> n)
        {
            UpdateList();
            foreach (var item in listNotification)
            {
                if (AccountController.CheckUser(item.FromId) && item.SenderDeleted==false)
                {
                    n.Add(item);
                }
            }
        }
        // GET: Notifications
        public ActionResult Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                SearchController s = new SearchController();
                return s.SearchOnDatabase(searchString);
            }
            else
            {
                if (AccountController.CheckPermission("Notification-Index"))
                {
                    List<NotificationViewModel> n = new List<NotificationViewModel>();
                    ReceiveEmail(n);
                    //if (n.Count > 1)
                    //{
                    //    listNotification.Sort((x, y) => y.CreatedDate.CompareTo(x.CreatedDate));
                    //}
                    foreach (var item in n.ToList())
                    {
                        if (item.Message.Length >= 30)
                        {
                            item.Message = item.Message.Substring(0, 30) + "...";
                        }
                    }
                    ViewBag.Count = n.Count;
                    return View(n);
                }
                else if (AccountController.CheckPermission("Notification-Sent"))
                {
                    return RedirectToAction("Sent");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            
        }
        public ActionResult Sent()
        {
            if (AccountController.CheckPermission("Notification-Sent"))
            {
                List<NotificationViewModel> n = new List<NotificationViewModel>();
                SentEmail(n);

                foreach (var item in n.ToList())
                {
                    if (item.Message.Length >= 30)
                    {
                        item.Message = item.Message.Substring(0, 30) + "...";
                    }
                }
                ViewBag.Count = n.Count;
                return View(n);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ReceiverDelete(string[] msg)
        {
            if (msg != null && msg.Length > 0)
            {
                foreach (var item in msg)
                {
                    var id = new Guid(item);
                    UpdateList();
                    NotificationViewModel notificationViewModel = listNotification.Find(x => x.Id == id);//db.NotificationViewModels.Find(id);
                    notificationViewModel.DeleteReceiver(notificationViewModel);
                }
                // If there is any book to delete
                // Perform your delete in the database or datasource HERE
            }
            // And finally, redirect to the action that lists the books
            // (let's assume it's Index)
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SenderDelete(string[] msg)
        {
            if (msg != null && msg.Length > 0)
            {
                foreach (var item in msg)
                {
                    var id = new Guid(item);
                    UpdateList();
                    NotificationViewModel notificationViewModel = listNotification.Find(x => x.Id == id);//db.NotificationViewModels.Find(id);
                    notificationViewModel.DeleteSender(notificationViewModel);
                }
                // If there is any book to delete
                // Perform your delete in the database or datasource HERE
            }
            // And finally, redirect to the action that lists the books
            // (let's assume it's Index)
            return RedirectToAction("Index");
        }

        public ActionResult Mailbox()
        {
            UpdateList();
            List<NotificationViewModel> n = new List<NotificationViewModel>();
            var notif = listNotification.Where(x => x.ReceiverDeleted == false && x.IsRead == false && x.ToId == AccountController.GetUser().Id);
            //listNotification.Reverse();
            foreach (var item in notif)
            {
                    if (item.Message.Length >= 30)
                    {
                        item.Message = item.Message.Substring(0, 30) + "...";
                    }
                    n.Add(item);
            }
            ViewBag.Count = n.Count;
            if (n.Count > 3)
            {
                return PartialView(n.OrderBy(x=>x.CreatedDate).Take(3));
            }
            else
            {
                return PartialView(n.OrderBy(x => x.CreatedDate));
            }
        }
        // GET: Notifications/Details/5
        public ActionResult Details(Guid? id,bool sender)
        {
            if (AccountController.CheckPermission("Notification-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                NotificationViewModel notificationViewModel = listNotification.Find(x => x.Id == id);//db.NotificationViewModels.Find(id);
                if (notificationViewModel == null)
                {
                    return HttpNotFound();
                }
                if (sender == false)
                {
                    notificationViewModel.IsRead = true;
                }
                notificationViewModel.UpdateNotification(notificationViewModel);
                return View(notificationViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Notifications/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("Notification-Create"))
            {
                UpdateList();
                ViewBag.FromId = new SelectList(listUser, "Id", "Username");
                ViewBag.ToId = new SelectList(listUser, "Id", "Username");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FromId,ToId,Message,IsRead,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] NotificationViewModel notificationViewModel)
        {
            if (ModelState.IsValid)
            {
                UpdateList();
                var user = listUser.Where(x => x.RoleName == "User" && x.IsDeleted == false);
                foreach (var item in user)
                {
                    notificationViewModel.Id = Guid.NewGuid();
                    notificationViewModel.FromId = AccountController.GetUser().Id;
                    notificationViewModel.ToId = item.Id;
                    notificationViewModel.AddNotification(notificationViewModel);
                }
                return RedirectToAction("Index");
            }
            ViewBag.FromId = new SelectList(listUser, "Id", "Username", notificationViewModel.FromId);
            ViewBag.ToId = new SelectList(listUser, "Id", "Username", notificationViewModel.ToId);
            return View(notificationViewModel);
        }

        // GET: Notifications/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UpdateList();
        //    NotificationViewModel notificationViewModel = listNotification.Find(x => x.Id == id);//db.NotificationViewModels.Find(id);
        //    if (notificationViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.FromId = new SelectList(listUser, "Id", "Username", notificationViewModel.FromId);
        //    ViewBag.ToId = new SelectList(listUser, "Id", "Username", notificationViewModel.ToId);
        //    return View(notificationViewModel);
        //}

        //// POST: Notifications/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,FromId,ToId,Message,IsRead,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] NotificationViewModel notificationViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        UpdateList();
        //        //notificationViewModel.UpdateNotification(notificationViewModel);
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.FromId = new SelectList(listUser, "Id", "Username", notificationViewModel.FromId);
        //    ViewBag.ToId = new SelectList(listUser, "Id", "Username", notificationViewModel.ToId);
        //    return View(notificationViewModel);
        //}

        //// GET: Notifications/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UpdateList();
        //    NotificationViewModel notificationViewModel = listNotification.Find(x => x.Id == id);//db.NotificationViewModels.Find(id);
        //    if (notificationViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(notificationViewModel);
        //}

        // POST: Notifications/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    UpdateList();
        //    NotificationViewModel notificationViewModel = listNotification.Find(x => x.Id == id);//db.NotificationViewModels.Find(id);
        //    notificationViewModel.DeleteNotification(notificationViewModel);
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
