using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fintech.CreditLineRequests.Domain.CreditLines.Entities;
using Fintech.CreditLineRequests.Domain.CreditLines.Enums;

namespace Fintech.CreditLineRequests.Domain.CreditLines.Repositories
{
    public interface ICreditLineRequestRepository
    {
        Task<List<CreditLineRequest>> GetByRequestByInterval(DateTime initialDateTime);
        Task<List<CreditLineRequest>> GetByStatus(CreditLineRequestsStatus status);
        Task Create(CreditLineRequest creditLineRequest);
    }
}
