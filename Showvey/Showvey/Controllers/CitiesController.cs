using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Showvey.Models;
using Showvey.ViewModels;

namespace Showvey.Controllers
{
    public class CitiesController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<CityViewModel> listCity = new List<CityViewModel>();
        private List<CountryViewModel> listCountry = new List<CountryViewModel>();

        //GET: Cities
        private void UpdateListCity()
        {
            var x = db.Cities.Include(c => c.Country);
            foreach (var item in x)
            {
                if (item.IsDeleted == false && item.Country.IsDeleted == false)
                {
                    listCity.Add(new CityViewModel(item));
                }

            }
            foreach (var item in db.Countries)
            {
                if (item.IsDeleted == false)
                {
                    CountryViewModel country = new CountryViewModel(item);
                    listCountry.Add(country);
                }
            }
        }
        public ActionResult Index()
        {

            UpdateListCity();
            
            return View(listCity);
        }
        [HttpGet]
        public ActionResult Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                SearchController s = new SearchController();
                return s.SearchOnDatabase(searchString);
            }
            else
            {
                if (AccountController.CheckPermission("City-Index"))
                {
                    UpdateListCity();
                    return View(listCity.OrderBy(x=>x.Name));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        //GET: Cities/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("City-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateListCity();
                CityViewModel cityViewModel = listCity.Find(x => x.Id == id);//db.CityViewModels.Find(id);
                if (cityViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(cityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        //GET: Cities/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("City-Create"))
            {
                UpdateListCity();
                ViewBag.CountryId = new SelectList(listCountry, "Id", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //POST: Cities/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CountryId,UserTotal,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId")] CityViewModel cityViewModel)
        {
            if (ModelState.IsValid)
            {
                cityViewModel.Id = Guid.NewGuid();
                cityViewModel.AddCity(cityViewModel);
                return RedirectToAction("Index");
            }
            UpdateListCity();
            ViewBag.CountryId = new SelectList(listCountry, "Id", "Name", cityViewModel.CountryId);
            return View(cityViewModel);
        }

        //GET: Cities/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("City-Edit"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateListCity();
                CityViewModel cityViewModel = listCity.Find(x => x.Id == id);// db.CityViewModels.Find(id);
                if (cityViewModel == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CountryId = new SelectList(listCountry, "Id", "Name", cityViewModel.CountryId);
                return View(cityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //POST: Cities/Edit/5
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CountryId,UserTotal,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId")] CityViewModel cityViewModel)
        {
            if (ModelState.IsValid)
            {
                UpdateListCity();
                cityViewModel.UpdateCity(cityViewModel);
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(listCountry, "Id", "Name", cityViewModel.CountryId);
            return View(cityViewModel);
        }

        //GET: Cities/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("City-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateListCity();
                CityViewModel cityViewModel = listCity.Find(x => x.Id == id);// db.CityViewModels.Find(id);
                if (cityViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(cityViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdateListCity();
            CityViewModel cityViewModel = listCity.Find(x=>x.Id==id);// db.CityViewModels.Find(id);
            cityViewModel.DeleteCity(cityViewModel);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
}
}
