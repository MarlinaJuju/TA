using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class QuestionType:Entity
    {
        [Required]
        public string Type { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}