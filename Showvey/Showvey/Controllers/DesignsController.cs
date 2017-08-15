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
    public class DesignsController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        List<QuestionViewModel> listQuestion = new List<QuestionViewModel>();
        List<ImageTypeViewModel> listImageType = new List<ImageTypeViewModel>();
        List<ImageViewModel> listImage = new List<ImageViewModel>();
        List<AnimateViewModel> listAnimate = new List<AnimateViewModel>();
        

        ImageViewModel i = new ImageViewModel();

        private void UpdateList()
        {
            var temp = db.Questions.Where(x => x.IsDeleted == false);
            foreach (var item in temp)
            {
                    listQuestion.Add(new QuestionViewModel(item));
            }
            foreach (var item in db.ImageTypes)
            {
                if (item.IsDeleted == false)
                {
                    listImageType.Add(new ImageTypeViewModel(item));
                }
            }
            foreach (var item in db.Images)
            {
                if (item.IsDeleted == false)
                {
                    listImage.Add(new ImageViewModel(item));
                }
            }
            var temp2 = db.Animates.Where(x => x.IsDeleted == false);
            foreach (var item in temp2)
            {
                    listAnimate.Add(new AnimateViewModel(item));
            }
        }

        //private void GetImage(string Type)
        //{
        //    UpdateList();
        //    foreach (var item in db.Images)
        //    {
        //        if (item.IsDeleted == false && item.ImageTypeId==(listImageType.Where(i=>i.Type.Equals(Type))).FirstOrDefault().Id)
        //        {
        //            listImage.Add(new ImageViewModel(item));
        //        }
        //    }
        //}

        [Route("Surveys/Design")]
        // GET: Designs/Create
        public ActionResult Create()
        {
            if (AccountController.CheckPermission("Design-Create"))
            {
                System.Web.HttpContext.Current.Session["action"] = "Survey-Create";
                UpdateList();
                AnimatesController a = new AnimatesController();

                ViewBag.Question = a.GetQuestion(AccountController.GetSurveyId()).OrderBy(x => x.Number);
                ViewBag.QuestionId = new SelectList(a.GetQuestion(AccountController.GetSurveyId()), "Id", "Content");
                ViewBag.ImageTypeId = listImageType.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        


        //-------------------------ajax called------------------------
        

        [HttpGet]
        public JsonResult Image(string type)
        {
            var temp = db.Images.Where(x => x.IsDeleted == false && x.ImageTypeId == new Guid(type));
            foreach (var item in temp)
            {
                    listImage.Add(new ImageViewModel(item));
            }

            return Json(listImage, JsonRequestBehavior.AllowGet); 
        }

        [HttpGet]
        public JsonResult AddCharacter(string questionId, double x, double y, double width, double height)
        {
            UpdateList();
            AnimateViewModel a = new AnimateViewModel();
            a = GetAnimatesType((listImageType.Where(z=>z.Type=="Character")).FirstOrDefault().Id, new Guid(questionId));
            if (a != null)
            {
                //var cek = listImage.Where(im => im.Name == imageName).FirstOrDefault().Id;
                
                if (height != 0)
                    a.Height = height;
                if (width != 0)
                    a.Width = width;
                a.PosX = x;
                a.PosY = y;
                a.UpdateAnimate();
                return Json(a, JsonRequestBehavior.AllowGet);
            }
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddAnimates(string imageType,string imageName, string questionId,double x,double y,double width,double height)
        {
            var c = imageName;
            UpdateList();
            AnimateViewModel a = new AnimateViewModel();
            a=GetAnimatesType(new Guid(imageType), new Guid(questionId));
            if (a != null)
            {
                //var cek = listImage.Where(im => im.Name == imageName).FirstOrDefault().Id;
                a.ImageId = new Guid(imageName);
                
                if(height!=0)
                    a.Height = height;
                if(width!=0)
                    a.Width = width;
                a.PosX = x;
                a.PosY = y;
                a.UpdateAnimate();
                return Json(a, JsonRequestBehavior.AllowGet);
            }
            else
            {
                a = new AnimateViewModel();
                a.Id = Guid.NewGuid();
                a.ImageId= new Guid(imageName);
                a.QuestionId =new Guid(questionId);
                a.Height = i.GetType((Guid)a.ImageId).Height;
                a.Width = i.GetType((Guid)a.ImageId).Width;
                a.PosX = 0;
                a.PosY = 0;

                //brutal
                if (imageType == "Background")
                {
                    a.Depth = 0;
                }
                else if (imageType == "Dialog")
                {
                    a.Depth = 1;
                }
                else if (imageType == "Character")
                {
                    a.Depth = 2;
                }
                a.AddAnimate();
                return Json(a, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAnimates(string questionId)
        {

            var temp = db.Animates.Where(x => x.IsDeleted == false && x.QuestionId == new Guid(questionId));
            AnimatesController ac = new AnimatesController();
            List<AnimateViewModel> a = new List<AnimateViewModel>();
            foreach (var item in temp)
            {
                AnimateViewModel animate = new AnimateViewModel(item);
                    string[] s = i.GetImageType((Guid)item.ImageId).Split(' ');
                    animate.imageType = s[0];
                    animate.Location = db.Images.Where(x=>x.Id==(Guid)item.ImageId).FirstOrDefault().Location;
                    a.Add(animate);
            }
            a.OrderBy(x => x.imageType);
            return Json(a, JsonRequestBehavior.AllowGet);
        }


        //-----for color

        public JsonResult SaveColor(string questionId, string color,int size)
        {
            UpdateList();
            QuestionViewModel q = listQuestion.Find(x => x.Id ==new Guid(questionId));
            if (q != null)
            {
                q.FontColor = color;
                q.FontSize = size;
                q.UpdateQuestion(q);
            }
            return Json( q,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetColor(string questionId)
        {
            UpdateList();
            string color = listQuestion.Find(x => x.Id ==new Guid(questionId)).FontColor;
            return Json(color, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSize(string questionId)
        {
            UpdateList();
            int size = listQuestion.Find(x => x.Id == new Guid(questionId)).FontSize;
            return Json(size, JsonRequestBehavior.AllowGet);
        }

        //------end color

        //---------------------------apply to all

        public JsonResult ApplyAll(string questionId,string color,int size)
        {
            if (AccountController.CheckSurveyId())
            {
                AnimateViewModel a;
                UpdateList();
                var id = new Guid(questionId);
                var surveyId = AccountController.GetSurveyId();
                var q = listQuestion.Where(x => x.SurveyId == surveyId);
                var animates = listAnimate.Where(x => x.QuestionId == id);
                if (animates != null)
                {
                    foreach (var item in q)
                    {
                        if(item.Id!=id)
                        {
                            item.FontColor = color;
                            item.FontSize = size;
                            item.UpdateQuestion(item);
                            var animates2 = listAnimate.Where(x => x.QuestionId == item.Id);

                            foreach (var i in animates)
                            {
                                var check = animates2.Where(x => x.imageType == i.imageType).FirstOrDefault();
                                if (check == null)
                                {
                                    a = new AnimateViewModel
                                    {
                                        Id = Guid.NewGuid(),
                                        QuestionId = item.Id,
                                        ImageId = i.ImageId,
                                        Height = i.Height,
                                        Width = i.Width,
                                        PosX = i.PosX,
                                        PosY = i.PosY,
                                        Depth = i.Depth
                                    };
                                    a.AddAnimate();
                                }
                                else
                                {
                                    check.ImageId = i.ImageId;
                                    check.Height = i.Height;
                                    check.Width = i.Width;
                                    check.PosX = i.PosX;
                                    check.PosY = i.PosY;
                                    check.Depth = i.Depth;
                                    check.UpdateAnimate();
                                }
                            }
                            if (animates.Count() < animates2.Count())
                            {
                                foreach (var i in animates)
                                {
                                    var check = animates2.Where(x => x.imageType != i.imageType);
                                    if (check != null)
                                    {
                                        foreach (var j in check)
                                        {
                                            j.DeleteAnimate();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Json(questionId, JsonRequestBehavior.AllowGet);
        }

        //-------------------------end ajax called------------------------

        //parameternya:
        //  - image type
        //  - image name
        //  - question id

        //cek pada animates question bersangkutan apakah sudah ada image type gitu
        //jika ada kembalikan

        //cek image type
        //jika ada tinggal update
        //jika tidak ada buat baru

        public AnimateViewModel GetAnimatesType(Guid type, Guid questionId)
        {
            UpdateList();
                var img = db.ImageTypes.Find(type).Type;
                var l = listAnimate.Where(x => x.QuestionId == questionId && x.imageType == img).FirstOrDefault();

                if (l != null)
                {
                    return l;
                }
            return null;
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
