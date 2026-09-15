using Microsoft.AspNetCore.Mvc;
using Domain.CaseManagement.Entity;
using Business.CaseManagement.Interface;

namespace API.CaseManagement.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SupportCaseController : ControllerBase
    {
        private readonly ISupportCase _supportCaseService;
        public SupportCaseController(ISupportCase supportCaseService)
        {
            _supportCaseService = supportCaseService;
        }
        [HttpGet(Name = "GetSupportCases")]
        public async Task<IEnumerable<SupportCase>> Get()
        {
            return await _supportCaseService.GetAllAsync();
        }

        [HttpGet("{id}", Name = "GetSupportCaseById")]
        public async Task<ActionResult<SupportCase>> Get(int id)
        {
            var supportCase = await _supportCaseService.GetByIdAsync(id);
            if (supportCase == null)
            {
                return NotFound();
            }
            return Ok(supportCase);
        }

        [HttpGet("search/{id}")]
        public async Task<ActionResult<SupportCase>> Search(int id)
        {
            var supportCase = await _supportCaseService.Search(id);
            if (supportCase == null)
            {
                return NotFound();
            }
            return Ok(supportCase);
        }

        [HttpPost]
        public async Task<ActionResult<SupportCase>> Post(SupportCase supportCase)
        {
            var createdSupportCase = await _supportCaseService.AddAsync(supportCase);
            return CreatedAtAction(nameof(Get), new { id = createdSupportCase.Id }, createdSupportCase);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SupportCase>> Put(int id, SupportCase supportCase)
        {
            if (id != supportCase.Id)
            {
                return BadRequest();
            }
            var supportCaseUpdated = await _supportCaseService.UpdateAsync(supportCase);
            return Ok(supportCaseUpdated);
        }

    }
}
