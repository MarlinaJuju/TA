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
    public class PermissionsController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<PermissionViewModel> listPermission = new List<PermissionViewModel>();

        // GET: PermissionViewModels
        public ActionResult Index()
        {
            UpdatePermission();
            return View(listPermission);
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
                if (AccountController.CheckPermission("Permission-Index"))
                {
                    UpdatePermission();
                    return View(listPermission.OrderBy(x => x.Name));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        private void UpdatePermission()
        {
            foreach (var item in db.Permissions)
            {
                if(item.IsDeleted==false)
                {
                    listPermission.Add(new PermissionViewModel(item));
                }
            }
        }

        // GET: PermissionViewModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("Permission-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdatePermission();
                PermissionViewModel permissionViewModel = listPermission.Find(x => x.Id == id);
                if (permissionViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(permissionViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: PermissionViewModels/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("Permission-Create"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PermissionViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId")] PermissionViewModel permissionViewModel)
        {
            if (ModelState.IsValid)
            {
                permissionViewModel.Id = Guid.NewGuid();
                permissionViewModel.AddPermission(permissionViewModel);
                return RedirectToAction("Index");
            }

            return View(permissionViewModel);
        }

        // GET: PermissionViewModels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("Permission-Edit"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdatePermission();
                PermissionViewModel permissionViewModel = listPermission.Find(x => x.Id == id);
                if (permissionViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(permissionViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PermissionViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId")] PermissionViewModel permissionViewModel)
        {
            if (ModelState.IsValid)
            {
                permissionViewModel.UpdatePermission(permissionViewModel);
                return RedirectToAction("Index");
            }
            return View(permissionViewModel);
        }

        // GET: PermissionViewModels/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("Permission-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdatePermission();
                PermissionViewModel permissionViewModel = listPermission.Find(x => x.Id == id);
                if (permissionViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(permissionViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PermissionViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdatePermission();
            PermissionViewModel permissionViewModel = listPermission.Find(x=>x.Id==id);
            permissionViewModel.DeletePermission(permissionViewModel);
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
