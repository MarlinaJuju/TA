using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class SurveyCategory : Entity
    {
        public string Name { get; set; }
        public ICollection<Survey> Surveys { get; set; }
    }
}