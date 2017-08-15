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
    public class SurveyTypesController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        List<SurveyTypeViewModel> listSurveyType = new List<SurveyTypeViewModel>();

        private void UpdateListSurveyType()
        {
            foreach (var item in db.SurveyTypes)
            {
                if (item.IsDeleted == false)
                {
                    listSurveyType.Add(new SurveyTypeViewModel(item));
                }
            }
        }

        // GET: SurveyTypes
        public ActionResult Index()
        {
            UpdateListSurveyType();
            return View(listSurveyType);
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
                if (AccountController.CheckPermission("SurveyType-Index"))
                {
                    UpdateListSurveyType();
                    return View(listSurveyType);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        // GET: SurveyTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("SurveyType-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateListSurveyType();
                SurveyTypeViewModel surveyTypeViewModel = listSurveyType.Find(x => x.Id == id);
                if (surveyTypeViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(surveyTypeViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: SurveyTypes/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("Access-Index"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: SurveyTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,SurveyTotal,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId")] SurveyTypeViewModel surveyTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                surveyTypeViewModel.Id = Guid.NewGuid();
                surveyTypeViewModel.AddSurveyType(surveyTypeViewModel);
                return RedirectToAction("Index");
            }

            return View(surveyTypeViewModel);
        }

        // GET: SurveyTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("SurveyType-Edit"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateListSurveyType();
                SurveyTypeViewModel surveyTypeViewModel = listSurveyType.Find(x => x.Id == id);
                if (surveyTypeViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(surveyTypeViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: SurveyTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,SurveyTotal,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId")] SurveyTypeViewModel surveyTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                surveyTypeViewModel.UpdateSurveyType(surveyTypeViewModel);
                return RedirectToAction("Index");
            }
            return View(surveyTypeViewModel);
        }

        // GET: SurveyTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("SurveyType-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateListSurveyType();
                SurveyTypeViewModel surveyTypeViewModel = listSurveyType.Find(x => x.Id == id);
                if (surveyTypeViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(surveyTypeViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: SurveyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdateListSurveyType();
            SurveyTypeViewModel surveyTypeViewModel = listSurveyType.Find(x=>x.Id==id);
            surveyTypeViewModel.DeleteSurveyType(surveyTypeViewModel);
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
