using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Response : Entity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public Guid ResponseAnswer { get; set; }
        public Guid RespondentId { get; set; }
        public Respondent Respondent { get; set; }
    }
}