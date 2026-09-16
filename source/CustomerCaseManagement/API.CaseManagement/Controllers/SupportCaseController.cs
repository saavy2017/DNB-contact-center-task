using Application.CaseManagement.DTO;
using Application.CaseManagement.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.CaseManagement.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SupportCaseController : ControllerBase
    {
        private readonly ISupportCaseService _supportCaseService;
        private readonly ISupportCaseFilterService _supportCaseFilterService;
        private readonly IValidator<SupportCaseRequest> _validator;
        private readonly IValidator<SupportCaseUpdateRequest> _validatorUpdate;
        public SupportCaseController(ISupportCaseService supportCaseService, ISupportCaseFilterService supportCaseFilterService, IValidator<SupportCaseRequest> validator, IValidator<SupportCaseUpdateRequest> validatorUpdate)
        {
            _supportCaseService = supportCaseService;
            _supportCaseFilterService = supportCaseFilterService;
            _validator = validator;
            _validatorUpdate = validatorUpdate;
        }
        [HttpGet(Name = "GetSupportCases")]
        public async Task<IEnumerable<SupportCaseResponse>> Get()
        {
            return await _supportCaseService.GetAllAsync();
        }

        [HttpGet("{id}", Name = "GetSupportCaseById")]
        public async Task<ActionResult<SupportCaseResponse>> Get(int id)
        {
            var supportCase = await _supportCaseService.GetByIdAsync(id);
            if (supportCase == null)
            {
                return null;
            }
            else if (!(string.IsNullOrWhiteSpace(supportCase.ErrorMessage)))
            {
                return BadRequest(new { error = supportCase.ErrorMessage });
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
        public async Task<ActionResult<SupportCaseResponse>> Post(SupportCaseRequest supportCase)
        {
            var validationResult = await _validator.ValidateAsync(supportCase);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage }).ToList();
                return BadRequest(new { errors = errors });
            }
            var createdSupportCase = await _supportCaseService.AddAsync(supportCase);
            return Ok(createdSupportCase);
        }

        [HttpPut("UpdateSupportCase")]
        public async Task<ActionResult<SupportCaseResponse>> Update(SupportCaseUpdateRequest supportCase)
        {
            var validationResult = await _validatorUpdate.ValidateAsync(supportCase);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage }).ToList();
                return BadRequest(new { errors = errors });
            }
            if (supportCase.ReferenceNumber <= 0)
            {
                return BadRequest(new { error = "Reference number must be greater than 0" });
            }
            
            var supportCaseUpdated = await _supportCaseService.UpdateAsync(supportCase);
            if (supportCaseUpdated != null && !(string.IsNullOrWhiteSpace(supportCaseUpdated.ErrorMessage)))
            {
                return BadRequest(new { error = supportCaseUpdated.ErrorMessage });
            }
            return Ok(supportCaseUpdated);
        }

    }
}
