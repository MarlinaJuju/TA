using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class ResponseViewModel:EntityViewModel
    {
        public Guid QuestionId { get; set; }
        public QuestionViewModel Question { get; set; }
        public Guid ResponseAnswer { get; set; }
        [Required]
        public Guid RespondentId { get; set; }
        public Respondent Respondent { get; set; }
        public string Answer { get; set; }
        public int Number { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public ResponseViewModel()
        {

        }
        public ResponseViewModel(Response item)
        {
            //this.BrowserName = item.BrowserName;
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
            this.Id = item.Id;
            //this.IPAdress = item.IPAdress;
            this.IsDeleted = item.IsDeleted;
            //this.IsRegistered = item.IsRegistered;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.QuestionId = item.QuestionId;
            //this.Question = new QuestionViewModel(item.Question);
            this.ResponseAnswer = item.ResponseAnswer;
            //this.UserId = item.UserId;
            //this.User = new UserViewModel(item.User);
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
            this.RespondentId = item.RespondentId;
            this.Answer = db.QuestionAnswers.Find(ResponseAnswer).Answer;
            this.Number = db.Questions.Find(QuestionId).Number;
        }
        public Response ToModel()
        {
            Response r = new Response
            {
                DeletionDate = this.DeletionDate,
                DeletionUserId = this.DeletionUserId,
                Id = this.Id,
                IsDeleted = this.IsDeleted,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                Question = db.Questions.Find(this.QuestionId),
                QuestionId = this.QuestionId,
                ResponseAnswer = this.ResponseAnswer,
                CreatedDate = this.CreatedDate,
                CreatedUserId = this.CreatedUserId,
                RespondentId = this.RespondentId,
                Respondent = db.Respondents.Find(this.RespondentId)
            };
            return r;
        }
        public ActionResult AddResponse(ResponseViewModel item)
        {
            try
            {
                Response r = item.ToModel();
                r.CreatedDate = DateTime.Now;
                db.Responses.Add(r);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {

                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        LogViewModel l = new LogViewModel
                        {
                            Id = Guid.NewGuid(),
                            CreatedDate = DateTime.Now,
                            Type = "Update",
                            Message = message
                        };
                        l.AddLog(l);
                        db.SaveChanges();
                        // raise a new exception nesting
                        // the current instance as InnerException
                    }
                }
                return new HttpStatusCodeResult(400);
            }
            //catch
            //{
            //    LogViewModel l = new LogViewModel
            //    {
            //        Id = Guid.NewGuid(),
            //        CreatedDate = DateTime.Now,
            //        Type = "Insertion",
            //        Message = "failed to insert response from" + this.IPAdress + " to database"
            //    };
            //    l.AddLog(l);
            //    return new HttpStatusCodeResult(400);
            //}
        }
        public ActionResult UpdateResponse(ResponseViewModel item)
        {
            try
            {
                Response c = db.Responses.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.IsDeleted = item.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    //c.BrowserName = item.BrowserName;
                    //c.IPAdress = item.IPAdress;
                    c.IsDeleted = item.IsDeleted;
                    //c.IsRegistered = item.IsRegistered;
                    c.Question = db.Questions.Find(item.QuestionId);
                    c.QuestionId = item.QuestionId;
                    c.ResponseAnswer = item.ResponseAnswer;
                    //c.User = db.Users.Find(item.UserId);
                    //c.UserId = item.UserId;
                    //c.CreatedDate = item.CreatedDate;
                    c.CreatedUserId = item.CreatedUserId;

                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteResponse(ResponseViewModel item)
        {
            try
            {
                Response c = db.Responses.Find(item.ToModel().Id);
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
                return new HttpStatusCodeResult(400);
            }
        }
    }
}