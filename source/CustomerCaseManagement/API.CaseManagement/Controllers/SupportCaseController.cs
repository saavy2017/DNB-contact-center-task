using Application.CaseManagement.DTO;
using Application.CaseManagement.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.CaseManagement.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SupportCaseController : ControllerBase
    {
        private readonly ISupportCaseService _supportCaseService;
        private readonly ISupportCaseFilterService _supportCaseFilterService;
        public SupportCaseController(ISupportCaseService supportCaseService, ISupportCaseFilterService supportCaseFilterService)
        {
            _supportCaseService = supportCaseService;
            _supportCaseFilterService = supportCaseFilterService;
        }
        [HttpGet(Name = "GetSupportCases")]
        public async Task<IEnumerable<SupportCaseResponse>> Get()
        {
            return await _supportCaseService.GetAllAsync();
        }

        [HttpGet("{id}", Name = "GetSupportCaseById")]
        public async Task<SupportCaseResponse> Get(int id)
        {
            var supportCase = await _supportCaseService.GetByIdAsync(id);
            if (supportCase == null)
            {
                return null;
            }
            return supportCase;
        }

        [HttpGet("SearchFilterSupportCases")]
        public async Task<List<SupportCaseResponse>> Search([FromQuery]SupportCaseFilterRequest request)
        {
            var supportCase = await _supportCaseFilterService.Search(request);
            return supportCase;
        }

        [HttpPost(("CreateSupportCase"))]
        public async Task<SupportCaseResponse> Post(SupportCaseRequest supportCase)
        {
            var createdSupportCase = await _supportCaseService.AddAsync(supportCase);
            return createdSupportCase;
        }

        [HttpPut("UpdateSupportCase")]
        public async Task<SupportCaseResponse> Update(SupportCaseRequest supportCase)
        {
            if (supportCase.ReferenceNumber <= 0)
            {
                return null;
            }
            var supportCaseUpdated = await _supportCaseService.UpdateAsync(supportCase);
            return supportCaseUpdated;
        }

    }
}
