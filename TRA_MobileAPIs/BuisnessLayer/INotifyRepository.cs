
using System.Threading.Tasks;
using TRA_MobileAPIs.ResponseDetails;

namespace TRA_MobileAPIs.BuisnessLayer
{
    public interface INotifyRepository
    {
        Task<getNotifyDetails> GetNotification(RequestParameter _reqPara);
        Task<NotificationDetails> postNotification(RequestParameter _reqPara);

    }
}
