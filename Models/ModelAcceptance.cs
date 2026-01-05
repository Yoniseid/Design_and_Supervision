using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelAcceptance
    {
        [Display(Name = "Id")]
        public int id { get; set; }

          public string tenderdocnumber { get; set; }
        //[Required(ErrorMessage = "AcceptanceLetterno is required.")]
        public string AcceptanceLetterno { get; set; }

        //[Required(ErrorMessage = "date is required.")]
        public DateTime date { get; set; }

       // [Required(ErrorMessage = "Attachement is required.")]
        public string attachement { get; set; }






    }
}