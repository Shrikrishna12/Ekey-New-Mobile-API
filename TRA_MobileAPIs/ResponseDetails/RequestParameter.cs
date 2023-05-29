using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRA_MobileAPIs.ResponseDetails
{
    public class RequestParameter
    {
        public Dictionary<string, string> Source { get; set; }
    }

    public class RequestAttachments
    {

        public RequestData Source { get; set; }
    }

    public class RequestData
    {
        public Dictionary<string, string> data { get; set; }
        public List<Dictionary<string, string>> attachments { get; set; }

    }
}