using System.Threading.Tasks;
using Fintech.CreditLine.Domain.CreditLineRequests.Models.Responses;
using Fintech.CreditLineRequests.Domain.CreditLines.Entities;

namespace Fintech.CreditLineRequests.Domain.CreditLines.Services
{
    public interface ICreditLineRequestService
    {
        Task<CreditLineRequestResponse> Create(CreditLineRequest creditLineRequest);
    }
}
