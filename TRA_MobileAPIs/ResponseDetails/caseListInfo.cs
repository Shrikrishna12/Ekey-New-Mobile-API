using System;
using System.Collections.Generic;

using System.Net;

using TRA_MobileAPIs.ViewModel;

namespace TRA_MobileAPIs.ResponseDetails
{
    public class caseListInfo
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public List<retrieveCase> CaseList { get; set; }
    }

    public class complaintInfo
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public List<getComplaint> complaintList { get; set; }
    }

    public class getComplaint
    {
        public string tra_name { get; set; }
        public string tra_complaintAr { get; set; }
        public Guid tra_complainttypeid { get; set; }
    }

    public class complaintSubTypeInfo
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public List<getComplaintSubType> complaintSubTypeList { get; set; }
    }

    public class getComplaintSubType
    {
        public string tra_name { get; set; }
        public string tra_complaintSubAr { get; set; }
        public Guid tra_complaintsubtypeid { get; set; }
    }


    public class serviceProviderDetails
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public List<getServiceProviders> ServiceProviders { get; set; }
    }

    public class getServiceProviders
    {
        public string tra_name { get; set; }
        public string tra_serviceProviderAr { get; set; }
        public Guid tra_serviceproviderid { get; set; }
    }

    public class serviceSubType
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public List<getServiceSubType> ServiceSubTypes { get; set; }
    }

    public class getServiceSubType
    {
        public string tra_name { get; set; }
        public string servcieSubtypeAr { get; set; }
        public Guid tra_servicesubtypeid { get; set; }
    }

    public class EnquiryTypesInfo
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public List<getEnquiryTypes> Enquiry_Types { get; set; }
    }

    public class getEnquiryTypes
    {
        public string tra_name { get; set; }
        public string tra_enquiryTypeAr { get; set; }
        public Guid tra_enquirytypeid { get; set; }
    }

}