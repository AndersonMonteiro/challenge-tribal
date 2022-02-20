using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tribal.CreditLine.Api.Controllers;
using Tribal.CreditLine.Api.Requests;
using Tribal.CreditLineRequests.Domain.CreditLines.Entities;
using Tribal.CreditLineRequests.Domain.CreditLines.Services;

namespace Tribal.CreditLineRequests.Api.Controllers
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
