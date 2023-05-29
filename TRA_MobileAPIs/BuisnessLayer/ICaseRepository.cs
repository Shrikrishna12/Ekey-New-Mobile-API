
using System.Threading.Tasks;
using TRA_MobileAPIs.ResponseDetails;

namespace TRA_MobileAPIs.BuisnessLayer
{
    public interface ICaseRepository
    {
        Task<CaseListDetails> GetCaseList(RequestParameter _reqPara);
        Task<EnquiryDetails> createEnquiry(RequestParameter request);
        Task<ServiceListDetails> GetService(RequestParameter _request);
        Task<ConversationDetails> createConversations(RequestAttachments _request);
        Task<SuggestionCase> CreateSuggestion(RequestParameter _reqPara);

        Task<ComplaintCase> CreateComplaint(RequestParameter _reqPara);

        Task<caseListInfo> getCaseDetails(RequestParameter _reqPara);

        Task<complaintInfo> getComplaints();

        Task<complaintSubTypeInfo> getComplaintSubTypes(RequestParameter _requestParameter);

        Task<serviceProviderDetails> getserviceProviders();

        Task<serviceSubType> getServiceSubTypes(RequestParameter _requestParameter);

        Task<EnquiryTypesInfo> getEnquiryTypes();
    }
}
