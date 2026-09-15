using Application.CaseManagement.Interface;
using Domain.CaseManagement.Entity;
using Application.CaseManagement.DTO;


namespace Application.CaseManagement.Service
{
    public class SupportCaseService : ISupportCaseService
    {
        private readonly ISupportCaseRepository _supportCaseRepository;
        public SupportCaseService(ISupportCaseRepository supportCaseRepository)
        {
            _supportCaseRepository = supportCaseRepository;
        }
        public async Task<SupportCaseRequest> AddAsync(SupportCaseRequest supportCaseRequest)
        {
            //Map DTO to Entity
            var supportCase = new SupportCase
            {
                ReferenceNumber = supportCaseRequest.ReferenceNumber,
                CustomerName = supportCaseRequest.CustomerName,
                CustomerEmail = supportCaseRequest.CustomerEmail,
                Subject = supportCaseRequest.Subject,
                Description = supportCaseRequest.Description,
                Priority = supportCaseRequest.Priority,
                Status = supportCaseRequest.Status,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = null
            };
            //Repository call 
            var response = await _supportCaseRepository.AddAsync(supportCase);

            //Map Entity to DTO
            var supportCaseResponse = new SupportCaseRequest
            {
                ReferenceNumber = response.ReferenceNumber,
                CustomerName = response.CustomerName,
                CustomerEmail = response.CustomerEmail,
                Subject = response.Subject,
                Description = response.Description,
                Priority = response.Priority,
                Status = response.Status
            };
            return supportCaseResponse;
        }

        public async Task<SupportCaseRequest> GetByIdAsync(int id)
        {
            var response = await _supportCaseRepository.GetByIdAsync(id);

            //Map Entity to DTO
            var supportCaseResponse = new SupportCaseRequest
            {
                ReferenceNumber = response.ReferenceNumber,
                CustomerName = response.CustomerName,
                CustomerEmail = response.CustomerEmail,
                Subject = response.Subject,
                Description = response.Description,
                Priority = response.Priority,
                Status = response.Status
            };
            return supportCaseResponse;
        }

        public async Task<List<SupportCaseRequest>> GetAllAsync()
        {
            var supportCases = await _supportCaseRepository.GetAllAsync();
            return supportCases.Select(sc => new SupportCaseRequest
            {
                ReferenceNumber = sc.ReferenceNumber,
                CustomerName = sc.CustomerName,
                CustomerEmail = sc.CustomerEmail,
                Subject = sc.Subject,
                Description = sc.Description,
                Priority = sc.Priority,
                Status = sc.Status
            }).ToList();
        }

        public async Task<SupportCaseRequest> UpdateAsync(SupportCaseRequest supportCaseRequest)
        {
            //Fetch existing Support Case
            var existingSupportCase = await _supportCaseRepository.GetByIdAsync(supportCaseRequest.ReferenceNumber);
            if (existingSupportCase == null)
            {
                throw new Exception($"Support case with reference number {supportCaseRequest.ReferenceNumber} not found.");
            }

            //Map existing Support Case with DTO
            existingSupportCase.CustomerName = supportCaseRequest.CustomerName;
            existingSupportCase.CustomerEmail = supportCaseRequest.CustomerEmail;
            existingSupportCase.Subject = supportCaseRequest.Subject;
            existingSupportCase.Description = supportCaseRequest.Description;
            existingSupportCase.Priority = supportCaseRequest.Priority;
            existingSupportCase.Status = supportCaseRequest.Status;

            var updatedSupportCase = await _supportCaseRepository.UpdateAsync(existingSupportCase);

            //Map DTO to Entity
            var response = new SupportCaseRequest
            {
                ReferenceNumber = updatedSupportCase.ReferenceNumber,
                CustomerName = updatedSupportCase.CustomerName,
                CustomerEmail = updatedSupportCase.CustomerEmail,
                Subject = updatedSupportCase.Subject,
                Description = updatedSupportCase.Description,
                Priority = updatedSupportCase.Priority,
                Status = updatedSupportCase.Status
            };

            return response;
        }
        public async Task<List<SupportCaseRequest>> Search(int id)
        {
            var supportCases = await _supportCaseRepository.Search(id);
            return supportCases.Select(sc => new SupportCaseRequest
            {
                ReferenceNumber = sc.ReferenceNumber,
                CustomerName = sc.CustomerName,
                CustomerEmail = sc.CustomerEmail,
                Subject = sc.Subject,
                Description = sc.Description,
                Priority = sc.Priority,
                Status = sc.Status
            }).ToList();
        }
    }
}

namespace Application.CaseManagement.Models
{
    public class SupportCaseRequest
    {
        // add required properties here that map to SupportCase
    }
}
