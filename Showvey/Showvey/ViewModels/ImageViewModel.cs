using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class ImageViewModel:EntityViewModel
    {
        [Required]
        public string Location { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid ImageTypeId { get; set; }
        [Required]
        public double Width { get; set; }
        [Required]
        public double Height { get; set; }
        public string Type { get; set; }
        public ImageTypeViewModel ImageType { get; set; }
        public ICollection<AnimateViewModel> Animates { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public ImageViewModel()
        {

        }
        public ImageViewModel(Image i)
        {
            this.Id = i.Id;
            //this.Animates = this.GetAnimateViewList(i.Animates);
            this.ImageTypeId = i.ImageTypeId;
            this.Width = i.Width;
            this.Height = i.Height;
            this.ImageType = new ImageTypeViewModel( db.ImageTypes.Find(i.ImageTypeId));
            if (this.ImageType.IsDeleted == true)
            {
                this.Type = "";
            }
            else
            {
                this.Type = this.ImageType.Type;
            }
            this.ModifiedDate = i.ModifiedDate;
            this.ModifiedUserId = i.ModifiedUserId;
            this.Name = i.Name;
            this.Location = i.Location;
            this.CreatedDate = i.CreatedDate;
            this.CreatedUserId = i.CreatedUserId;
            this.DeletionDate = i.DeletionDate;
            this.DeletionUserId = i.DeletionUserId;
            this.IsDeleted = i.IsDeleted;
        }
        public Image ToModel()
        {
            Image img = new Image();
            img.DeletionDate = this.DeletionDate;
                img.DeletionUserId = this.DeletionUserId;
                img.Id = this.Id;
                img.ImageType = db.ImageTypes.Find(this.ImageTypeId);
                img.Location = this.Location;
                img.ModifiedDate = this.ModifiedDate;
                img.ModifiedUserId = this.ModifiedUserId;
                img.Name = this.Name;
                img.CreatedDate = this.CreatedDate;
                img.CreatedUserId = this.CreatedUserId;
                img.ImageTypeId = this.ImageTypeId;
                img.IsDeleted = this.IsDeleted;
            img.Width = this.Width;
            img.Height = this.Height;
            
            return img;
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
        private void GetAnimateList(ICollection<AnimateViewModel> item, ICollection<Animate> from)
        {
            //            List<Animate> a = new List<Animate>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        from.Add(i.ToModel());
                    }
                }
            }
//            return a;
        }
        public string GetImageType(Guid imageId)
        {
            string type = "none";
            ImageViewModel i = new ImageViewModel(db.Images.Find(imageId));
            if (i != null && i.IsDeleted==false)
            {
                type = db.ImageTypes.Find(i.ImageTypeId).Type;
            }
            return type;
        }
        public ImageTypeViewModel GetType(Guid imageId)
        {
            ImageTypeViewModel type = new ImageTypeViewModel();
            ImageViewModel i = new ImageViewModel(db.Images.Find(imageId));
            if (i != null && i.IsDeleted == false)
            {
                type = new ImageTypeViewModel(db.ImageTypes.Find(i.ImageTypeId));
            }
            return type;
        }
        public ActionResult AddImage(ImageViewModel item)
        {
            try
            {
                Image i = item.ToModel();
                item.GetAnimateList(item.Animates,i.Animates);
                i.CreatedDate = DateTime.Now;
                db.Images.Add(i);
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
                    Message = "failed to insert image " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdateImage(ImageViewModel item)
        {
            try
            {
                Image c = db.Images.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.IsDeleted = item.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    item.GetAnimateList(item.Animates,c.Animates);
                    c.ImageType = db.ImageTypes.Find(item.ImageTypeId);
                    c.Location = item.Location;
                    //c.CreatedDate = item.CreatedDate;
                    c.CreatedUserId = item.CreatedUserId;
                    c.ImageTypeId = item.ImageTypeId;
                    c.Name = item.Name;
                    c.Name = item.Name;
                    c.Height = item.Height;
                    c.Width = item.Width;
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
                    Message = "failed to update image " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteImage(ImageViewModel item)
        {
            try
            {
                Image c = db.Images.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.IsDeleted = true;
                    c.DeletionDate = DateTime.Now;
                    foreach (var i in db.Animates)
                    {
                        if (i.ImageId == c.Id)
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
                    Message = "failed to delete image " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(200);
            }
        }
    }
}