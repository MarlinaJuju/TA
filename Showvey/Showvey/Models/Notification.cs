using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Notification:Entity
    {
        public Guid? FromId { get; set; }
        public Guid? ToId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public bool ReceiverDeleted { get; set; }
        public DateTime? ReceiverDeletedDate { get; set; }
        public bool SenderDeleted { get; set; }
        public DateTime? SenderDeletedDate { get; set; }
    }
}