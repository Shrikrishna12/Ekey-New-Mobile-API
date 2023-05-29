using System;

using System.Web.Http;
using System.Threading.Tasks;
using TRA_MobileAPIs.BuisnessLayer;
using TRA_MobileAPIs.ResponseDetails;

using System.Threading;
using TRA_MobileAPIs.Authorization;

namespace TRA_MobileAPIs.Controllers
{

    public class ContactsController : ApiController
    {
        IcontactRepository _IContactRepo = new contactRepository();

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Contacts/registerConsumer")]
        public async Task<IHttpActionResult> registerConsumerToContact([FromBody] RequestParameter _requestParameters)
        {

            try
            {
                var _responseDetails = await _IContactRepo.RegisterConsumers(_requestParameters);
                return Ok(_responseDetails);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [BasicAuthentication]
        [HttpPost]
        [Route("api/Contacts/getConsumerInformation")]
        public async Task<IHttpActionResult> GetConsumerDetails([FromBody] RequestParameter _requestParameters)
        {

            try
            {
                var _responseDetails = await _IContactRepo.GetConsumerInfo(_requestParameters);
                return Ok(_responseDetails);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [BasicAuthentication]
        [HttpPost]
        [Route("api/Contacts/updateConsumerInformation")]
        public async Task<IHttpActionResult> updateConsumer([FromBody] RequestParameter _requestParameters)
        {

            try
            {
                var _responseDetails = await _IContactRepo.updateConsumerInformation(_requestParameters);
                return Ok(_responseDetails);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [BasicAuthentication]
        [HttpGet]
        [Route("api/Contacts/getNationality")]
        public async Task<IHttpActionResult> getNationality()
        {
            try
            {
                var _responseDetails = await _IContactRepo.getNations();
                return Ok(_responseDetails);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
