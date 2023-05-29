using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRA_MobileAPIs.ViewModel
{
    public class caseVM
    {
        public string CaseID { get; set; }
        public DateTime? createdon { get; set; }
        public string statuscode { get; set; }
    }

    public class CaseVM
    {
        public const string caseType = "casetypecode";
        public const string contactId = "customerid";
        public const string complaintType = "tra_complainttype";
        public const string complaintTypeId = "tra_complainttypeid";
        public const string complaintSubType = "tra_complaintsubtype";
        public const string complaintSubTypeId = "tra_complaintsubtypeid";
        public const string DisputeAccno = "tra_disputednumber";
        public const string ownerDisAcc = "tra_ownerofdisputednumber";
        public const string onweName = "tra_nameofowner";
        public const string cprNoOwner = "tra_cprnumberofowner";
        public const string discription = "description";
        public const string serviceProvider = "tra_serviceprovider";
        public const string srvcprdId = "tra_serviceproviderid";
        public const string service = "tra_service";
        public const string srvcId = "tra_serviceid";
        public const string serviceSubtype = "tra_servicesubtype";
        public const string srvcSubId = "tra_servicesubtypeid";
        public const string subscriptionType = "tra_subscriptiontype";
        public const string dateofcomplaintSP = "tra_dateofcomplaintagainstserviceprovider";
        public const string serviceProviderRef = "tra_serviceprovidercasereference";
        public const string caseQ1 = "tra_casequestion1";
        public const string caseQ2 = "tra_casequestion2";
        public const string caseQ3 = "tra_casequestion3";
        public const string origin = "caseorigincode";
        public const string caseNo = "caseid";
        public const string SubmissionDate = "createdon";
        public const string srvcType = "tra_servicetype";

    }

    
    public class AnnotationData
    {

        public string DocumentBody { get; set; }
        public string fileName { get; set; }

    }
    public class CaseLable
    {

        public const string CaseType = "casetypecode";
        public const string ContactId = "customerid";
        public const string EnquiryType = "tra_enquirytype";
        public const string enquirytyprid = "tra_enquirytypeid";
        public const string origin = "caseorigincode";

        public const string description = "description";
        public const string ticket = "ticketnumber";
        public const string status = "statuscode";
        public const string create = "createdon";

        public const string srvcProviderId = "tra_serviceproviderid";
        public const string srvcProvide = "tra_serviceprovider";

        public const string caseId = "incidentid";
        public const string _caseID = "caseID";
        public const string comments ="tra_comment";

        public const string srvcId = "tra_serviceid";

        public const string srvcNm = "tra_name";
        public const string srvc = "tra_service";

        public const string StateCode = "statecode";

        public const string traConsumer = "tra_consumer";


    }

    public class Annotation
    {

        public const string documents = "documentbody";
        public const string subject = "subject";
        public const string notes = "notetext";
        public const string objectId = "objectid";
        public const string fileName = "filename";

    }

    public static class ContactLookup
    {

        public const string contactEntity = "contact";
        public const string contact_Name = "fullname";
    }
    public static class ComplaintLookup
    {
        public const string ComplaintType = "tra_complainttype";
        public const string Complaintname = "tra_name";
    }
    public static class ServiceProviderLookup
    {
        public const string serviceProvider = "tra_serviceprovider";
        public const string servicepname = "tra_name";
    }
    public static class serviceTypeLookup
    {
        public const string serviceSubType = "tra_servicesubtype";
        public const string servicetypename = "tra_name";
    }
    public static class servicLookup
    {
        public const string Servicee = "tra_service";
        public const string servicename = "tra_name";
    }
    public static class ComplaintsubTypeLookup
    {
        public const string complaintSubType = "tra_complaintsubtype";
        public const string subname = "tra_name";
    }
    public static class incidentLookpup
    {

        public const string incidentEntity = "incident";
        public const string incidentNm = "ticketnumber";
    }
    public static class EnquiryTypeLookup
    {

        public const string EnquiryType = "tra_enquirytype";
        public const string enquiryName = "tra_name";
    }
    public class servcClass
    {

        public Guid tra_serviceid { get; set; }
        public string tra_service { get; set; }
        public string tra_serviceAr { get; set; }
    }
    public class Is_Status
    {
        public string IsActive { get; set; }
        public string _entityNm { get; set; }

        public Guid _entityId { get; set; }

    
    }
    public class retrieveCase
    {
        public string casetypecode { get; set; }
        public DateTime? createdon { get; set; }
        public string tra_serviceprovider { get; set; }
        public string tra_service { get; set; }
        public string tra_subscriptiontype { get; set; }
        public string tra_complainttype { get; set; }
        public string tra_complaintsubtype { get; set; }
        public string tra_serviceprovidercasereference { get; set; }
        public string tra_disputednumber { get; set; }
        public string description { get; set; }
        //public string caseNumber { get; set; }
    }
}