using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class RoleViewModel:EntityViewModel
    {
        [Required]
        public string Name { get; set; }
        public ICollection<AccessViewModel> Accesses { get; set; }
        public ICollection<UserViewModel> Users { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public RoleViewModel(Role item)
        {
            this.Accesses = new List<AccessViewModel>();
            this.Accesses=this.GetAccessViewList(item.Accesses);
            this.Id = item.Id;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.Name = item.Name;
            this.Users = this.GetUserViewList(item.Users);
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
            this.IsDeleted = item.IsDeleted;
            
        }
        public RoleViewModel()
        {

        }
        public Role ToModel()
        {
            Role r = new Role
            {
                Accesses = this.GetAccessList(this.Accesses),
                DeletionDate = this.DeletionDate,
                DeletionUserId = this.DeletionUserId,
                Id = this.Id,
                IsDeleted = this.IsDeleted,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                Name = this.Name,
                Users = this.GetUserList(this.Users),
                CreatedDate= this.CreatedDate,
                CreatedUserId= this.CreatedUserId
            };
            return r;
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
        private ICollection<UserViewModel> GetUserViewList(ICollection<User> item)
        {
            List<UserViewModel> a = new List<UserViewModel>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        a.Add(new UserViewModel(i));
                    }
                }
            }
            return a;
        }
        private ICollection<User> GetUserList(ICollection<UserViewModel> item)
        {
            List<User> a = new List<User>();
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
        public ActionResult AddRole(RoleViewModel item)
        {
            try
            {
                Role r = item.ToModel();
                r.Accesses = item.GetAccessList(item.Accesses);
                r.Users = item.GetUserList(item.Users);
                r.CreatedDate = DateTime.Now;
                db.Roles.Add(r);
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
                    Message = "failed to insert role " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdateRole(RoleViewModel item)
        {
            try
            {
                Role c = db.Roles.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    c.IsDeleted = item.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.Accesses = item.GetAccessList(item.Accesses);
                    c.Name = item.Name;
                    c.Users = item.GetUserList(item.Users);
                    //c.CreatedDate = item.CreatedDate;
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
                    Message = "failed to update role " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteRole(RoleViewModel item)
        {
            try
            {
                Role c = db.Roles.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.IsDeleted = true;
                    c.DeletionDate = DateTime.Now;
                    foreach (var i in db.Accesses)
                    {
                        if (i.RoleId == c.Id)
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
                    Message = "failed to delete role " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
    }
}