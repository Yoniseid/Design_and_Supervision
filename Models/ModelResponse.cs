using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelResponse
    {

        /*id	int	Unchecked
tender_id	int	Checked
RequestNo	int	Checked
TechnicalDocAttach	nvarchar(100)	Checked
FinancialDocAttach	nvarchar(100)	Checked
Otherattachement	nvarchar(100)	Checked
ResponseRemark	nvarchar(200)	Checked */


        [Display(Name = "Id")]
        public int id { get; set; }

        [Required(ErrorMessage = "Tender Document Number is required.")]
        public int tender_id { get; set; }

        [Required(ErrorMessage = "Request No required.")]
        public int RequestNo { get; set; }
        public string SubDepartment { get; set; }
        public string TechDocProposal { get; set; }
        //[Required(ErrorMessage = "Financial Doc Attach  is required.")]
         public string FinDocProposal { get; set; }
        public string FinalProposal { get; set; }
        public string FinalAgreement { get; set; }
        //[Required(ErrorMessage = "Other Attachement  is required.")]
        public DateTime ResponseDate { get; set; }

        public string Responsible_Personnel { get; set; }
        public string Status { get; set; }
       public string OtherAttachement { get; set; }
       public string Approved { get; set; }

        [Required(ErrorMessage = " Remark is required.")]
        public string ResponseRemark { get; set; }
        public int isdeleted { get; set; }
    }
}
 