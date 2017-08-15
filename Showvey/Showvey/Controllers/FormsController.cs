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
    public class FormsController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private List<SurveyViewModel> listSurvey = new List<SurveyViewModel>();
        private List<QuestionViewModel> listQuestion = new List<QuestionViewModel>();
        private static bool IsPreview=true;
        private void UpdateList()
        {
            foreach (var item in db.Surveys)
            {
                if (item.IsDeleted == false)
                {
                    listSurvey.Add(new SurveyViewModel(item));
                }
            }
            foreach (var item in db.Questions)
            {
                if (item.IsDeleted == false)
                {
                    listQuestion.Add(new QuestionViewModel(item));
                }
            }
        }
        // GET: Forms
        public ActionResult Index()
        {
            var id = Request.QueryString["id"];
            if (id != null)
            {
                UpdateList();
                SurveyViewModel s = listSurvey.Find(l => l.Id == new Guid(id));
                if (s != null)
                {
                    IsPreview = false;
                    ViewBag.Survey = id;
                    AccountController.RememberSurveyId(new Guid(id));
                }
            }
            else
            {
                Guid s = AccountController.GetSurveyId();
                if (s != null)
                {
                    ViewBag.Survey = s;
                }
            }
            return View();
        }

        public ActionResult Sample(string sample)
        {
                UpdateList();
                SurveyViewModel s = listSurvey.Find(l => l.Title== "Sample");
                if (s != null)
                {
                    IsPreview = false;
                ViewBag.Survey = s.Id;
                AccountController.RememberSurvey(s);
                AccountController.RememberSurveyId(s.Id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Finish()
        {
            return View();
        }

        //public ActionResult Index(Guid id)
        //{
        //    if (id != null)
        //    {
        //        UpdateList();
        //        SurveyViewModel s = listSurvey.Find(l => l.Id == id);
        //        if (s != null)
        //        {
        //            IsPreview = false;
        //            ViewBag.Survey = id;
        //        }
        //    }
        //    return View();
        //}

        //-------------------------ajax called------------------------

        public JsonResult Initialization(string id)
        {
            List<QuestionViewModel> q = new List<QuestionViewModel>();
            AccountController.ClearRespondent();
            AccountController.ClearQuestionAnswer();
            var questions = db.Questions.Where(x => x.SurveyId ==new Guid(id)).ToList();
            if (questions != null)
            {
                foreach (var item in questions)
                {
                    q.Add(new QuestionViewModel(item));
                }
                foreach (var item in q)
                {
                    item.GetAnimateViewList();
                    item.GetQuestionAnswerViewList();
                }
            }
            return Json(q.OrderBy(x=>x.Number), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveAnswers(string answer,string answers,string questionid, string ipaddress, string browser)
        {
            List<QuestionAnswerViewModel> q = AccountController.GetQuestionAnswer();
            RespondentViewModel r = AccountController.GetRespondent();
            if (r == null)
            {
                r = new RespondentViewModel();
                r.Id = Guid.NewGuid();
                if (AccountController.CheckUser())
                {
                    r.UserId = AccountController.GetUser().Id;
                    r.IsRegistered = true;
                }
                r.IPAdress = ipaddress;
                r.BrowserName = browser;
                r.SurveyId = AccountController.GetSurveyId();
            }
            //q.Add(new QuestionViewModel{Id = new Guid(questionid)});

            //int i = r.Count - 1;
            if (r.Responses == null)
            {
                r.Responses = new List<ResponseViewModel>();
            }
            if (q == null)
            {
                q = new List<QuestionAnswerViewModel>();
            }
            if (answer != null)
            {
                QuestionAnswerViewModel qa = new QuestionAnswerViewModel
                {
                    Id = Guid.NewGuid(),
                    QuestionId = new Guid(questionid),
                    Answer = answer
                };
                q.Add(qa);
                AccountController.RememberQuestionAnswer(q);
                
                ResponseViewModel rs = new ResponseViewModel
                {
                    Id = Guid.NewGuid(),
                    QuestionId = new Guid(questionid),
                    ResponseAnswer = qa.Id,
                    RespondentId=r.Id
                };
                r.Responses.Add(rs);
            }
            else if (answers != "")
            {
                string[] a = answers.Split(',');
                //UpdateList();
                //var type = listQuestion.Find(x=>x.Id==new Guid(questionid)).Type;
                //if (type == "Yes/No")
                //{
                //    answers = new List<string>();
                //}
                foreach (var item in a)
                {
                    if (item != "")
                    {
                        ResponseViewModel rs = new ResponseViewModel
                        {
                            Id = Guid.NewGuid(),
                            QuestionId = new Guid(questionid),
                            ResponseAnswer = new Guid(item),
                            RespondentId = r.Id
                        };
                        r.Responses.Add(rs);
                    }
                }
            }
            AccountController.RememberRespondent(r);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveData()
        {
            if (IsPreview == false)
            {
                List<QuestionAnswerViewModel> q = AccountController.GetQuestionAnswer();
                RespondentViewModel r = AccountController.GetRespondent();
                if (q != null)
                {
                    foreach (var item in q)
                    {
                        item.AddQuestionAnswer(item);
                    }
                }
                if (r != null)
                {
                    r.AddRespondent(r);
                    foreach (var item in r.Responses)
                    {
                        item.AddResponse(item);
                    }
                }
            }
            AccountController.ClearQuestionAnswer();
            AccountController.ClearQuestion();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //-------------------------end ajax called------------------------

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
