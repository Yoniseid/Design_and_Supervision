using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelTenderDetail
    {
               

        [Display(Name = "Id")]
        public int id { get; set; }

        [Required(ErrorMessage = "Tender Document Number is required.")]
        public int Tenderid { get; set; }

        [Required(ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Responsible Personnel is required.")]
        public string Responsible_Personnel { get; set; }

        [Required(ErrorMessage = "Team Id is required.")]
        public int AssignedTeamId { get; set; }

        [Required(ErrorMessage = "Assigned Team is required.")]
        public string AssignedTeam { get; set; }

        [Required(ErrorMessage = "Date Of Assign is required.")]
        public DateTime DateOfAssign { get; set; }

        [Required(ErrorMessage = "Initial Deadline Date is required.")]
        public DateTime initialdeadlinedate { get; set; }

        //[Required(ErrorMessage = "Attachment is required.")]
        public string Attachment { get; set; }

        [Required(ErrorMessage = "Remark is required.")]
        public string Remark { get; set; }
       
        public DateTime FinalizedDate { get; set; }
     
        public string ApprovedorRejected { get; set; }
        public string Status { get; set; }
        public int isdeleted { get; set; }
        public string lasteditedby { get; set; }
        public string ResponseAttachement { get; set; }




    }
}