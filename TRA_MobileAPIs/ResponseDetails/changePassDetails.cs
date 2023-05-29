using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TRA_MobileAPIs.ResponseDetails
{
    public class ChangePassDetails
    {
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
    }
    public class resetPassDetails
    {
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}