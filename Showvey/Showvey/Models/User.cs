using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class User:Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public Guid? CityId { get; set; }
        public City City { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public Guid RoleId { get; set; }
        public bool IsComplete { get; set; }
        public ICollection<Survey> Surveys { get; set; }
        public ICollection<Response> Responses { get; set; }
        public Role Role { get; set; }
        public string PasswordToken { get; set; }
        public bool TokenActivated { get; set; }
        public DateTime? PasswordTokenExpired { get; set; }
    }
}