using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Role:Entity
    {
        public string Name { get; set; }
        public ICollection<Access> Accesses { get; set; }
        public ICollection<User> Users { get; set; }
    }
}