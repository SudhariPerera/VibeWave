using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeWave.Models.ViewModels;


namespace VibeWave.Models
{
    public class HomeIndexViewModel
    {
        public string SearchString { get; set; }
        public int? CategoryId { get; set; }// you can choose to fill it or not.
        public List<HomeConcertVM> Concerts{ get; set; }//one(actor) to many(concert)
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
