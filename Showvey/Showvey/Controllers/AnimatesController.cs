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
    public class AnimatesController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private IQueryable<Animate> x;
        private List<AnimateViewModel> listAnimate = new List<AnimateViewModel>();
        private List<ImageViewModel> listImage = new List<ImageViewModel>();
        private List<QuestionViewModel> listQuestion = new List<QuestionViewModel>();
        private void UpdateList()
        {
            x = db.Animates.Include(a => a.Image).Include(a => a.Question);
            foreach (var item in x)
            {
                if (item.IsDeleted == false && item.Image.IsDeleted==false && item.Question.IsDeleted==false)
                {
                    listAnimate.Add(new AnimateViewModel(item));
                }
            }
            foreach (var item in db.Questions)
            {
                if (item.IsDeleted == false && item.QuestionTypeId!=null && item.SurveyId!=null)
                {
                    listQuestion.Add(new QuestionViewModel(item));
                }
            }
            foreach (var item in db.Images)
            {
                if (item.IsDeleted == false)
                {
                    listImage.Add(new ImageViewModel(item));
                }
            }
        }

        public List<QuestionViewModel> GetQuestion()
        {
            UpdateList();
            return listQuestion;
        }
        public List<QuestionViewModel> GetQuestion(Guid surveyId)
        {
            UpdateList();
            List<QuestionViewModel> q = new List<QuestionViewModel>();
            foreach (var item in listQuestion)
            {
                if (item.SurveyId == surveyId)
                {
                    q.Add(item);
                }
            }
            return q;
        }
        public List<ImageViewModel> GetImage()
        {
            UpdateList();
            return listImage;
        }

        //// GET: Animates
        //public ActionResult Index()
        //{
        //    UpdateList();
        //    return View(listAnimate);
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
        //        return View(listAnimate);
        //    }
        //}
        //// GET: Animates/Details/5
        //public ActionResult Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UpdateList();
        //    AnimateViewModel animateViewModel = listAnimate.Find(x => x.Id == id);//db.AnimateViewModels.Find(id);
        //    if (animateViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(animateViewModel);
        //}

        //// GET: Animates/Create
        //public ActionResult Create()
        //{
        //    UpdateList();
        //    ViewBag.ImageId = new SelectList(listImage, "Id", "Location");
        //    ViewBag.QuestionId = new SelectList(listQuestion, "Id", "Content");
        //    return View();
        //}

        //// POST: Animates/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,QuestionId,ImageId,Width,Height,PosX,PosY,Depth,TimeStart,TimeEnd,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] AnimateViewModel animateViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        animateViewModel.Id = Guid.NewGuid();
        //        animateViewModel.AddAnimate();
        //        return RedirectToAction("Index");
        //    }
        //    UpdateList();
        //    ViewBag.ImageId = new SelectList(listImage, "Id", "Location", animateViewModel.ImageId);
        //    ViewBag.QuestionId = new SelectList(listQuestion, "Id", "Content", animateViewModel.QuestionId);
        //    return View(animateViewModel);
        //}

        //// GET: Animates/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UpdateList();
        //    AnimateViewModel animateViewModel = listAnimate.Find(x => x.Id == id);//db.AnimateViewModels.Find(id);
        //    if (animateViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ImageId = new SelectList(listImage, "Id", "Location", animateViewModel.ImageId);
        //    ViewBag.QuestionId = new SelectList(listQuestion, "Id", "Content", animateViewModel.QuestionId);
        //    return View(animateViewModel);
        //}

        //// POST: Animates/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,QuestionId,ImageId,Width,Height,PosX,PosY,Depth,TimeStart,TimeEnd,IsDeleted,ModifiedDate,ModifiedUserId,DeletionDate,DeletionUserId,CreatedDate,CreatedUserId")] AnimateViewModel animateViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        animateViewModel.UpdateAnimate();
        //        return RedirectToAction("Index");
        //    }
        //    UpdateList();
        //    ViewBag.ImageId = new SelectList(listImage, "Id", "Location", animateViewModel.ImageId);
        //    ViewBag.QuestionId = new SelectList(listQuestion, "Id", "Content", animateViewModel.QuestionId);
        //    return View(animateViewModel);
        //}

        //// GET: Animates/Delete/5
        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UpdateList();
        //    AnimateViewModel animateViewModel = listAnimate.Find(x => x.Id == id);//db.AnimateViewModels.Find(id);
        //    if (animateViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(animateViewModel);
        //}

        //// POST: Animates/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Guid id)
        //{
        //    UpdateList();
        //    AnimateViewModel animateViewModel = listAnimate.Find(x => x.Id == id);//db.AnimateViewModels.Find(id);
        //    animateViewModel.DeleteAnimate();
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
