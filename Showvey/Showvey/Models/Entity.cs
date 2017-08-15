using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.Models
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? DeletionUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }

        public Entity()
        {
            Id = new Guid();
            IsDeleted = false;
            CreatedDate = DateTime.Now;
            //createduser
        }
    }
}