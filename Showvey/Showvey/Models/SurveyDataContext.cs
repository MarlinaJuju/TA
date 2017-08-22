using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class SurveyDataContext:DbContext
    {
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SurveyType> SurveyTypes { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<ImageType> ImageTypes { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Access> Accesses { get; set; }
        public virtual DbSet<Response> Responses { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<Animate> Animates { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Respondent> Respondents { get; set; }
        public virtual DbSet<SurveyCategory> SurveyCategories { get; set; }
    }
}