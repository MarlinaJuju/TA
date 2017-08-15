using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class QuestionViewModel:EntityViewModel
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public Guid QuestionTypeId { get; set; }
        public QuestionTypeViewModel QuestionType { get; set; }
        [Required]
        public TimeSpan TimeLength { get; set; }
        [Required]
        public Guid SurveyId { get; set; }
        public SurveyViewModel Survey { get; set; }
        public ICollection<QuestionAnswerViewModel> QuestionAnswers { get; set; }
        public ICollection<ResponseViewModel> Responses { get; set; }
        public ICollection<AnimateViewModel> Animates { get; set; }
        public bool IsFreeText { get; set; }
        public int Count { get; set; }
        public bool IsScale { get; set; }
        public string FontColor { get; set; }
        public int FontSize { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public string Type { get; set; }
        public QuestionViewModel(Question item)
        {
            this.Id = item.Id;
            //this.Animates = this.GetAnimateViewList(item.Animates);
            this.Content = item.Content;
            this.IsFreeText = item.IsFreeText;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.Number = item.Number;
            this.QuestionAnswers = this.GetQuestionAnswerViewList(item.QuestionAnswers);
            this.QuestionType = new QuestionTypeViewModel( db.QuestionTypes.Find(item.QuestionTypeId));
            this.QuestionTypeId = item.QuestionTypeId;
            this.Responses = this.GetResponseViewList(item.Responses);
            this.Survey = new SurveyViewModel(db.Surveys.Find(item.SurveyId));
            this.SurveyId = item.SurveyId;
            this.TimeLength = item.TimeLength;
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
            this.IsDeleted = item.IsDeleted;
            this.Count = item.Count;
            this.IsScale = item.IsScale;
            this.FontColor = item.FontColor;
            this.Type = db.QuestionTypes.Find(QuestionTypeId).Type;
            this.FontSize = item.FontSize;
        }
        public QuestionViewModel()
        {

        }
        public Question ToModel()
        {
            Question q = new Question();

            q.Animates = this.GetAnimateList(this.Animates);
            q.Content = this.Content;
            q.DeletionDate = this.DeletionDate;
            q.DeletionUserId = this.DeletionUserId;
            q.Id = this.Id;
            q.IsDeleted = this.IsDeleted;
            q.IsFreeText = this.IsFreeText;
            q.ModifiedDate = this.ModifiedDate;
            q.ModifiedUserId = this.ModifiedUserId;
            q.Number = this.Number;
            q.QuestionAnswers = this.GetQuestionAnswerList(this.QuestionAnswers);
            q.QuestionType = db.QuestionTypes.Find(this.QuestionTypeId);
            q.QuestionTypeId = this.QuestionTypeId;
            q.Responses = this.GetResponseList(this.Responses);
            q.SurveyId = this.SurveyId;
            q.Survey = db.Surveys.Find(this.SurveyId);
            q.TimeLength = this.TimeLength;
            q.CreatedDate = this.CreatedDate;
            q.CreatedUserId = this.CreatedUserId;
            q.Count = this.Count;
            q.IsScale = this.IsScale;
            q.FontColor = this.FontColor;
            q.FontSize = this.FontSize;

            return q;
        }
        

        private ICollection<AnimateViewModel> GetAnimateViewList(ICollection<Animate> item)
        {
            List<AnimateViewModel> a = new List<AnimateViewModel>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        a.Add(new AnimateViewModel(i));
                    }
                }
            }
            return a;
        }
        public void GetAnimateViewList()
        {
            this.Animates = new List<AnimateViewModel>();
                foreach (var i in db.Animates)
                {
                    if (i.IsDeleted == false && i.QuestionId==this.Id)
                    {
                        this.Animates.Add(new AnimateViewModel(i));
                    }
                }
        }
        public void GetQuestionAnswerViewList()
        {
            this.QuestionAnswers = new List<QuestionAnswerViewModel>();
            foreach (var i in db.QuestionAnswers)
            {
                if (i.IsDeleted == false && i.QuestionId == this.Id)
                {
                    this.QuestionAnswers.Add(new QuestionAnswerViewModel(i));
                }
            }
        }
        public void GetResponsesViewList()
        {
            this.Responses = new List<ResponseViewModel>();
            foreach (var i in db.Responses)
            {
                if (i.IsDeleted == false && i.QuestionId == this.Id)
                {
                    this.Responses.Add(new ResponseViewModel(i));
                }
            }
        }
        private ICollection<Animate> GetAnimateList(ICollection<AnimateViewModel> item)
        {
            List<Animate> a = new List<Animate>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        a.Add(i.ToModel());
                    }
                }
            }
            return a;
        }
        private ICollection<ResponseViewModel> GetResponseViewList(ICollection<Response> item)
        {
            List<ResponseViewModel> a = new List<ResponseViewModel>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        a.Add(new ResponseViewModel(i));
                    }
                }
            }
            return a;
        }
        private ICollection<Response> GetResponseList(ICollection<ResponseViewModel> item)
        {
            List<Response> a = new List<Response>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        a.Add(i.ToModel());
                    }
                }
            }
            return a;
        }
        private ICollection<QuestionAnswer> GetQuestionAnswerList(ICollection<QuestionAnswerViewModel> item)
        {
            List<QuestionAnswer> a = new List<QuestionAnswer>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        a.Add(i.ToModel());
                    }
                }
            }
            return a;
        }
        private ICollection<QuestionAnswerViewModel> GetQuestionAnswerViewList(ICollection<QuestionAnswer> item)
        {
            List<QuestionAnswerViewModel> a = new List<QuestionAnswerViewModel>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        a.Add(new QuestionAnswerViewModel(i));
                    }
                }
            }
            return a;
        }
        public ActionResult AddQuestion(QuestionViewModel item)
        {
            try
            {
                Question q = item.ToModel();
                q.CreatedDate = DateTime.Now;
                db.Questions.Add(q);
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
                    Message = "failed to insert question " + this.Content + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdateQuestion(QuestionViewModel item)
        {
            try
            {
                Question c = db.Questions.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.IsDeleted = item.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    c.Animates = item.GetAnimateList(item.Animates);
                    c.Content = item.Content;
                    c.IsFreeText = item.IsFreeText;
                    c.Number = item.Number;
                    c.QuestionAnswers = item.GetQuestionAnswerList(item.QuestionAnswers);
                    c.QuestionType = db.QuestionTypes.Find(item.QuestionTypeId);
                    c.QuestionTypeId = item.QuestionTypeId;
                    c.Responses = item.GetResponseList(item.Responses);
                    c.Survey = db.Surveys.Find(item.SurveyId);
                    c.SurveyId = item.SurveyId;
                    c.TimeLength = item.TimeLength;
                    //c.CreatedDate = item.CreatedDate;
                    c.CreatedUserId = item.CreatedUserId;
                    c.Count = item.Count;
                    c.IsScale = item.IsScale;
                    c.FontColor = item.FontColor;
                    c.FontSize = item.FontSize;

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
                    Message = "failed to update question " + this.Content + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteQuestion(QuestionViewModel item)
        {
            try
            {
                Question c = db.Questions.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.IsDeleted = true;
                    c.DeletionDate = DateTime.Now;
                    foreach (var i in db.Animates)
                    {
                        if (i.QuestionId == c.Id)
                        {
                            i.IsDeleted = true;
                        }
                    }
                    foreach (var i in db.QuestionAnswers)
                    {
                        if (i.QuestionId == c.Id)
                        {
                            i.IsDeleted = true;
                        }
                    }
                    foreach (var i in db.Responses)
                    {
                        if (i.QuestionId == c.Id)
                        {
                            i.IsDeleted = true;
                        }
                    }
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
                    Message = "failed to delete question " + this.Content + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
    }
}