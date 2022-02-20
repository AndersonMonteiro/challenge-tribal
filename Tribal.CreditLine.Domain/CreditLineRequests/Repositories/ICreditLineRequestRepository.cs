using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tribal.CreditLineRequests.Domain.CreditLines.Entities;
using Tribal.CreditLineRequests.Domain.CreditLines.Enums;

namespace Tribal.CreditLineRequests.Domain.CreditLines.Repositories
{
    public interface ICreditLineRequestRepository
    {
        Task<List<CreditLineRequest>> GetByRequestByInterval(DateTime initialDateTime);
        Task<List<CreditLineRequest>> GetByStatus(CreditLineRequestsStatus status);
        Task Create(CreditLineRequest creditLineRequest);
    }
}
