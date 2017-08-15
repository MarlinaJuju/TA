using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class AccessViewModel:EntityViewModel
    {
        [Required]
        public Guid RoleId { get; set; }
        public RoleViewModel Role { get; set; }
        [Required]
        public Guid PermissionId { get; set; }
        public PermissionViewModel Permission { get; set; }
        public bool IsGranted { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public AccessViewModel(Access item)
        {
            this.Id = item.Id;
            this.IsDeleted = item.IsDeleted;
            this.IsGranted = item.IsGranted;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.Permission = new PermissionViewModel(db.Permissions.Find(item.PermissionId));
            this.PermissionId = item.PermissionId;
            this.Role = new RoleViewModel(db.Roles.Find(item.RoleId));
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
        }
        public AccessViewModel()
        {

        }
        public Access ToModel()
        {
            Access a = new Access
            {
                Id = this.Id,
                IsDeleted = this.IsDeleted,
                IsGranted = this.IsGranted,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                Permission = db.Permissions.Find(this.PermissionId),
                PermissionId = this.PermissionId,
                Role = db.Roles.Find(this.RoleId),
                RoleId = this.RoleId,
                DeletionDate= this.DeletionDate,
                DeletionUserId= this.DeletionUserId,
                CreatedDate=this.CreatedDate,
                CreatedUserId=this.CreatedUserId
            };
            return a;
        }
        //ko, kalau pakai actionresult bgmn? Boleh g?
        public ActionResult AddAccess()
        {
            try
            {
                Access a = this.ToModel();
                a.CreatedDate = DateTime.Now;
                db.Accesses.Add(a);
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Insertion",
                    Message = "failed to insert access to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
        public ActionResult UpdateAccess()
        {
            try
            {
                Access c = db.Accesses.Find(this.ToModel().Id);
                if (c != null)
                {
                    c.Id = this.Id;
                    c.IsDeleted = this.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = this.ModifiedUserId;
                    c.DeletionDate = this.DeletionDate;
                    c.DeletionUserId = this.DeletionUserId;
                    c.IsGranted = this.IsGranted;
                    c.Permission = db.Permissions.Find(this.PermissionId);
                    c.PermissionId = this.PermissionId;
                    c.Role = db.Roles.Find(this.RoleId);
                    c.RoleId = this.RoleId;
                    //c.CreatedDate = this.CreatedDate;
                    c.CreatedUserId = this.CreatedUserId;
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id =Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Update",
                    Message = "failed to update access to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult DeleteAccess()
        {
            try
            {
                Access c = db.Accesses.Find(this.ToModel().Id);
                if (c != null)
                {
                    c.IsDeleted = true;
                    c.DeletionDate = DateTime.Now;
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Deletion",
                    Message = "failed to delete access to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
    }
}