
using System.Threading.Tasks;
using TRA_MobileAPIs.ResponseDetails;

namespace TRA_MobileAPIs.BuisnessLayer
{
    public interface IcontactRepository
    {
        Task<ConsumerRegisterDetails> RegisterConsumers(RequestParameter _reqPara);
        Task<ContactListDetails> GetConsumerInfo(RequestParameter _reqPara);
        Task<UpdateConsumerDetails> updateConsumerInformation(RequestParameter _reqPara);

        Task<NationalityDetails> getNations();
    }
}
