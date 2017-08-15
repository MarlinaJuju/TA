using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.ViewModels
{
    public class EntityViewModel
    {
        public bool IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? DeletionDate { get; set; }
        public Guid? DeletionUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        [Required]
        public Guid Id { get; set; }
    }
}