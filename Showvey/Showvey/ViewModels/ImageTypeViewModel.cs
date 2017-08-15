using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Showvey.ViewModels
{
    public class ImageTypeViewModel:EntityViewModel
    {
        [Required]
        public string Type { get; set; }
        public ICollection<ImageViewModel> Images { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        [Required]
        public double Width { get; set; }
        [Required]
        public double Height { get; set; }
        public ImageTypeViewModel(ImageType item)
        {
            this.Id = item.Id;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.Type = item.Type;
            //this.Images = this.GetImageViewList(item.Images);
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
            this.IsDeleted = item.IsDeleted;
            this.Height = item.Height;
            this.Width = item.Width;
        }
        public ImageTypeViewModel(){}
        public ImageType ToModel()
        {
            ImageType i = new ImageType
            {
                Id = this.Id,
                IsDeleted = this.IsDeleted,
                Images = this.GetImageList(this.Images),
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                Type = this.Type,
                CreatedDate = this.CreatedDate,
                CreatedUserId = this.CreatedUserId,
                DeletionDate = this.DeletionDate,
                DeletionUserId = this.DeletionUserId,
                Width = this.Width,
                Height=this.Height
            };
            return i;
        }
        private ICollection<ImageViewModel> GetImageViewList(ICollection<Image> item)
        {
            List<ImageViewModel> img = new List<ImageViewModel>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        img.Add(new ImageViewModel(i));
                    }
                }
            }
            return img;
        }
        private ICollection<Image> GetImageList(ICollection<ImageViewModel> item)
        {
            List<Image> img = new List<Image>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        img.Add(i.ToModel());
                    }
                }
            }
            return img;
        }
        public ActionResult AddImageType(ImageTypeViewModel item)
        {
            try
            {
                ImageType i = item.ToModel();
                i.CreatedDate = DateTime.Now;
                db.ImageTypes.Add(i);
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
                    Message = "failed to insert image type " + this.Type + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdateImageType(ImageTypeViewModel item)
        {
            try
            {
                ImageType c = db.ImageTypes.Find(item.ToModel().Id);
                if (c != null)
                {
                    //c.Id = item.Id;
                    c.IsDeleted = item.IsDeleted;
                    c.Images = item.GetImageList(item.Images);
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.Type = item.Type;
                    //c.CreatedDate = item.CreatedDate;
                    c.CreatedUserId = item.CreatedUserId;
                    c.Width = item.Width;
                    c.Height = item.Height;
                    //c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    db.SaveChanges();
                }
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
                //catch ()
                //{
                //    LogViewModel l = new LogViewModel
                //    {
                //        Id = Guid.NewGuid(),
                //        CreatedDate = DateTime.Now,
                //        Type = "Update",
                //        Message = "failed to update image type " + this.Type + " to database"
                //    };
                //    l.AddLog(l);


                //    return new HttpStatusCodeResult(200);
                //}

                return new HttpStatusCodeResult(200);
            }
        }
        public ActionResult DeleteImageType(ImageTypeViewModel item)
        {
            try
            {
                ImageType c = db.ImageTypes.Find(item.ToModel().Id);
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
                    Message = "failed to delete image type " + this.Type + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(200);
            }
        }
    }
}