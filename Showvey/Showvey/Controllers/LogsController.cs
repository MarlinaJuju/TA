using Showvey.Models;
using Showvey.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Showvey.Controllers
{
    public class LogsController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<LogViewModel> listLog = new List<LogViewModel>();
        private void UpdateList()
        {
            foreach (var item in db.Logs)
            {
                listLog.Add(new LogViewModel(item));
            }
        }
        // GET: PermissionViewModels
        public ActionResult Index()
        {
            UpdateList();
            return View(listLog);
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
                if (AccountController.CheckPermission("Log-Index"))
                {
                    UpdateList();
                    return View(listLog.OrderByDescending(x=>x.CreatedDate));
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // GET: PermissionViewModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("Log-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                LogViewModel logViewModel = listLog.Find(x => x.Id == id);
                if (logViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(logViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogBox()
        {
            UpdateList();
            //listLog.Reverse();
            foreach (var item in listLog)
            {
                if (item.Message.Length > 30)
                {
                    item.Message = item.Message.Substring(0, 30) + "...";
                }
            }
            ViewBag.Count = listLog.Count;
            if (listLog.Count > 3)
            {
                return PartialView(listLog.OrderByDescending(x => x.CreatedDate).Take(3));
            }
            else
            {
                return PartialView(listLog.OrderBy(x => x.CreatedDate));
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