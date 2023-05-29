
using System.Collections.Generic;

using System.Net;
using TRA_MobileAPIs.ViewModel;

namespace TRA_MobileAPIs.ResponseDetails
{
    public class CaseListDetails
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public List<caseVM> CaseDetails { get; set; }
    }

}