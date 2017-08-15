using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class PermissionViewModel:EntityViewModel
    {
        [Required]
        public string Name { get; set; }
        public ICollection<AccessViewModel> Accesses { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public PermissionViewModel(Permission p)
        {
            this.Id = p.Id;
            this.Name = p.Name;
            this.Accesses = this.GetAccessViewList(p.Accesses);
            this.IsDeleted = p.IsDeleted;
            this.ModifiedDate = p.ModifiedDate;
            this.ModifiedUserId = p.ModifiedUserId;
            this.CreatedDate = p.CreatedDate;
            this.CreatedUserId = p.CreatedUserId;
            this.DeletionDate = p.DeletionDate;
            this.DeletionUserId = p.DeletionUserId;
            
        }
        public PermissionViewModel(){}
        public Permission ToModel()
        {
            Permission p = new Permission
            {
                Id=this.Id,
                DeletionDate= this.DeletionDate,
                DeletionUserId= this.DeletionUserId,
                IsDeleted= this.IsDeleted,
                ModifiedDate= this.ModifiedDate,
                ModifiedUserId= this.ModifiedUserId,
                Name= this.Name,
                CreatedDate= this.CreatedDate,
                CreatedUserId= this.CreatedUserId
            };
            p.Accesses = this.GetAccessList(this.Accesses);
            return p;
        }
        private ICollection<AccessViewModel> GetAccessViewList(ICollection<Access> item)
        {
            List<AccessViewModel> a = new List<AccessViewModel>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        a.Add(new AccessViewModel(i));
                    }
                }
            }
            return a;
        }
        private ICollection<Access> GetAccessList(ICollection<AccessViewModel> item)
        {
            List<Access> a = new List<Access>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        a.Add(i.ToModel());
                    }
                }
            }
            return a;
        }

        public ActionResult AddPermission(PermissionViewModel item)
        {
            try
            {
                Permission p = item.ToModel();
                p.CreatedDate = DateTime.Now;
                db.Permissions.Add(p);
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
                    Message = "failed to insert permission " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdatePermission(PermissionViewModel item)
        {
            try
            {
                Permission c = db.Permissions.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    c.IsDeleted = item.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.Name = item.Name;
                    c.Accesses = item.GetAccessList(item.Accesses);
                    c.CreatedUserId = item.CreatedUserId;

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
                    Type = "Update",
                    Message = "failed to update permission " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeletePermission(PermissionViewModel item)
        {
            try
            {
                Permission c = db.Permissions.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.IsDeleted = true;
                    c.DeletionDate = DateTime.Now;
                    foreach (var i in db.Accesses)
                    {
                        if (i.PermissionId == c.Id)
                        {
                            i.IsDeleted = true;
                        }
                    }
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
                    Message = "failed to delete permission " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
    }
}