using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class ImageType:Entity
    {
        public string Type { get; set; }
        public ICollection<Image> Images { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}