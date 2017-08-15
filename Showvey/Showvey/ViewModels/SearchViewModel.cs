using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Showvey.ViewModels
{
    public class SearchViewModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Table { get; set; }
        public string Model { get; set; }
    }
}