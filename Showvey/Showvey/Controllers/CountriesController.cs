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
    public class CountriesController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        List<CountryViewModel> listCountry = new List<CountryViewModel>();
        CountryViewModel country = new CountryViewModel();
        // GET: CountryViewModels
        public ActionResult Index()
        {
            UpdateListCountry();
            return View(listCountry);
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
                if (AccountController.CheckPermission("Country-Index"))
                {
                    UpdateListCountry();
                    return View(listCountry.OrderBy(x=>x.Name));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        private List<CountryViewModel> UpdateListCountry()
        {
            CountryViewModel c = new CountryViewModel();
            foreach (var item in db.Countries)
            {
                if (item.IsDeleted == false)
                {
                    CountryViewModel country = new CountryViewModel(item);
                    listCountry.Add(country);
                }
            }
            return listCountry;
        }
        // GET: CountryViewModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("Country-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateListCountry();
                CountryViewModel countryViewModel = listCountry.Find(x => x.Id == id);
                if (countryViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(countryViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: CountryViewModels/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("Country-Create"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: CountryViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId")] CountryViewModel countryViewModel)
        {
            if (ModelState.IsValid)
            {
                countryViewModel.Id = Guid.NewGuid();
                country.AddCountry(countryViewModel);
                //db.CountryViewModels.Add(countryViewModel);
                return RedirectToAction("Index");
            }

            return View(countryViewModel);
        }

        // GET: CountryViewModels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("Country-Edit"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateListCountry();
                CountryViewModel countryViewModel = listCountry.Find(x => x.Id == id);//db.CountryViewModels.Find(id);
                if (countryViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(countryViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: CountryViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId")] CountryViewModel countryViewModel)
        {
            if (ModelState.IsValid)
            {
                UpdateListCountry();
                country.UpdateCountry(countryViewModel);
                return RedirectToAction("Index");
            }
            return View(countryViewModel);
        }

        // GET: CountryViewModels/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("Country-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateListCountry();
                CountryViewModel countryViewModel = listCountry.Find(x => x.Id == id); //db.CountryViewModels.Find(id);
                if (countryViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(countryViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: CountryViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdateListCountry();
            CountryViewModel countryViewModel = listCountry.Find(x=>x.Id==id);//db.CountryViewModels.Find(id);
            country.DeleteCountry(countryViewModel);
            //db.CountryViewModels.Remove(countryViewModel);
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
