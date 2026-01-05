using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelTender
    {



        [Display(Name = "Id")]
        public int id { get; set; }
        public string InitiationType { get; set; }
        //[Required(ErrorMessage = "Tender Document Number is required.")]

        public string TenderDocNumber { get; set; }

        //public int ClientNo { get; set; }
        public string  ClientName { get; set; }

        //[Required(ErrorMessage = "Project Name is required.")]
        public string ProjectName { get; set; }
        

        //[Required(ErrorMessage = "Project Name is required.")]
      
        public int ReceiverId { get; set; }


        public string ReceivedBy { get; set; }

        //[Required(ErrorMessage = "Letter Date is required.")]
        public DateTime LetterDate { get; set; }
        public DateTime TenderClosingDate { get; set; }

        //[Required(ErrorMessage = "Type Of Work is required.")]
        public string TypeOfWork { get; set; }
        public DateTime RegisterationDate { get; set; }

       // [Required(ErrorMessage = "TOR Attachement is required.")]
        public string Main_Attachement { get; set; }

       // [Required(ErrorMessage = "Attachement is required.")]
        public string Other_Attachment { get; set; }

       // [Required(ErrorMessage = "TOR Attachement is required.")]
        public string Remark { get; set; } 

        public string Status { get; set; }

        public int IsDelete { get; set; }
        public string LastEditedBy { get; set; }





    }
}