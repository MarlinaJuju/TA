using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class AnimateViewModel:EntityViewModel
    {
        [Required]
        public Guid? QuestionId { get; set; }
        public QuestionViewModel Question { get; set; }
        [Required]
        public Guid? ImageId { get; set; }
        public ImageViewModel Image { get; set; }
        [Required]
        public double Width { get; set; }
        [Required]
        public double Height { get; set; }
        [Required]
        public double PosX { get; set; }
        [Required]
        public double PosY { get; set; }
        [Required]
        public int Depth { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }

        //extended
        public string imageType { get; set; }
        public string Location { get; set; }

        private SurveyDataContext db = new SurveyDataContext();
        public AnimateViewModel()
        {

        }
        public AnimateViewModel(Animate item)
        {
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
            this.Depth = item.Depth;
            this.Height = item.Height;
            this.Id = item.Id;
            this.ImageId = item.ImageId;
            //this.Image = new ImageViewModel(item.Image);
            this.IsDeleted = item.IsDeleted;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.PosX = item.PosX;
            this.PosY = item.PosY;
            this.QuestionId = item.QuestionId;
            //this.Question = new QuestionViewModel(item.Question);
            this.TimeEnd = item.TimeEnd;
            this.TimeStart = item.TimeStart;
            this.Width = item.Width;
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
            this.imageType = db.ImageTypes.Find((db.Images.Find(this.ImageId).ImageTypeId)).Type;
            this.Location = db.Images.Find(this.ImageId).Location;
        }
        public Animate ToModel()
        {
            Animate a = new Animate
            {
                DeletionDate = this.DeletionDate,
                DeletionUserId = this.DeletionUserId,
                Depth = this.Depth,
                Height = this.Height,
                Id = this.Id,
                ImageId = this.ImageId,
                Image = db.Images.Find(this.ImageId),
                IsDeleted = this.IsDeleted,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                PosX = this.PosX,
                PosY = this.PosY,
                Question = db.Questions.Find(this.QuestionId),
                QuestionId = this.QuestionId,
                //TimeEnd = this.TimeEnd,
                //TimeStart = this.TimeStart,
                Width = this.Width,
                CreatedDate= this.CreatedDate,
                CreatedUserId= this.CreatedUserId
            };
            return a;
        }
        public ActionResult AddAnimate()
        {
            try
            {
                Animate a = this.ToModel();
                a.CreatedDate = DateTime.Now;
                db.Animates.Add(a);
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
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
                //catch
                //{
                //    LogViewModel l = new LogViewModel
                //    {
                //        Id = Guid.NewGuid(),
                //        CreatedDate = DateTime.Now,
                //        Type = "Insertion",
                //        Message = "failed to insert animation to database"
                //    };
                //    l.AddLog(l);
                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
        public ActionResult UpdateAnimate()
        {
            try
            {
                Animate c = db.Animates.Find(this.ToModel().Id);
                if (c != null)
                {
                    c.Id = this.Id;
                    c.IsDeleted = this.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = this.ModifiedUserId;
                    c.DeletionDate = this.DeletionDate;
                    c.DeletionUserId = this.DeletionUserId;
                    c.Depth = this.Depth;
                    c.Height = this.Height;
                    c.ImageId = this.ImageId;
                    c.Image = db.Images.Find(this.ImageId);
                    c.PosX = this.PosX;
                    c.PosY = this.PosY;
                    c.Question = db.Questions.Find(this.QuestionId);
                    c.QuestionId = this.QuestionId;
                    c.TimeEnd = this.TimeEnd;
                    c.TimeStart = this.TimeStart;
                    c.Width = this.Width;
                    //c.CreatedDate = this.CreatedDate;
                    c.CreatedUserId = this.CreatedUserId;
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Update",
                    Message = "failed to update animation to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult DeleteAnimate()
        {
            try
            {
                Animate c = db.Animates.Find(this.ToModel().Id);
                if (c != null)
                {
                    c.IsDeleted = true;
                    c.DeletionDate = DateTime.Now;
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Deletion",
                    Message = "failed to delete access to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
    }
}