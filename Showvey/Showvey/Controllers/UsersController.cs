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
using System.Text;
using System.Configuration;
using System.Security.Cryptography;

namespace Showvey.Controllers
{
    public class UsersController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private IQueryable<User> x;
        private List<UserViewModel> listUser = new List<UserViewModel>();
        private List<RoleViewModel> listRole = new List<RoleViewModel>();
        private List<CityViewModel> listCity = new List<CityViewModel>();
        private void UpdateList()
        {
            x = db.Users.Include(u => u.City).Include(u=>u.Role);
            foreach (var item in x)
            {
                if (item.IsDeleted == false && item.Role.IsDeleted==false && item.City.IsDeleted==false)
                {
                    listUser.Add(new UserViewModel(item));
                }
            }
            foreach (var item in db.Cities)
            {
                if (item.IsDeleted == false)
                {
                    listCity.Add(new CityViewModel(item));
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
        // GET: Users
        public ActionResult Index()
        {
            UpdateList();
            return View(listUser);
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
                if (AccountController.CheckPermission("User-Index"))
                {
                    UpdateList();
                    return View(listUser);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // GET: Users/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("User-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                UserViewModel userViewModel = listUser.Find(x => x.Id == id);//db.UserViewModels.Find(id);
                if (userViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(userViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("User-Create"))
            {
                UpdateList();
                ViewBag.CityId = new SelectList(listCity, "Id", "Name");
                ViewBag.RoleId = new SelectList(listRole, "Id", "Name");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Password,RetypePassword,FirstName,LastName,Username,Gender,CityId,PhoneNumber,Birthdate,RoleId")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                userViewModel.Id = Guid.NewGuid();
                userViewModel.Password = UserViewModel.Encrypt(userViewModel.Password);
                userViewModel.AddUser(userViewModel);
                return RedirectToAction("Index");
            }
            UpdateList();
            ViewBag.CityId = new SelectList(listCity, "Id", "Name", userViewModel.CityId);
            ViewBag.RoleId = new SelectList(listRole, "Id", "Name", userViewModel.RoleId);
            return View(userViewModel);
        }

        // GET: Users/Create
        public ActionResult Register()
        {
            UpdateList();
            ViewBag.CityId = new SelectList(listCity, "Id", "Name");
            ViewBag.RoleId = new SelectList(listRole, "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Id,Email,Password,FirstName,LastName,Username,Gender,CityId,PhoneNumber,Birthdate,LastLogin,IsActive,IsBlocked,RoleId,IsComplete,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId,RetypePassword")] UserViewModel userViewModel)
        {
            try
            {
                UpdateList();
                if (ModelState.IsValid)
                {
                    userViewModel.Id = Guid.NewGuid();
                    userViewModel.Role = listRole[0];
                    userViewModel.RoleId = listRole[0].Id;
                    userViewModel.Password = UserViewModel.Encrypt(userViewModel.Password);

                    AccountController.SendEmail("Welcome to Showvey!", "Hello " + userViewModel.Username + ", thank you for registration in Showvey. For completing the registration, please click link below: http://localhost:13177?id=" + userViewModel.Id,userViewModel.Email);
                    userViewModel.AddUser(userViewModel);
                    return RedirectToAction("Index");

                }
                ViewBag.CityId = new SelectList(listCity, "Id", "Name", userViewModel.CityId);
                ViewBag.RoleId = new SelectList(listRole, "Id", "Name", userViewModel.RoleId);
                return View(userViewModel);
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("User-Edit"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                UserViewModel userViewModel = listUser.Find(x => x.Id == id);//db.UserViewModels.Find(id);
                if (userViewModel == null)
                {
                    return HttpNotFound();
                }
                userViewModel.Password= UserViewModel.Decrypt(userViewModel.Password); ;
                ViewBag.CityId = new SelectList(listCity, "Id", "Name", userViewModel.CityId);
                ViewBag.RoleId = new SelectList(listRole, "Id", "Name", userViewModel.RoleId);
                return View(userViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Password,FirstName,LastName,Username,Gender,CityId,PhoneNumber,Birthdate,LastLogin,IsActive,IsBlocked,RoleId,IsComplete,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] UserViewModel userViewModel)
        {
            //if (ModelState.IsValid)
            //{
            userViewModel.Password = UserViewModel.Encrypt(userViewModel.Password);
            UserViewModel u =new UserViewModel(db.Users.Find(userViewModel.Id));
            u.Password = userViewModel.Password;
            u.FirstName = userViewModel.FirstName;
            u.LastName = userViewModel.LastName;
            u.Username = userViewModel.Username;
            u.Gender = userViewModel.Gender;
            u.CityId = userViewModel.CityId;
            u.PhoneNumber = userViewModel.PhoneNumber;
            u.UpdateUser(u);
            AccountController.RememberUser(u);
                //userViewModel.UpdateUser(userViewModel);
                return RedirectToAction("Index");
            //}
            //UpdateList();
            //ViewBag.CityId = new SelectList(listCity, "Id", "Name", userViewModel.CityId);
            //ViewBag.RoleId = new SelectList(listRole, "Id", "Name", userViewModel.RoleId);
            //return View(userViewModel);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("User-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                UserViewModel userViewModel = listUser.Find(x => x.Id == id);//db.UserViewModels.Find(id);
                if (userViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(userViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdateList();
            UserViewModel userViewModel = listUser.Find(x => x.Id == id);//db.UserViewModels.Find(id);
            userViewModel.DeleteUser(userViewModel);
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
