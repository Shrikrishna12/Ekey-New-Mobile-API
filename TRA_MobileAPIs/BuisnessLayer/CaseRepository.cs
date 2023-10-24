using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TRA_MobileAPIs.ConfigSettings;
using TRA_MobileAPIs.ProxyClasses;
using TRA_MobileAPIs.ResponseDetails;
using TRA_MobileAPIs.ViewModel;

namespace TRA_MobileAPIs.BuisnessLayer
{
    public class CaseRepository:ICaseRepository
    {
        Boolean IsDisputeNo;
        OrganizationService organization = new OrganizationService();
        LookupInfo _lookups = new LookupInfo();
        verifiykeys _verifyKeys = new verifiykeys();
        Guid _Id = Guid.Empty;


        public async Task<ComplaintCase> CreateComplaint(RequestParameter _reqPara)
        {
            ComplaintCase _complaintRes = new ComplaintCase();
            string API = "CreateComplaint";
            try
            {
                if (_reqPara.Source != null)
                {

                    EntityData _caseMetaData = new EntityData();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    _caseMetaData._service = organization.GetCRMService(configData);
                    string caseNum = "";

                    if (_caseMetaData._service != null)
                    {
                        checkkeyPair verifyKeys = _verifyKeys.checkKeys(_reqPara.Source, API);

                        if (verifyKeys._IsTrue == true)
                        {
                            Incident Case = new Incident();
                            // Case.LogicalName = configData.caseEntity;

                            _complaintRes = IterateForComplaint(_reqPara, _caseMetaData, configData, ref _complaintRes, ref Case);
                            ///Code removed from here
                            //Case=(Incident)Case;
                            Guid CaseId = _caseMetaData._service.Create(Case);
                            if (CaseId != Guid.Empty)
                            {
                                Entity createdCase = _caseMetaData._service.Retrieve(configData.caseEntity, CaseId, new ColumnSet("ticketnumber"));
                                if (createdCase != null)
                                {
                                    caseNum = createdCase.Attributes["ticketnumber"].ToString();
                                }

                            }
                            if (caseNum != null && caseNum != string.Empty)
                            {
                                _complaintRes.Message = "Complaint created Successfully";
                                _complaintRes.Status = HttpStatusCode.OK;
                                _complaintRes.CaseNo = caseNum;
                                _complaintRes.caseGuid = CaseId;
                            }
                            else
                            {
                                _complaintRes.Message = "Failed to create Case";
                                _complaintRes.Status = HttpStatusCode.NotFound;
                            }

                        }
                        else
                        {
                            _complaintRes.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                            _complaintRes.Status = HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        _complaintRes.Status = HttpStatusCode.ServiceUnavailable;
                    }
                }
                else
                {
                    _complaintRes.Status = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                _complaintRes.Message = msg;
                _complaintRes.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => _complaintRes);
        }
        public ComplaintCase IterateForComplaint(RequestParameter _reqPara, EntityData _caseMetaData, ConfigData configData, ref ComplaintCase _complaintRes, ref Incident Case)
        {
            ConvertHelper objC = new ConvertHelper();
            foreach (KeyValuePair<string, string> i in _reqPara.Source)
            {
                _caseMetaData.EntitytKey = i.Key;
                _caseMetaData.EntityValue = i.Value;
                string fullname = "";

                if (i.Key == contactVM.ContactID)
                {
                    _caseMetaData.EntitytKey = "customerid";
                    _caseMetaData.EntityValue = i.Value;
                }
                else
                {
                    _caseMetaData.EntitytKey = i.Key;
                    _caseMetaData.EntityValue = i.Value;
                }
                if (i.Key == contactVM.ContactID)
                {
                    if (i.Value != string.Empty)
                    {
                        string guid = _caseMetaData.EntityValue;
                        Guid contactId = Guid.Parse(guid);

                        Entity Existing_contact = _caseMetaData._service.Retrieve(configData.ContactEntity, contactId, new ColumnSet(ContactLookup.contact_Name));
                        if (Existing_contact != null)
                        {
                            Case[_caseMetaData.EntitytKey] = new EntityReference(ContactLookup.contactEntity, contactId);//tra_nationality
                        }
                        else
                        {
                            _complaintRes.Message = "Contact Not found";
                            _complaintRes.Status = HttpStatusCode.NotFound;

                        }
                    }
                    else
                    {
                        _complaintRes.Message = "Please Provide required fields";
                        _complaintRes.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (_caseMetaData.EntitytKey == CaseVM.caseType)//optionset
                {
                    if (i.Value != string.Empty)
                    {
                        if (i.Value == "1")
                        {
                            Case[_caseMetaData.EntitytKey] = new OptionSetValue(Convert.ToInt32(i.Value));
                        }
                        else
                        {
                            _complaintRes.Message = "Please Select the Complaint Case Type";
                            _complaintRes.Status = HttpStatusCode.NotAcceptable;

                        }
                    }
                    else
                    {
                        _complaintRes.Message = "Please Provide required fields";
                        _complaintRes.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (i.Key == CaseVM.complaintTypeId)//lookup
                {
                    if (i.Value != string.Empty)
                    {

                        string compId = i.Value;
                        Guid id = Guid.Parse(compId);
                        _caseMetaData.EntitytKey = CaseVM.complaintType;
                        Case[CaseVM.complaintType] = new EntityReference(ComplaintLookup.ComplaintType, id);//tra_nationality

                    }
                    else
                    {
                        _complaintRes.Message = "Please Provide required fields";
                        _complaintRes.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (i.Key == CaseVM.complaintSubTypeId)//lookup
                {
                    if (i.Value != string.Empty)
                    {

                        string comptypeId = i.Value;
                        Guid id1 = Guid.Parse(comptypeId);
                        Case[ComplaintsubTypeLookup.complaintSubType] = new EntityReference(ComplaintsubTypeLookup.complaintSubType, id1);//tra_nationality
                    }

                }
                else if (_caseMetaData.EntitytKey == CaseVM.DisputeAccno)//string
                {
                    Case[_caseMetaData.EntitytKey] = _caseMetaData.EntityValue;
                }
                else if (_caseMetaData.EntitytKey == CaseVM.ownerDisAcc)//two option
                {
                    if (i.Value != string.Empty)
                    {
                        if (i.Value == "true")
                        {
                            Case[_caseMetaData.EntitytKey] = true;
                        }
                        else
                        {
                            Case[_caseMetaData.EntitytKey] = false;
                        }
                    }
                }
                else if (_caseMetaData.EntitytKey == CaseVM.cprNoOwner)//string
                {
                    if (IsDisputeNo == false)
                    {
                        if (i.Value != string.Empty)
                        {
                            Case[_caseMetaData.EntitytKey] = _caseMetaData.EntityValue;
                        }
                        else
                        {
                            _complaintRes.Message = "Please Provide required fields";
                            _complaintRes.Status = HttpStatusCode.NotAcceptable;

                        }

                    }
                    else if (IsDisputeNo == true)
                    {
                        Case[_caseMetaData.EntitytKey] = string.Empty;
                    }

                }
                else if (_caseMetaData.EntitytKey == CaseVM.onweName)
                {
                    if (IsDisputeNo == false)
                    {
                        if (i.Value != string.Empty)
                        {
                            Case[_caseMetaData.EntitytKey] = i.Value;
                        }
                        else
                        {
                            _complaintRes.Message = "Please Provide required fields";
                            _complaintRes.Status = HttpStatusCode.NotAcceptable;

                        }

                    }
                    else if (IsDisputeNo == true)
                    {
                        Case[_caseMetaData.EntitytKey] = string.Empty;
                    }

                }
                else if (i.Key == CaseVM.srvcprdId)//lookup
                {
                    if (i.Value != string.Empty)
                    {

                        string srvcprdId = i.Value;
                        Guid id12 = Guid.Parse(srvcprdId);
                        Case[ServiceProviderLookup.serviceProvider] = new EntityReference(ServiceProviderLookup.serviceProvider, id12);


                    }
                    else
                    {
                        _complaintRes.Message = "Please Provide required fields";
                        _complaintRes.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (i.Key == CaseVM.srvcId)//lookup
                {
                    if (i.Value != string.Empty)
                    {
                        string srvcId = i.Value;
                        Guid id13 = Guid.Parse(srvcId);
                        Case[servicLookup.Servicee] = new EntityReference(servicLookup.Servicee, id13);
                    }
                    else
                    {
                        _complaintRes.Message = "Please Provide required fields";
                        _complaintRes.Status = HttpStatusCode.NotAcceptable;
                    }
                }
                else if (_caseMetaData.EntitytKey == CaseVM.serviceProviderRef)//string
                {
                    //_caseMetaData.EntityAtrribute = _caseMetaData.EntityValue;
                    Case[i.Key] = i.Value;
                }
                else if (i.Key == CaseVM.srvcSubId)//lookup
                {
                    if (i.Value != string.Empty)
                    {
                        string srvcsubId = i.Value;
                        Guid id14 = Guid.Parse(srvcsubId);
                        Case[serviceTypeLookup.serviceSubType] = new EntityReference(serviceTypeLookup.serviceSubType, id14);

                    }
                }
                else if (_caseMetaData.EntitytKey == CaseVM.dateofcomplaintSP)//dob
                {
                    //Case[i.Key] = Convert.ToDateTime(i.Value);
                    string dateComplaintSP = i.Value;
                    DateTime dateSP = DateTime.ParseExact(dateComplaintSP, "dd/MM/yyyy", null);
                    Case[i.Key] = dateSP;
                }
                else if (_caseMetaData.EntitytKey == CaseVM.caseQ1)
                {
                    if (i.Value != string.Empty)
                    {
                        Case[i.Key] = i.Value;
                    }
                    else
                    {
                        _complaintRes.Message = "Please Provide required fields";
                        _complaintRes.Status = HttpStatusCode.NotAcceptable;

                    }

                }
                else if (_caseMetaData.EntitytKey == CaseVM.caseQ2)
                {
                    if (i.Value != string.Empty)
                    {
                        Case[i.Key] = i.Value;
                    }
                    else
                    {
                        _complaintRes.Message = "Please Provide required fields";
                        _complaintRes.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (_caseMetaData.EntitytKey == CaseVM.caseQ3)
                {
                    if (i.Value != string.Empty)
                    {
                        Case[i.Key] = i.Value;
                    }
                    else
                    {
                        _complaintRes.Message = "Please Provide required fields";
                        _complaintRes.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (_caseMetaData.EntitytKey == CaseVM.origin)//option set
                {
                    if (i.Value != string.Empty)
                    {
                        try
                        {
                            Case[_caseMetaData.EntitytKey] = new OptionSetValue(Convert.ToInt32(i.Value));

                        }
                        catch (Exception ee)
                        {
                            _complaintRes.Message = "Please Provide valid Origin type" + ee;
                            _complaintRes.Status = HttpStatusCode.NotFound;

                        }
                    }
                    else
                    {
                        _complaintRes.Message = "Please Provide required fields";
                        _complaintRes.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (_caseMetaData.EntitytKey == CaseVM.subscriptionType)//option set
                {
                    if (i.Value != string.Empty)
                    {
                        try
                        {
                            Case[_caseMetaData.EntitytKey] = new OptionSetValue(Convert.ToInt32(i.Value));

                        }
                        catch (Exception ee)
                        {
                            _complaintRes.Message = "Please Provide valid Subscription type" + ee;
                            _complaintRes.Status = HttpStatusCode.NotFound;

                        }

                    }
                    else
                    {
                        _complaintRes.Message = "Please Provide required fields";
                        _complaintRes.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (_caseMetaData.EntitytKey == CaseVM.discription)
                {
                    //_caseMetaData.EntityAtrribute = _caseMetaData.EntityValue;
                    Case[i.Key] = i.Value;
                }

            }
            return _complaintRes;
        }

        public async Task<ConversationDetails> createConversations(RequestAttachments _request)
        {
            ConversationDetails _conversationDetails = new ConversationDetails();
            string API = "createConversations";
            try
            {           
                EntityData _conversionMetaData = new EntityData();
                string ticketNo = "";
                if (_request.Source != null)
                {

                    // if (_request.Source.attachments.Count <= 4)
                    //{
                        ConvertHelper objC = new ConvertHelper();
                        ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                        _conversionMetaData._service = organization.GetCRMService(configData);
                        if (_conversionMetaData._service != null)
                        {
                            checkkeyPair verifyKeys = _verifyKeys.checkKeys(_request.Source.data, API);

                            if (verifyKeys._IsTrue == true)
                            {
                                Entity _conversionEntity = new Entity(configData.converstionEnity);
                                foreach (KeyValuePair<string, string> conversionkeyValue in _request.Source.data)
                                {
                                    if (conversionkeyValue.Key == "caseID")
                                    {
                                        _conversionMetaData.EntitytKey = "tra_caseid";
                                        _conversionMetaData.EntityValue = conversionkeyValue.Value;
                                    }
                                    else
                                    {
                                        _conversionMetaData.EntitytKey = conversionkeyValue.Key;
                                        _conversionMetaData.EntityValue = conversionkeyValue.Value;
                                    }
                                    if (CaseLable._caseID == conversionkeyValue.Key)
                                    {
                                        if (conversionkeyValue.Value != string.Empty)
                                        {
                                            string caseId = conversionkeyValue.Value;
                                            Guid _incidentId = Guid.Parse(caseId);

                                            if (_incidentId != Guid.Empty)
                                            {
                                                QueryExpression _query = new QueryExpression(configData.caseEntity);
                                                _query.ColumnSet = new ColumnSet(CaseLable.ticket);
                                                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                                                filter.AddCondition(CaseLable.caseId, ConditionOperator.Equal, _incidentId);
                                                _query.Criteria = filter;
                                                EntityCollection caseCollection = _conversionMetaData._service.RetrieveMultiple(_query);

                                                if (caseCollection != null && caseCollection.Entities != null && caseCollection.Entities.Count > 0)
                                                {
                                                    foreach (Entity itemIncident in caseCollection.Entities)
                                                    {
                                                        ticketNo = itemIncident.Attributes[CaseLable.ticket].ToString();
                                                        _Id = (Guid)(itemIncident.Attributes[CaseLable.caseId]);
                                                    }
                                                    if (_incidentId == _Id)
                                                    {
                                                        _conversionMetaData.ID = _Id;


                                                        _conversionMetaData.parentmasterentityname = incidentLookpup.incidentEntity;
                                                        _conversionMetaData.masterconditionkey = incidentLookpup.incidentNm;

                                                        _conversionEntity[_conversionMetaData.EntitytKey] = new EntityReference(_conversionMetaData.parentmasterentityname, _conversionMetaData.ID);
                                                    }
                                                    else
                                                    {
                                                        _conversationDetails.Message = "Case ID or Case No Not found";
                                                        _conversationDetails.Status = HttpStatusCode.NotFound;
                                                        return _conversationDetails;
                                                    }
                                                }
                                                else
                                                {
                                                    _conversationDetails.Message = "Case ID or Case No Not found";
                                                    _conversationDetails.Status = HttpStatusCode.NotFound;
                                                    return _conversationDetails;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _conversationDetails.Message = "Please Enter Incident or Case Id";
                                            _conversationDetails.Status = HttpStatusCode.NotAcceptable;
                                            return _conversationDetails;
                                        }
                                    }
                                    else
                                    if (CaseLable.comments == _conversionMetaData.EntitytKey)
                                    {
                                        if (conversionkeyValue.Value != string.Empty)
                                        {
                                            _conversionEntity[_conversionMetaData.EntitytKey] = _conversionMetaData.EntityValue;

                                        }
                                        else
                                        {
                                            _conversationDetails.Message = "Please Provide required fields";
                                            _conversationDetails.Status = HttpStatusCode.NotAcceptable;
                                            return _conversationDetails;
                                        }
                                    }
                                }
                               _conversionEntity.Attributes.Add("tra_commentby", new OptionSetValue(1));
                               _conversionEntity[CaseLable.traConsumer] = Convert.ToBoolean(true);
                                 Guid conversionId = _conversionMetaData._service.Create(_conversionEntity);
                                if (conversionId != null && conversionId != Guid.Empty)
                                {

                                    EntityData _annotMetaData = new EntityData();
                                    List<AnnotationData> _lystAnnotation = new List<AnnotationData>();
                                    List<Entity> AnnotationEntityList = new List<Entity>();


                                    int i = 1;

                                    int fileCount =Convert.ToInt32(configData.files);
                                    for (int m = 0; m < fileCount; m++)
                                    {

                                        AnnotationData _annotFilesItem = new AnnotationData();
                                        if (_request.Source.attachments.Count > m)
                                        {
                                            _annotFilesItem.DocumentBody = _request.Source.attachments[m][$"documentbody{i}"];


                                            if (_annotFilesItem.DocumentBody == string.Empty)
                                            {
                                                _conversationDetails.Status = HttpStatusCode.NotAcceptable;
                                                _conversationDetails.Message = $"please provide documentbody{i}";
                                                return _conversationDetails;
                                            }
                                            _annotFilesItem.fileName = _request.Source.attachments[m][$"filename{i}"];
                                            if (_annotFilesItem.fileName == string.Empty)
                                            {
                                                _conversationDetails.Status = HttpStatusCode.NotAcceptable;
                                                _conversationDetails.Message = $"Please provide filename{i}";
                                                return _conversationDetails;

                                            }
                                            _lystAnnotation.Add(_annotFilesItem);
                                            i++;
                                        }
                                        
                                    }
                                  

                                    if (_lystAnnotation != null && _lystAnnotation.Count > 0)
                                    {

                                        for (int j = 0; j < _lystAnnotation.Count; j++)
                                        {
                                            Entity annotationEntity = new Entity(configData.annotation);
                                            annotationEntity[Annotation.documents] = _lystAnnotation[j].DocumentBody;
                                            annotationEntity[Annotation.fileName] = _lystAnnotation[j].fileName;
                                            AnnotationEntityList.Add(annotationEntity);
                                        }

                                        if (AnnotationEntityList != null && AnnotationEntityList.Count > 0)
                                        {
                                            int m = 0;
                                            foreach (var dataAttachment in AnnotationEntityList)
                                            {

                                                AnnotationEntityList[m].Attributes.Add("objectid", new EntityReference(configData.converstionEnity, conversionId));
                                                _conversionMetaData._service.Create(AnnotationEntityList[m]);
                                                m++;
                                            }

                                            _conversationDetails.Status = HttpStatusCode.OK;
                                            _conversationDetails.Message = "Conversation created with Attachment";
                                            return _conversationDetails;

                                        }
                                    }

                                }

                                if (conversionId != null && conversionId != Guid.Empty)
                                {
                                    _conversationDetails.Status = HttpStatusCode.OK;
                                    _conversationDetails.Message = "Conversation created without Attachment";
                                }
                                else
                                {
                                    _conversationDetails.Status = HttpStatusCode.BadRequest;
                                    _conversationDetails.Message = "Failed To create";
                                }
                            }
                            else
                            {
                                _conversationDetails.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                                _conversationDetails.Status = HttpStatusCode.NotFound;
                            }
                        }

                        // }
                        //else
                        //{ 
                        //    _conversationDetails.Status = HttpStatusCode.BadRequest;
                        //    _conversationDetails.Message = "only 4 attachment required";
                    //}
                    //}
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                _conversationDetails.Message = msg;
                _conversationDetails.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => _conversationDetails);
        }  

        public async Task<EnquiryDetails> createEnquiry(RequestParameter request)
        {
            EnquiryDetails _enquiryDetails = new EnquiryDetails();
            string API = "createEnquiry";
            try
            {
                if (request.Source != null )
                {
                    ConvertHelper objC = new ConvertHelper();
                    EntityData _caseMetaData = new EntityData();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
       
                    _caseMetaData._service = organization.GetCRMService(configData);

                    if (_caseMetaData._service != null)
                    {
                      checkkeyPair verifyKeys = _verifyKeys.checkKeys(request.Source, API);
                        if(verifyKeys._IsTrue==true)
                        {
                            Incident _caseEntity = new Incident();
                            _enquiryDetails = IterateForEnquiry(request, _caseMetaData, configData, ref _enquiryDetails, ref _caseEntity);

                            Guid CaseId = _caseMetaData._service.Create(_caseEntity);
                            string caseNumber = "";
                            if (CaseId != Guid.Empty)
                            {
                                Entity createdCase = _caseMetaData._service.Retrieve(configData.caseEntity, CaseId, new ColumnSet("ticketnumber"));
                                if (createdCase != null)
                                {
                                    caseNumber = createdCase.Attributes["ticketnumber"].ToString();
                                }

                            }
                            if (caseNumber != null&&caseNumber!=string.Empty)
                            {
                                _enquiryDetails.Status = HttpStatusCode.OK;
                                _enquiryDetails.Message = "Case Enquiry created Successfully";
                                _enquiryDetails.CaseID = caseNumber;
                                _enquiryDetails.caseGuid = CaseId;
                            }
                            else
                            {
                                _enquiryDetails.Status = HttpStatusCode.NotFound;
                                _enquiryDetails.Message = "Case Enquiry failed to create";
                            }
                        }
                        else
                        {
                            _enquiryDetails.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                            _enquiryDetails.Status = HttpStatusCode.NotFound;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                _enquiryDetails.Message = msg;
                _enquiryDetails.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(()=>_enquiryDetails);
        }

        public EnquiryDetails IterateForEnquiry (RequestParameter request, EntityData _caseMetaData, ConfigData configData, ref EnquiryDetails _enquiryDetails, ref Incident _caseEntity)
        {
            ConvertHelper objC = new ConvertHelper();
            foreach (KeyValuePair<string, string> item in request.Source)
            {
                if (item.Key == contactVM.ContactID)
                {
                    _caseMetaData.EntitytKey = "customerid";
                    _caseMetaData.EntityValue = item.Value;
                }
                else
                {
                    _caseMetaData.EntitytKey = item.Key;
                    _caseMetaData.EntityValue = item.Value;
                }
                string fullname = "";
                if (item.Key == contactVM.ContactID)
                {
                    if (item.Value != string.Empty)
                    {
                        string guid = _caseMetaData.EntityValue;
                        Guid contactId = Guid.Parse(guid);

                        Entity Existing_contact = _caseMetaData._service.Retrieve(configData.ContactEntity, contactId, new ColumnSet(ContactLookup.contact_Name));
                        if (Existing_contact != null)
                        {
                            _caseEntity[_caseMetaData.EntitytKey] = new EntityReference(ContactLookup.contactEntity, contactId);//tra_nationality
                        }
                        else
                        {
                            _enquiryDetails.Message = "Contact Not found";
                            _enquiryDetails.Status = HttpStatusCode.NotFound;

                        }
                    }
                    else
                    {
                        _enquiryDetails.Message = "Please Provide required fields";
                        _enquiryDetails.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (_caseMetaData.EntitytKey == CaseLable.CaseType)
                {
                    if (item.Value != string.Empty)
                    {
                        if (item.Value == "2")
                        {
                            _caseEntity[_caseMetaData.EntitytKey] = new OptionSetValue(Convert.ToInt32(item.Value));
                        }
                        else
                        {
                            _enquiryDetails.Message = "Please Select the Enquiry Case Type";
                            _enquiryDetails.Status = HttpStatusCode.NotAcceptable;

                        }
                    }
                    else
                    {
                        _enquiryDetails.Message = "Please Provide required fields";
                        _enquiryDetails.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (item.Key == CaseLable.enquirytyprid)
                {
                    if (item.Value != string.Empty)
                    {

                        string EnqID = item.Value;
                        Guid id = Guid.Parse(EnqID);
                        _caseMetaData.EntitytKey = CaseVM.complaintType;
                        _caseEntity[CaseLable.EnquiryType] = new EntityReference(EnquiryTypeLookup.EnquiryType, id);

                    }
                    else
                    {
                        _enquiryDetails.Message = "Please fill the required field";
                        _enquiryDetails.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (_caseMetaData.EntitytKey == CaseLable.origin)
                {
                    if (item.Value != string.Empty)
                    {
                        try
                        {
                            _caseEntity[_caseMetaData.EntitytKey] = new OptionSetValue(Convert.ToInt32(item.Value));

                        }
                        catch (Exception ee)
                        {
                            _enquiryDetails.Message = "Please Provide valid Origin type" + ee;
                            _enquiryDetails.Status = HttpStatusCode.NotFound;

                        }
                    }
                    else
                    {
                        _enquiryDetails.Message = "Please Provide required fields";
                        _enquiryDetails.Status = HttpStatusCode.NotAcceptable;

                    }
                }
                else if (_caseMetaData.EntitytKey == CaseLable.description)
                {
                    _caseEntity[item.Key] = item.Value;
                }

            }
            return _enquiryDetails;
        }

        public async Task<SuggestionCase> CreateSuggestion(RequestParameter _reqPara)
        {
            SuggestionCase _suggestionRes = new SuggestionCase();
            string API = "CreateSuggestion";
            try
            {
                if (_reqPara.Source != null)
                {
                    ConvertHelper objC = new ConvertHelper();
                    EntityData _caseMetaData = new EntityData();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    _caseMetaData._service = organization.GetCRMService(configData);

                    string caseNumber = "";

                    if (_caseMetaData._service != null)
                    {
                        checkkeyPair verifyKeys = _verifyKeys.checkKeys(_reqPara.Source, API);
                        if (verifyKeys._IsTrue == true)
                        {
                            Entity Case = new Entity(configData.caseEntity);
                            Case.LogicalName = configData.caseEntity;
                            foreach (KeyValuePair<string, string> i in _reqPara.Source)
                            {
                                string fullname = "";
                                if (i.Key == "contactid")
                                {
                                    _caseMetaData.EntitytKey = "customerid";
                                    _caseMetaData.EntityValue = i.Value;
                                }
                                else
                                {
                                    _caseMetaData.EntitytKey = i.Key;
                                    _caseMetaData.EntityValue = i.Value;
                                }
                                if (_caseMetaData.EntitytKey == "customerid")
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        string guid = _caseMetaData.EntityValue;
                                        Guid contactId = Guid.Parse(guid);
                                        if (contactId != Guid.Empty)
                                        {
                                            QueryExpression query = new QueryExpression(configData.ContactEntity);
                                            query.ColumnSet = new ColumnSet("fullname");
                                            FilterExpression filter = new FilterExpression(LogicalOperator.And);
                                            filter.AddCondition("contactid", ConditionOperator.Equal, contactId);

                                            query.Criteria = filter;

                                            EntityCollection _entities = _caseMetaData._service.RetrieveMultiple(query);

                                            if (_entities!=null&&_entities.Entities != null && _entities.Entities.Count > 0)
                                            {
                                                foreach (Entity ContactName in _entities.Entities)
                                                {
                                                    fullname = ContactName.Attributes.Contains(contactVM.fullnm) ? (string)ContactName.Attributes[contactVM.fullnm].ToString() : string.Empty;
                                                    _Id = (Guid)(ContactName.Attributes[contactVM.ContactID]);
                                                }
                                                if (contactId == _Id)
                                                {
                                                    _caseMetaData.ID = _Id;//sameena mark
                                                    _caseMetaData.parentmasterentityname = ContactLookup.contactEntity;//contact
                                                    _caseMetaData.masterconditionkey = ContactLookup.contact_Name;//fullname
                                                }
                                                else
                                                {
                                                    _suggestionRes.Status = HttpStatusCode.NotFound;
                                                    _suggestionRes.Message = "Contact Not found";
                                                    return _suggestionRes;
                                                }
                                            }
                                            else
                                            {
                                                _suggestionRes.Status = HttpStatusCode.NotFound;
                                                _suggestionRes.Message = "Contact Not found";
                                                return _suggestionRes;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _suggestionRes.Status = HttpStatusCode.NotAcceptable;
                                        _suggestionRes.Message = "Please Provide Contact Id";
                                        return _suggestionRes;
                                    }
                                }
                                else if (_caseMetaData.EntitytKey == CaseVM.caseType)//optionset
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        if (i.Value == "3")
                                        {
                                            _caseMetaData.EntityAtrribute = _caseMetaData.EntityValue;
                                        }
                                        else
                                        {
                                            _suggestionRes.Message = "Please Select the Suggestion Case Type";
                                            _suggestionRes.Status = HttpStatusCode.NotAcceptable;
                                            return _suggestionRes;
                                        }
                                    }
                                    else
                                    {
                                        _suggestionRes.Message = "Please Provide required fields";
                                        _suggestionRes.Status = HttpStatusCode.NotAcceptable;
                                        return _suggestionRes;
                                    }
                                }
                                else if (_caseMetaData.EntitytKey == CaseVM.origin)//option set
                                {
                                    if (i.Value != string.Empty)
                                    {
                                        bool IsOption = CasePickupStatus(_caseMetaData, i.Value, Case);

                                        if (IsOption == true)
                                        {
                                            _caseMetaData.EntityAtrribute = i.Value;
                                        }
                                        else if (IsOption == false)
                                        {
                                            _suggestionRes.Message = "Please Provide valid Origin type";
                                            _suggestionRes.Status = HttpStatusCode.NotFound;
                                            return _suggestionRes;
                                        }
                                    }
                                    else
                                    {
                                        _suggestionRes.Message = "Please Provide required fields";
                                        _suggestionRes.Status = HttpStatusCode.NotAcceptable;
                                        return _suggestionRes;
                                    }
                                }
                                else if (_caseMetaData.EntitytKey == CaseVM.discription)
                                {
                                    _caseMetaData.EntityAtrribute = _caseMetaData.EntityValue;
                                }
                                _caseMetaData._retrieveAttrRequest = new RetrieveAttributeRequest
                                {
                                    EntityLogicalName = configData.caseEntity,
                                    LogicalName = _caseMetaData.EntitytKey,
                                    RetrieveAsIfPublished = true
                                };
                                if (_caseMetaData._retrieveAttrRequest != null)
                                {
                                    _caseMetaData._retrieveAttrResponse = (RetrieveAttributeResponse)_caseMetaData._service.Execute(_caseMetaData._retrieveAttrRequest);
                                    _caseMetaData.retrievedAttributeMetadata = (AttributeMetadata)_caseMetaData._retrieveAttrResponse.AttributeMetadata;
                                    Case = objC.ConvertDatatype(_caseMetaData, Case);
                                }
                            }
                            Guid _CaseID = _caseMetaData._service.Create(Case);
                            if (_CaseID != Guid.Empty)
                            {
                                QueryExpression _query = new QueryExpression(configData.caseEntity);
                                _query.ColumnSet = new ColumnSet("ticketnumber");
                                FilterExpression _filter = new FilterExpression(LogicalOperator.And);

                                _filter.AddCondition("incidentid", ConditionOperator.Equal, _CaseID);

                                _query.Criteria = _filter;

                                EntityCollection _entities1 = _caseMetaData._service.RetrieveMultiple(_query);
                                if (_entities1!=null&&_entities1.Entities != null &&_entities1.Entities.Count > 0)
                                {
                                    foreach (Entity caseName in _entities1.Entities)
                                    {
                                        caseNumber = caseName.Attributes["ticketnumber"].ToString();
                                    }
                                }
                            }
                            if (caseNumber != null&&caseNumber!=string.Empty)
                            {
                                _suggestionRes.Message = "Suggestion created Successfully";
                                _suggestionRes.Status = HttpStatusCode.OK;
                                _suggestionRes.CaseID = caseNumber;
                                _suggestionRes.caseGuid = _CaseID;
                            }
                            else
                            {
                                _suggestionRes.Message = "Failed to create Case";
                                _suggestionRes.Status = HttpStatusCode.NotFound;
                            }
                        }
                        else
                        {
                            _suggestionRes.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                            _suggestionRes.Status = HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        _suggestionRes.Status = HttpStatusCode.ServiceUnavailable;
                    }
                }
                else
                {
                    _suggestionRes.Status = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                _suggestionRes.Message = msg;
                _suggestionRes.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => _suggestionRes);
        }
        public async Task<caseListInfo> getCaseDetails(RequestParameter _reqPara)
        {
            caseListInfo caseResponse = new caseListInfo();
            string API = "getCaseDetails";
            try
            {
                if (_reqPara.Source != null)
                {
                    string caseNum = "";
                    EntityData _caseEntity = new EntityData();
                    ConfigData _configInfo = ConfigEncrypt.GetCrmCredentials();
                    List<retrieveCase> _list = new List<retrieveCase>();
                    _caseEntity._service = organization.GetCRMService(_configInfo);

                    if (_caseEntity._service != null)
                    {
                        checkkeyPair verifyKeys = _verifyKeys.checkKeys(_reqPara.Source, API);
                        if (verifyKeys._IsTrue == true)
                        {
                            foreach (KeyValuePair<string, string> item in _reqPara.Source)
                            {
                                _caseEntity.EntitytKey = item.Key;
                                _caseEntity.EntityValue = item.Value;

                                if (CaseVM.caseNo == item.Key)
                                {
                                    if (item.Value != string.Empty)
                                    {
                                        caseNum = item.Value;
                                    }
                                    else
                                    {
                                        caseResponse.Status = HttpStatusCode.NotFound;
                                        caseResponse.Message = "Please Provide Case Id";
                                        return caseResponse;
                                    }
                                }
                            }
                            if (caseNum != null && caseNum != string.Empty)
                            {
                                QueryExpression _queryCase = new QueryExpression();

                                _queryCase.EntityName = _configInfo.caseEntity;
                                _queryCase.ColumnSet = new ColumnSet(new string[]
                                {
                                   CaseVM.caseType,
                                   CaseVM.SubmissionDate,
                                   CaseVM.serviceProvider,
                                   CaseVM.service,
                                   CaseVM.subscriptionType,
                                   CaseVM.complaintType,
                                   CaseVM.complaintSubType,
                                   CaseVM.serviceProviderRef,
                                   CaseVM.DisputeAccno,
                                   CaseVM.discription
                                });
                                FilterExpression _filterExp = new FilterExpression(LogicalOperator.And);
                               _filterExp.AddCondition(CaseLable.ticket, ConditionOperator.Equal, caseNum);

                                _queryCase.Criteria = _filterExp;

                                EntityCollection _CaseEntity = _caseEntity._service.RetrieveMultiple(_queryCase);

                                if (_CaseEntity!=null&&_CaseEntity.Entities != null && _CaseEntity.Entities.Count > 0)
                                {
                                    foreach (Entity _CaseEntities in _CaseEntity.Entities)
                                    {
                                        retrieveCase _retreiveAttr = new retrieveCase();

                                        string caseType = _CaseEntities.Attributes.Contains(CaseVM.caseType) ? (string)_CaseEntities.FormattedValues[CaseVM.caseType] : string.Empty;
                                        _retreiveAttr.casetypecode = caseType;

                                        DateTime? _submissionDate = _CaseEntities.Attributes.Contains(CaseVM.SubmissionDate) ? (DateTime)_CaseEntities.Attributes[CaseVM.SubmissionDate] : (DateTime?)null;
                                        _retreiveAttr.createdon = _submissionDate;

                                        string serviceProvider = _CaseEntities.Attributes.Contains(CaseVM.serviceProvider) ? ((EntityReference)_CaseEntities.Attributes[CaseVM.serviceProvider]).Name : null;
                                        _retreiveAttr.tra_serviceprovider = serviceProvider;

                                        string service1 = _CaseEntities.Attributes.Contains(CaseVM.service) ? ((EntityReference)_CaseEntities.Attributes[CaseVM.service]).Name : null;
                                        _retreiveAttr.tra_service = service1;

                                        string complainttype = _CaseEntities.Attributes.Contains(CaseVM.complaintType) ? ((EntityReference)_CaseEntities.Attributes[CaseVM.complaintType]).Name : null;
                                        _retreiveAttr.tra_complainttype = complainttype;

                                        string complaintsubtype = _CaseEntities.Attributes.Contains(CaseVM.complaintSubType) ? ((EntityReference)_CaseEntities.Attributes[CaseVM.complaintSubType]).Name : null;
                                        _retreiveAttr.tra_complaintsubtype = complaintsubtype;

                                        string disputeNum = _CaseEntities.Attributes.Contains(CaseVM.DisputeAccno) ? (string)_CaseEntities.Attributes[CaseVM.DisputeAccno] : string.Empty;
                                        _retreiveAttr.tra_disputednumber = disputeNum;

                                        string caseDes = _CaseEntities.Attributes.Contains(CaseVM.discription) ? (string)_CaseEntities.Attributes[CaseVM.discription] : string.Empty;
                                        _retreiveAttr.description = caseDes;

                                        string typeofService = _CaseEntities.Attributes.Contains(CaseVM.subscriptionType) ? (string)_CaseEntities.FormattedValues[CaseVM.subscriptionType] : string.Empty;
                                        _retreiveAttr.tra_subscriptiontype = typeofService;

                                        string _serviceProviderRef = _CaseEntities.Attributes.Contains(CaseVM.serviceProviderRef) ? (string)_CaseEntities.Attributes[CaseVM.serviceProviderRef] : string.Empty;
                                        _retreiveAttr.tra_serviceprovidercasereference = _serviceProviderRef;

                                        _list.Add(_retreiveAttr);
                                    }
                                    caseResponse.CaseList = _list;
                                    caseResponse.Status = HttpStatusCode.OK;
                                    caseResponse.Message = "Get Case details Successfully";
                                }
                                else
                                {
                                    caseResponse.Status = HttpStatusCode.NotFound;
                                    caseResponse.Message = "Case Id or Case No Not found";
                                    return caseResponse;
                                }
                            }
                        }
                        else
                        {
                            caseResponse.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                            caseResponse.Status = HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {
                        caseResponse.Status = HttpStatusCode.ServiceUnavailable;
                    }
                }
                else
                {
                    caseResponse.Status = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                caseResponse.Message = msg;
                caseResponse.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => caseResponse);
        }

        public async Task<CaseListDetails> GetCaseList(RequestParameter _reqPara)
        {
            CaseListDetails _caseDetails = new CaseListDetails();
            string API = "GetCaseList";
            try
            {
                if (_reqPara.Source != null )
                {
                    EntityData _caseMetaData = new EntityData();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    List<caseVM> lystCase = new List<caseVM>();
                    _caseMetaData._service = organization.GetCRMService(configData);

                    if (_caseMetaData._service != null)
                    {
                        checkkeyPair verifyKeys = _verifyKeys.checkKeys(_reqPara.Source, API);
                        if (verifyKeys._IsTrue == true)
                        {
                            string ContactId = _reqPara.Source[contactVM.ContactID].ToString();
                            if (ContactId != string.Empty)
                            {
                                Guid ContactIDs = Guid.Parse(ContactId);
                                if (ContactIDs != Guid.Empty&&ContactIDs!=null)
                                {
                                    QueryExpression _casequery = new QueryExpression(configData.caseEntity);
                                    _casequery.ColumnSet = new ColumnSet(new string[]
                                    {
                                       CaseLable.ticket,
                                       CaseLable.status,
                                       CaseLable.create
                                    });

                                    FilterExpression _filterExpression = new FilterExpression(LogicalOperator.And);
                                    _filterExpression.AddCondition(contactVM.ContactID, ConditionOperator.Equal, ContactIDs);
                                    _casequery.Criteria = _filterExpression;

                                    EntityCollection _caseEntity = _caseMetaData._service.RetrieveMultiple(_casequery);

                                    if (_caseEntity!=null&&_caseEntity.Entities != null && _caseEntity.Entities.Count > 0)
                                    {
                                        foreach (Entity item in _caseEntity.Entities)
                                        {
                                           caseVM _caseAttr = new caseVM();                
                                           _caseAttr.CaseID =item.Attributes.Contains(CaseLable.ticket)?(string) item.Attributes[CaseLable.ticket].ToString():string.Empty;
                                           _caseAttr.createdon =item.Attributes.Contains(CaseLable.create)? (DateTime)(item.Attributes[CaseLable.create]):(DateTime?)null;
                                           _caseAttr.statuscode =item.Attributes.Contains(CaseLable.status)?(string)item.FormattedValues[CaseLable.status].ToString():string.Empty;
                                           lystCase.Add(_caseAttr);                                          
                                        }
                                        _caseDetails.Message = "Case List";
                                        _caseDetails.CaseDetails = lystCase;
                                        _caseDetails.Status = HttpStatusCode.OK;
                                    }
                                    else
                                    {
                                        _caseDetails.Message = "cases not exist for entered customer id";
                                        _caseDetails.Status = HttpStatusCode.NotFound;
                                    }
                                }
                            }
                            else
                            {
                                _caseDetails.Message = "Please Provide Contact Id";
                                _caseDetails.Status = HttpStatusCode.NotAcceptable;
                            }
                        }
                        else
                        {
                            _caseDetails.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                            _caseDetails.Status = HttpStatusCode.NotFound;
                        }
                    }                   
                }
                else
                {
                    _caseDetails.Status = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                _caseDetails.Message = msg;
                _caseDetails.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => _caseDetails);
        }
        public async Task<ServiceListDetails> GetService(RequestParameter _request)
        {
            List<servcClass> serviceList = new List<servcClass>();
            ServiceListDetails _getService = new ServiceListDetails();
            string API = "GetService";
            try
            {
                if (_request.Source != null)
                {
                    EntityData _caseMetaData = new EntityData();
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    _caseMetaData._service = organization.GetCRMService(configData);

                    if (_caseMetaData._service != null)
                    {
                        checkkeyPair verifyKeys = _verifyKeys.checkKeys(_request.Source, API);

                        if (verifyKeys._IsTrue == true)
                        {
                            string srvcId = "";
                            string srvcProviderNm = "";

                            foreach (KeyValuePair<string, string> item in _request.Source)
                            {
                                _caseMetaData.EntitytKey = item.Key;
                                _caseMetaData.EntityValue = item.Value;

                                if (CaseLable.srvcProviderId == item.Key)
                                {
                                    if (item.Value != string.Empty)
                                    { 
                                        srvcId = _caseMetaData.EntityValue;

                                        Guid serviceProvideId = Guid.Parse(srvcId);
                                        if (serviceProvideId != Guid.Empty&&serviceProvideId!=null)
                                        {
                                            QueryExpression _queryExp1 = new QueryExpression(CaseLable.srvcProvide);

                                            _queryExp1.ColumnSet = new ColumnSet(new string[]
                                            {
                                                 CaseLable.srvcNm
                                            });
                                            FilterExpression filter1 = new FilterExpression(LogicalOperator.And);
                                            filter1.AddCondition(CaseLable.srvcProviderId, ConditionOperator.Equal, serviceProvideId);

                                            _queryExp1.Criteria = filter1;

                                            EntityCollection _Entity1 = _caseMetaData._service.RetrieveMultiple(_queryExp1);
                                            if (_Entity1!=null&&_Entity1.Entities != null && _Entity1.Entities.Count > 0)
                                            {
                                                foreach (Entity data1 in _Entity1.Entities)
                                                {
                       
                                                  srvcProviderNm =data1.Attributes.Contains(CaseLable.srvcNm)?(string) data1.Attributes[CaseLable.srvcNm].ToString():string.Empty;
                                                    
                                                }
                                            }
                                            else
                                            {
                                                _getService.Status = HttpStatusCode.NotFound;
                                                _getService.Message = "Service Provider Not Found";
                                                return _getService;
                                            }

                                            QueryExpression _queryExp = new QueryExpression(CaseLable.srvc);

                                            _queryExp.ColumnSet = new ColumnSet(new string[]
                                            {
                                                CaseLable.srvcNm,"tra_servicear"
                                            });
                                            FilterExpression filter = new FilterExpression(LogicalOperator.And);
                                            filter.AddCondition("statecode", ConditionOperator.Equal, 0);
                                            filter.AddCondition(CaseLable.srvcProvide, ConditionOperator.Equal, serviceProvideId);

                                            _queryExp.Criteria = filter;

                                            EntityCollection _Entity = _caseMetaData._service.RetrieveMultiple(_queryExp);
                                            if (_Entity!=null&&_Entity.Entities != null && _Entity.Entities.Count > 0)
                                            {
                                                foreach (Entity data in _Entity.Entities)
                                                {
                                                    servcClass _srvcData = new servcClass();
                                                   _srvcData.tra_service =data.Attributes.Contains(CaseLable.srvcNm)?(string) data.Attributes[CaseLable.srvcNm].ToString():string.Empty;
                                                    _srvcData.tra_serviceAr = data.Attributes.Contains("tra_servicear") ? (string)data.Attributes["tra_servicear"].ToString() : string.Empty;
                                                    _srvcData.tra_serviceid =data.Attributes.Contains(CaseLable.srvcId)? (Guid)(data.Attributes[CaseLable.srvcId]):Guid.Empty;

                                                   serviceList.Add(_srvcData);
                        
                                                }
                                            }
                                            else
                                            {
                                                _getService.Status = HttpStatusCode.NotFound;
                                                _getService.Message = "Service Provider Not Found";
                                                return _getService;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        _getService.Status = HttpStatusCode.NotFound;
                                        _getService.Message = "Please Provide service provider id";
                                        return _getService;
                                    }
                                }
                            }
                            _getService.tra_serviceprovider = srvcProviderNm;
                            _getService.serviceList = serviceList;
                            _getService.Status = HttpStatusCode.OK;
                            _getService.Message = "Service List";
                        }
                        else
                        {
                            _getService.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                            _getService.Status = HttpStatusCode.NotFound;
                        }
                    }
                    else
                    {


                    }
                }
                else
                {
                    _getService.Status = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                _getService.Message = msg;
                _getService.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => _getService);          
        }


        public async Task<complaintInfo> getComplaints()
        {
            EntityData _contatdata = new EntityData();
            complaintInfo _getComplaints = new complaintInfo();
            List<getComplaint> lystComplaints = new List<getComplaint>();
            try
            {
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                _contatdata._service = organization.GetCRMService(configData);
                if (_contatdata._service != null)
                {
                    QueryExpression _queryData = new QueryExpression(CaseVM.complaintType);
                    _queryData.ColumnSet = new ColumnSet(CaseLable.srvcNm, "tra_complainttypear");
                    FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                    _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _filterData.AddCondition("tra_showinconsumerportal", ConditionOperator.Equal, true);
                    _queryData.Criteria = _filterData;
                    EntityCollection _entityCase = _contatdata._service.RetrieveMultiple(_queryData);
                    if (_entityCase != null && _entityCase.Entities != null && _entityCase.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityCase.Entities)
                        {
                            getComplaint _getcomlaint = new getComplaint();
                            _getcomlaint.tra_name = item.Contains(CaseLable.srvcNm) ? (string)item.Attributes[CaseLable.srvcNm] : string.Empty;
                            _getcomlaint.tra_complaintAr = item.Contains("tra_complainttypear") ? (string)item.Attributes["tra_complainttypear"] : string.Empty;
                            _getcomlaint.tra_complainttypeid = item.Contains(CaseVM.complaintTypeId) ? (Guid)item.Attributes[CaseVM.complaintTypeId] : Guid.Empty;
                            lystComplaints.Add(_getcomlaint);
                           
                        }

                        if (lystComplaints != null && lystComplaints.Count > 0)
                        {

                            _getComplaints.Status = HttpStatusCode.OK;
                            _getComplaints.Message = "Successfully get Complaint Types";
                            _getComplaints.complaintList = lystComplaints;

                        }
                        else
                        {
                            _getComplaints.Status = HttpStatusCode.NotFound;
                            _getComplaints.Message = "Complaints Not Found";

                        }

                    }
                    else
                    {
                        _getComplaints.Status = HttpStatusCode.NotFound;
                        _getComplaints.Message = "Failed To get Data";

                    }


                }

            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                _getComplaints.Message = msg;
                _getComplaints.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => _getComplaints);
        }

        public async Task<complaintSubTypeInfo> getComplaintSubTypes(RequestParameter _requestParameter)
        {

            EntityData _caseData = new EntityData();
            complaintSubTypeInfo _responseData = new complaintSubTypeInfo();
            List<getComplaintSubType> getComplaintsList = new List<getComplaintSubType>();
            string API = "getComplaintSubTypes";
            try
            {
                if (_requestParameter.Source != null)
                {
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    _caseData._service = organization.GetCRMService(configData);

                    if (_caseData._service != null)
                    {
                        checkkeyPair verifyKeys = _verifyKeys.checkKeys(_requestParameter.Source, API);

                        if (verifyKeys._IsTrue == true)
                        {
                            string complaintTypeId = _requestParameter.Source[CaseVM.complaintTypeId].ToString();

                            if (complaintTypeId != null && complaintTypeId != string.Empty)
                            {
                                Guid complaintSub = Guid.Parse(complaintTypeId);

                                QueryExpression _queryComplaintData = new QueryExpression(CaseVM.complaintSubType);
                                _queryComplaintData.ColumnSet = new ColumnSet(CaseLable.srvcNm, "tra_complaintsubtypear");
                                FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                                _filterData.AddCondition(CaseVM.complaintType, ConditionOperator.Equal, complaintSub);
                                _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                                _queryComplaintData.Criteria = _filterData;
                                EntityCollection complaintSubData = _caseData._service.RetrieveMultiple(_queryComplaintData);
                                if (complaintSubData != null && complaintSubData.Entities != null && complaintSubData.Entities.Count > 0)
                                {
                                    foreach (Entity itemComplaintSub in complaintSubData.Entities)
                                    {
                                        getComplaintSubType _getComplaintSub = new getComplaintSubType();
                                        _getComplaintSub.tra_name = itemComplaintSub.Contains(CaseLable.srvcNm) ? (string)itemComplaintSub.Attributes[CaseLable.srvcNm] : string.Empty;
                                        _getComplaintSub.tra_complaintSubAr = itemComplaintSub.Contains("tra_complaintsubtypear") ? (string)itemComplaintSub.Attributes["tra_complaintsubtypear"] : string.Empty;
                                        _getComplaintSub.tra_complaintsubtypeid = itemComplaintSub.Contains(CaseVM.complaintSubTypeId) ? (Guid)itemComplaintSub.Attributes[CaseVM.complaintSubTypeId] : Guid.Empty;
                                        getComplaintsList.Add(_getComplaintSub);
                                    }

                                    if (getComplaintsList != null && getComplaintsList.Count > 0)
                                    {
                                        _responseData.Status = HttpStatusCode.OK;
                                        _responseData.Message = "Successfully get complaintSub types";
                                        _responseData.complaintSubTypeList = getComplaintsList;

                                    }
                                    else
                                    {
                                        _responseData.Status = HttpStatusCode.NotFound;
                                        _responseData.Message = "ComplaintSubTypes Not Found";
                                    }

                                }
                                else
                                {
                                    _responseData.Status = HttpStatusCode.NotFound;
                                    _responseData.Message = "ComplaintSubTypes Not Found";
                                }

                            }
                            else
                            {
                                _responseData.Status = HttpStatusCode.BadRequest;
                                _responseData.Message = "Please provide complaintType Id";

                            }
                        }
                        else
                        {
                            _responseData.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                            _responseData.Status = HttpStatusCode.NotFound;

                        }
                    }

                }
                else
                {
                    _responseData.Status = HttpStatusCode.BadRequest;
                    _responseData.Message = "Some thing gone wrong";
                }
            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                _responseData.Message = msg;
                _responseData.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => _responseData);
        }

        public async Task<serviceProviderDetails> getserviceProviders()
        {

            EntityData _contatdata = new EntityData();
            serviceProviderDetails srvcDetails = new serviceProviderDetails();
            List<getServiceProviders> lystservcprdList = new List<getServiceProviders>();
            try
            {
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                _contatdata._service = organization.GetCRMService(configData);
                if (_contatdata._service != null)
                {
                    QueryExpression _queryData = new QueryExpression(CaseVM.serviceProvider);
                    _queryData.ColumnSet = new ColumnSet(CaseLable.srvcNm, "tra_nameofserviceproviderar");
                    FilterExpression _filterData = new FilterExpression(LogicalOperator.And);
                    _filterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryData.Criteria = _filterData;
                    EntityCollection _entityCase = _contatdata._service.RetrieveMultiple(_queryData);
                    if (_entityCase != null && _entityCase.Entities != null && _entityCase.Entities.Count > 0)
                    {

                        foreach (Entity item in _entityCase.Entities)
                        {

                            getServiceProviders _getsrvcPrdInfo = new getServiceProviders();
                            _getsrvcPrdInfo.tra_name = item.Contains(CaseLable.srvcNm) ? (string)item.Attributes[CaseLable.srvcNm] : string.Empty;
                            _getsrvcPrdInfo.tra_serviceProviderAr = item.Contains("tra_nameofserviceproviderar") ? (string)item.Attributes["tra_nameofserviceproviderar"] : string.Empty;
                            _getsrvcPrdInfo.tra_serviceproviderid = item.Contains(CaseVM.srvcprdId) ? (Guid)item.Attributes[CaseVM.srvcprdId] : Guid.Empty;
                            lystservcprdList.Add(_getsrvcPrdInfo);
                        }

                        if (lystservcprdList != null && lystservcprdList.Count > 0)
                        {

                            srvcDetails.Status = HttpStatusCode.OK;
                            srvcDetails.Message = "Successfully get ServiceProviders";
                            srvcDetails.ServiceProviders = lystservcprdList;

                        }
                        else
                        {
                            srvcDetails.Status = HttpStatusCode.NotFound;
                            srvcDetails.Message = "ServiceProviders Not Found";

                        }

                    }
                    else
                    {
                        srvcDetails.Status = HttpStatusCode.NotFound;
                        srvcDetails.Message = "ServiceProviders Not Found";

                    }


                }

            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                srvcDetails.Message = msg;
                srvcDetails.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => srvcDetails);
        }

        public async Task<serviceSubType> getServiceSubTypes(RequestParameter _requestParameter)
        {
            EntityData _caseData = new EntityData();
            serviceSubType _ServiceSubTypeData = new serviceSubType();
            List<servcClass> lystSrvc = new List<servcClass>();
            serviceProvideVM serviceVm = new serviceProvideVM();
            List<getServiceSubType> getSrvcSubList = new List<getServiceSubType>();
            string API = "getServiceSubTypes";
            try
            {
                if (_requestParameter.Source != null )
                {
                    ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                    _caseData._service = organization.GetCRMService(configData);

                    if (_caseData._service != null)
                    {
                        checkkeyPair verifyKeys = _verifyKeys.checkKeys(_requestParameter.Source, API);
                        if (verifyKeys._IsTrue == true)
                        {

                            string srvcProviders = string.Empty;
                            string serviceId = string.Empty;

                            foreach (var keyValueData in _requestParameter.Source)
                            {
                                if (keyValueData.Key == CaseVM.srvcprdId)
                                {
                                    if (keyValueData.Value != string.Empty)
                                    {
                                        srvcProviders = keyValueData.Value;
                                    }
                                    else
                                    {
                                        _ServiceSubTypeData.Status = HttpStatusCode.NotAcceptable;
                                        _ServiceSubTypeData.Message = "Please Provide serviceProvider_Id";
                                        return _ServiceSubTypeData;

                                    }

                                }
                                else if (keyValueData.Key == CaseVM.srvcId)
                                {

                                    if (keyValueData.Value != string.Empty)
                                    {
                                        serviceId = keyValueData.Value;
                                    }
                                    else
                                    {
                                        _ServiceSubTypeData.Status = HttpStatusCode.NotAcceptable;
                                        _ServiceSubTypeData.Message = "Please Provide service_Id";
                                        return _ServiceSubTypeData;

                                    }

                                }
                            }

                            if (srvcProviders != string.Empty && serviceId != string.Empty)
                            {
                                Guid srvcProvidersId = Guid.Parse(srvcProviders);
                                Guid srvcId = Guid.Parse(serviceId);

                                QueryExpression _quarySrvcSubData = new QueryExpression(CaseVM.service);
                                _quarySrvcSubData.ColumnSet = new ColumnSet(new string[]{
                               CaseLable.srvcNm,CaseVM.serviceProvider
                            });
                                FilterExpression _SrvcSubfilterData = new FilterExpression(LogicalOperator.And);

                                _SrvcSubfilterData.AddCondition(CaseLable.srvcProvide, ConditionOperator.Equal, srvcProvidersId);
                                _SrvcSubfilterData.AddCondition("statecode", ConditionOperator.Equal, 0);
                                _SrvcSubfilterData.AddCondition(CaseVM.srvcId, ConditionOperator.Equal, srvcId);
                                _quarySrvcSubData.Criteria = _SrvcSubfilterData;


                                EntityCollection ec = _caseData._service.RetrieveMultiple(_quarySrvcSubData);

                                if (ec != null && ec.Entities != null && ec.Entities.Count > 0)
                                {
                                    foreach (Entity itemData in ec.Entities)
                                    {
                                        serviceVm.srvc = itemData.Contains(CaseLable.srvcNm) ? (string)itemData.Attributes[CaseLable.srvcNm].ToString() : string.Empty;
                                        serviceVm.srcvProvideId = itemData.Contains(CaseVM.serviceProvider) ? ((EntityReference)itemData.Attributes[CaseVM.serviceProvider]).Id : Guid.Empty;
                                    }
                                }
                                else
                                {
                                    _ServiceSubTypeData.Status = HttpStatusCode.NotFound;
                                    _ServiceSubTypeData.Message = "Service Not found against service Provider";
                                    return _ServiceSubTypeData;
                                }
                                if (serviceVm.srvc != null && serviceVm.srcvProvideId != null && serviceVm.srvc != string.Empty && serviceVm.srcvProvideId != Guid.Empty)
                                {
                                    QueryExpression _quarySrvcSubData1 = new QueryExpression(CaseVM.serviceSubtype);
                                    _quarySrvcSubData1.ColumnSet = new ColumnSet(CaseLable.srvcNm,"tra_servicesubtypear");
                                    FilterExpression f = new FilterExpression(LogicalOperator.And);
                                    f.AddCondition("statecode", ConditionOperator.Equal, 0);
                                    f.AddCondition(CaseVM.serviceProvider, ConditionOperator.Equal, serviceVm.srcvProvideId);
                                    f.AddCondition(CaseVM.srvcType, ConditionOperator.Equal, serviceVm.srvc);
                                    _quarySrvcSubData1.Criteria = f;
                                    EntityCollection ec1 = _caseData._service.RetrieveMultiple(_quarySrvcSubData1);

                                    if (ec1 != null && ec1.Entities != null && ec1.Entities.Count > 0)
                                    {
                                        foreach (Entity itemSrvcSubData in ec1.Entities)
                                        {
                                            getServiceSubType _getServiceSub = new getServiceSubType();
                                            _getServiceSub.tra_name = serviceVm.srvc = itemSrvcSubData.Contains(CaseLable.srvcNm) ? (string)itemSrvcSubData.Attributes[CaseLable.srvcNm].ToString() : string.Empty;
                                            _getServiceSub.servcieSubtypeAr = serviceVm.srvc = itemSrvcSubData.Contains("tra_servicesubtypear") ? (string)itemSrvcSubData.Attributes["tra_servicesubtypear"].ToString() : string.Empty;
                                            _getServiceSub.tra_servicesubtypeid = itemSrvcSubData.Contains(CaseVM.srvcSubId) ? (Guid)itemSrvcSubData.Attributes[CaseVM.srvcSubId] : Guid.Empty;
                                            getSrvcSubList.Add(_getServiceSub);
                                        }

                                        if (getSrvcSubList != null && getSrvcSubList.Count > 0)
                                        {
                                            _ServiceSubTypeData.Status = HttpStatusCode.OK;
                                            _ServiceSubTypeData.Message = "ServiceSubTypes get successfully";
                                            _ServiceSubTypeData.ServiceSubTypes = getSrvcSubList;
                                        }
                                        else
                                        {
                                            _ServiceSubTypeData.Status = HttpStatusCode.NotFound;
                                            _ServiceSubTypeData.Message = "ServiceSubTypes Not Found";

                                        }

                                    }
                                    else
                                    {

                                        _ServiceSubTypeData.Status = HttpStatusCode.NotFound;
                                        _ServiceSubTypeData.Message = "ServiceSubTypes Not Found";
                                    }
                                }
                            }
                           
                        }
                        else
                        {
                            _ServiceSubTypeData.Status = HttpStatusCode.NotFound;
                            _ServiceSubTypeData.Message = "Please Provide " + verifyKeys.Keys + ",It's Mandatory Field";
                        
                        }
                    }

                }
                else
                {
                    _ServiceSubTypeData.Status = HttpStatusCode.BadRequest;
                    _ServiceSubTypeData.Message = "Some thing gone wrong";
                }
            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                _ServiceSubTypeData.Message = msg;
                _ServiceSubTypeData.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => _ServiceSubTypeData);
        }

        public async Task<EnquiryTypesInfo> getEnquiryTypes()
        {

            EntityData _contatdata = new EntityData();
            EnquiryTypesInfo _enquiryTypesDetails = new EnquiryTypesInfo();
            List<getEnquiryTypes> lystEnquiryTypes = new List<getEnquiryTypes>();
            try
            {
                ConfigData configData = ConfigEncrypt.GetCrmCredentials();
                _contatdata._service = organization.GetCRMService(configData);
                if (_contatdata._service != null)
                {
                    QueryExpression _queryEnquiry = new QueryExpression("tra_enquirytype");
                    _queryEnquiry.ColumnSet = new ColumnSet(CaseLable.srvcNm, "tra_enquirytypear");
                    FilterExpression _filterEnquiry = new FilterExpression(LogicalOperator.And);
                    _filterEnquiry.AddCondition("statecode", ConditionOperator.Equal, 0);
                    _queryEnquiry.Criteria = _filterEnquiry;
                    EntityCollection _entityEnquiry = _contatdata._service.RetrieveMultiple(_queryEnquiry);
                    if (_entityEnquiry != null && _entityEnquiry.Entities != null && _entityEnquiry.Entities.Count > 0)
                    {

                        foreach (Entity itemEnquiry in _entityEnquiry.Entities)
                        {
                            getEnquiryTypes enquiry = new getEnquiryTypes();
                            enquiry.tra_name = itemEnquiry.Contains(CaseLable.srvcNm) ? (string)itemEnquiry.Attributes[CaseLable.srvcNm] : string.Empty;
                            enquiry.tra_enquiryTypeAr = itemEnquiry.Contains("tra_enquirytypear") ? (string)itemEnquiry.Attributes["tra_enquirytypear"] : string.Empty;
                            enquiry.tra_enquirytypeid = itemEnquiry.Contains("tra_enquirytypeid") ? (Guid)itemEnquiry.Attributes["tra_enquirytypeid"] : Guid.Empty;
                            lystEnquiryTypes.Add(enquiry);
                        }

                        if (lystEnquiryTypes != null && lystEnquiryTypes.Count > 0)
                        {

                            _enquiryTypesDetails.Status = HttpStatusCode.OK;
                            _enquiryTypesDetails.Message = "Successfully get Enquiry Types";
                            _enquiryTypesDetails.Enquiry_Types = lystEnquiryTypes;

                        }
                        else
                        {
                            _enquiryTypesDetails.Status = HttpStatusCode.NotFound;
                            _enquiryTypesDetails.Message = "Enquiry_Types Not Found";

                        }

                    }
                    else
                    {
                        _enquiryTypesDetails.Status = HttpStatusCode.NotFound;
                        _enquiryTypesDetails.Message = "Enquiry_Types Not Found";

                    }


                }

            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                _enquiryTypesDetails.Message = msg;
                _enquiryTypesDetails.Status = HttpStatusCode.NotAcceptable;
            }
            return await Task.Run(() => _enquiryTypesDetails);
        }


        /// <summary>
        /// //////////////////////////////////////////////////////
        /// </summary>
        /// <param name="entityData"></param>
        /// <param name="lookup"></param>
        /// <param name="i_value"></param>
        /// <returns></returns>
        public Is_Status CaselookupStatus(EntityData entityData, LookupInfo lookup, string i_value)
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

                if (entityCollection!=null&&entityCollection.Entities != null && entityCollection.Entities.Count > 0)
                {
                    foreach (Entity itemNationality in entityCollection.Entities)
                    {
                        status.IsActive =itemNationality.Attributes.Contains(lookup.statusCode)? itemNationality.FormattedValues[lookup.statusCode].ToString():string.Empty;
                        status._entityNm=itemNationality.Attributes.Contains(lookup.traNm)?itemNationality.Attributes[lookup.traNm].ToString():string.Empty;
                        status._entityId =itemNationality.Attributes.Contains(lookup.parentId)?(Guid)(itemNationality.Attributes[lookup.parentId]):Guid.Empty;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
      
        public Is_Status SubTypes(EntityData entityData, LookupInfo lookup, string i_value)
        {
            Is_Status _isStatus = new Is_Status();
            Guid EntityId = Guid.Empty;
            try
            {
                QueryExpression _querySubTypes = new QueryExpression(lookup.entityName);
                _querySubTypes.ColumnSet = new ColumnSet(lookup.arrayData);

                 FilterExpression _filters = new FilterExpression(LogicalOperator.And);
                _filters.AddCondition(lookup.childId, ConditionOperator.Equal, i_value);
          
                _filters.AddCondition(lookup.entityNm1, ConditionOperator.Equal, entityData._guidId);

                _querySubTypes.Criteria = _filters;

                EntityCollection entityCollection = entityData._service.RetrieveMultiple(_querySubTypes);

                if (entityCollection!=null&&entityCollection.Entities != null && entityCollection.Entities.Count > 0)
                {
                    foreach (Entity itemSubTypes in entityCollection.Entities)
                    {
                        _isStatus.IsActive =itemSubTypes.Attributes.Contains(lookup.statusCode)?itemSubTypes.FormattedValues[lookup.statusCode].ToString():string.Empty;
                        _isStatus._entityId =itemSubTypes.Attributes.Contains(lookup.childId) ? (Guid)(itemSubTypes.Attributes[lookup.childId]):Guid.Empty;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _isStatus;
        }
        public bool CasePickupStatus(EntityData entityData,string i,Entity _entity)
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

    }
}