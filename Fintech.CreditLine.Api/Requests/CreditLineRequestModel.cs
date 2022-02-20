using System;

namespace Fintech.CreditLine.Api.Requests
{
    public class CreditLineRequestModel
    {
        public string FoundingType { get; set; }
        public double CashBalance { get; set; }
        public double MonthlyRevenue { get; set; }
        public double RequestedCreditLine { get; set; }
        public DateTime RequestedDate { get; set; }
    }
}
