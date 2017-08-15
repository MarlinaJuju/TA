using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Country:Entity
    {
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}