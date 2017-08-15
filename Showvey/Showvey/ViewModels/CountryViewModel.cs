using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class CountryViewModel : EntityViewModel
    {
        [Required]
        public string Name { get; set; }
        public ICollection<CityViewModel> Cities { get; set; }
        public int UserTotal { get; set; }
        public int SurveyTotal { get; set; }
        private SurveyDataContext db = new SurveyDataContext();

        public CountryViewModel(Country country)
        {
            this.Id = country.Id;
            this.IsDeleted = country.IsDeleted;
            this.DeletionUserId = country.DeletionUserId;
            this.ModifiedDate = country.ModifiedDate;
            this.ModifiedUserId = country.ModifiedUserId;
            this.Name = country.Name;
            this.Cities = this.GetCityViewList(country.Id);
            this.UserTotal = this.GetUserTotal(this.Id);
            this.SurveyTotal = this.GetSurveyTotal(this.Id);
            this.CreatedDate = country.CreatedDate;
            this.CreatedUserId = country.CreatedUserId;
        }
        public CountryViewModel() { }
        public Country ToModel()
        {
            Country c = new Country
            {
                Id = this.Id,
                DeletionDate = this.DeletionDate,
                IsDeleted = this.IsDeleted,
                DeletionUserId = this.DeletionUserId,
                ModifiedDate = this.ModifiedDate,
                ModifiedUserId = this.ModifiedUserId,
                Name = this.Name
            };
            c.Cities = this.GetCityList(this.Cities);
            return c;
        }
        private int GetUserTotal(Guid id)
        {
            int tot = 0;
            var user = db.Cities.Where(x => x.CountryId == Id).Select(x => x);
            CityViewModel c = new CityViewModel();
            foreach (var item in user)
            {
                if (item.IsDeleted == false)
                {
                    tot+=c.GetUserTotal(item.Id);
                }
            }
            return tot;
        }
        private int GetSurveyTotal(Guid id)
        {
            int tot = 0;
            var user = db.Cities.Where(x => x.CountryId == Id).Select(x => x);
            CityViewModel c = new CityViewModel();
            foreach (var item in user)
            {
                if (item.IsDeleted == false)
                {
                    tot += c.GetSurveyTotal(item.Id);
                }
            }
            return tot;
        }
        private ICollection<CityViewModel> GetCityViewList(Guid id)
        {
            List<CityViewModel> l = new List<CityViewModel>();
               foreach (var item in db.Cities)
                {
                    if (item.IsDeleted == false && item.CountryId==id)
                    {
                        l.Add(new CityViewModel(item));
                    }
                }
                
            
            return l;
        }

        private ICollection<City> GetCityList(ICollection<CityViewModel> i)
        {
            List<City> l = new List<City>();
            if (i != null)
            {
                foreach (var item in i)
                {
                    l.Add(item.ToModel());
                }
                
            }
            return l;
        }
        public ActionResult AddCountry(CountryViewModel item)
        {
            try
            {
                Country c = item.ToModel();
                c.Cities = item.GetCityList(item.Cities);
                db.Countries.Add(c);
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
                    Message = "failed to insert country " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdateCountry(CountryViewModel item)
        {
            try
            {
                Country c = db.Countries.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.DeletionDate = item.DeletionDate;
                    c.IsDeleted = item.IsDeleted;
                    c.DeletionUserId = item.DeletionUserId;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.Name = item.Name;
                    c.Cities = item.GetCityList(item.Cities);
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
                    Message = "failed to update country " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteCountry(CountryViewModel item)
        {
            try
            {
                Country c = db.Countries.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.IsDeleted = true;
                    c.DeletionDate = DateTime.Now;
                    //foreach (var i in db.Cities)
                    //{
                    //    if (i.CountryId == c.Id)
                    //    {
                    //        i.IsDeleted = true;
                    //    }
                    //}
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
                    Message = "failed to delete country " + this.Name + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }

        }
    }
}