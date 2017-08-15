using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class QuestionAnswerViewModel:EntityViewModel
    {
        [Required]
        public Guid QuestionId { get; set; }
        public QuestionViewModel Question { get; set; }
        [Required]
        public string Answer { get; set; }
        public int OrderNumber { get; set; }
        public int Total { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public QuestionAnswerViewModel()
        {

        }
        public QuestionAnswerViewModel(QuestionAnswer item)
        {
            this.Answer = item.Answer;
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
            this.Id = item.Id;
            this.IsDeleted = item.IsDeleted;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.OrderNumber = item.OrderNumber;
            //this.Question = new QuestionViewModel(item.Question);
            this.QuestionId = item.QuestionId;
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
            
        }
        public QuestionAnswer ToModel()
        {
            QuestionAnswer q = new QuestionAnswer
            {
                Answer = this.Answer,
                DeletionDate = this.DeletionDate,
                DeletionUserId = this.DeletionUserId,
                Id = this.Id,
                IsDeleted = this.IsDeleted,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                OrderNumber = this.OrderNumber,
                Question = db.Questions.Find(this.QuestionId),
                QuestionId = this.QuestionId,
                CreatedDate=this.CreatedDate,
                CreatedUserId=this.CreatedUserId
            };
            return q;
        }

        public void GetTotal()
        {
            this.Total = db.Responses.Where(x => x.ResponseAnswer == this.Id).Count();
        }

        public ActionResult AddQuestionAnswer(QuestionAnswerViewModel item)
        {
            try
            {
                QuestionAnswer q = item.ToModel();
                q.CreatedDate = DateTime.Now;
                db.QuestionAnswers.Add(q);
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
                    Message = "failed to insert question answer " + this.Answer + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }

        }
        public ActionResult UpdateQuestionAnswer(QuestionAnswerViewModel item)
        {
            try
            {
                QuestionAnswer c = db.QuestionAnswers.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.IsDeleted = item.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    c.Answer = item.Answer;
                    c.OrderNumber = item.OrderNumber;
                    c.Question = db.Questions.Find(item.QuestionId);
                    c.QuestionId = item.QuestionId;
                    c.CreatedUserId = item.CreatedUserId;
                    //c.CreatedDate = item.CreatedDate;

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
                    Message = "failed to update question answer " + this.Answer + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteQuestionAnswer(QuestionAnswerViewModel item)
        {
            try
            {
                QuestionAnswer c = db.QuestionAnswers.Find(item.ToModel().Id);
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
                    Message = "failed to delete question answer " + this.Answer + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
    }
}