using Application.CaseManagement.Interface;
using Domain.CaseManagement.Entity;
using Application.CaseManagement.DTO;
using Application.CaseManagement.Enums;
using System.Runtime;
using Microsoft.Extensions.Logging;

namespace Application.CaseManagement.Service
{
    public class SupportCaseService : ISupportCaseService
    {
        private readonly ISupportCaseRepository _supportCaseRepository;
        private readonly ILogger<SupportCaseService> _logger;
        public SupportCaseService(ISupportCaseRepository supportCaseRepository, ILogger<SupportCaseService> logger)
        {
            _supportCaseRepository = supportCaseRepository;
            _logger = logger;
        }
        public async Task<SupportCaseResponse> AddAsync(SupportCaseRequest supportCaseRequest)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "AddAsync:An error occurred while adding a support case.");
                throw;
            }
        }

        public async Task<SupportCaseResponse> GetByIdAsync(int id)
        {
            try
            {
                var response = await _supportCaseRepository.GetByIdAsync(id);

                if (response == null)
                {
                    var SupportCaseResponseNull = new SupportCaseResponse
                    {
                        ErrorMessage = $"Support case with Id {id} not found."
                    };
                    return SupportCaseResponseNull;
                }
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetByIdAsync:An error occurred while fetching a support case.");
                throw;
            }
        }

        public async Task<SupportCaseResponse> GetByRefIdAsync(int refId)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetByRefIdAsync :An error occurred while fetching a support case.");
                throw;
            }
        }

        public async Task<List<SupportCaseResponse>> GetAllAsync()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync :An error occurred while fetching support cases.");
                throw;
            }
        }

        public async Task<SupportCaseResponse> UpdateAsync(SupportCaseUpdateRequest supportCaseRequest)
        {
            try
            {
                string errorMessages = null;
                //Fetch existing Support Case
                var existingSupportCase = await _supportCaseRepository.GetByRefIdAsync(supportCaseRequest.ReferenceNumber);
                if (existingSupportCase == null)
                {
                    errorMessages = $"Support case with reference number {supportCaseRequest.ReferenceNumber} not found.";
                    var responseNullError = new SupportCaseResponse
                    {
                        ErrorMessage = errorMessages
                    };
                    return responseNullError;
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
                    errorMessages = $"Resolved support cases cannot be changed to {supportCaseRequest.Status} Status.";
                }
                else if (existingSupportCase.Status == CaseStatus.InProgress.ToString() && supportCaseRequest.Status == CaseStatus.Open.ToString())
                {
                    errorMessages = $"InProgress support cases cannot be changed to {supportCaseRequest.Status} Status.";
                }

                if (string.IsNullOrWhiteSpace(errorMessages))
                {
                    existingSupportCase.Status = supportCaseRequest.Status;

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
                        Status = updatedSupportCase.Status,
                        ErrorMessage = errorMessages
                    };

                    return response;
                }
                var responseError = new SupportCaseResponse
                {
                    ErrorMessage = errorMessages
                };
                return responseError;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync:An error occurred while updating a support case.");
                throw;
            }
        }
    }
}