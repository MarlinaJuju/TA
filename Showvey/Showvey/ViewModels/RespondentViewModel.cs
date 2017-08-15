using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class RespondentViewModel:EntityViewModel
    {
        [Required]
        public string BrowserName { get; set; }
        [Required]
        public string IPAdress { get; set; }
        public bool IsRegistered { get; set; }
        [Required]
        public Guid SurveyId { get; set; }
        public Guid UserId { get; set; }
        public List<ResponseViewModel> Responses { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public RespondentViewModel()
        {

        }
        public RespondentViewModel(Respondent item)
        {
            this.BrowserName = item.BrowserName;
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
            this.Id = item.Id;
            this.IPAdress = item.IPAdress;
            this.IsDeleted = item.IsDeleted;
            this.IsRegistered = item.IsRegistered;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
            this.SurveyId = item.SurveyId;
            this.UserId = item.UserId;
        }

        public Respondent ToModel()
        {
            Respondent r = new Respondent
            {
                DeletionDate = this.DeletionDate,
                DeletionUserId = this.DeletionUserId,
                Id = this.Id,
                IsDeleted = this.IsDeleted,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                CreatedDate = this.CreatedDate,
                CreatedUserId = this.CreatedUserId,
                BrowserName=this.BrowserName,
                IPAdress=this.IPAdress,
                IsRegistered=this.IsRegistered,
                SurveyId=this.SurveyId,
                UserId=this.UserId
            };
            return r;
        }
        public void GetResponse()
        {
            if (this.Responses == null)
            {
                this.Responses = new List<ResponseViewModel>();
            }
            var r = db.Responses.Where(x => x.RespondentId == this.Id);
            List<ResponseViewModel> re = new List<ResponseViewModel>();
            foreach (var item in r)
            {
                re.Add(new ResponseViewModel(item));
            }
            this.Responses = re.OrderBy(x => x.Number).ToList();
        }

        public ActionResult AddRespondent(RespondentViewModel item)
        {
            try
            {
                Respondent r = item.ToModel();
                r.CreatedDate = DateTime.Now;
                db.Respondents.Add(r);
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
        public ActionResult UpdateRespondent(RespondentViewModel item)
        {
            try
            {
                Respondent c = db.Respondents.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.IsDeleted = item.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    c.BrowserName = item.BrowserName;
                    c.IPAdress = item.IPAdress;
                    c.IsDeleted = item.IsDeleted;
                    c.IsRegistered = item.IsRegistered;
                    c.CreatedUserId = item.CreatedUserId;
                    c.SurveyId = item.SurveyId;
                    c.UserId = item.UserId;

                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteRespondent(RespondentViewModel item)
        {
            try
            {
                Respondent c = db.Respondents.Find(item.ToModel().Id);
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