using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class QuestionAnswer:Entity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public string Answer { get; set; }
        public int OrderNumber { get; set; }
    }
}