using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TRA_MobileAPIs.ResponseDetails
{
    public class ComplaintCase
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        //public Guid CaseID { get; set; }
        public string CaseNo { get; set; }

        public Guid caseGuid { get; set; }
    }
}