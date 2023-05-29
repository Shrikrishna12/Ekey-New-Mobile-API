
using System.Threading.Tasks;
using TRA_MobileAPIs.ResponseDetails;

namespace TRA_MobileAPIs.BuisnessLayer
{
    public interface ILoginRepository
    {
        //Task<ChangePassDetails> changePassword(RequestParameter _reqPara);
        //Task<resetPassDetails> resetPassword(RequestParameter _reqPara);

        //Task<LoginDetail> LoginDetails(RequestParameter _reqPara);
        Task<LoginDetail> LoginDetailsEkey(string CPRN);
    }
}
