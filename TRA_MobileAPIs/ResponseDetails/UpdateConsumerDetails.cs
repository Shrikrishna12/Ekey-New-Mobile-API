using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TRA_MobileAPIs.ResponseDetails
{
    public class UpdateConsumerDetails
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
    }

    public class NationalityDetails
    {
       public HttpStatusCode Status { get; set; }
       public string Message { get; set; }
        public List<GetNationality> _NationalityList { get; set; }
    }

    public class GetNationality
    {

        public string tra_name { get; set; }
        public string nationalityAr { get; set; }
        public Guid tra_nationalityid { get; set; }
    }
}