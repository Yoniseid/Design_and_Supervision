using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelCommitement
    {
        [Display(Name = "Id")]
        public int id { get; set; }

        //[Required(ErrorMessage = "SLA number is required.")]
        public int SLAno { get; set; }


        [Required(ErrorMessage = "Commitment Letterno is required.")]
        public string CommitmentLetterno { get; set; }
       // public DateTime date { get; set; }

        //[Required(ErrorMessage = "Attachement is required.")]
        public string attachement { get; set; }
       
        public string ProjectTitle { get; set; }
    }
}

