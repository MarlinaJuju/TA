using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class SurveyTypeViewModel:EntityViewModel
    {
        [Required]
        public string Type { get; set; }
        public ICollection<SurveyViewModel> Surveys { get; set; }
        public int SurveyTotal { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public SurveyTypeViewModel(SurveyType surveyType )
        {
            this.Id = surveyType.Id;
            this.ModifiedDate = surveyType.ModifiedDate;
            this.ModifiedUserId = surveyType.ModifiedUserId;
            this.Type = surveyType.Type;
            this.Surveys = this.GetSurveyViewList(surveyType.Surveys);
            this.SurveyTotal = db.Surveys.Where(x=>x.SurveyTypeId==Id).Count();
            this.CreatedDate = surveyType.CreatedDate;
            this.CreatedUserId = surveyType.CreatedUserId;
            this.DeletionDate = surveyType.DeletionDate;
            this.DeletionUserId = surveyType.DeletionUserId;
            this.IsDeleted = surveyType.IsDeleted;
            
        }
        public SurveyTypeViewModel(){ }
        public SurveyType ToModel()
        {
            SurveyType s = new SurveyType
            {
                Id = this.Id,
                DeletionDate = this.DeletionDate,
                DeletionUserId = this.DeletionUserId,
                IsDeleted = this.IsDeleted,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                Surveys = this.GetSurveyList(this.Surveys),
                Type = this.Type,
                CreatedDate= this.CreatedDate,
                CreatedUserId= this.CreatedUserId
            };
            return s;
        }
        private int GetSurveyTotal(ICollection<SurveyViewModel> s)
        {
            return s.Count();
        }
        private ICollection<SurveyViewModel> GetSurveyViewList(ICollection<Survey> item)
        {
            List<SurveyViewModel> s = new List<SurveyViewModel>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    s.Add(new SurveyViewModel(i));
                }
            }
            return s;
        }

        private ICollection<Survey> GetSurveyList(ICollection<SurveyViewModel> item)
        {
            List<Survey> s = new List<Survey>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    s.Add(i.ToModel());
                }
            }
            return s;
        }

        public ActionResult AddSurveyType(SurveyTypeViewModel item)
        {
            try
            {
                SurveyType c = item.ToModel();
                c.Surveys = item.GetSurveyList(item.Surveys);
                c.CreatedDate = DateTime.Now;
                db.SurveyTypes.Add(c);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Insertion",
                    Message = "failed to insert survey type " + this.Type + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdateSurveyType(SurveyTypeViewModel item)
        {
            try
            {
                SurveyType c = db.SurveyTypes.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    c.IsDeleted = item.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.Surveys = this.GetSurveyList(item.Surveys);
                    c.Type = item.Type;
                    //c.CreatedDate = item.CreatedDate;
                    c.CreatedUserId = item.CreatedUserId;

                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Update",
                    Message = "failed to update survey type " + this.Type + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteSurveyType(SurveyTypeViewModel item)
        {
            try
            {
                SurveyType c = db.SurveyTypes.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.IsDeleted = true;
                    c.DeletionDate = DateTime.Now;
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Deletion",
                    Message = "failed to delete survey type " + this.Type + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
    }
}