using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelSLA
    {


        [Display(Name = "Id")]
        public int id { get; set; }

        [Required(ErrorMessage = "projectno is required.")]
        public int Projectno { get; set; }


        public int InternalId { get; set; }
        public string SentTo { get; set; }

        [Required(ErrorMessage = "SLA number is required.")]
        public string SLAnumber { get; set; }
        public DateTime dateofsigned { get; set; }

       
        public string attachement { get; set; }




    }
}
