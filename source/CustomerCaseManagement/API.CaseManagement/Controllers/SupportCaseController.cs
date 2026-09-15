using Microsoft.AspNetCore.Mvc;
using Domain.CaseManagement.Entity;

namespace API.CaseManagement.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SupportCaseController : ControllerBase
    {
        [HttpGet(Name = "GetSupportCases")]
        public IEnumerable<SupportCase> Get()
        {
            return null;
        }
    }
}
