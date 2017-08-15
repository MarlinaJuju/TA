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
    public class ImageTypesController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<ImageTypeViewModel> listImageType = new List<ImageTypeViewModel>();

        private void UpdateList()
        {
            foreach (var item in db.ImageTypes)
            {
                if (item.IsDeleted == false)
                {
                    listImageType.Add(new ImageTypeViewModel(item));
                }
            }
        }
        

        // GET: ImageTypes
        public ActionResult Index()
        {
            UpdateList();
            return View(listImageType);
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
                if (AccountController.CheckPermission("ImageType-Index"))
                {
                    UpdateList();
                    return View(listImageType.OrderBy(x => x.Type));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
        }
        // GET: ImageTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("ImageType-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                ImageTypeViewModel imageTypeViewModel = listImageType.Find(x => x.Id == id);
                if (imageTypeViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(imageTypeViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: ImageTypes/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("ImageType-Create"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ImageTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,Width,Height,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId")] ImageTypeViewModel imageTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                imageTypeViewModel.Id = Guid.NewGuid();
                imageTypeViewModel.AddImageType(imageTypeViewModel);
                return RedirectToAction("Index");
            }

            return View(imageTypeViewModel);
        }

        // GET: ImageTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("ImageType-Edit"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                ImageTypeViewModel imageTypeViewModel = listImageType.Find(x => x.Id == id);
                if (imageTypeViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(imageTypeViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ImageTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,Width,Height,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId")] ImageTypeViewModel imageTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                imageTypeViewModel.UpdateImageType(imageTypeViewModel);
                return RedirectToAction("Index");
            }
            return View(imageTypeViewModel);
        }

        // GET: ImageTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("ImageType-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                ImageTypeViewModel imageTypeViewModel = listImageType.Find(x => x.Id == id);
                if (imageTypeViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(imageTypeViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ImageTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdateList();
            ImageTypeViewModel imageTypeViewModel = listImageType.Find(x=>x.Id==id);
            imageTypeViewModel.DeleteImageType(imageTypeViewModel);
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
