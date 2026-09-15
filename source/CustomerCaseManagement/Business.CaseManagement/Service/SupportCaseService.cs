using Application.CaseManagement.Interface;
using Domain.CaseManagement.Entity;
using Application.CaseManagement.DTO;
using Application.CaseManagement.Enums;
using System.Runtime;

namespace Application.CaseManagement.Service
{
    public class SupportCaseService : ISupportCaseService
    {
        private readonly ISupportCaseRepository _supportCaseRepository;
        public SupportCaseService(ISupportCaseRepository supportCaseRepository)
        {
            _supportCaseRepository = supportCaseRepository;
        }
        public async Task<SupportCaseResponse> AddAsync(SupportCaseRequest supportCaseRequest)
        {
            //Map DTO to Entity
            var supportCase = new SupportCase
            {
                CustomerName = supportCaseRequest.CustomerName,
                CustomerEmail = supportCaseRequest.CustomerEmail,
                Subject = supportCaseRequest.Subject,
                Description = string.IsNullOrWhiteSpace(supportCaseRequest.Description) ? "Description not provided" : supportCaseRequest.Description,
                Priority = supportCaseRequest.Priority,
                Status = CaseStatus.Open.ToString(),
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = null
            };
            //Repository call 
            var response = await _supportCaseRepository.AddAsync(supportCase);

            //Map Entity to DTO
            var supportCaseResponse = new SupportCaseResponse
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

        public async Task<SupportCaseResponse> GetByIdAsync(int id)
        {
            var response = await _supportCaseRepository.GetByIdAsync(id);

            //Map Entity to DTO
            var supportCaseResponse = new SupportCaseResponse
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

        public async Task<SupportCaseResponse> GetByRefIdAsync(int refId)
        {
            var response = await _supportCaseRepository.GetByRefIdAsync(refId);

            //Map Entity to DTO
            var supportCaseResponse = new SupportCaseResponse   
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

        public async Task<List<SupportCaseResponse>> GetAllAsync()
        {
            var supportCases = await _supportCaseRepository.GetAllAsync();
            return supportCases.Select(sc => new SupportCaseResponse
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

        public async Task<SupportCaseResponse> UpdateAsync(SupportCaseRequest supportCaseRequest)
        {
            //Fetch existing Support Case
            var existingSupportCase = await _supportCaseRepository.GetByRefIdAsync(supportCaseRequest.ReferenceNumber);
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
            existingSupportCase.ModifiedOn = DateTime.UtcNow;

            //Status change logic
            if (existingSupportCase.Status == CaseStatus.Resolved.ToString() && supportCaseRequest.Status != CaseStatus.Resolved.ToString())
            {
                throw new Exception($"Resolved support cases cannot be changed to {supportCaseRequest.Status} Status.");
            }
            else if (existingSupportCase.Status == CaseStatus.InProgress.ToString() && supportCaseRequest.Status == CaseStatus.Open.ToString())
            {
                throw new Exception($"InProgress support cases cannot be changed to {supportCaseRequest.Status} Status.");
            }
            else
            {
                existingSupportCase.Status = supportCaseRequest.Status;
            }

            var updatedSupportCase = await _supportCaseRepository.UpdateAsync(existingSupportCase);

            //Map DTO to Entity
            var response = new SupportCaseResponse
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
    }
}