using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TRA_MobileAPIs.ResponseDetails
{
    public class LoginDetail
    {
        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }

        public Guid ContactID { get; set; }

        public string emailAddress { get; set; }

        public string CPRNID { get; set; }
    }
}