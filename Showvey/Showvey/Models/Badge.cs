using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Badge : Entity
    {
        public string Name { get; set; }
        //jumlah requirement
        public int Quantity { get; set; }

    }
}