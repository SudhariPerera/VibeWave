using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeWave.Models.ViewModels
{
    public class CalendarVM
    {
        public string SearchString { get; set; }
        public int? CategoryId { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public List<Concert> Concerts { get; set; }
    }
}
