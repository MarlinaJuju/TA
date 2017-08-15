using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Animate:Entity
    {
        public Guid? QuestionId { get; set; }
        public Question Question { get; set; }
        public Guid? ImageId { get; set; }
        public Image Image { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public int Depth { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public Animate()
        {
            TimeStart = TimeSpan.FromSeconds(1);
            TimeEnd = TimeSpan.FromSeconds(1);
        }
    }
}