using Tribal.CreditLineRequests.Domain.CreditLines.Enums;

namespace Tribal.CreditLine.Domain.CreditLineRequests.Models.Responses
{
    public class CreditLineRequestResponse
    {
        public string Result { get; private set; }
        public string Details { get; private set; }

        private CreditLineRequestResponse(string result, string details)
        {
            Result = result;
            Details = details;
        }

        public static CreditLineRequestResponse CreateFromStatus(CreditLineRequestsStatus status)
        {
            if (status == CreditLineRequestsStatus.REJECTED)
                    return new CreditLineRequestResponse("REJECTED", "The application informed was rejected");

            return new CreditLineRequestResponse("ACCEPTED", "The application informed was authorized");
        }
    }
}
