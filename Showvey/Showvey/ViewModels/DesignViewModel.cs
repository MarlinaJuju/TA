using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.ViewModels
{
    public class DesignViewModel:EntityViewModel
    {
        [Required]
        public Guid? QuestionId { get; set; }
        public QuestionViewModel Question { get; set; }
        [Required]
        public Guid? ImageId { get; set; }
        public ImageViewModel Image { get; set; }
        [Required]
        public Guid? ImageTypeId { get; set; }
        public ImageTypeViewModel ImageType { get; set; }
        [Required]
        public double Width { get; set; }
        [Required]
        public double Height { get; set; }
        [Required]
        public double PosX { get; set; }
        [Required]
        public double PosY { get; set; }
        [Required]
        public int Depth { get; set; }
        [Required]
        public DateTime TimeStart { get; set; }
        [Required]
        public DateTime TimeEnd { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public DesignViewModel()
        {
                
        }
    }
}