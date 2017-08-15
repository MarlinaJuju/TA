using Showvey.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Showvey.ViewModels
{
    public class LogViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        SurveyDataContext db = new SurveyDataContext();
        public LogViewModel()
        {

        }
        public LogViewModel(Log item)
        {
            this.Id = item.Id;
            this.Type = item.Type;
            this.Message = item.Message;
            this.CreatedDate = item.CreatedDate;
        }
        public Log ToModel()
        {
            Log l = new Log
            {
                Id = this.Id,
                CreatedDate = this.CreatedDate,
                Message = this.Message,
                Type = this.Type
            };
            return l;
        }
        public ActionResult AddLog(LogViewModel item)
        {
            try
            {
                Log i = item.ToModel();
                i.CreatedDate = DateTime.Now;
                db.Logs.Add(i);
                db.SaveChanges();
                return new HttpStatusCodeResult(200);
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
        }
    }
}