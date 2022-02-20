using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Fintech.CreditLine.Api.Controllers;
using Fintech.CreditLine.Api.Requests;
using Fintech.CreditLineRequests.Domain.CreditLines.Entities;
using Fintech.CreditLineRequests.Domain.CreditLines.Services;

namespace Fintech.CreditLineRequests.Api.Controllers
{
    [Route("v1/credit-line-requests")]
    [ApiController]
    public class CreditLineRequestController : BaseController
    {
        private readonly protected ICreditLineRequestService _creditLineRequestService;

        public CreditLineRequestController(
            ICreditLineRequestService creditLineRequestService)
        {
            _creditLineRequestService = creditLineRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreditLineRequestModel creditLineRequest)
        {
            CreditLineRequest creditLine = CreditLineRequest.CreateFromRequest(
                creditLineRequest.FoundingType,
                creditLineRequest.CashBalance,
                creditLineRequest.MonthlyRevenue,
                creditLineRequest.RequestedCreditLine,
                creditLineRequest.RequestedDate
                );

            var response = await _creditLineRequestService.Create(creditLine);

            return Ok(response);
        }
    }
}
