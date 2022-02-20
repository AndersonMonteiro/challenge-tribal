using FluentValidation;
using System;
using Fintech.CreditLine.Api.Requests;
using Fintech.CreditLineRequests.Domain.CreditLines.Enums;

namespace Fintech.CreditLine.Api.Validations
{
    public class CreditLineRequestValidator : AbstractValidator<CreditLineRequestModel>
    {
        public CreditLineRequestValidator()
        {
            RuleFor(c => c.FoundingType)
                .IsEnumName(typeof(FoundingTypes)).WithMessage("Field 'FoundingType' should be SME or STARTUP")
                .NotEmpty().WithMessage("Field 'FoundingType' is required.");

            RuleFor(c => c.CashBalance)
                .NotEmpty().WithMessage("Field 'CashBalance' is required.");

            RuleFor(c => c.MonthlyRevenue)
                .GreaterThan(0).WithMessage("Field 'MonthlyRevenue' must be greater than 0")
                .NotEmpty().WithMessage("Field 'MonthlyRevenue' is required.");

            RuleFor(c => c.RequestedCreditLine)
                .NotEmpty().WithMessage("Field 'RequestedCreditLine' is required.")
                .GreaterThan(0).WithMessage("Field 'RequestedCreditLine' must be greater than 0");

            RuleFor(c => c.RequestedDate)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Field 'RequestedDate' must be later or equals the current date")
                .NotEmpty().WithMessage("Field 'RequestedDate' is required.");
        }
    }
}
