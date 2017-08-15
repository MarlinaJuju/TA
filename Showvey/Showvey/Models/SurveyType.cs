using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class SurveyType:Entity
    {
        public string Type { get; set; }
        public ICollection<Survey> Surveys { get; set; }
    }
}