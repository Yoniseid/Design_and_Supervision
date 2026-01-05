using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelRequest
    {
        

        [Display(Name = "Id")]
        public int id { get; set; }

        [Required(ErrorMessage = "Tender Document Number is required.")]
        public int tender_id { get; set; }
        public string RequestMode { get; set; }
        [Required(ErrorMessage = "Request Type required.")] 
        public string RequestType { get; set; }

        //[Required(ErrorMessage = "Requesting Department Id is required.")]
       // public int ToDepId { get; set; }
        [Required(ErrorMessage = "Requesting Department  is required.")]
        public string Sentto { get; set; }

        public DateTime RequestDate { get; set; }
       // [Required(ErrorMessage = "Department Head required.")]
        public string DeprtmentHead { get; set; }
        

        //  [Required(ErrorMessage = "To Dept is required")]

        public string ToDepId { get; set; }

        [Required(ErrorMessage = " Remark is required.")]
        public string RequestRemark { get; set; }
       // [Required(ErrorMessage = " Attachement is required.")]
        public string RequestAttachement { get; set; }
        public int isdeleted { get; set; }



    }
}