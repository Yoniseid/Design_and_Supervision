using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelLegal
    {
        public int id { set; get; }
        public int tenderid { set; get; }
        public DateTime date { set; get; }
        public string RequestType { set; get; }
        public string attachement { set; get; }
        public int EmployeeId { set; get; }
        public string Responsible_Personnel { set; get; }
        //public int SenderDepId { set; get; }
        //public int SenderDepIdDepName { set; get; }
        public string Remark { set; get; }
        public string status { set; get; }
        public int isdeleted { set; get; }
        public string lasteditedby { set; get; }

    } 
}