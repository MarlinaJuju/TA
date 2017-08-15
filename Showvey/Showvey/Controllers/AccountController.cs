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
using System.Net.Mail;

namespace Showvey.Controllers
{
    public class AccountController : Controller
    {
        private SurveyDataContext db = new SurveyDataContext();
        private IQueryable<User> x;
        private List<UserViewModel> listUser = new List<UserViewModel>();
        private List<RoleViewModel> listRole = new List<RoleViewModel>();
        private List<CityViewModel> listCity = new List<CityViewModel>();
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

        [AllowAnonymous]
        public ActionResult Login()
        {
                return View(new LoginViewModel());
        }

        public ActionResult Logout()
        {
            ClearPermission();
            ClearUser();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel l)
        {
                string pw = l.GetPassword(l.Email);
                l.Password = UserViewModel.Encrypt(l.Password);
                if (string.IsNullOrEmpty(pw))
                {
                    l.Password = UserViewModel.Decrypt(l.Password);
                    ModelState.AddModelError("", "The user login or password provided is incorrect.");
                }
                else
                {
                    if (l.Password.Equals(pw))
                    {
                        l.Authenticate();
                        Session["permission"] = l.Permissions;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        //l.Email = UserViewModel.Decrypt(pw);
                        l.Password = UserViewModel.Decrypt(l.Password);
                        ModelState.AddModelError("", "The password provided is incorrect.");
                    }
                }
                return View(l);
        }

        [AllowAnonymous]
        public ActionResult RecoveryPassword()
        {
            if (AccountController.CheckSession("token"))
            {
                if (AccountController.GetSession("token") == "true")
                {
                    return View(new LoginViewModel());
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RecoveryPassword(LoginViewModel l)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "The user login or password provided is incorrect.");
            }
            l.Id = new Guid();
            if (l.Password != l.RetypePassword)
            {
                ModelState.AddModelError("", "Password and Retype password doesn't match");
            }
            UpdateList();
            UserViewModel u = listUser.Find(y => y.Email == l.Email);
            if (u != null)
            {
                u.Password = UserViewModel.Encrypt(l.Password);
                u.ModifiedDate = DateTime.Now;
                u.UpdateUser(u);
                return RedirectToAction("Login","Account");
            }
            return View(l);
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ForgotPasswordViewModel l)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            l.Id = new Guid();
            UpdateList();
            var u = listUser.Find(y => y.Email == l.Email);
            if (u != null)
            {
                string token = u.TokenGenerate(4);
                u.PasswordTokenExpired = DateTime.Now.Date.AddDays(2);
                u.TokenActivated = false;
                u.UpdateUser(u);
                AccountController.RememberEmail(u.Email);
                SendEmail("Recovery your account", "Hello, to recovery your account, please insert this code below: " + token, l.Email);
                return RedirectToAction("TokenInsertion", "Account");
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult TokenInsertion()
        {
            if (AccountController.CheckEmail())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult TokenInsertion(string token)
        {
            if (token == "" || token == null)
            {
                ModelState.AddModelError("", "Your token is incorrect!");
            }
            UpdateList();
            var u = listUser.Find(l => l.PasswordToken == token);
            if (u != null)
            {
                if (u.PasswordTokenExpired > DateTime.Now && u.TokenActivated==false)
                {
                    u.TokenActivated = true;
                    u.PasswordToken = null;
                    u.PasswordTokenExpired = null;
                    u.UpdateUser(u);
                    AccountController.RememberSession("token", "true");
                    return RedirectToAction("RecoveryPassword", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Your token is expired!");
                }
            }
            return View();
        }

        public static void SendEmail(string title, string text,string email)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential("ceo.showvey@gmail.com", "Ukm12345*");
            //smtpClient.UseDefaultCredentials = true;
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;



            MailMessage mail = new MailMessage("ceo.showvey@gmail.com", email);

            //Setting From , To and CC
            mail.From = new MailAddress("ceo.showvey@gmail.com", "Showvey");
            mail.To.Add(new MailAddress(email));
            mail.Subject = title;
            mail.Body = text;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            smtpClient.Send(mail.From.ToString(), mail.To.ToString(), mail.Subject, mail.Body);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //--------------Session------------------------
        public static bool CheckPermission(string check)
        {
            var p = System.Web.HttpContext.Current.Session["Access"] as List<AccessViewModel>;
            if (p != null)
            {
                foreach (var item in p)
                {
                    if (item.Permission.Name == check)
                    {
                        if (item.IsGranted == true)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }
        public static bool CheckUser(string check)
        {
            var p = System.Web.HttpContext.Current.Session["User"] as UserViewModel;
            if (p != null)
            {
                    if (p.Username == check)
                    {
                        return true;
                    }
                
            }
            return false;
        }
        public static bool CheckUser()
        {
            var p = System.Web.HttpContext.Current.Session["User"] as UserViewModel;
            if (p != null)
            {
                    return true;
            }
            return false;
        }
        public static bool CheckUser(Guid? id)
        {
            var p = System.Web.HttpContext.Current.Session["User"] as UserViewModel;
            if (p != null)
            {
                if (p.Id == id)
                {
                    return true;
                }

            }
            return false;
        }
        public static bool CheckSurveyId(string check)
        {
            var p = System.Web.HttpContext.Current.Session["SurveyId"];
            if (p != null)
            {
                if (p.ToString() == check)
                {
                    return true;
                }

            }
            return false;
        }
        public static bool CheckSurveyId()
        {
            var p = System.Web.HttpContext.Current.Session["SurveyId"];
            if (p != null)
            {
                    return true;
            }
            return false;
        }
        public static bool CheckEmail()
        {
            var p = System.Web.HttpContext.Current.Session["Email"];
            if (p != null)
            {
                return true;
            }
            return false;
        }
        public static bool CheckSession(string name)
        {
            var p = System.Web.HttpContext.Current.Session[name];
            if (p != null)
            {
                return true;
            }
            return false;
        }
        public static Guid GetSurveyId()
        {
            var p = System.Web.HttpContext.Current.Session["SurveyId"];
            return new Guid(p.ToString());
        }
        public static SurveyViewModel GetSurvey()
        {
            var p = System.Web.HttpContext.Current.Session["Survey"] as SurveyViewModel;
            return p;
        }
        public static UserViewModel GetUser()
        {
            var p = System.Web.HttpContext.Current.Session["User"] as UserViewModel;
            return p;
        }
        public static List<QuestionViewModel> GetQuestion()
        {
            var p = System.Web.HttpContext.Current.Session["Questions"] as List<QuestionViewModel>;
            return p;
        }
        public static RespondentViewModel GetRespondent()
        {
            var p = System.Web.HttpContext.Current.Session["Respondent"] as RespondentViewModel;
            return p;
        }
        public static List<QuestionAnswerViewModel> GetQuestionAnswer()
        {
            var p = System.Web.HttpContext.Current.Session["QuestionAnswer"] as List<QuestionAnswerViewModel>;
            return p;
        }
        public static string GetEmail()
        {
            var p = System.Web.HttpContext.Current.Session["Email"].ToString();
            return p;
        }
        public static string GetSession(string name)
        {
            var p = System.Web.HttpContext.Current.Session[name].ToString();
            return p;
        }
        public static void RememberUser(UserViewModel obj)
        {
            System.Web.HttpContext.Current.Session["User"]=obj;
        }
        public static void RememberPermission(List<AccessViewModel> obj)
        {
            System.Web.HttpContext.Current.Session["Access"] = obj;
        }
        public static void RememberSurvey(SurveyViewModel obj)
        {
            System.Web.HttpContext.Current.Session["Survey"] = obj;
        }
        public static void RememberSurveyId(Guid obj)
        {
            System.Web.HttpContext.Current.Session["SurveyId"] = obj;
        }
        public static void RememberQuestion(List<QuestionViewModel> obj)
        {
            System.Web.HttpContext.Current.Session["Questions"] = obj;
        }
        public static void RememberRespondent(RespondentViewModel obj)
        {
            System.Web.HttpContext.Current.Session["Respondent"] = obj;
        }
        public static void RememberQuestionAnswer(List<QuestionAnswerViewModel> obj)
        {
            System.Web.HttpContext.Current.Session["QuestionAnswer"] = obj;
        }
        public static void RememberEmail(string obj)
        {
            System.Web.HttpContext.Current.Session["Email"] = obj;
        }
        public static void RememberSession(string name,string obj)
        {
            System.Web.HttpContext.Current.Session[name] = obj;
        }
        public static int Count()
        {
            if (System.Web.HttpContext.Current.Session["Count"] == null)
            {
                System.Web.HttpContext.Current.Session["Count"] = 1;
                var p = System.Web.HttpContext.Current.Session["Count"] as int?;
                return (int)p;
            }
            else
            {
                var p=System.Web.HttpContext.Current.Session["Count"] as int?;
                p++;
                System.Web.HttpContext.Current.Session["Count"] = p;
                return (int)p;
            }
        }
        public static void ClearUser()
        {
            System.Web.HttpContext.Current.Session["User"]=null;
        }
        public static void ClearPermission()
        {
            System.Web.HttpContext.Current.Session["Access"] = null;
        }
        public static void ClearSurveyId()
        {
            System.Web.HttpContext.Current.Session["SurveyId"] = null;
        }
        public static void ClearSurvey()
        {
            System.Web.HttpContext.Current.Session["Survey"] = null;
        }
        public static void ClearCount()
        {
            System.Web.HttpContext.Current.Session["Count"] = null;
        }
        public static void ClearQuestion()
        {
            System.Web.HttpContext.Current.Session["Questions"] = null;
        }
        public static void ClearRespondent()
        {
            System.Web.HttpContext.Current.Session["Respondent"] = null;
        }
        public static void ClearQuestionAnswer()
        {
            System.Web.HttpContext.Current.Session["QuestionAnswer"] = null;
        }
        public static void ClearEmail()
        {
            System.Web.HttpContext.Current.Session["Email"] = null;
        }
        public static void ClearSession(string name)
        {
            System.Web.HttpContext.Current.Session[name] = null;
        }
    }
}
