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
    public class RolesController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<RoleViewModel> listRole = new List<RoleViewModel>();

        private void UpdateList()
        {
            foreach (var item in db.Roles)
            {
                if (item.IsDeleted == false)
                {
                    listRole.Add(new RoleViewModel(item));
                }
            }
        }
        // GET: Roles
        public ActionResult Index()
        {
            UpdateList();
            return View(listRole);
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
                if (AccountController.CheckPermission("Role-Index"))
                {
                    UpdateList();
                    return View(listRole);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        // GET: Roles/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("Role-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                RoleViewModel roleViewModel = listRole.Find(x => x.Id == id); //db.RoleViewModels.Find(id);
                if (roleViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(roleViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("Role-Create"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                roleViewModel.Id = Guid.NewGuid();
                roleViewModel.AddRole(roleViewModel);
                return RedirectToAction("Index");
            }

            return View(roleViewModel);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("Role-Edit"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                RoleViewModel roleViewModel = listRole.Find(x => x.Id == id);//db.RoleViewModels.Find(id);
                if (roleViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(roleViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                roleViewModel.UpdateRole(roleViewModel);
                return RedirectToAction("Index");
            }
            return View(roleViewModel);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("Role-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                RoleViewModel roleViewModel = listRole.Find(x => x.Id == id);//db.RoleViewModels.Find(id);
                if (roleViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(roleViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdateList();
            RoleViewModel roleViewModel = listRole.Find(x => x.Id == id);
            roleViewModel.DeleteRole(roleViewModel);
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
