using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TRA_MobileAPIs.ViewModel;

namespace TRA_MobileAPIs
{

    public class checkkeyPair
    {

        public bool _IsTrue { get; set; }
        public string Keys { get; set; }

        public string ApI { get; set; }

    }
    public class verifiykeys
    {
        bool IsCheck;
        public checkkeyPair checkKeys(Dictionary<string,string> keys,string API)
        {
            checkkeyPair _checkkeys = new checkkeyPair();
            try
            {
                if (API == "RegisterConsumers")
                {
                    bool firstname = keys.Keys.Contains("firstname");
                    if (firstname == false)
                    {

                        _checkkeys.Keys = "First Name";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool lastname = keys.Keys.Contains("lastname");
                    if (lastname == false)
                    {
                        _checkkeys.Keys = "Last Name";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool nation = keys.Keys.Contains(contactVM.nationId);
                    if (nation == false)
                    {
                        _checkkeys.Keys = "Nationality";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool idtype = keys.Keys.Contains("tra_idtype");
                    if (idtype == false)
                    {
                        _checkkeys.Keys = "ID Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool ID_num = keys.Keys.Contains("tra_idnumber");
                    if (ID_num == false)
                    {
                        _checkkeys.Keys = "ID Number";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }


                    bool phone = keys.Keys.Contains("mobilephone");
                    if (phone == false)
                    {
                        _checkkeys.Keys = "Mobile Phone";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool secquizz = keys.Keys.Contains(contactVM.securityQuizzId);
                    if (secquizz == false)
                    {

                        _checkkeys.Keys = "Secuiry Question";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool ans = keys.Keys.Contains("tra_securityanswer");
                    if (ans == false)
                    {

                        _checkkeys.Keys = "security Answere";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool _consumerType = keys.Keys.Contains("tra_consumertype");
                    if (_consumerType == false)
                    {

                        _checkkeys.Keys = "Consumer_Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool emails = keys.Keys.Contains("emailaddress1");
                    if (emails == false)
                    {

                        _checkkeys.Keys = "Email Id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool country = keys.Keys.Contains("tra_countrycode");
                    if (country == false)
                    {

                        _checkkeys.Keys = "conutry Code";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
                else if (API == "GetConsumerInfo")
                {
                    bool emails = keys.Keys.Contains("emailaddress1");
                    if (emails == false)
                    {

                        _checkkeys.Keys = "Email Id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
                else if(API== "UpdateConsumer")
                {
                    bool contactid = keys.Keys.Contains("contactid");
                    if (contactid == false)
                    {

                        _checkkeys.Keys = "Contact Id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool firstname = keys.Keys.Contains("firstname");
                    if (firstname == false)
                    {

                        _checkkeys.Keys = "First Name";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool lastname = keys.Keys.Contains("lastname");
                    if (lastname == false)
                    {

                        _checkkeys.Keys = "Last Name";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool country = keys.Keys.Contains("tra_countrycode");
                    if (country == false)
                    {

                        _checkkeys.Keys = "conutry Code";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool phone = keys.Keys.Contains("mobilephone");
                    if (phone == false)
                    {

                        _checkkeys.Keys = "Mobile Phone";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool emails = keys.Keys.Contains("emailaddress1");
                    if (emails == false)
                    {

                        _checkkeys.Keys = "Email Id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
                else if(API== "CreateComplaint")
                {

                    bool contactid = keys.Keys.Contains("contactid");
                    if (contactid == false)
                    {

                        _checkkeys.Keys = "Contact Id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool caseType = keys.Keys.Contains("casetypecode");
                    if (caseType == false)
                    {


                        _checkkeys.Keys = "Case Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool complaint_type = keys.Keys.Contains(CaseVM.complaintTypeId);
                    if (complaint_type == false)
                    {
                        _checkkeys.Keys = "Complaint Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool disputeNo = keys.Keys.Contains("tra_ownerofdisputednumber");
                    if (disputeNo == true)
                    {
                        string values = keys["tra_ownerofdisputednumber"].ToString();
                        if (values == "true")
                        {
                            IsCheck = true;

                        }
                        else if (values == "false")
                        {
                            IsCheck = false;

                        }

                    }

                    bool crpNo_Owner = keys.Keys.Contains("tra_cprnumberofowner");
                    if (IsCheck == false)
                    {
                        if (crpNo_Owner == false)
                        {
                            _checkkeys.Keys = "OwnerOf_CRPNo";
                            _checkkeys._IsTrue = false;
                            return _checkkeys;
                        }
                    }
                  

                    bool name_ofOwner = keys.Keys.Contains("tra_nameofowner");
                    if (IsCheck == false)
                    {
                        if (name_ofOwner == false)
                        {
                            _checkkeys.Keys = "NameOf_Owner";
                            _checkkeys._IsTrue = false;
                            return _checkkeys;
                        }
                    }
                    bool srvcProvider = keys.Keys.Contains(CaseVM.srvcprdId);
                    if (srvcProvider == false)
                    {
                        _checkkeys.Keys = "Service_Provider";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool srvc = keys.Keys.Contains(CaseVM.srvcId);
                    if (srvc == false)
                    {
                        _checkkeys.Keys = "Service";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool srvcSubType = keys.Keys.Contains(CaseVM.srvcSubId);
                    if (srvcSubType == false)
                    {
                        _checkkeys.Keys = "Service_SubType";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool quizz1 = keys.Keys.Contains("tra_casequestion1");
                    if (quizz1 == false)
                    {
                        _checkkeys.Keys = "Question_1";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool quizz2 = keys.Keys.Contains("tra_casequestion2");
                    if (quizz2 == false)
                    {
                        _checkkeys.Keys = "Question_2";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool quizz3 = keys.Keys.Contains("tra_casequestion3");
                    if (quizz3 == false)
                    {
                        _checkkeys.Keys = "Question_3";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool origin = keys.Keys.Contains("caseorigincode");
                    if (origin == false)
                    {


                        _checkkeys.Keys = "Origin_Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool subscriptions = keys.Keys.Contains("tra_subscriptiontype");
                    if (subscriptions == false)
                    {


                        _checkkeys.Keys = "Subscription_Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }  

                }
                else if(API== "createEnquiry")
                {

                    bool contactid = keys.Keys.Contains("contactid");
                    if (contactid == false)
                    {

                        _checkkeys.Keys = "Contact Id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool caseType = keys.Keys.Contains("casetypecode");
                    if (caseType == false)
                    {


                        _checkkeys.Keys = "Case Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool tra_enquirytype = keys.Keys.Contains(CaseLable.enquirytyprid);
                    if (tra_enquirytype == false)
                    {
                        _checkkeys.Keys = "Enquiry_Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool origin = keys.Keys.Contains("caseorigincode");
                    if (origin == false)
                    {


                        _checkkeys.Keys = "Origin_Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
                else if (API== "CreateSuggestion")
                {


                    bool contactid = keys.Keys.Contains("contactid");
                    if (contactid == false)
                    {

                        _checkkeys.Keys = "Contact Id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                    bool caseType = keys.Keys.Contains("casetypecode");
                    if (caseType == false)
                    {


                        _checkkeys.Keys = "Case Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    

                    bool origin = keys.Keys.Contains("caseorigincode");
                    if (origin == false)
                    {


                        _checkkeys.Keys = "Origin_Type";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
                else if(API== "getCaseDetails")
                {

                    bool _Case = keys.Keys.Contains("caseid");
                    if (_Case == false)
                    {


                        _checkkeys.Keys = "Case_id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
                else if(API== "GetCaseList")
                {


                    bool _contactid = keys.Keys.Contains("contactid");
                    if (_contactid == false)
                    {


                        _checkkeys.Keys = "Contact_id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
                else if(API== "GetService")
                {

                    bool _srvcProviderId = keys.Keys.Contains("tra_serviceproviderid");
                    if (_srvcProviderId == false)
                    {


                        _checkkeys.Keys = "ServiceProvider_id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
                else if (API == "createConversations")
                {
                    bool incidentId = keys.Keys.Contains("caseID");
                    if (incidentId == false)
                    {


                        _checkkeys.Keys = "Case_id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                    bool comments = keys.Keys.Contains("tra_comment");
                    if (comments == false)
                    {


                        _checkkeys.Keys = "Comment";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }

                }                              
                else if (API == "EkeyLoginDetails")
                {
                    bool emails = keys.Keys.Contains("CPRNID");
                        if (emails == false)
                        {

                            _checkkeys.Keys = "tra_idnumber";
                            _checkkeys._IsTrue = true;
                            return _checkkeys;
                        }
                    else
                    {
                        _checkkeys.Keys = "tra_idnumber";
                    }
                }
                else if(API== "getNotify")
                {
                    bool contactid = keys.Keys.Contains("contactid");
                    if (contactid == false)
                    {

                        _checkkeys.Keys = "Contact Id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
                else if (API == "getServiceSubTypes")
                {
                    bool serviceProviderId = keys.Keys.Contains(CaseVM.srvcprdId);
                    if (serviceProviderId == false)
                    {

                        _checkkeys.Keys = "serviceProviderId";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }


                    bool serviceId = keys.Keys.Contains(CaseVM.srvcId);
                    if (serviceId == false)
                    {

                        _checkkeys.Keys = "serviceId";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
                else if (API == "getComplaintSubTypes")
                {

                    bool complaintTypeid = keys.Keys.Contains(CaseVM.complaintTypeId);
                    if (complaintTypeid == false)
                    {

                        _checkkeys.Keys = "ComplaintType_Id";
                        _checkkeys._IsTrue = false;
                        return _checkkeys;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
             _checkkeys._IsTrue = true ;
            return _checkkeys;
        }
    }
}