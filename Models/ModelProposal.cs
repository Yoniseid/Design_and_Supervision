using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Design_and_Supervion_Issue_Tracking.Models
{
    public class ModelProposal
    {
        public int id { set; get; }
        public int Legalid { set; get; }
        public string ProposalorContract { set; get; }
        public string TechnicalProposal { set; get; }
        public string FinancialProposal { set; get; }
       public string ContactAttachement { set; get; }
       public DateTime date { set; get; }
       public int  VersionNumber { set; get; }
       public string minutes { set; get; }
       public string Remark { set; get; }
       public int isdeleted { set; get; }
        public string lasteditedby { set; get; }
    }
}