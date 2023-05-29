using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using TRA_MobileAPIs.ViewModel;

namespace TRA_MobileAPIs.ResponseDetails
{
    public class ConsumerRegisterDetails
    {

        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public Guid ContactID { get; set; }

      
     

     
        public ConsumerRegisterDetails()
        {
            Status = 0;
            Message = string.Empty;

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }



                disposedValue = true;
            }
        }



        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);

        }
        #endregion
        public class ServerInput
        {
            public ServerInput()
            {
                version = "12.2.4.3";
                sessionID = "123123123123123123";
                validFor = "36000";
            }
            public string sessionID { get; set; }
            public string version { get; set; }
            public string validFor { get; set; }
        }
    }
}