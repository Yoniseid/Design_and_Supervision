using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelLegalResponse
    {
        [Display(Name = "Id")]
        public int id { get; set; }

        [Required(ErrorMessage = "Tender Document Number is required.")]
        public int tender_id { get; set; }
   
        public string Responsible_Personnel{ get; set; }
        public int EmpId { get; set; }
        [Required(ErrorMessage = "Request No required.")]
        public int RequestNo { get; set; }

        public string FinalProposal { get; set; }
        public string FinalAgreement { get; set; }
        public string OtherAttachement { get; set; }

        public DateTime ResponseDate { get; set; }
        [Required(ErrorMessage = " Remark is required.")]
        public string ResponseRemark { get; set; }


        public string Approved { get; set; }
        public string Status { get; set; }

        public int isdeleted { get; set; }
    }
}