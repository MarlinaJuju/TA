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
    public class ImagesController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        //private IQueryable<Image> x;
        private List<ImageViewModel> listImages = new List<ImageViewModel>();
        private List<ImageTypeViewModel> listImageTypes = new List<ImageTypeViewModel>();
        private void UpdateList()
        {
            var x= db.Images.Include(k => k.ImageType);
            foreach (var item in x)
            {
                if (item.IsDeleted == false)
                {
                    listImages.Add(new ImageViewModel(item));
                }
            }
            foreach (var item in db.ImageTypes)
            {
                if (item.IsDeleted == false)
                {
                    listImageTypes.Add(new ImageTypeViewModel(item));
                }
            }
        }

        public List<ImageViewModel> GetCharacters()
        {
            List<ImageViewModel> c = new List<ImageViewModel>();
            Guid id = listImageTypes.Find(x => x.Type == "Character").Id;
            UpdateList();
            foreach (var item in listImages)
            {
                if (item.ImageTypeId == id)
                {
                    c.Add(item);
                }
            }
            return c;
        }


        // GET: Images
        public ActionResult Index()
        {
            UpdateList();
            //var imageViewModels = db.Images.Include(i => i.ImageType);
            return View(listImages);
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
                if (AccountController.CheckPermission("Image-Index"))
                {
                    UpdateList();
                    return View(listImages.OrderBy(x=>x.Name));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        // GET: Images/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("Image-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                ImageViewModel imageViewModel = listImages.Find(x => x.Id == id);//db.ImageViewModels.Find(id);
                if (imageViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(imageViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: Images/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("Image-Create"))
            {
                UpdateList();
                ViewBag.ImageTypeId = new SelectList(listImageTypes, "Id", "Type");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Location,Name,ImageTypeId,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] ImageViewModel imageViewModel)
        {
            if (ModelState.IsValid)
            {
                imageViewModel.Id = Guid.NewGuid();
                imageViewModel.AddImage(imageViewModel);
                return RedirectToAction("Index");
            }
            ViewBag.ImageTypeId = new SelectList(listImageTypes, "Id", "Type", imageViewModel.ImageTypeId);
            return View(imageViewModel);
        }

        // GET: Images/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("Image-Edit"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                ImageViewModel imageViewModel = listImages.Find(x => x.Id == id);//db.ImageViewModels.Find(id);
                if (imageViewModel == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ImageTypeId = new SelectList(listImageTypes, "Id", "Type", imageViewModel.ImageTypeId);
                return View(imageViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Location,Name,ImageTypeId,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] ImageViewModel imageViewModel)
        {
            if (ModelState.IsValid)
            {
                imageViewModel.UpdateImage(imageViewModel);
                return RedirectToAction("Index");
            }
            UpdateList();
            ViewBag.ImageTypeId = new SelectList(listImageTypes, "Id", "Type", imageViewModel.ImageTypeId);
            return View(imageViewModel);
        }

        // GET: Images/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("Image-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                ImageViewModel imageViewModel = listImages.Find(x => x.Id == id);//db.ImageViewModels.Find(id);
                if (imageViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(imageViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdateList();
            ImageViewModel imageViewModel = listImages.Find(x => x.Id == id);//db.ImageViewModels.Find(id);
            imageViewModel.DeleteImage(imageViewModel);
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
