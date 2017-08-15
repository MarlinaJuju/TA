using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class QuestionTypeViewModel:EntityViewModel
    {
        [Required]
        public string Type { get; set; }
        public ICollection<QuestionViewModel> Questions { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public QuestionTypeViewModel(QuestionType item)
        {
            this.Id = item.Id;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.Type = item.Type;
            this.Questions = this.GetQuestionViewList(item.Questions);
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
            this.IsDeleted = item.IsDeleted;
            
        }
        public QuestionTypeViewModel()
        {

        }
        public QuestionType ToModel()
        {
            QuestionType q = new QuestionType
            {
                Id = this.Id,
                IsDeleted = this.IsDeleted,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                Questions = this.GetQuestionList(this.Questions),
                Type = this.Type,
                CreatedDate= this.CreatedDate,
                CreatedUserId= this.CreatedUserId,
                DeletionDate= this.DeletionDate,
                DeletionUserId= this.DeletionUserId
            };
            return q;
        }
        private ICollection<QuestionViewModel> GetQuestionViewList(ICollection<Question> item)
        {
            List<QuestionViewModel> q = new List<QuestionViewModel>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        q.Add(new QuestionViewModel(i));
                    }
                }
            }
            return q;
        }

        private ICollection<Question> GetQuestionList(ICollection<QuestionViewModel> item)
        {
            List<Question> q = new List<Question>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        q.Add(i.ToModel());
                    }
                }
            }
            return q;
        }

        public ActionResult AddQuestionType(QuestionTypeViewModel item)
        {
            try
            {
                QuestionType q = item.ToModel();
                q.Questions = item.GetQuestionList(item.Questions);
                q.CreatedDate = DateTime.Now;
                db.QuestionTypes.Add(q);
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
                    Message = "failed to insert question type " + this.Type + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdatQuestionType(QuestionTypeViewModel item)
        {
            try
            {
                QuestionType c = db.QuestionTypes.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.DeletionDate = item.DeletionDate;
                    c.IsDeleted = item.IsDeleted;
                    c.DeletionUserId = item.DeletionUserId;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.Questions = item.GetQuestionList(item.Questions);
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
                    Message = "failed to update question type " + this.Type + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteQuestionType(QuestionTypeViewModel item)
        {
            try
            {
                QuestionType c = db.QuestionTypes.Find(item.ToModel().Id);
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
                    Id = new Guid(),
                    CreatedDate = DateTime.Now,
                    Type = "Deletion",
                    Message = "failed to delete question type " + this.Type + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(200);
            }
        }
    }
}