using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
            public enum gender
        {
            Male,Female
        }
    public class UserViewModel:EntityViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string RetypePassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public Guid? CityId { get; set; }
        public CityViewModel City { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public Guid RoleId { get; set; }
        public bool IsComplete { get; set; }
        public ICollection<SurveyViewModel> Surveys { get; set; }
        public ICollection<ResponseViewModel> Responses { get; set; }
        public RoleViewModel Role { get; set; }
        public string CityName { get; set; }
        public string RoleName { get; set; }
        public string PasswordToken { get; set; }
        public DateTime? PasswordTokenExpired { get; set; }
        public bool TokenActivated { get; set; }
        private SurveyDataContext db = new SurveyDataContext();
        private static Random random = new Random();
        public string TokenGenerate(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            PasswordToken= new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return PasswordToken;
        }
        public UserViewModel(User item)
        {
            
            this.Birthdate = item.Birthdate;
            this.Email = item.Email;
            this.City = new CityViewModel(db.Cities.Find(item.CityId));
            if (this.City.IsDeleted == true)
            {
                this.CityName = "";
            }
            else
            {
                this.CityName = this.City.Name;
            }
            this.CityId = item.CityId;
            this.FirstName = item.FirstName;
            this.Gender =item.Gender;
            this.Id = item.Id;
            this.IsActive = item.IsActive;
            this.IsBlocked = item.IsBlocked;
            this.IsComplete = item.IsComplete;
            this.IsDeleted = item.IsDeleted;
            this.LastLogin = item.LastLogin;
            this.LastName = item.LastName;
            this.ModifiedDate = item.ModifiedDate;
            this.ModifiedUserId = item.ModifiedUserId;
            this.Password = item.Password;
            this.PhoneNumber = item.PhoneNumber;
            this.Responses = this.GetResponseViewList(item.Responses);
            this.Role = new RoleViewModel(db.Roles.Find(item.RoleId));
            this.PasswordToken = item.PasswordToken;
            this.PasswordTokenExpired = item.PasswordTokenExpired;
            this.TokenActivated = item.TokenActivated;
            if (this.Role.IsDeleted == true)
            {
                this.RoleName = "";
            }
            else
            {
                this.RoleName = this.Role.Name;
            }
            this.RoleId = item.RoleId;
            this.Surveys = this.GetSurveyViewList(item.Surveys);
            this.Username = item.Username;
            this.CreatedDate = item.CreatedDate;
            this.CreatedUserId = item.CreatedUserId;
            this.DeletionDate = item.DeletionDate;
            this.DeletionUserId = item.DeletionUserId;
        }
        public UserViewModel()
        {
        }
        public User ToModel()
        {
            User u = new User();
            u.Birthdate = this.Birthdate;
                u.City = db.Cities.Find(this.CityId);
                u.CityId = this.CityId;
                u.DeletionDate = this.DeletionDate;
                u.DeletionUserId = this.DeletionUserId;
                u.Email = this.Email;
                u.FirstName = this.FirstName;
                u.Gender = this.Gender.ToString();
                u.Id = this.Id;
                u.IsActive = this.IsActive;
                u.IsBlocked = this.IsBlocked;
                u.IsComplete = this.IsComplete;
                u.IsDeleted = this.IsDeleted;
                u.LastLogin = this.LastLogin;
                u.LastName = this.LastName;
                u.ModifiedDate = this.ModifiedDate;
                u.ModifiedUserId = this.ModifiedUserId;
                u.Password = this.Password;
                u.PhoneNumber = this.PhoneNumber;
                u.Responses = this.GetResponseList(this.Responses);
                u.Role = db.Roles.Find(this.RoleId);
                u.RoleId = this.RoleId;
                u.Surveys = this.GetSurveyList(this.Surveys);
                u.Username = this.Username;
                u.CreatedDate = this.CreatedDate;
                u.CreatedUserId = this.CreatedUserId;
            u.PasswordToken = this.PasswordToken;
            u.PasswordTokenExpired = this.PasswordTokenExpired;
            u.TokenActivated = this.TokenActivated;
            return u;
        }
        private ICollection<ResponseViewModel> GetResponseViewList(ICollection<Response> item)
        {
            List<ResponseViewModel> r = new List<ResponseViewModel>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        r.Add(new ResponseViewModel(i));
                    }
                }
            }
            return r;
        }
        private ICollection<Response> GetResponseList(ICollection<ResponseViewModel> item)
        {
            List<Response> r = new List<Response>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        r.Add(i.ToModel());
                    }
                }
            }
            return r;
        }
        private ICollection<SurveyViewModel> GetSurveyViewList(ICollection<Survey> item)
        {
            List<SurveyViewModel> u = new List<SurveyViewModel>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        u.Add(new SurveyViewModel(i));
                    }
                }
            }
            return u;
        }
        private ICollection<Survey> GetSurveyList(ICollection<SurveyViewModel> item)
        {
            List<Survey> u = new List<Survey>();
            if (item != null)
            {
                foreach (var i in item)
                {
                    if (i.IsDeleted == false)
                    {
                        u.Add(i.ToModel());
                    }
                }
            }
            return u;
        }
        public ActionResult AddUser(UserViewModel item)
        {
            try
            {
                User u = item.ToModel();
                u.CreatedDate = DateTime.Now;
            u.LastLogin = DateTime.Now;
                db.Users.Add(u);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Insertion",
                    Message = "failed to insert user from " + this.CityName + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult UpdateUser(UserViewModel item)
        {
            try
            {
                User c = db.Users.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.Id = item.Id;
                    c.IsDeleted = item.IsDeleted;
                    c.ModifiedDate = DateTime.Now;
                    c.ModifiedUserId = item.ModifiedUserId;
                    c.DeletionDate = item.DeletionDate;
                    c.DeletionUserId = item.DeletionUserId;
                    c.Birthdate = item.Birthdate;
                    c.City = db.Cities.Find(item.CityId);
                    c.CityId = item.CityId;
                    c.Email = item.Email;
                    c.FirstName = item.FirstName;
                    c.Gender = item.Gender.ToString();
                    c.IsActive = item.IsActive;
                    c.IsBlocked = item.IsBlocked;
                    c.IsComplete = item.IsComplete;
                    c.LastLogin = item.LastLogin;
                    c.LastName = item.LastName;
                    c.Password = item.Password;
                    c.PhoneNumber = item.PhoneNumber;
                    c.Responses = item.GetResponseList(item.Responses);
                    c.Role = db.Roles.Find(item.RoleId);
                    c.RoleId = item.RoleId;
                    c.Surveys = item.GetSurveyList(item.Surveys);
                    c.Username = item.Username;
                    c.CreatedUserId = item.CreatedUserId;
//                    c.CreatedDate = item.CreatedDate;
                    c.PasswordTokenExpired = item.PasswordTokenExpired;
                    c.PasswordToken = item.PasswordToken;
                    c.TokenActivated = item.TokenActivated;
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Update",
                    Message = "failed to update user from " + this.CityName + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult DeleteUser(UserViewModel item)
        {
            try
            {
                User c = db.Users.Find(item.ToModel().Id);
                if (c != null)
                {
                    c.IsDeleted = true;
                    c.DeletionDate = DateTime.Now;
                    //foreach (var i in db.Responses)
                    //{
                    //    //if (i.UserId == c.Id)
                    //    //{
                    //    //    i.IsDeleted = true;
                    //    //}
                    //}
                    foreach (var i in db.Surveys)
                    {
                        if (i.UserId == c.Id)
                        {
                            i.IsDeleted = true;
                        }
                    }
                    db.SaveChanges();
                }
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                LogViewModel l = new LogViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    Type = "Deletion",
                    Message = "failed to delete user from " + this.CityName + " to database"
                };
                l.AddLog(l);
                return new HttpStatusCodeResult(400);
            }
        }
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            // Get the key from config file

            string key = (string)settingsReader.GetValue("CopyHitamRobusta",
                                                             typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = (string)settingsReader.GetValue("CopyHitamRobusta",
                                                         typeof(String));

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //release any resource held by the MD5CryptoServiceProvider

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}