using Microsoft.AspNetCore.Mvc;

namespace Tribal.CreditLine.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected string SelfLink
            => $"{ControllerContext?.HttpContext?.Request?.Host.Value}{ControllerContext?.HttpContext?.Request?.Path}";
    }
}
