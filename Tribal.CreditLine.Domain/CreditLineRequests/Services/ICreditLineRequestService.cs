using System.Threading.Tasks;
using Tribal.CreditLine.Domain.CreditLineRequests.Models.Responses;
using Tribal.CreditLineRequests.Domain.CreditLines.Entities;

namespace Tribal.CreditLineRequests.Domain.CreditLines.Services
{
    public interface ICreditLineRequestService
    {
        Task<CreditLineRequestResponse> Create(CreditLineRequest creditLineRequest);
    }
}
