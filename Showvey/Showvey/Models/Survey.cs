using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Survey:Entity
    {
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid SurveyTypeId { get; set; }
        public SurveyType SurveyType { get; set; }
        public ICollection<Question> Questions { get; set; }
        public bool IsBlock { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}