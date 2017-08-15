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
    public class SurveysController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<SurveyViewModel> listSurvey = new List<SurveyViewModel>();
        private List<SurveyTypeViewModel> listSurveyType = new List<SurveyTypeViewModel>();
        private List<UserViewModel> listUser = new List<UserViewModel>();
        
        private void UpdateList()
        {
           var c = db.Surveys.Where(x=>x.IsDeleted==false);
            foreach (var item in c)
            {
                    listSurvey.Add(new SurveyViewModel(item));
            }
            foreach (var item in db.SurveyTypes)
            {
                if (item.IsDeleted == false)
                {
                    listSurveyType.Add(new SurveyTypeViewModel(item));
                }
            }
            foreach (var item in db.Users)
            {
                if(item.IsDeleted==false)
                {
                            listUser.Add(new UserViewModel(item));
                 
                }
            }
        }
        // GET: Surveys
        public ActionResult Index()
        {
            UpdateList();
            System.Web.HttpContext.Current.Session["action"] = "Survey-Index";
            return View(listSurvey);
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
                if (AccountController.CheckPermission("Survey-Index"))
                {
                    System.Web.HttpContext.Current.Session["action"] = "Survey-Index";
                    UpdateList();
                    var user = AccountController.GetUser();
                    if (user.RoleName == "User")
                    {
                        return View(listSurvey.Where(x=>x.UserId==user.Id && x.IsDeleted==false));
                    }
                    else
                    {
                        return View(listSurvey);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult Banks()
        {
            var survey = db.Surveys.Where(x => x.IsDeleted == false);
            List<SurveyViewModel> surveys = new List<SurveyViewModel>();
            foreach (var item in survey)
            {
                SurveyViewModel s = new SurveyViewModel(item);
                s.GetQuestionViewList();
                s.GetAnimate();
                surveys.Add(s);
            }
                return View(surveys);
        }

        //-------------Create Design---------------

        //// GET: Animates/Create
        //public ActionResult Design()
        //{
        //    if (AccountController.CheckPermission("Access-Index"))
        //    {
        //        UpdateList();
        //        return View(listAccess.OrderBy(x => x.CreatedDate));
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    AnimatesController a = new AnimatesController();
        //    ImagesController i = new ImagesController();
            
        //    ViewBag.QuestionId = new SelectList(a.GetQuestion(AccountController.GetSurveyId()), "Id", "Content");
        //    return View();
        //}

        //// POST: Animates/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Design([Bind(Include = "Id,QuestionId,ImageId,Width,Height,PosX,PosY,Depth,TimeStart,TimeEnd,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] AnimateViewModel animateViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        animateViewModel.Id = Guid.NewGuid();
        //        animateViewModel.AddAnimate();
        //        return RedirectToAction("Index");
        //    }
        //    //UpdateList();
        //    //ViewBag.ImageId = new SelectList(listImage, "Id", "Location", animateViewModel.ImageId);
        //    //ViewBag.QuestionId = new SelectList(listQuestion, "Id", "Content", animateViewModel.QuestionId);
        //    return View(animateViewModel);
        //}

        ////------------------end create design------------------------------

        // GET: Surveys/Details/5
        public ActionResult Details(Guid? id)
        {
            if (AccountController.CheckPermission("Survey-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                SurveyViewModel surveyViewModel = listSurvey.Find(x => x.Id == id);//db.SurveyViewModels.Find(id);
                surveyViewModel.GetQuestionViewList();
                List<int> response=new List<int>();
                foreach (var item in surveyViewModel.Questions)
                {
                    item.GetResponsesViewList();
                    item.GetQuestionAnswerViewList();
                    response.Add(item.Responses.Count);
                    foreach (var i in item.QuestionAnswers)
                    {
                        i.GetTotal();
                    }
                }
                ViewBag.TotalResponse = response;
                ViewBag.ResponseTotal = surveyViewModel.Questions.ElementAt(0).Responses.Count();
                if (surveyViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(surveyViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        public ActionResult Raw(Guid? id)
        {
            if (AccountController.CheckPermission("Survey-Detail"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                List<RespondentViewModel> listRespondent = new List<RespondentViewModel>();
                RespondentViewModel respondent;
                var r = db.Respondents.Where(x => x.SurveyId == id);
                foreach (var item in r)
                {
                    respondent = new RespondentViewModel(item);
                    respondent.GetResponse();
                    listRespondent.Add(respondent);
                }
                ViewBag.Respondent = listRespondent;

                UpdateList();
                SurveyViewModel surveyViewModel = listSurvey.Find(x => x.Id == id);//db.SurveyViewModels.Find(id);
                surveyViewModel.GetQuestionViewList();
                List<int> response = new List<int>();
                foreach (var item in surveyViewModel.Questions)
                {
                    //item.GetResponsesViewList();
                    item.GetQuestionAnswerViewList();
                    response.Add(item.Responses.Count);
                    foreach (var i in item.QuestionAnswers)
                    {
                        i.GetTotal();
                    }
                }
                ViewBag.TotalResponse = response;
                ViewBag.ResponseTotal = surveyViewModel.Questions.ElementAt(0).Responses.Count();
                if (surveyViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(surveyViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CreateNew()
        {
            AccountController.ClearSurvey();
            AccountController.ClearSurveyId();
            return RedirectToAction("Create");
        }

        // GET: Surveys/Create
        public ActionResult Create(string error)
        {
            if (AccountController.CheckPermission("Survey-Create"))
            {
                if (error != null)
                {
                    ModelState.AddModelError("Questions", error);
                }
                UpdateList();
                System.Web.HttpContext.Current.Session["action"] = "Survey-Create";
                if (AccountController.CheckSession("Edit"))
                {
                    ViewBag.edit = true;
                    AccountController.ClearSession("Edit");
                }
                else
                {
                    ViewBag.edit = false;
                }
                ViewBag.SurveyTypeId = new SelectList(listSurveyType, "Id", "Type");
                ViewBag.UserId = new SelectList(listUser, "Id", "Username");

                QuestionsController q = new QuestionsController();
                ViewBag.QuestionTypeId = new SelectList(q.GetQuestionType(), "Id", "Type");


                if (AccountController.CheckSurveyId())
                {
                    return View(AccountController.GetSurvey());
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Surveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SurveyTypeId,Title,UserId,UserName,Description,SurveyType,IsBlock,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] SurveyViewModel surveyViewModel,string CreateSurvey)
        {
            QuestionsController q = new QuestionsController();
            if (ModelState.IsValid)
            {
                if (CreateSurvey == "Create Survey")
                {
                    surveyViewModel.Id = Guid.NewGuid();
                    surveyViewModel.UserId = AccountController.GetUser().Id;
                    surveyViewModel.AddSurvey(surveyViewModel);
                    AccountController.RememberSurveyId(surveyViewModel.Id);
                    UpdateList();
                    AccountController.RememberSurvey(listSurvey.Find(x=>x.Id==surveyViewModel.Id));
                    AccountController.ClearCount();
                    
                    ViewBag.SurveyTypeId = new SelectList(listSurveyType, "Id", "Type");
                    ViewBag.UserId = new SelectList(listUser, "Id", "Username");

                    ViewBag.QuestionTypeId = new SelectList(q.GetQuestionType(), "Id", "Type");
                    return View(surveyViewModel);
                }
                else if (CreateSurvey == "Edit Survey")
                {
                    SurveyViewModel s = AccountController.GetSurvey();
                    s.Title = surveyViewModel.Title;
                    s.Description = surveyViewModel.Description;
                    s.SurveyTypeId = surveyViewModel.SurveyTypeId;
                    s.ModifiedUserId = AccountController.GetUser().Id;
                    TemporaryEdit(s);
                }

            }
            UpdateList();
            ViewBag.QuestionTypeId = new SelectList(q.GetQuestionType(), "Id", "Type");
            ViewBag.SurveyTypeId = new SelectList(listSurveyType, "Id", "Type");
            ViewBag.UserId = new SelectList(listUser, "Id", "Username");
            return View(surveyViewModel);
        }

        // GET: Surveys/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (AccountController.CheckPermission("Survey-Edit"))
            {
                AccountController.RememberSurveyId((Guid)id);
                AccountController.RememberSession("Edit", "true");
                UpdateList();
                AccountController.RememberSurvey(listSurvey.Find(x => x.Id == id));
                return RedirectToAction("Create");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Surveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TemporaryEdit(SurveyViewModel surveyViewModel)
        {
            
                if (ModelState.IsValid)
                { 
                    surveyViewModel.UpdateSurvey(surveyViewModel);
                AccountController.RememberSurvey(surveyViewModel);
                    ViewBag.SurveyTypeId = new SelectList(listSurveyType, "Id", "Type", surveyViewModel.SurveyTypeId);
                    ViewBag.UserId = new SelectList(listUser, "Id", "Username", surveyViewModel.UserId);
                    return RedirectToAction("Create", "Surveys", new {surveyViewModel =surveyViewModel});
                }
                UpdateList();
                ViewBag.SurveyTypeId = new SelectList(listSurveyType, "Id", "Type", surveyViewModel.SurveyTypeId);
                ViewBag.UserId = new SelectList(listUser, "Id", "Username", surveyViewModel.UserId);
                return View(surveyViewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Title,UserId,SurveyTypeId,UserName,Description,SurveyType,IsBlock,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] SurveyViewModel surveyViewModel, string CreateSurvey)
        //{
        //    if (CreateSurvey == "Edit Survey")
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            surveyViewModel.UpdateSurvey(surveyViewModel);
        //            ViewBag.SurveyTypeId = new SelectList(listSurveyType, "Id", "Type", surveyViewModel.SurveyTypeId);
        //            ViewBag.UserId = new SelectList(listUser, "Id", "Username", surveyViewModel.UserId);
        //            return RedirectToAction("Create", "Surveys", new { surveyViewModel = surveyViewModel });
        //        }
        //        UpdateList();
        //        ViewBag.SurveyTypeId = new SelectList(listSurveyType, "Id", "Type", surveyViewModel.SurveyTypeId);
        //        ViewBag.UserId = new SelectList(listUser, "Id", "Username", surveyViewModel.UserId);
        //        return View(surveyViewModel);
        //    }
        //    UpdateList();
        //    ViewBag.SurveyTypeId = new SelectList(listSurveyType, "Id", "Type", surveyViewModel.SurveyTypeId);
        //    ViewBag.UserId = new SelectList(listUser, "Id", "Username", surveyViewModel.UserId);
        //    return View(surveyViewModel);
        //}

        // GET: Surveys/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (AccountController.CheckPermission("Survey-Delete"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                UpdateList();
                SurveyViewModel surveyViewModel = listSurvey.Find(x => x.Id == id);//db.SurveyViewModels.Find(id);
                if (surveyViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(surveyViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Surveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UpdateList();
            SurveyViewModel surveyViewModel = listSurvey.Find(x => x.Id == id);//db.SurveyViewModels.Find(id);
            surveyViewModel.DeleteSurvey(surveyViewModel);
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
