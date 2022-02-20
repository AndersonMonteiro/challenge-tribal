using System;
using Fintech.CreditLineRequests.Domain.CreditLines.Enums;

namespace Fintech.CreditLineRequests.Domain.CreditLines.Entities
{
    public class CreditLineRequest
    {
        public Guid Id { get; private set; }
        public CreditLineRequestsStatus Status { get; set; }
        public FoundingTypes FoundingType { get; private set; }
        public double CashBalance { get; private set; }
        public double MonthlyRevenue { get; private set; }
        public double RequestedCreditLine { get; private set; }
        public DateTime RequestedDate { get; private set; }
        public DateTime CreatedDate{ get; private set; }

        private CreditLineRequest(
            Guid id,
            FoundingTypes foundingType,
            double cashBalance,
            double monthlyRevenue,
            double requestedCreditLine,
            DateTime requestedDate,
            DateTime createdDate
            )
        {
            Id = id;
            FoundingType = foundingType;
            CashBalance = cashBalance;
            MonthlyRevenue = monthlyRevenue;
            RequestedCreditLine = requestedCreditLine;
            RequestedDate = requestedDate;
            CreatedDate = createdDate;
        }

        public static CreditLineRequest CreateFromRequest(
            string foundingType,
            double cashBalance,
            double monthlyRevenue,
            double requestedCreditLine,
            DateTime requestedDate
            )
        {
            Enum.TryParse(foundingType.ToUpper(), out FoundingTypes currentoundingType);
            return new CreditLineRequest(Guid.NewGuid(), currentoundingType, cashBalance, monthlyRevenue, requestedCreditLine, requestedDate, DateTime.Now);
        }
    }
}
