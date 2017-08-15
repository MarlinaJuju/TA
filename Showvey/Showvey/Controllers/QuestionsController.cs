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
    public class QuestionsController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private IQueryable<Question> x;
        private List<QuestionViewModel> listQuestion = new List<QuestionViewModel>();
        private List<QuestionTypeViewModel> listQuestionType = new List<QuestionTypeViewModel>();
        private List<SurveyViewModel> listSurvey = new List<SurveyViewModel>();
        private void UpdateList()
        {
            x = db.Questions.Include(q => q.QuestionType).Include(q => q.Survey);
            foreach (var item in x)
            {
                if (item.IsDeleted == false)
                {
                    listQuestion.Add(new QuestionViewModel(item));
                }
            }
            foreach (var item in db.QuestionTypes)
            {
                if (item.IsDeleted == false)
                {
                    listQuestionType.Add(new QuestionTypeViewModel(item));
                }
            }
            foreach (var item in db.Surveys)
            {
                if (item.IsDeleted == false)
                {
                    listSurvey.Add(new SurveyViewModel(item));
                }
            }
        }
        public List<QuestionTypeViewModel> GetQuestionType()
        {
            UpdateList();
            return listQuestionType;
        }
        //// GET: Questions
        //public ActionResult Index()
        //{
        //    UpdateList();
        //    return View(listQuestion);
        //}
        //[HttpGet]
        //public ActionResult Index(string searchString)
        //{
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        SearchController s = new SearchController();
        //        return s.SearchOnDatabase(searchString);
        //    }
        //    else
        //    {
        //        UpdateList();
        //        return View(listQuestion);
        //    }
        //}
        //// GET: Questions/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UpdateList();
        //    QuestionViewModel questionViewModel = listQuestion.Find(x => x.Id == id);//db.QuestionViewModels.Find(id);
        //    if (questionViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(questionViewModel);
        //}

        //// GET: Questions/Create
        //public ActionResult Create()
        //{
        //    UpdateList();
        //    ViewBag.QuestionTypeId = new SelectList(listQuestionType, "Id", "Type");
        //    ViewBag.SurveyId = new SelectList(listSurvey, "Id", "Title");
        //    return View();
        //}

        //// POST: Questions/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Content,Number,QuestionTypeId,TimeLength,SurveyId,IsFreeText,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] QuestionViewModel questionViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        questionViewModel.Id = Guid.NewGuid();
        //        questionViewModel.AddQuestion(questionViewModel);
        //        return RedirectToAction("Index");
        //    }
        //    UpdateList();
        //    ViewBag.QuestionTypeId = new SelectList(listQuestionType, "Id", "Type", questionViewModel.QuestionTypeId);
        //    ViewBag.SurveyId = new SelectList(listSurvey, "Id", "Title", questionViewModel.SurveyId);
        //    return View(questionViewModel);
        //}

        //------------Create Question in User----------------------------
        
        public ActionResult TemporaryQuestion()
        {
            //UpdateList();
            var id = AccountController.GetSurveyId();
            var questions = db.Questions.Where(x => x.IsDeleted == false && x.SurveyId == id);
            List<QuestionViewModel> q = new List<QuestionViewModel>();
            QuestionViewModel question;
            foreach (var item in questions)
            {
                question = new QuestionViewModel(item);
                question.GetQuestionAnswerViewList();
                q.Add(question);
               
            }
            ViewBag.QuestionTypeId = new SelectList(listQuestionType, "Id", "Type");
            //            q.OrderBy(x => x.Number);
            return View(q.OrderBy(x => x.Number));
        }

        // GET: Questions/Create
        //public ActionResult CreateQuestion()
        //{
        //    UpdateList();
        //    ViewBag.QuestionTypeId = new SelectList(listQuestionType, "Id", "Type");
        //    ViewBag.SurveyId = new SelectList(listSurvey, "Id", "Title");
        //    return View();
        //}

        public ActionResult CreateQuestion(Guid QuestionTypeId, string Content, int? Count, string[] dynamicField)
        {
            QuestionViewModel questionViewModel = new QuestionViewModel();
            if (QuestionTypeId != null && Content != null && Content != "")
            {
                questionViewModel.Id = Guid.NewGuid();
                questionViewModel.QuestionTypeId = QuestionTypeId;
                questionViewModel.Content = Content;
                questionViewModel.Number = AccountController.Count();
                questionViewModel.SurveyId = AccountController.GetSurveyId();
                questionViewModel.Survey = AccountController.GetSurvey();
                questionViewModel.TimeLength = TimeSpan.FromSeconds(3);

                if (Count != null)
                {
                    if (Count < 2)
                    {
                        return RedirectToAction("Create", "Surveys", new { error = "Scale should more than two" });
                    }
                    questionViewModel.IsScale = true;
                    questionViewModel.Count = (int)Count;
                    questionViewModel.AddQuestion(questionViewModel);
                }
                else if (dynamicField.Count() > 0 && dynamicField[0]!="")
                {
                    questionViewModel.AddQuestion(questionViewModel);

                    List<QuestionAnswerViewModel> answer = new List<QuestionAnswerViewModel>();
                    QuestionAnswerViewModel q = new QuestionAnswerViewModel();
                    int i = 1;
                    foreach (var item in dynamicField)
                    {
                        if(item!="" && item != null)
                        {
                            q = new QuestionAnswerViewModel();
                            q.Id = Guid.NewGuid();
                            q.QuestionId = questionViewModel.Id;
                            q.Answer = item;
                            q.OrderNumber = i;
                            answer.Add(q);
                            i++;
                        }
                        else
                        {
                            return RedirectToAction("Create", "Surveys", new { error = "Answer can't be empty" });
                        }
                    }
                    foreach (var item in answer)
                    {
                        item.AddQuestionAnswer(item);
                    }
                }
                else
                {
                    questionViewModel.IsFreeText = true;
                    questionViewModel.AddQuestion(questionViewModel);
                }
                TemporaryQuestion();
                UpdateList();
                ViewBag.QuestionTypeId = new SelectList(listQuestionType, "Id", "Type", questionViewModel.QuestionTypeId);
                ViewBag.SurveyId = new SelectList(listSurvey, "Id", "Title", questionViewModel.SurveyId);
                return RedirectToAction("Create", "Surveys");
            }
            UpdateList();
            ViewBag.QuestionTypeId = new SelectList(listQuestionType, "Id", "Type", questionViewModel.QuestionTypeId);
            ViewBag.SurveyId = new SelectList(listSurvey, "Id", "Title", questionViewModel.SurveyId);
            return RedirectToAction("Create", "Surveys", new { error = "Question can't be empty"});
        }

        //edit question

        public ActionResult EditQuestion(string Id, string Content, int? Count, string[] editdynamicField)
        {
            QuestionViewModel questionViewModel = new QuestionViewModel(db.Questions.Find(new Guid(Id)));
            if (questionViewModel != null)
            {
                questionViewModel.Content = Content;
                if (questionViewModel.Type == "Checkbox")
                {
                    QuestionAnswerViewModel answer;
                    var answers = db.QuestionAnswers.Where(x => x.QuestionId == questionViewModel.Id);
                    int i = 0;
                    foreach (var item in answers)
                    {
                        answer = new QuestionAnswerViewModel(item);
                        if (editdynamicField.Count() > i)
                        {
                            answer.Answer = editdynamicField[i];
                            answer.UpdateQuestionAnswer(answer);
                        }
                        else
                        {
                            answer.DeleteQuestionAnswer(answer);
                        }
                        i++;
                    }
                    if (answers.Count() < editdynamicField.Count())
                    {
                        for (int j = i; j < editdynamicField.Count(); j++)
                        {
                            answer = new QuestionAnswerViewModel
                            {
                                Id = Guid.NewGuid(),
                                QuestionId = questionViewModel.Id,
                                Answer = editdynamicField[j]
                            };
                            answer.AddQuestionAnswer(answer);
                        }
                    }
                }
                questionViewModel.UpdateQuestion(questionViewModel);
            }

            return RedirectToAction("Create","Surveys");
        }

        //end edit question

        public ActionResult DeleteQuestion(Guid Id)
        {
            QuestionViewModel questionViewModel = new QuestionViewModel(db.Questions.Find(Id));
            if (questionViewModel != null)
            {
                questionViewModel.DeleteQuestion(questionViewModel);
                if (questionViewModel.Type == "Checkbox")
                {
                    QuestionAnswerViewModel answer;
                    var answers = db.QuestionAnswers.Where(x => x.QuestionId == questionViewModel.Id);
                    int i = 0;
                    foreach (var item in answers)
                    {
                        answer = new QuestionAnswerViewModel(item);
                        answer.DeleteQuestionAnswer(answer);
                    }
                }
            }
            return RedirectToAction("Create", "Surveys");
        }


        //// POST: Questions/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult CreateQuestion([Bind(Include = "Id,Content,Number,QuestionTypeId,TimeLength,SurveyId,IsFreeText,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] QuestionViewModel questionViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        questionViewModel.Id = Guid.NewGuid();
        //        questionViewModel.Number = AccountController.Count();
        //        questionViewModel.SurveyId = AccountController.GetSurveyId();
        //        questionViewModel.Survey = AccountController.GetSurvey();
        //        questionViewModel.TimeLength = TimeSpan.FromSeconds(3);
        //        questionViewModel.AddQuestion(questionViewModel);
        //        TemporaryQuestion();
        //        UpdateList();
        //        ViewBag.QuestionTypeId = new SelectList(listQuestionType, "Id", "Type", questionViewModel.QuestionTypeId);
        //        ViewBag.SurveyId = new SelectList(listSurvey, "Id", "Title", questionViewModel.SurveyId);
        //        return PartialView("Create", "Surveys");
        //    }
        //    UpdateList();
        //    ViewBag.QuestionTypeId = new SelectList(listQuestionType, "Id", "Type", questionViewModel.QuestionTypeId);
        //    ViewBag.SurveyId = new SelectList(listSurvey, "Id", "Title", questionViewModel.SurveyId);
        //    return View(questionViewModel);
        //}

        //----------------end------------------------------


        // GET: Questions/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UpdateList();
        //    QuestionViewModel questionViewModel = listQuestion.Find(x => x.Id == id);//db.QuestionViewModels.Find(id);
        //    if (questionViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.QuestionTypeId = new SelectList(listQuestionType, "Id", "Type", questionViewModel.QuestionTypeId);
        //    ViewBag.SurveyId = new SelectList(listSurvey, "Id", "Title", questionViewModel.SurveyId);
        //    return View(questionViewModel);
        //}

        //// POST: Questions/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Content,Number,QuestionTypeId,TimeLength,SurveyId,IsFreeText,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] QuestionViewModel questionViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        questionViewModel.UpdateQuestion(questionViewModel);
        //        return RedirectToAction("Index");
        //    }
        //    UpdateList();
        //    ViewBag.QuestionTypeId = new SelectList(listQuestionType, "Id", "Type", questionViewModel.QuestionTypeId);
        //    ViewBag.SurveyId = new SelectList(listSurvey, "Id", "Title", questionViewModel.SurveyId);
        //    return View(questionViewModel);
        //}

        //// GET: Questions/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UpdateList();
        //    QuestionViewModel questionViewModel = listQuestion.Find(x => x.Id == id);//db.QuestionViewModels.Find(id);
        //    if (questionViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(questionViewModel);
        //}

        //// POST: Questions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    UpdateList();
        //    QuestionViewModel questionViewModel = listQuestion.Find(x => x.Id == id);//db.QuestionViewModels.Find(id);
        //    questionViewModel.DeleteQuestion(questionViewModel);
        //    return RedirectToAction("Index");
        //}

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
