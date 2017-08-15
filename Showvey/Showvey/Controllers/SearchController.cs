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
    public class SearchController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<SearchViewModel> listSearch = new List<SearchViewModel>();
        // GET: Search
        public ActionResult Index()
        {
            var list =(List < SearchViewModel >)System.Web.HttpContext.Current.Session["lists"];
            return View(list);
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
                var list = (List<SearchViewModel>)System.Web.HttpContext.Current.Session["lists"];
                return View(list);
            }
        }
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult SearchOnDatabase(string searchString)
        {
            UserViewModel user = AccountController.GetUser();
            if (user.RoleName == "Admin")
            {
                var temp = db.Cities.Where(t => t.Name.Contains(searchString) && t.IsDeleted == false);
                if (temp != null)
                {
                    foreach (var item in temp)
                    {
                        listSearch.Add(new SearchViewModel
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Table = "City",
                            Model = "Cities"
                        });
                    }
                }
                var temp2 = db.ImageTypes.Where(t => t.Type.Contains(searchString) && t.IsDeleted == false);
                if (temp2 != null)
                {
                    foreach (var item in temp2)
                    {
                        listSearch.Add(new SearchViewModel
                        {
                            Id = item.Id,
                            Name = item.Type,
                            Table = "Image Type",
                            Model = "ImageTypes"
                        });
                    }
                }
                var temp3 = db.Permissions.Where(t => t.Name.Contains(searchString) && t.IsDeleted == false);
                if (temp3 != null)
                {
                    foreach (var item in temp3)
                    {
                        listSearch.Add(new SearchViewModel
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Table = "Permission",
                            Model = "Permissions"
                        });
                    }
                }
                var temp4 = db.QuestionTypes.Where(t => t.Type.Contains(searchString) && t.IsDeleted == false);
                if (temp4 != null)
                {
                    foreach (var item in temp4)
                    {
                        listSearch.Add(new SearchViewModel
                        {
                            Id = item.Id,
                            Name = item.Type,
                            Table = "Question Type",
                            Model = "QuestionTypes"
                        });
                    }
                }
                var temp5 = db.Roles.Where(t => t.Name.Contains(searchString) && t.IsDeleted == false);
                if (temp5 != null)
                {
                    foreach (var item in temp5)
                    {
                        listSearch.Add(new SearchViewModel
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Table = "Role",
                            Model = "Roles"
                        });
                    }
                }
                var temp6 = db.SurveyTypes.Where(t => t.Type.Contains(searchString) && t.IsDeleted == false);
                if (temp6 != null)
                {
                    foreach (var item in temp6)
                    {
                        listSearch.Add(new SearchViewModel
                        {
                            Id = item.Id,
                            Name = item.Type,
                            Table = "Survey Type",
                            Model = "SurveyTypes"
                        });
                    }
                }
                var temp7 = db.Surveys.Where(t => t.Title.Contains(searchString) && t.IsDeleted == false);
                if (temp7 != null)
                {
                    foreach (var item in temp7)
                    {
                        listSearch.Add(new SearchViewModel
                        {
                            Id = item.Id,
                            Name = item.Title,
                            Table = "Survey",
                            Model = "Surveys"
                        });
                    }
                }
                var temp8 = db.Users.Where(t => t.Username.Contains(searchString) && t.IsDeleted == false);
                if (temp8 != null)
                {
                    foreach (var item in temp8)
                    {
                        listSearch.Add(new SearchViewModel
                        {
                            Id = item.Id,
                            Name = item.Username,
                            Table = "User",
                            Model = "Users"
                        });
                    }
                }
                var temp9 = db.Countries.Where(t => t.Name.Contains(searchString) && t.IsDeleted == false);

                if (temp9 != null)
                {
                    foreach (var item in temp9)
                    {
                        listSearch.Add(new SearchViewModel
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Table = "Country",
                            Model = "Countries"
                        });
                    }
                }
            }
            else if (user.RoleName == "User")
            {
                var s = db.Surveys.Where(t => (t.Title.Contains(searchString) || t.Description.Contains(searchString)) && t.IsDeleted == false);
                if (s != null)
                {
                    foreach (var item in s)
                    {
                        listSearch.Add(new SearchViewModel
                        {
                            Id = item.Id,
                            Name = item.Title,
                            Table = "Survey",
                            Model = "Surveys"
                        });
                    }
                }
            }
            if (listSearch != null && listSearch.Count>0)
            {
               System.Web.HttpContext.Current.Session["lists"] = listSearch;
                return RedirectToAction("Index","Search");
            }
            else
            {
                return RedirectToAction("NotFound", "Search");
            }
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
