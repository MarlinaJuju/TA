using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class CityViewModel : EntityViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<UserViewModel> Users { get; set; }
        public int UserTotal { get; set; }
        public int SurveyTotal { get; set; }
        public string CountryName { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        public CityViewModel(City city)
        {
            this.Id = city.Id;
            this.CountryId = city.CountryId;
            this.Country = db.Countries.Find(CountryId);
            if(this.Country.IsDeleted == true)
            {
                this.CountryName = "";
            }
            else{
                this.CountryName = this.Country.Name;
            }
            this.Name = city.Name;
            this.ModifiedDate = city.ModifiedDate;
            this.ModifiedUserId = city.ModifiedUserId;
            this.Users = GetUserViewList(city.Users);
            UserTotal = this.GetUserTotal(city.Id);
            SurveyTotal = this.GetSurveyTotal(city.Id);
            this.DeletionDate = city.DeletionDate;
            this.CreatedUserId = city.DeletionUserId;
            this.CreatedDate = city.CreatedDate;
            this.CreatedUserId = city.CreatedUserId;
        }
        public CityViewModel()
        {
            
        }

        public City ToModel()
        {
            City c = new City
            {
                Id = this.Id,
                CountryId = this.CountryId,
                Name = this.Name,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                IsDeleted = this.IsDeleted,
                DeletionDate = this.DeletionDate,
                DeletionUserId = this.DeletionUserId,
                //Users = this.GetUserList(this.Users),
                Country=db.Countries.Find(this.CountryId),
                CreatedDate=this.CreatedDate,
                CreatedUserId=this.CreatedUserId
            };
        return c;
        }
        public ICollection<UserViewModel> GetUserViewList(ICollection<User> user)
        {
            List<UserViewModel> u = new List<UserViewModel>();
            if (user != null)
            {
                foreach (var item in user)
                {
                    if (item.IsDeleted == false)
                    {
                        u.Add(new UserViewModel(item));
                    }
                }
            }
            return u;
        }

    public ICollection<User> GetUserList(ICollection<UserViewModel> user)
    {
        List<User> u = new List<User>();
        if (user != null)
        {
            foreach (var item in user)
            {
                    u.Add(item.ToModel());
            }
        }
        return u;
    }
    public int GetSurveyTotal(Guid id)
        {
            int tot = 0;
            var user = db.Surveys.Where(x => x.User.CityId == id).Select(x => x);
            foreach (var item in user)
            {
                if (item.IsDeleted == false)
                {
                    tot++;
                }
            }
            return tot;
        }
        public int GetUserTotal(Guid Id)
        {
            int tot = 0;
            var user = db.Users.Where(x => x.CityId == Id).Select(x=>x);
            foreach (var item in user)
            {
                if (item.IsDeleted == false)
                {
                    tot++;
                }
            }
            return tot;
        }
        public ActionResult AddCity(CityViewModel item)
        {
            try
            {
                City c = item.ToModel();
                c.Users = item.GetUserList(item.Users);
                c.CreatedDate = DateTime.Now;
                db.Cities.Add(c);
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
                    Message = "failed to insert city " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdateCity(CityViewModel item)
        {
            try
            {
                City c = db.Cities.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.CountryId = item.CountryId;
                    c.Name = item.Name;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.IsDeleted = item.IsDeleted;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    c.Users = item.GetUserList(item.Users);
                    c.Country = db.Countries.Find(c.CountryId);
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
                    Message = "failed to update city " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteCity(CityViewModel item)
        {
            try
            {
                City c = db.Cities.Find(item.ToModel().Id);
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
                    Message = "failed to delete city " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
    }

}