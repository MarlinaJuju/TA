using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Respondent:Entity
    {
        public string BrowserName { get; set; }
        public string IPAdress { get; set; }
        public bool IsRegistered { get; set; }
        public List<Response> Responses { get; set; }
        public Guid SurveyId { get; set; }
        public Guid UserId { get; set; }
        //public User User { get; set; }
        //public Survey Survey { get; set; }
    }
}