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
        public SupportCaseController(ISupportCaseService supportCaseService)
        {
            _supportCaseService = supportCaseService;
        }
        [HttpGet(Name = "GetSupportCases")]
        public async Task<IEnumerable<SupportCaseRequest>> Get()
        {
            return await _supportCaseService.GetAllAsync();
        }

        [HttpGet("{id}", Name = "GetSupportCaseById")]
        public async Task<ActionResult<SupportCaseRequest>> Get(int id)
        {
            var supportCase = await _supportCaseService.GetByIdAsync(id);
            if (supportCase == null)
            {
                return NotFound();
            }
            return Ok(supportCase);
        }

        [HttpGet("search/{id}")]
        public async Task<ActionResult<SupportCaseRequest>> Search(int id)
        {
            var supportCase = await _supportCaseService.Search(id);
            if (supportCase == null)
            {
                return NotFound();
            }
            return Ok(supportCase);
        }

        [HttpPost(("CreateSupportCase"))]
        public async Task<SupportCaseRequest> Post(SupportCaseRequest supportCase)
        {
            var createdSupportCase = await _supportCaseService.AddAsync(supportCase);
            return createdSupportCase;
        }

        [HttpPut("UpdateSupportCase")]
        public async Task<ActionResult<SupportCaseRequest>> Update(SupportCaseRequest supportCase)
        {
            if (supportCase.ReferenceNumber <= 0)
            {
                return BadRequest();
            }
            var supportCaseUpdated = await _supportCaseService.UpdateAsync(supportCase);
            return Ok(supportCaseUpdated);
        }

    }
}
