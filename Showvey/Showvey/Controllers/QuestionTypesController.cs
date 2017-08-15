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
    public class QuestionTypesController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<QuestionTypeViewModel> listQuestionType = new List<QuestionTypeViewModel>();

        private void UpdateList()
        {
            foreach (var item in db.QuestionTypes)
            {
                if (item.IsDeleted == false)
                {
                    listQuestionType.Add(new QuestionTypeViewModel(item));
                }
            }
        }
        // GET: QuestionTypes
        public ActionResult Index()
        {
            UpdateList();
            return View(listQuestionType); //View(db.QuestionTypeViewModels.ToList());
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
                if (AccountController.CheckPermission("QuestionType-Index"))
                {
                    UpdateList();
                    return View(listQuestionType);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        // GET: QuestionTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("QuestionType-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                QuestionTypeViewModel questionTypeViewModel = listQuestionType.Find(x => x.Id == id);//db.QuestionTypeViewModels.Find(id);
                if (questionTypeViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(questionTypeViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: QuestionTypes/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("QuestionType-Create"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: QuestionTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] QuestionTypeViewModel questionTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                questionTypeViewModel.Id = Guid.NewGuid();
                questionTypeViewModel.AddQuestionType(questionTypeViewModel);
                return RedirectToAction("Index");
            }

            return View(questionTypeViewModel);
        }

        // GET: QuestionTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("QuestionType-Edit"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                QuestionTypeViewModel questionTypeViewModel = listQuestionType.Find(x => x.Id == id);//db.QuestionTypeViewModels.Find(id);
                if (questionTypeViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(questionTypeViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: QuestionTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] QuestionTypeViewModel questionTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                questionTypeViewModel.UpdatQuestionType(questionTypeViewModel);
                return RedirectToAction("Index");
            }
            return View(questionTypeViewModel);
        }

        // GET: QuestionTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("QuestionType-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                QuestionTypeViewModel questionTypeViewModel = listQuestionType.Find(x => x.Id == id);//db.QuestionTypeViewModels.Find(id);
                if (questionTypeViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(questionTypeViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: QuestionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdateList();
            QuestionTypeViewModel questionTypeViewModel = listQuestionType.Find(x => x.Id == id);//db.QuestionTypeViewModels.Find(id);
            questionTypeViewModel.DeleteQuestionType(questionTypeViewModel);
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
