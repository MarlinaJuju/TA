using Showvey.Controllers;
using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Key]
        public Guid Id { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string RetypePassword { get; set; }
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public List<AccessViewModel> Permissions { get; set; }
        private SurveyDataContext db = new SurveyDataContext();

        public void Authenticate()
        {
            UserId = GetId(Email);
            var r = db.Users.Find(UserId);
            AccountController.RememberUser(new UserViewModel(r));
            var x = db.Roles.Find(r.RoleId);
            if (x != null)
            {
                ICollection<Access> a = db.Accesses.Where(y => y.RoleId == x.Id && y.IsDeleted == false).ToList();
                Permissions = GetPermissions(a);
                AccountController.RememberPermission(Permissions);
            }
            UserViewModel u = new UserViewModel(r);
            u.IsActive = true;
            u.LastLogin = DateTime.Now;
            u.UpdateUser(u);
        }
        public Guid GetId(string email)
        {
            var id = db.Users.Where(u => u.Email.ToLower().Equals(email));
            if (id.Any())
            {
                return id.FirstOrDefault().Id;
            }
            else
            {
                return Guid.Empty;
            }
        }
        public string GetPassword(string email)
        {
            var pw = db.Users.Where(u => u.Email.ToLower().Equals(email));
            if (pw.Any())
            {
                return pw.FirstOrDefault().Password;
            }
            else
            {
                return string.Empty;
            }
        }
        public List<AccessViewModel> GetPermissions(ICollection<Access> p)
        {
            if (p.Any())
            {
                List<AccessViewModel> a = new List<AccessViewModel>();
                foreach (var item in p)
                {
                    a.Add(new AccessViewModel(item));
                }
                return a;
            }
            else
                return null;
        }

    }
}