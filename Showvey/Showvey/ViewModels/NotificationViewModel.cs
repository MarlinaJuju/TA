using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class NotificationViewModel:EntityViewModel
    {
        
        public Guid? FromId { get; set; }
        
        public Guid? ToId { get; set; }
        
        public string Message { get; set; }
        public string FromUsername { get; set; }
        public string ToUsername { get; set; }
        public bool IsRead { get; set; }
        public bool ReceiverDeleted { get; set; }
        public DateTime? ReceiverDeletedDate { get; set; }
        public bool SenderDeleted { get; set; }
        public DateTime? SenderDeletedDate { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public NotificationViewModel(Notification item)
        {
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
            this.FromId = item.FromId;
            this.Id = item.Id;
            this.IsDeleted = item.IsDeleted;
            this.IsRead = item.IsRead;
            this.Message = item.Message;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.ToId = item.ToId;
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
            this.FromUsername = this.GetUsername(FromId);
            this.ToUsername = this.GetUsername(ToId);
            this.ReceiverDeleted = item.ReceiverDeleted;
            this.ReceiverDeletedDate = item.ReceiverDeletedDate;
            this.SenderDeleted = item.SenderDeleted;
            this.SenderDeletedDate = item.SenderDeletedDate;
        }
        public string GetUsername(Guid? id)
        {

            var name = db.Users.Find(id);
            if (name == null)
                return "";
            else
                return name.Username;
        }
        //public string GetId(string username)
        //{

        //    var id = db.Users.Find();
        //    if (id == null)
        //        return "";
        //    else
        //        return id;
        //}
        public NotificationViewModel()
        {

        }
        public Notification ToModel()
        {
            Notification n = new Notification
            {
                DeletionDate = this.DeletionDate,
                DeletionUserId = this.DeletionUserId,
                FromId = this.FromId,
                Id = this.Id,
                IsDeleted = this.IsDeleted,
                IsRead = this.IsRead,
                Message = this.Message,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                ToId = this.ToId,
                CreatedDate= this.CreatedDate,
                CreatedUserId= this.CreatedUserId,
                ReceiverDeleted=this.ReceiverDeleted,
                ReceiverDeletedDate=this.ReceiverDeletedDate,
                SenderDeletedDate=this.SenderDeletedDate,
                SenderDeleted=this.SenderDeleted
            };
            return n;
        }
        public ActionResult AddNotification(NotificationViewModel item)
        {
            try
            {
                Notification n = item.ToModel();
                n.CreatedDate = DateTime.Now;
                db.Notifications.Add(n);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Insertion",
                    Message = "failed to sent mail to " + this.ToUsername
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteReceiver(NotificationViewModel item)
        {
            try
            {
                Notification c = db.Notifications.Find(item.ToModel().Id);
                if (c != null)
                {
                    if (c.SenderDeleted == true)
                    {
                        c.IsDeleted = true;
                        c.DeletionDate = DateTime.Now;
                    }
                    c.ReceiverDeleted = true;
                    c.ReceiverDeletedDate = DateTime.Now;
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Deletion",
                    Message = "failed to delete mail to " + this.ToUsername
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }

        public ActionResult DeleteSender(NotificationViewModel item)
        {
            try
            {
                Notification c = db.Notifications.Find(item.ToModel().Id);
                if (c != null)
                {
                    if (c.ReceiverDeleted == true)
                    {
                        c.IsDeleted = true;
                        c.DeletionDate = DateTime.Now;
                    }
                    c.SenderDeleted = true;
                    c.SenderDeletedDate = DateTime.Now;
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Deletion",
                    Message = "failed to delete mail to " + this.ToUsername
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }

        public ActionResult UpdateNotification(NotificationViewModel item)
        {
            try
            {
                Notification c = db.Notifications.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    c.IsDeleted = item.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.FromId = item.FromId;
                    c.IsRead = item.IsRead;
                    c.Message = item.Message;
                    c.CreatedUserId = item.CreatedUserId;
                    c.ReceiverDeleted = item.ReceiverDeleted;
                    c.ReceiverDeletedDate = item.ReceiverDeletedDate;
                    c.SenderDeleted = item.SenderDeleted;
                    c.SenderDeletedDate = item.SenderDeletedDate;
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
        }
    }
}