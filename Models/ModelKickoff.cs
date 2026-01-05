using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelKickoff
    {
        [Display(Name = "Id")]
        public int id { get; set; }

       // [Required(ErrorMessage = "projectno is required.")]
        public int projectno { get; set; }
        public int ProjectName { get; set; }

       // [Required(ErrorMessage = "Letter number is required.")]
        public string KickoffmeetingLetterno { get; set; }
        public DateTime date { get; set; }

       // [Required(ErrorMessage = "Attachement is required.")]
        public string attachement { get; set; }
    }
}