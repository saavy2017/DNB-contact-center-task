using Microsoft.AspNetCore.Mvc;

namespace CustomerCaseManagement.Controllers
{
    [ApiController]
    [Route("api")]
    public class SupportCase : ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<SupportCase> Get()
        {
            return null;
        }
    }
}
