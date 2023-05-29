using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using TRA_MobileAPIs.ViewModel;

namespace TRA_MobileAPIs.ResponseDetails
{
    public class ContactListDetails
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public List<ContactAttribute> ListContacts { get; set; }

    }
}