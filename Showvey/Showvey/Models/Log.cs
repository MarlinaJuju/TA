using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Log
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}