using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace VibeWave.Models
{
    public class HomeIndexViewModel
    {
        public string SearchString { get; set; }
        public int? CategoryId { get; set; }// you can choose to fill it or not.
        public List<Concert> Concerts{ get; set; }//one(actor) to many(concert)

    }
}
