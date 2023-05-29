using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using TRA_MobileAPIs.ViewModel;

namespace TRA_MobileAPIs.ResponseDetails
{
    public class getNotifyDetails
    {

        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
        public List<NotifyVm> Notification_Details { get; set; }
    }

    public class NotificationDetails
    {

        public string Message { get; set; }
        public HttpStatusCode Status { get; set; }
      //  public List<NotifyVm> Notification_Details { get; set; }
    }

}