using Showvey.Models;
using Showvey.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;

namespace Showvey.Controllers
{
    public class HomeController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<UserViewModel> listUser = new List<UserViewModel>();
        private List<RoleViewModel> listRole = new List<RoleViewModel>();
        private List<CityViewModel> listCity = new List<CityViewModel>();
        private IQueryable<User> x;
        private void UpdateList()
        {
            x = db.Users.Include(u => u.City).Include(u => u.Role);
            foreach (var item in x)
            {
                if (item.IsDeleted == false && item.Role.IsDeleted == false && item.City.IsDeleted == false)
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
        public ActionResult Index()
        {
            AccountController.ClearSurvey();
            AccountController.ClearSurveyId();
            var id = Request.QueryString["id"];
            if (id != null)
            {
                UpdateList();
                UserViewModel u = listUser.Find(l => l.Id == new Guid( id));
                if (u != null)
                {
                    u.IsComplete = true;
                    u.UpdateUser(u);
                }
            }
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}