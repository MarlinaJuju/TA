using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class City:Entity
    {
        public string Name { get; set; }
        
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<User> Users { get; set; }
    }
}