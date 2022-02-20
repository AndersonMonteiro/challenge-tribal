using System;
using System.Linq;
using System.Threading.Tasks;
using Fintech.CreditLine.Domain.CreditLineRequests.Models.Responses;
using Fintech.CreditLine.Domain.Exceptions;
using Fintech.CreditLineRequests.Domain.CreditLines.Entities;
using Fintech.CreditLineRequests.Domain.CreditLines.Enums;
using Fintech.CreditLineRequests.Domain.CreditLines.Repositories;
using Fintech.CreditLineRequests.Domain.CreditLines.Services;

namespace Fintech.CreditLineRequests.Services
{
    public class CreditLineRequestService : ICreditLineRequestService
    {
        private readonly ICreditLineRequestRepository _creditLineRequestRepository;

        public CreditLineRequestService(ICreditLineRequestRepository creditLineRequestRepository)
        {
            _creditLineRequestRepository = creditLineRequestRepository;
        }

        public async Task<CreditLineRequestResponse> Create(CreditLineRequest creditLineRequest)
        {
            ProccessApplicationStatus(ref creditLineRequest);

            await ValidateCompliance(creditLineRequest);

            await _creditLineRequestRepository.Create(creditLineRequest);

            return CreditLineRequestResponse.CreateFromStatus(creditLineRequest.Status);
        }

        private static void ProccessApplicationStatus(ref CreditLineRequest application)
        {
            switch (application.FoundingType)
            {
                case FoundingTypes.SME:
                    if (application.RequestedCreditLine <= application.MonthlyRevenue / 5)
                        application.Status = CreditLineRequestsStatus.ACCEPTED;
                    else
                        application.Status = CreditLineRequestsStatus.REJECTED;
                    break;
                case FoundingTypes.STARTUP:
                    if (application.RequestedCreditLine <= Math.Max(application.CashBalance / 3, application.MonthlyRevenue / 5))
                        application.Status = CreditLineRequestsStatus.ACCEPTED;
                    else
                        application.Status = CreditLineRequestsStatus.REJECTED;
                    break;
            }
        }

        private async Task ValidateCompliance(CreditLineRequest creditLineRequest)
        {
            var lastRequests = await _creditLineRequestRepository.GetByRequestByInterval(DateTime.Now.AddSeconds(-120));

            if (creditLineRequest.Status == CreditLineRequestsStatus.ACCEPTED)
            {
                if (lastRequests.Count > 2)
                    throw new TooManyRequestsException("Too many requests");
            }
            else if ((creditLineRequest.Status == CreditLineRequestsStatus.REJECTED))
            {
                var lastRequestsInRange = lastRequests.Where(a => a.CreatedDate >= (DateTime.Now.AddSeconds(-30))).ToList();

                if (lastRequestsInRange.Count > 0)
                {
                    throw new TooManyRequestsException("Too many requests");
                }

                var failedRequests = await _creditLineRequestRepository.GetByStatus(CreditLineRequestsStatus.REJECTED);
                if (failedRequests.Count > 3)
                    throw new AppException("A sales agent will contact you");
            }
        }
    }
}
