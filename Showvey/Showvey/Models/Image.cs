using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Image:Entity
    {
        public string Location { get; set; }
        public string Name { get; set; }
        public Guid ImageTypeId { get; set; }
        public ImageType ImageType { get; set; }
        public ICollection<Animate> Animates { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}