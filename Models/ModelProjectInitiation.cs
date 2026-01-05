using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelProjectInitiation
    {




        [Display(Name = "Id")]
        public int id { get; set; }

        //[Required(ErrorMessage = "projectno is required.")]
        public int projectno { get; set; }

        //[Required(ErrorMessage = "Client no is required.")]
        public int clientno { get; set; }

       // [Required(ErrorMessage = "Project Manger is required.")]
        public int ProjectManagerid { get; set; }

       // [Required(ErrorMessage = "Responsibele Dept is required.")]
        public int ResponsibleDeptId { get; set; }

       // [Required(ErrorMessage = "Branch  is required.")]
        public int BranchId { get; set; }


       // [Required(ErrorMessage = "Acceptance Letter is required.")]
        public int AcceptanceLetterno { get; set; }

       // [Required(ErrorMessage = "Acceptance Letter is required.")]
        public int kickoffmeetingLetterno { get; set; }

        //[Required(ErrorMessage = "Kickoff meeting date is required.")]
        public DateTime kickoffmeetingdate { get; set; }

                     
      //  [Required(ErrorMessage = "Attachement is required.")]
        public string kickoffmeetingminute { get; set; }

       // [Required(ErrorMessage = "Attachement is required.")]
        public string remark { get; set; }

        //[Required(ErrorMessage = "Remark is required.")]
        //public  int Isdeleted { get; set; }
        //[Required(ErrorMessage = "Attachement is required.")]
        //public string lasteditedby { get; set; }












    }
}