using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Showvey.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class SurveyViewModel:EntityViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid SurveyTypeId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public SurveyTypeViewModel SurveyType { get; set; }
        public string Type { get; set; }
        public bool IsBlock { get; set; }
        public bool IsCompleted { get; set; }
        public ICollection<QuestionViewModel> Questions { get; set; }
        public List<AnimateViewModel> ShowAnimate { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        
        public void GetAnimate()
        {
            ShowAnimate = new List<AnimateViewModel>();
            if (Questions.Count >0)
            {
                Guid id = Questions.ElementAt(0).Id;
                var animate = db.Animates.Where(x => x.QuestionId == id);
                if (animate != null)
                {
                    foreach (var item in animate)
                    {
                        ShowAnimate.Add(new AnimateViewModel(item));
                    }
                }
            }
            ShowAnimate.OrderBy(x => x.imageType);
        }

        public ICollection<Question> GetQuestionList(ICollection<QuestionViewModel> item)
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
        public ICollection<QuestionViewModel> GetQuestionViewList(ICollection<Question> item)
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
        public void GetQuestionViewList()
        {
            if (this.Questions == null)
            {
                this.Questions = new List<QuestionViewModel>();
            }
            List<QuestionViewModel> q = new List<QuestionViewModel>();
                foreach (var i in db.Questions)
                {
                    if (i.IsDeleted == false && i.SurveyId==this.Id)
                    {
                        q.Add(new QuestionViewModel(i));
                    }
                }
            this.Questions=q.OrderBy(x => x.Number).ToList();
        }



        public void GetUserName(Guid? id)
        {

            var name=db.Users.Find(id);
            if (name == null)
                UserName= "";
            else
                UserName= name.Username;
        }
        public void GetSurveyType(Guid? id)
        {
            var name = db.SurveyTypes.Find(id);
            if (name == null)
                Type= "";
            else
                Type= name.Type;
        }
        public ActionResult AddSurvey(SurveyViewModel s)
        {
            try
            {
                var item = s.ToModel();
                item.CreatedDate = DateTime.Now;
                db.Surveys.Add(item);
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
                    Message = "failed to insert survey from " + this.UserName + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdateSurvey(SurveyViewModel s)
        {
            try
            {
                Survey item = db.Surveys.Find(s.ToModel().Id);
                if (item != null)
                {
                    item.IsBlock = s.IsBlock;
                    item.IsDeleted = s.IsDeleted;
                    item.ModifiedDate = DateTime.Now;
                    item.ModifiedUserId = s.ModifiedUserId;
                    item.SurveyTypeId = s.SurveyTypeId;
                    item.Title = s.Title;
                    item.DeletionDate = s.DeletionDate;
                    item.DeletionUserId = s.DeletionUserId;
                    //item.CreatedDate = s.CreatedDate;
                    item.CreatedUserId = s.CreatedUserId;
                    item.Description = s.Description;
                    //item.Id = s.Id;
                    item.Questions = s.GetQuestionList(s.Questions);
                    item.SurveyType = db.SurveyTypes.Find(s.SurveyTypeId);
                    item.User = db.Users.Find(s.UserId);
                    //item.UserId = s.UserId;
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(200);
            }
            catch(System.Data.Entity.Validation.DbEntityValidationException dbEx)
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
        }
        public ActionResult DeleteSurvey(SurveyViewModel s)
        {
            try
            {
                Survey item = db.Surveys.Find(s.ToModel().Id);
                if (item != null)
                {
                    item.DeletionDate = DateTime.Now;
                    item.IsDeleted = true;
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
                    Message = "failed to delete survey from " + this.UserName + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public Survey ToModel()
        {
            //ViewModel ke Model
            return new Survey {
                Id=this.Id,
                SurveyTypeId=this.SurveyTypeId,
                Title=this.Title,
                UserId = this.UserId,
                CreatedDate=this.CreatedDate,
                CreatedUserId=this.CreatedUserId,
                DeletionDate=this.DeletionDate,
                DeletionUserId=this.DeletionUserId,
                Description=this.Description,
                IsBlock=this.IsBlock,
                IsDeleted=this.IsDeleted,
                ModifiedDate=this.ModifiedDate,
                ModifiedUserId=this.ModifiedUserId,
                Questions=this.GetQuestionList(this.Questions)
            };
        }
        public SurveyViewModel() { }
        public SurveyViewModel(Survey s)
        {
            this.Id = s.Id;
            this.SurveyTypeId = s.SurveyTypeId;
            this.Title = s.Title;
            this.UserId = s.UserId;
            this.IsBlock = false;
            this.GetUserName(this.UserId);
            this.GetSurveyType(this.SurveyTypeId);
            this.SurveyType = new SurveyTypeViewModel(db.SurveyTypes.Find(s.SurveyTypeId));
            this.CreatedDate = s.CreatedDate;
            this.CreatedUserId = s.CreatedUserId;
            this.DeletionDate = s.DeletionDate;
            this.DeletionUserId = s.DeletionUserId;
            this.Description = s.Description;
            this.IsDeleted = s.IsDeleted;
            this.ModifiedDate = s.ModifiedDate;
            this.ModifiedUserId = s.ModifiedUserId;
        }
    }
}