using System;
using System.Threading.Tasks;
using System.Web.Http;
using TRA_MobileAPIs.ConfigSettings;
using TRA_MobileAPIs.Authorization;
using TRA_MobileAPIs.BuisnessLayer;
using TRA_MobileAPIs.ResponseDetails;

namespace TRA_MobileAPIs.Controllers
{

    public class CaseController : ApiController
    {
        ICaseRepository _IcaseRepo = new CaseRepository();

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Case/getCaseList")]
        public async Task<IHttpActionResult> GetCaseList([FromBody]RequestParameter _requestParameters)
        {
      
            try
            {
                var _responseDetails = await _IcaseRepo.GetCaseList(_requestParameters);
                return Ok(_responseDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Case/createEnquiry")]
        public async Task<IHttpActionResult> createEnquiry([FromBody]RequestParameter requestParameter)
        {
            Helper.writeLog("Request In time for createEnquiry: " + DateTime.Now.ToString());

            try
            {
                
                var EnquiryDatails = await _IcaseRepo.createEnquiry(requestParameter);
                Helper.writeLog("Request In time for createEnquiry: " + DateTime.Now.ToString());

                return Ok(EnquiryDatails);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Case/getService")]
        public async Task<IHttpActionResult> GetService([FromBody]RequestParameter _requestParameter)
        {
            try
            {
                var serviceList = await _IcaseRepo.GetService(_requestParameter);
                return Ok(serviceList); 
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Case/createConversation")]
        public async Task<IHttpActionResult> createConversations([FromBody]RequestAttachments _requestParameter)
        {
            
            try
            {
                var coversations = await _IcaseRepo.createConversations(_requestParameter);
                return Ok(coversations);
             
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
    
        [BasicAuthentication]
        [HttpPost]
        [Route("api/Case/createComplaint")]
        public async Task<IHttpActionResult> CreateComplaint([FromBody]RequestParameter _reqPara)
        {
            Helper.writeLog("Request In time for CreateComplaint: " + DateTime.Now.ToString());

            try
            {
                var _caseRespDetails = await _IcaseRepo.CreateComplaint(_reqPara);
                Helper.writeLog("Request In time for CreateComplaint: " + DateTime.Now.ToString());

                return Ok(_caseRespDetails);
             
            }
            catch (Exception ex)
            {

                throw ex;
            }
         

        }

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Case/createSuggestion")]
        public async Task<IHttpActionResult> CreateSuggestion([FromBody]RequestParameter _reqPara)
        {
        //   Helper.writeLog("Request In time for createSuggestion: " + DateTime.Now.ToString());
            try
            {
                var suggestionRes = await _IcaseRepo.CreateSuggestion(_reqPara);

               // Helper.writeLog("Response out time for createSuggestion: " + DateTime.Now.ToString());

                return Ok(suggestionRes);
            
            }
            catch (Exception ex)
            {

                throw ex;
            }
         

        }

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Case/getCaseDetails")]
        public async Task<IHttpActionResult> getCaseDetails([FromBody]RequestParameter _reqPara)
        {
      
            try
            {
                var _caseResponse = await _IcaseRepo.getCaseDetails(_reqPara);
                return Ok(_caseResponse);
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        
        }


        [BasicAuthentication]
        [HttpGet]
        [Route("api/Case/getComplaintType")]
        public async Task<IHttpActionResult> getComplaints()
        {
            try
            {
                var _caseResponse = await _IcaseRepo.getComplaints();
                return Ok(_caseResponse);
            }
            catch (Exception ex)
            {

                throw ex;
            }
    
        }

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Case/getComplaintSubType")]
        public async Task<IHttpActionResult> getComplaintSubType([FromBody]RequestParameter _reqPara)
        {

            try
            {
                var _caseResponse = await _IcaseRepo.getComplaintSubTypes(_reqPara);

                return Ok(_caseResponse);
            }
            catch (Exception ex)
            {

                throw ex;
            }
          

        }
        [BasicAuthentication]
        [HttpGet]
        [Route("api/Case/getServiceProviders")]
        public async Task<IHttpActionResult> getServiceProviders()
        {

            try
            {
                var _servicePrdResponse = await _IcaseRepo.getserviceProviders();

                return Ok(_servicePrdResponse);
            }
            catch (Exception ex)
            {

                throw ex;
            }
       
        }

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Case/getServiceSubTypes")]
        public async Task<IHttpActionResult> getServiceSubTypes([FromBody]RequestParameter _reqPara)
        {
          

            try
            {
                var _serviceSubResponse = await _IcaseRepo.getServiceSubTypes(_reqPara);

                return Ok(_serviceSubResponse);
            }
            catch (Exception ex)
            {

                throw ex;
            }
       

        }

        [BasicAuthentication]
        [HttpGet]
        [Route("api/Case/getEnquiryType")]
        public async Task<IHttpActionResult> getEnquiry()
        {

            try
            {
                var enquiry = await _IcaseRepo.getEnquiryTypes();
                return Ok(enquiry);
            }
            catch (Exception ex)
            {

                throw ex;
            }
         
        }

    }
}
