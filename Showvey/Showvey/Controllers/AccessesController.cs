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
    public class AccessesController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private IQueryable<Access> x;
        private List<AccessViewModel> listAccess = new List<AccessViewModel>();
        private List<PermissionViewModel> listPermission = new List<PermissionViewModel>();
        private List<RoleViewModel> listRole = new List<RoleViewModel>();
        private void UpdateList()
        {
            x = db.Accesses.Include(a => a.Permission).Include(a => a.Role);
            foreach (var item in x)
            {
                if (item.IsDeleted == false && item.Permission.IsDeleted==false && item.Role.IsDeleted==false)
                {
                    listAccess.Add(new AccessViewModel(item));
                }
            }
            foreach (var item in db.Permissions)
            {
                if (item.IsDeleted == false)
                {
                    listPermission.Add(new PermissionViewModel(item));
                }
            }
            foreach (var item in db.Roles)
            {
                if (item.IsDeleted == false)
                {
                    listRole.Add(new RoleViewModel(item));
                }
            }
        }

        // GET: Accesses
        public ActionResult Index()
        {
            //if(AccountController.CheckSession("Access Index"))
            //{
                UpdateList();
                return View(listAccess);
            //}
            //return RedirectToAction("Index", "Home");
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
                if (AccountController.CheckPermission("Access-Index"))
                {
                    UpdateList();
                    return View(listAccess.OrderBy(x => x.CreatedDate));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
        }
        // GET: Accesses/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("Access-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                AccessViewModel accessViewModel = listAccess.Find(x => x.Id == id);//db.AccessViewModels.Find(id);
                if (accessViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(accessViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: Accesses/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("Access-Create"))
            {
                UpdateList();
                ViewBag.PermissionId = new SelectList(listPermission.OrderBy(x => x.Name), "Id", "Name");
                ViewBag.RoleId = new SelectList(listRole, "Id", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Accesses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoleId,PermissionId,IsGranted,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] AccessViewModel accessViewModel)
        {
            if (ModelState.IsValid)
            {
                accessViewModel.Id = Guid.NewGuid();
                accessViewModel.AddAccess();
                return RedirectToAction("Index");
            }
            UpdateList();
            ViewBag.PermissionId = new SelectList(listPermission, "Id", "Name", accessViewModel.PermissionId);
            ViewBag.RoleId = new SelectList(listRole, "Id", "Name", accessViewModel.RoleId);
            return View(accessViewModel);
        }

        // GET: Accesses/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("Access-Edit"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                AccessViewModel accessViewModel = listAccess.Find(x => x.Id == id);//db.AccessViewModels.Find(id);
                if (accessViewModel == null)
                {
                    return HttpNotFound();
                }
                UpdateList();
                ViewBag.PermissionId = new SelectList(listPermission, "Id", "Name", accessViewModel.PermissionId);
                ViewBag.RoleId = new SelectList(listRole, "Id", "Name", accessViewModel.RoleId);
                return View(accessViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Accesses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoleId,PermissionId,IsGranted,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] AccessViewModel accessViewModel)
        {
            if (ModelState.IsValid)
            {
                accessViewModel.UpdateAccess();
                return RedirectToAction("Index");
            }
            UpdateList();
            ViewBag.PermissionId = new SelectList(listPermission, "Id", "Name", accessViewModel.PermissionId);
            ViewBag.RoleId = new SelectList(listRole, "Id", "Name", accessViewModel.RoleId);
            return View(accessViewModel);
        }

        // GET: Accesses/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("Access-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                AccessViewModel accessViewModel = listAccess.Find(x => x.Id == id);//db.AccessViewModels.Find(id);
                if (accessViewModel == null)
                {
                    return HttpNotFound();
                }
                return PartialView(accessViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Accesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdateList();
            AccessViewModel accessViewModel = listAccess.Find(x => x.Id == id);//db.AccessViewModels.Find(id);
            accessViewModel.DeleteAccess();
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
