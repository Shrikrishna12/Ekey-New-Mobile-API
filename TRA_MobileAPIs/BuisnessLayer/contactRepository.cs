using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TRA_MobileAPIs.ConfigSettings;
using TRA_MobileAPIs.ResponseDetails;
using TRA_MobileAPIs.ViewModel;

namespace TRA_MobileAPIs.BuisnessLayer
{
    public class contactRepository : IcontactRepository
    {

        OrganizationService organization = new OrganizationService();
        verifiykeys _verifyKeys = new verifiykeys();
        public async Task<ContactListDetails> GetConsumerInfo(RequestParameter _reqPara)
        {
            retrieveContact _retreiveAttr = new retrieveContact();
            ContactListDetails _contactListDetails = new ContactListDetails();
            string API = "GetConsumerInfo";

            try
            {
                if (_reqPara.Source != null)
                {
                    EntityData _contactEntity = new EntityData();

                    ContactAttribute _contactAttribute = new ContactAttribute();
                    ConfigData _configInfo = ConfigEncrypt.GetCrmCredentials();
                    List<ContactAttribute> lyst = new List<ContactAttribute>();
                    _contactEntity._service = organization.GetCRMService(_configInfo);

                    if (_contactEntity._service != null)
                    {
                        checkkeyPair verifyKeys = _verifyKeys.checkKeys(_reqPara.Source, API);

                        if (verifyKeys._IsTrue == true)
                        {
                            foreach (KeyValuePair<string, string> item in _reqPara.Source)
                            {
                                _contactEntity.EntitytKey = item.Key;
                                _contactEntity.EntityValue = item.Value;

                                if (contactVM.Email == item.Key)
                                {
                                    if (item.Value != string.Empty)
                                    {
                                        _retreiveAttr.emails = item.Value;
                                    }
                                    else
                                    {
                                        _contactListDetails.Message = "Please Provide required fields";
                                        _contactListDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _contactListDetails;
                                    }
                                }
                                else if (contactVM.MSISDN == item.Key)
                                {
                                    if (item.Value != string.Empty)
                                    {
                                        string msidDn = item.Value;
                                        int _msisdn;

                                        bool _Istrue = Int32.TryParse(msidDn, out _msisdn);

                                        _retreiveAttr.msisdn = _msisdn;
                                    }
                                }
                                else if (contactVM.ICCID == item.Key)
                                {
                                    if (item.Value != string.Empty)
                                    {
                                        _retreiveAttr.iccid = item.Value;
                                    }
                                }
                                else if (contactVM.googleToken == item.Key)
                                {
                                    if (item.Value != string.Empty)
                                    {
                                        _retreiveAttr.googleToken = item.Value;
                                    }
                                }
                                else if (contactVM.FBToken == item.Key)
                                {
                                    if (item.Value != string.Empty)
                                    {
                                        _retreiveAttr.fbToken = item.Value;
                                    }
                                }
                                else if (contactVM.tweetToken == item.Key)
                                {
                                    if (item.Value != string.Empty)
                                    {
                                        _retreiveAttr.tweetToken = item.Value;
                                    }
                                }
                                else if (contactVM.IMSI == item.Key)
                                {
                                    if (item.Value != string.Empty)
                                    {
                                        string imsI = item.Value;
                                        int _imsii;

                                        bool _Istrue = Int32.TryParse(imsI, out _imsii);

                                        _retreiveAttr.imsi = _imsii;
                                    }

                                }
                            }

                            QueryExpression _queryContacts = new QueryExpression();

                            _queryContacts.EntityName = _configInfo.ContactEntity;
                            _queryContacts.ColumnSet = new ColumnSet(new string[]
                            {
                        contactVM.Firstname,
                        contactVM.Lastname,
                        contactVM.Email,
                        contactVM.Nationality,
                        contactVM.countrycode,
                        contactVM.mobile_Phone,
                        contactVM.DOB,
                        contactVM.consumerType,
                        contactVM.IDType,
                        contactVM.IDNumber,
                        contactVM.preferLang,
                        contactVM.securityAns,
                        contactVM.securityQuizz,
                        contactVM.MSISDN,
                        contactVM.ICCID,
                        contactVM.IMSI,
                        contactVM.Reg_id

                            });

                            FilterExpression _filterExpress = new FilterExpression(LogicalOperator.Or);
                            if (_retreiveAttr.emails != null)
                            {
                                _filterExpress.AddCondition(contactVM.Email, ConditionOperator.Equal, _retreiveAttr.emails);
                            }
                            if (_retreiveAttr.iccid != null)
                            {
                                _filterExpress.AddCondition(contactVM.ICCID, ConditionOperator.Equal, _retreiveAttr.iccid);
                            }
                            if (_retreiveAttr.googleToken != null)
                            {
                                _filterExpress.AddCondition(contactVM.googleToken, ConditionOperator.Equal, _retreiveAttr.googleToken);
                            }
                            if (_retreiveAttr.tweetToken != null)
                            {
                                _filterExpress.AddCondition(contactVM.tweetToken, ConditionOperator.Equal, _retreiveAttr.tweetToken);
                            }
                            if (_retreiveAttr.fbToken != null)
                            {
                                _filterExpress.AddCondition(contactVM.FBToken, ConditionOperator.Equal, _retreiveAttr.fbToken);
                            }
                            if (_retreiveAttr.msisdn != null)
                            {
                                _filterExpress.AddCondition(contactVM.MSISDN, ConditionOperator.Equal, _retreiveAttr.msisdn);
                            }
                            if (_retreiveAttr.imsi != null)
                            {
                                _filterExpress.AddCondition(contactVM.IMSI, ConditionOperator.Equal, _retreiveAttr.imsi);
                            }
                            _queryContacts.Criteria = _filterExpress;

                            EntityCollection _entities = _contactEntity._service.RetrieveMultiple(_queryContacts);

                            if (_entities != null && _entities.Entities != null && _entities.Entities.Count > 0)
                            {
                                foreach (Entity _contactEntities in _entities.Entities)
                                {

                                    ContactAttribute _contactAtt = new ContactAttribute();

                                    Guid contactid = _contactEntities.Attributes.Contains(contactVM.ContactID) ? (Guid)(_contactEntities.Attributes[contactVM.ContactID]) : Guid.Empty;
                                    _contactAtt.contactID = contactid;

                                    string FN = _contactEntities.Attributes.Contains(contactVM.Firstname) ? (string)_contactEntities.Attributes[contactVM.Firstname] : string.Empty;
                                    _contactAtt.firstname = FN;

                                    string LN = _contactEntities.Attributes.Contains(contactVM.Lastname) ? (string)_contactEntities.Attributes[contactVM.Lastname] : string.Empty;
                                    _contactAtt.lastname = LN;

                                    string Email = _contactEntities.Attributes.Contains(contactVM.Email) ? (string)_contactEntities.Attributes[contactVM.Email] : string.Empty;
                                    _contactAtt.emailaddress1 = Email;

                                    string nationality = _contactEntities.Attributes.Contains(contactVM.Nationality) ? (string)((EntityReference)_contactEntities.Attributes[contactVM.Nationality]).Name : null;
                                    _contactAtt.tra_nationality = nationality;

                                    string countryCode = _contactEntities.Attributes.Contains(contactVM.countrycode) ? (string)_contactEntities.Attributes[contactVM.countrycode] : string.Empty;
                                    _contactAtt.tra_countrycode = countryCode;

                                    string mobile = _contactEntities.Attributes.Contains(contactVM.mobile_Phone) ? (string)_contactEntities.Attributes[contactVM.mobile_Phone] : string.Empty;
                                    _contactAtt.mobilephone = mobile;


                                    DateTime? DOB = _contactEntities.Attributes.Contains(contactVM.DOB) ? (DateTime)_contactEntities.Attributes[contactVM.DOB] : (DateTime?)null;
                                    _contactAtt.birthdate = DOB;

                                    string conumerType = _contactEntities.Attributes.Contains(contactVM.consumerType) ? (string)_contactEntities.FormattedValues[contactVM.consumerType].ToString() : string.Empty;
                                    _contactAtt.tra_consumertype = conumerType;

                                    string idType = _contactEntities.Attributes.Contains(contactVM.IDType) ? (string)_contactEntities.FormattedValues[contactVM.IDType].ToString() : string.Empty;
                                    _contactAtt.tra_idtype = idType;

                                    string idnumber = _contactEntities.Attributes.Contains(contactVM.IDNumber) ? (string)_contactEntities.Attributes[contactVM.IDNumber].ToString() : string.Empty;
                                    _contactAtt.tra_idnumber = idnumber;

                                    string language = _contactEntities.Attributes.Contains(contactVM.preferLang) ? (string)_contactEntities.FormattedValues[contactVM.preferLang].ToString() : string.Empty;
                                    _contactAtt.tra_preferredlanguage = language;

                                    string quizz = _contactEntities.Attributes.Contains(contactVM.securityQuizz) ? (string)((EntityReference)_contactEntities.Attributes[contactVM.securityQuizz]).Name : null;
                                    _contactAtt.tra_securityquestion = quizz;

                                    string ans = _contactEntities.Attributes.Contains(contactVM.securityAns) ? (string)_contactEntities.Attributes[contactVM.securityAns].ToString() : string.Empty;
                                    _contactAtt.tra_securityanswer = ans;


                                    int? msisdn = _contactEntities.Attributes.Contains(contactVM.MSISDN) ? (int)_contactEntities.Attributes[contactVM.MSISDN] : (int?)null;
                                    _contactAtt.tra_msisdn = msisdn;

                                    string iccid = _contactEntities.Attributes.Contains(contactVM.ICCID) ? (string)_contactEntities.Attributes[contactVM.ICCID].ToString() : String.Empty;
                                    _contactAtt.tra_iccid = iccid;

                                    int? imsi = _contactEntities.Attributes.Contains(contactVM.IMSI) ? (int)_contactEntities.Attributes[contactVM.IMSI] : (int?)null;
                                    _contactAtt.tra_imsi = imsi;


                                    string regId = _contactEntities.Attributes.Contains(contactVM.Reg_id) ? (string)_contactEntities.Attributes[contactVM.Reg_id].ToString() : string.Empty;
                                    _contactAtt.tra_registrationid = regId;

                                    lyst.Add(_contactAtt);

                                }

                                _contactListDetails.ListContacts = lyst;
                                _contactListDetails.Status = HttpStatusCode.OK;
                                _contactListDetails.Message = "Get Contact Successfully";

                            }
                            else
                            {
                                _contactListDetails.Status = HttpStatusCode.NotFound;
                                _contactListDetails.Message = "Please provide valid Info";
                            }
                        }
                        else
                        {
                            _contactListDetails.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                            _contactListDetails.Status = HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        _contactListDetails.Status = HttpStatusCode.ServiceUnavailable;
                    }
                }
                else
                {
                    _contactListDetails.Status = HttpStatusCode.BadRequest;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return await Task.Run(() => _contactListDetails);
        }
        public async Task<ConsumerRegisterDetails> RegisterConsumers(RequestParameter _reqPara)
        {
            ConsumerRegisterDetails _consumerDetails = new ConsumerRegisterDetails();
            string API = "RegisterConsumers";
            string nationality = "";
            try
            {
                if (_reqPara.Source != null)
                {
                    ConvertHelper objC = new ConvertHelper();
                    EntityData _contactMetaData = new EntityData();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    _contactMetaData._service = organization.GetCRMService(configData);
                    LookupInfo _lookups = new LookupInfo();
                    if (_contactMetaData._service != null)
                    {

                        checkkeyPair _verifyKeyss = _verifyKeys.checkKeys(_reqPara.Source, API);

                        if (_verifyKeyss._IsTrue == true)
                        {
                            Entity Contact = new Entity(configData.ContactEntity);
                            Contact.LogicalName = configData.ContactEntity;

                            foreach (KeyValuePair<string, string> i in _reqPara.Source)
                            {
                                _contactMetaData.EntitytKey = i.Key;
                                _contactMetaData.EntityValue = i.Value;

                                if (_contactMetaData.EntitytKey == contactVM.Firstname)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }
                                }

                                else if (i.Key == contactVM.nationId)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        _lookups.entityName = NationalityLookup.tra_nationality;
                                        _lookups.statusCode = CaseLable.StateCode;
                                        _lookups.traNm = NationalityLookup.tra_name;
                                        _lookups.parentId = "tra_nationalityid";
                                        _lookups.arrayData = new string[] { NationalityLookup.tra_name,
                                        CaseLable.StateCode};

                                        Is_Status Islookup = LookupStatus(_contactMetaData, _lookups, i.Value);
                                        if (Islookup.IsActive == "Active")
                                        {
                                            nationality = Islookup._entityNm;
                                            _contactMetaData.ID = Islookup._entityId;
                                            _contactMetaData.EntitytKey = NationalityLookup.tra_nationality;
                                            _contactMetaData.parentmasterentityname = NationalityLookup.tra_nationality;
                                            _contactMetaData.masterconditionkey = NationalityLookup.tra_name;
                                        }
                                        else if (Islookup.IsActive == "Inactive")
                                        {
                                            _consumerDetails.Message = "Nationality is not Active";
                                            _consumerDetails.Status = HttpStatusCode.MethodNotAllowed;
                                            return _consumerDetails;
                                        }
                                        else if (Islookup.IsActive == string.Empty || Islookup.IsActive == null)
                                        {
                                            _consumerDetails.Message = "Invalid Nationality";
                                            _consumerDetails.Status = HttpStatusCode.NotFound;
                                            return _consumerDetails;
                                        }
                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }
                                }
                                else if (i.Key == contactVM.securityQuizzId)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        _lookups.entityName = SecurityQuizzLookUp.tra_securityquestion;
                                        _lookups.statusCode = CaseLable.StateCode;
                                        _lookups.traNm = SecurityQuizzLookUp.tra_name;
                                        _lookups.parentId = "tra_securityquestionid";
                                        _lookups.arrayData = new string[] { SecurityQuizzLookUp.tra_name,
                                        CaseLable.StateCode };

                                        Is_Status Islookup = LookupStatus(_contactMetaData, _lookups, i.Value);

                                        if (Islookup.IsActive == "Active")
                                        {
                                            _contactMetaData.ID = Islookup._entityId;
                                            _contactMetaData.EntitytKey = SecurityQuizzLookUp.tra_securityquestion;
                                            _contactMetaData.parentmasterentityname = SecurityQuizzLookUp.tra_securityquestion;
                                            _contactMetaData.masterconditionkey = SecurityQuizzLookUp.tra_name;
                                        }
                                        else if (Islookup.IsActive == "Inactive")
                                        {
                                            _consumerDetails.Message = "Security Question is not Active";
                                            _consumerDetails.Status = HttpStatusCode.OK;
                                            return _consumerDetails;
                                        }
                                        else if (Islookup.IsActive == string.Empty || Islookup.IsActive == null)
                                        {
                                            _consumerDetails.Message = "Invalid Security Question";
                                            _consumerDetails.Status = HttpStatusCode.NotFound;
                                            return _consumerDetails;
                                        }
                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.securityAns)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.source)
                                {

                                    if (i.Value != string.Empty)
                                    {
                                        bool IsOptions = pickupStatus(_contactMetaData, Contact, i.Value);

                                        if (IsOptions == true)
                                        {
                                            _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                        }
                                        else if (IsOptions == false)
                                        {
                                            _consumerDetails.Message = "Please Provide valid Source type";
                                            return _consumerDetails;
                                        }
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.countrycode)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        _contactMetaData.EntityAtrribute = i.Value;
                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.Lastname)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        _contactMetaData.EntityAtrribute = i.Value;

                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.MSISDN)
                                {
                                    _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                }

                                else if (_contactMetaData.EntitytKey == contactVM.consumerType)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        bool IsOptions = pickupStatus(_contactMetaData, Contact, i.Value);

                                        if (IsOptions == true)
                                        {
                                            _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;

                                        }
                                        else if (IsOptions == false)
                                        {
                                            _consumerDetails.Message = "Please Provide valid Consumer type";
                                            _consumerDetails.Status = HttpStatusCode.NotFound;
                                            return _consumerDetails;
                                        }
                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }

                                }
                                else if (_contactMetaData.EntitytKey == contactVM.DOB)
                                {


                                    _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;


                                }
                                else if (_contactMetaData.EntitytKey == contactVM.Email)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        string _column = contactVM.Email;

                                        bool IsDuplicate = _IsExist(_column, i.Value, Contact, _contactMetaData);

                                        if (IsDuplicate == true)
                                        {
                                            _contactMetaData.EntityAtrribute = i.Value;
                                        }
                                        else
                                        {
                                            _consumerDetails.Message = "Email Id are already exist";
                                            _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                            return _consumerDetails;
                                        }
                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.FBToken)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        string _column = contactVM.FBToken;

                                        bool IsDuplicate = _IsExist(_column, i.Value, Contact, _contactMetaData);

                                        if (IsDuplicate == true)
                                        {
                                            _contactMetaData.EntityAtrribute = i.Value;
                                        }
                                        else
                                        {
                                            _consumerDetails.Message = "Facebook token is duplicated";
                                            _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                            return _consumerDetails;
                                        }
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.tweetToken)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        string _column = contactVM.tweetToken;

                                        bool IsDuplicate = _IsExist(_column, i.Value, Contact, _contactMetaData);

                                        if (IsDuplicate == true)
                                        {
                                            _contactMetaData.EntityAtrribute = i.Value;
                                        }
                                        else
                                        {
                                            _consumerDetails.Message = "Tweeter token is duplicated";
                                            _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                            return _consumerDetails;
                                        }
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.googleToken)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        string _column = contactVM.googleToken;

                                        bool IsDuplicate = _IsExist(_column, i.Value, Contact, _contactMetaData);

                                        if (IsDuplicate == true)
                                        {

                                            _contactMetaData.EntityAtrribute = i.Value;
                                        }
                                        else
                                        {

                                            _consumerDetails.Message = "Google token is duplicated";
                                            _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                            return _consumerDetails;
                                        }
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.IMEINo)
                                {
                                    _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.IMSI)
                                {
                                    _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                }

                                else if (_contactMetaData.EntitytKey == contactVM.preferLang)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        bool IsOption = pickupStatus(_contactMetaData, Contact, i.Value);

                                        if (IsOption == true)
                                        {
                                            _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                        }
                                        else if (IsOption == false)
                                        {
                                            _consumerDetails.Message = "Please Provide valid Language type";
                                            _consumerDetails.Status = HttpStatusCode.NotFound;
                                            return _consumerDetails;
                                        }
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.mobile_Phone)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.IDType)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        if (nationality == "Kingdom of Bahrain")
                                        {
                                            _contactMetaData.EntityAtrribute = "2";
                                        }
                                        else
                                        {
                                            bool IsOptionValue = pickupStatus(_contactMetaData, Contact, i.Value);

                                            if (IsOptionValue == true)
                                            {
                                                _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                            }
                                            else if (IsOptionValue == false)
                                            {
                                                _consumerDetails.Message = "Please Provide valid ID type";
                                                _consumerDetails.Status = HttpStatusCode.NotFound;
                                                return _consumerDetails;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.ICCID)
                                {
                                    _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.IDNumber)
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        string _column = contactVM.IDNumber;

                                        bool IsDuplicate = _IsExist(_column, i.Value, Contact, _contactMetaData);

                                        if (IsDuplicate == true)
                                        {
                                            _contactMetaData.EntityAtrribute = i.Value;
                                        }
                                        else
                                        {
                                            _consumerDetails.Message = "ID Number is already exist";
                                            _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                            return _consumerDetails;
                                        }
                                    }
                                    else
                                    {
                                        _consumerDetails.Message = "Please Provide required fields";
                                        _consumerDetails.Status = HttpStatusCode.NotAcceptable;
                                        return _consumerDetails;
                                    }
                                }
                                else if (_contactMetaData.EntitytKey == contactVM.Reg_id)
                                {
                                    _contactMetaData.EntityAtrribute = _contactMetaData.EntityValue;
                                }

                                _contactMetaData._retrieveAttrRequest = new RetrieveAttributeRequest
                                {
                                    EntityLogicalName = configData.ContactEntity,
                                    LogicalName = _contactMetaData.EntitytKey,
                                    RetrieveAsIfPublished = true
                                };

                                if (_contactMetaData._retrieveAttrRequest != null)
                                {
                                    _contactMetaData._retrieveAttrResponse = (RetrieveAttributeResponse)_contactMetaData._service.Execute(_contactMetaData._retrieveAttrRequest);
                                    _contactMetaData.retrievedAttributeMetadata = (AttributeMetadata)_contactMetaData._retrieveAttrResponse.AttributeMetadata;

                                    Contact = objC.ConvertDatatype(_contactMetaData, Contact);
                                }
                            }

                            Contact.Attributes.Add("tra_iscreatedfromportal", true);

                            Contact[contactVM.spContact] = Convert.ToBoolean(false);
                            Guid ContactId = _contactMetaData._service.Create(Contact);

                            if (ContactId != null && ContactId != Guid.Empty)
                            {
                                _consumerDetails.ContactID = ContactId;
                                _consumerDetails.Message = "Contact created Successfully";
                                _consumerDetails.Status = HttpStatusCode.OK;
                            }
                            else
                            {
                                _consumerDetails.Message = "Failed to create contact";
                                _consumerDetails.Status = HttpStatusCode.NotFound;
                            }
                        }
                        else if (_verifyKeyss._IsTrue == false)
                        {

                            _consumerDetails.Message = "Please Provide " + _verifyKeyss.Keys + ",It's Mandatory Field";
                            _consumerDetails.Status = HttpStatusCode.NotFound;
                        }
                    }
                }
                else
                {
                    _consumerDetails.Message = "Please Provide valid Source";
                    _consumerDetails.Status = HttpStatusCode.NotFound;
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                _consumerDetails.Message = msg;
                _consumerDetails.Status = HttpStatusCode.NotAcceptable;

            }
            return await Task.Run(() => _consumerDetails);
        }

        public Is_Status LookupStatus(EntityData entityData, LookupInfo lookup, string i_value)
        {
            Is_Status status = new Is_Status();
            try
            {
                QueryExpression _queryNational = new QueryExpression(lookup.entityName);
                _queryNational.ColumnSet = new ColumnSet(lookup.arrayData);


                FilterExpression _filters = new FilterExpression(LogicalOperator.And);
                _filters.AddCondition(lookup.parentId, ConditionOperator.Equal, i_value);
                _queryNational.Criteria = _filters;

                EntityCollection entityCollection = entityData._service.RetrieveMultiple(_queryNational);

                if (entityCollection != null && entityCollection.Entities != null && entityCollection.Entities.Count > 0)
                {

                    foreach (Entity itemNationality in entityCollection.Entities)
                    {
                        status.IsActive = itemNationality.FormattedValues[lookup.statusCode].ToString();
                        status._entityId = (Guid)(itemNationality.Attributes[lookup.parentId]);
                        status._entityNm = itemNationality.Attributes["tra_name"].ToString();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return status;
        }

        public bool _IsExist(string _coloumn, string values, Entity _entity, EntityData entityData)
        {
            try
            {
                QueryExpression _qfb = new QueryExpression(_entity.LogicalName);
                _qfb.ColumnSet = new ColumnSet(_coloumn);
                FilterExpression _filterFb = new FilterExpression(LogicalOperator.And);
                _filterFb.AddCondition(_coloumn, ConditionOperator.Equal, values);

                _qfb.Criteria = _filterFb;

                EntityCollection _collection = entityData._service.RetrieveMultiple(_qfb);

                if (_collection != null && _collection.Entities != null && _collection.Entities.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool pickupStatus(EntityData entityData, Entity _entity, string i)
        {
            try
            {
                List<string> lookupData = new List<string>();
                entityData._retrieveAttrRequest = new RetrieveAttributeRequest
                {
                    EntityLogicalName = _entity.LogicalName,
                    LogicalName = entityData.EntitytKey,
                    RetrieveAsIfPublished = true
                };

                if (entityData._retrieveAttrRequest != null)
                {
                    entityData._retrieveAttrResponse = (RetrieveAttributeResponse)entityData._service.Execute(entityData._retrieveAttrRequest);
                    entityData.retrievedAttributeMetadata = (AttributeMetadata)entityData._retrieveAttrResponse.AttributeMetadata;
                }

                EnumAttributeMetadata data = (EnumAttributeMetadata)entityData.retrievedAttributeMetadata;
                foreach (OptionMetadata picklist in data.OptionSet.Options)
                {
                    string optionsetText = picklist.Value.ToString();
                    lookupData.Add(optionsetText);
                }

                var OptionInfo = lookupData.Contains(i);

                if (OptionInfo == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<UpdateConsumerDetails> updateConsumerInformation(RequestParameter _reqPara)
        {
            UpdateConsumerDetails _updateDetails = new UpdateConsumerDetails();
            string API = "UpdateConsumer";
            try
            {
                if (_reqPara.Source != null)
                {

                    EntityData _contatdata = new EntityData();
                    ConvertHelper objC = new ConvertHelper();
                    EntityData _contactMetaData = new EntityData();
                    updateContact _updateCont = new updateContact();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    _contactMetaData._service = organization.GetCRMService(configData);

                    if (_contactMetaData._service != null)
                    {
                        checkkeyPair _verifyKeyss = _verifyKeys.checkKeys(_reqPara.Source, API);

                        if (_verifyKeyss._IsTrue == true)
                        {
                            string contactId = _reqPara.Source[contactVM.ContactID].ToString();
                            if (contactId != string.Empty && contactId != null)
                            {
                                Guid contactid = Guid.Parse(contactId);
                                _reqPara.Source.Remove(contactVM.ContactID);

                                if (contactid != Guid.Empty && contactid != null)
                                {
                                    Entity Contact = new Entity(configData.ContactEntity, contactid);
                                    Contact.LogicalName = configData.ContactEntity;

                                    QueryExpression _qContact = new QueryExpression(Contact.LogicalName);
                                    FilterExpression _fupdates = new FilterExpression(LogicalOperator.And);
                                    _fupdates.AddCondition(contactVM.ContactID, ConditionOperator.Equal, contactid);
                                    _qContact.Criteria = _fupdates;

                                    EntityCollection updatesEntity = _contactMetaData._service.RetrieveMultiple(_qContact);

                                    if (updatesEntity != null && updatesEntity.Entities != null && updatesEntity.Entities.Count > 0)
                                    {
                                        foreach (KeyValuePair<string, string> item in _reqPara.Source)
                                        {
                                            _contatdata.EntitytKey = item.Key;
                                            _contatdata.EntityValue = item.Value;

                                            if (contactVM.Firstname == item.Key)
                                            {
                                                if (item.Value != string.Empty)
                                                {
                                                    _updateCont._fname = item.Value;
                                                }
                                                else
                                                {
                                                    _updateDetails.Message = "Please Provide required fields";
                                                    _updateDetails.Status = HttpStatusCode.NotAcceptable;
                                                    return await Task.Run(() => _updateDetails);
                                                }
                                            }
                                            else
                                            if (contactVM.Lastname == item.Key)
                                            {
                                                if (item.Value != string.Empty)
                                                {
                                                    _updateCont._lname = item.Value;
                                                }
                                                else
                                                {
                                                    _updateDetails.Message = "Please Provide required fields";
                                                    _updateDetails.Status = HttpStatusCode.NotAcceptable;
                                                    return await Task.Run(() => _updateDetails);
                                                }
                                            }
                                            else
                                            if (contactVM.countrycode == item.Key)
                                            {
                                                if (item.Value != string.Empty)
                                                {
                                                    _updateCont.countrycode = item.Value;
                                                }
                                                else
                                                {
                                                    _updateDetails.Message = "Please Provide required fields";
                                                    _updateDetails.Status = HttpStatusCode.NotAcceptable;
                                                    return await Task.Run(() => _updateDetails);
                                                }
                                            }
                                            else
                                            if (contactVM.Email == _contatdata.EntitytKey)
                                            {
                                                if (item.Value != string.Empty)
                                                {
                                                    IsEmailExist IsExists = EmailCheck(Contact, _contactMetaData, item.Value, contactid);

                                                    if (IsExists.IsExistEmail == true && IsExists.email == item.Value && IsExists.contactId == contactid && IsExists.email != null)
                                                    {
                                                        _updateCont.email = _contatdata.EntityValue;
                                                    }
                                                    else
                                                    {
                                                        string _column = contactVM.Email;

                                                        bool IsDuplicate = _IsExist(_column, item.Value, Contact, _contactMetaData);

                                                        if (IsDuplicate == true)
                                                        {
                                                            _updateCont.email = _contatdata.EntityValue;

                                                        }
                                                        else
                                                        {
                                                            _updateDetails.Message = "Email Id are already exist";
                                                            _updateDetails.Status = HttpStatusCode.NotAcceptable;
                                                            return await Task.Run(() => _updateDetails);

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    _updateDetails.Message = "Please Provide required fields";
                                                    _updateDetails.Status = HttpStatusCode.NotAcceptable;
                                                    return await Task.Run(() => _updateDetails);
                                                }
                                            }
                                            else
                                            if (contactVM.mobile_Phone == item.Key)
                                            {
                                                if (item.Value != string.Empty)
                                                {
                                                    _updateCont.mobphone = item.Value;
                                                }
                                                else
                                                {
                                                    _updateDetails.Message = "Please Provide required fields";
                                                    _updateDetails.Status = HttpStatusCode.NotAcceptable;
                                                    return await Task.Run(() => _updateDetails);
                                                }
                                            }
                                        }
                                        if (_updateCont._fname != null)
                                        {
                                            Contact[contactVM.Firstname] = _updateCont._fname;
                                        }
                                        if (_updateCont._lname != null)
                                        {
                                            Contact[contactVM.Lastname] = _updateCont._lname;
                                        }
                                        if (_updateCont.countrycode != null)
                                        {
                                            Contact[contactVM.countrycode] = _updateCont.countrycode;
                                        }
                                        if (_updateCont.mobphone != null)
                                        {
                                            Contact[contactVM.mobile_Phone] = _updateCont.mobphone;
                                        }
                                        if (_updateCont.email != null)
                                        {
                                            Contact[contactVM.Email] = _updateCont.email;
                                        }
                                        _contactMetaData._service.Update(Contact);
                                        _updateDetails.Status = HttpStatusCode.OK;
                                        _updateDetails.Message = "Update Contact Successfully";
                                    }
                                    else
                                    {
                                        _updateDetails.Status = HttpStatusCode.NotFound;
                                        _updateDetails.Message = "Please enter valid contact Id";
                                    }
                                }
                            }
                            else
                            {
                                _updateDetails.Status = HttpStatusCode.NotAcceptable;
                                _updateDetails.Message = "Please Provide Contact Id";
                            }
                        }
                        else
                        {
                            _updateDetails.Message = "Please Provide " + _verifyKeyss.Keys + ",It's Mandatory Field";
                            _updateDetails.Status = HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        _updateDetails.Status = HttpStatusCode.ServiceUnavailable;
                    }
                }
                else
                {
                    _updateDetails.Status = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                _updateDetails.Message = msg;
                _updateDetails.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => _updateDetails);
        }

        public IsEmailExist EmailCheck(Entity entity, EntityData _entityInfo, string i, Guid contactId)
        {
            IsEmailExist _emailExist = new IsEmailExist();
            try
            {
                QueryExpression _qEmail = new QueryExpression(entity.LogicalName);
                _qEmail.ColumnSet = new ColumnSet(contactVM.Email);
                FilterExpression filters = new FilterExpression(LogicalOperator.And);
                filters.AddCondition(contactVM.ContactID, ConditionOperator.Equal, contactId);
                filters.AddCondition(contactVM.Email, ConditionOperator.Equal, i);
                _qEmail.Criteria = filters;

                EntityCollection entity1 = _entityInfo._service.RetrieveMultiple(_qEmail);

                if (entity1 != null && entity1.Entities != null && entity1.Entities.Count > 0)
                {
                    foreach (Entity _emailItem in entity1.Entities)
                    {
                        _emailExist.IsExistEmail = true;
                        _emailExist.email = _emailItem.Attributes[contactVM.Email].ToString();
                        _emailExist.contactId = (Guid)(_emailItem.Attributes[contactVM.ContactID]);
                    }
                }
                else
                {
                    _emailExist.email = null;
                    _emailExist.IsExistEmail = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _emailExist;
        }

        public async Task<NationalityDetails> getNations()
        {
            EntityData _contatdata = new EntityData();
            NationalityDetails _getNationData = new NationalityDetails();
            List<GetNationality> lystNations = new List<GetNationality>();
            try
            {
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                _contatdata._service = organization.GetCRMService(configData);
                if (_contatdata._service != null)
                {
                    QueryExpression _queryData = new QueryExpression(contactVM.Nationality);
                    _queryData.ColumnSet = new ColumnSet("tra_name", "tra_nationalityar");
                    FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                    _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryData.Criteria = _filterData;
                    EntityCollection _entityNation = _contatdata._service.RetrieveMultiple(_queryData);
                    if (_entityNation != null && _entityNation.Entities != null && _entityNation.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityNation.Entities)
                        {
                            GetNationality _getNation = new GetNationality();
                            _getNation.tra_name = item.Contains(CaseLable.srvcNm) ? (string)item.Attributes[CaseLable.srvcNm] : string.Empty;
                            _getNation.nationalityAr = item.Contains("tra_nationalityar") ? (string)item.Attributes["tra_nationalityar"] : string.Empty;
                            _getNation.tra_nationalityid = item.Contains(contactVM.nationId) ? (Guid)item.Attributes[contactVM.nationId] : Guid.Empty;
                            lystNations.Add(_getNation);
                        }

                    }

                    _getNationData.Status = HttpStatusCode.OK;
                    _getNationData.Message = "Succssfully get nationality";
                    _getNationData._NationalityList = lystNations;

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return await Task.Run(() => _getNationData);
        }
    }
}