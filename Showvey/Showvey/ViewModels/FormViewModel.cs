using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.ViewModels
{
    public class FormViewModel
    {
        [Key]
        public Guid Id { get; set; }
    }
}