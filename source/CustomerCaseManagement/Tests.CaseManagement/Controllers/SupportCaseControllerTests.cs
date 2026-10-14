using API.CaseManagement.Controllers;
using Application.CaseManagement.DTO;
using Application.CaseManagement.Interface;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Tests.CaseManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.CaseManagement.Controllers
{
    [TestFixture]
    public class SupportCaseControllerTests
    {
        private Mock<ISupportCaseService> _mockSupportCaseService;
        private Mock<ISupportCaseFilterService> _mockFilterService;
        private Mock<IValidator<SupportCaseRequest>> _mockValidator;
        private Mock<IValidator<SupportCaseUpdateRequest>> _mockValidatorUpdate;
        private SupportCaseController _controller;

        [SetUp]
        public void Setup()
        {
            _mockSupportCaseService = new Mock<ISupportCaseService>();
            _mockFilterService = new Mock<ISupportCaseFilterService>();
            _mockValidator = new Mock<IValidator<SupportCaseRequest>>();
            _mockValidatorUpdate = new Mock<IValidator<SupportCaseUpdateRequest>>();

            _controller = new SupportCaseController(
                _mockSupportCaseService.Object,
                _mockFilterService.Object,
                _mockValidator.Object,
                _mockValidatorUpdate.Object
            );
        }

        #region GET All Tests

        [Test]
        public async Task Get_ShouldReturnAllCases()
        {
            // Arrange
            var cases = new List<SupportCaseResponse>
            {
                new SupportCaseResponse { ReferenceNumber = 1001, CustomerName = "John" },
                new SupportCaseResponse { ReferenceNumber = 1002, CustomerName = "Jane" }
            };

            _mockSupportCaseService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(cases);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            _mockSupportCaseService.Verify(s => s.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task Get_WithNoCases_ShouldReturnEmptyList()
        {
            // Arrange
            _mockSupportCaseService.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<SupportCaseResponse>());

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        #endregion

        #region GET By ID Tests

        [Test]
        public async Task Get_WithValidId_ShouldReturnOkObjectResult()
        {
            // Arrange
            var response = new SupportCaseResponse
            {
                ReferenceNumber = 1001,
                CustomerName = "John Doe",
                CustomerEmail = "john@example.com"
            };

            _mockSupportCaseService.Setup(s => s.GetByIdAsync(1))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(response));
        }

        [Test]
        public async Task Get_WithNullResult_ShouldReturnNull()
        {
            // Arrange
            _mockSupportCaseService.Setup(s => s.GetByIdAsync(999))
                .ReturnsAsync((SupportCaseResponse)null);

            // Act
            var result = await _controller.Get(999);

            // Assert
            Assert.That(result.Result, Is.Null);
        }

        [Test]
        public async Task Get_WithServiceError_ShouldReturnBadRequest()
        {
            // Arrange
            var errorResponse = new SupportCaseResponse
            {
                ErrorMessage = "Case not found"
            };

            _mockSupportCaseService.Setup(s => s.GetByIdAsync(999))
                .ReturnsAsync(errorResponse);

            // Act
            var result = await _controller.Get(999);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        #endregion

        #region POST Create Tests

        [Test]
        public async Task Post_WithValidRequest_ShouldReturnOkResult()
        {
            // Arrange
            var request = new SupportCaseRequest
            {
                CustomerName = "John Doe",
                CustomerEmail = "john@example.com",
                Subject = "Test",
                Priority = "High"
            };

            var response = new SupportCaseResponse
            {
                ReferenceNumber = 1001,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail
            };

            _mockValidator.Setup(v => v.ValidateAsync(request, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            _mockSupportCaseService.Setup(s => s.AddAsync(request))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Post(request);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            _mockSupportCaseService.Verify(s => s.AddAsync(request), Times.Once);
        }

        [Test]
        public async Task Post_WithInvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new SupportCaseRequest
            {
                CustomerName = "",
                CustomerEmail = "invalid-email"
            };

            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("CustomerName", "Customer name is required")
            });

            _mockValidator.Setup(v => v.ValidateAsync(request, CancellationToken.None))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.Post(request);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            _mockSupportCaseService.Verify(s => s.AddAsync(It.IsAny<SupportCaseRequest>()), Times.Never);
        }

        [Test]
        public async Task Post_WithNullRequest_ShouldHandleGracefully()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _controller.Post(null));
        }

        #endregion

        #region PUT Update Tests

        [Test]
        public async Task Update_WithValidRequest_ShouldReturnOkResult()
        {
            // Arrange
            var request = new SupportCaseUpdateRequest
            {
                ReferenceNumber = 1001,
                Status = "Closed"
            };

            var response = new SupportCaseResponse
            {
                ReferenceNumber = 1001,
                Status = "Closed"
            };

            var validationResult = new ValidationResult();
            _mockValidatorUpdate.Setup(v => v.ValidateAsync(request, CancellationToken.None))
                .ReturnsAsync(validationResult);

            _mockSupportCaseService.Setup(s => s.UpdateAsync(request))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Update(request);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            _mockSupportCaseService.Verify(s => s.UpdateAsync(request), Times.Once);
        }

        [Test]
        public async Task Update_WithInvalidReferenceNumber_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new SupportCaseUpdateRequest
            {
                ReferenceNumber = 0,  // Invalid
                Status = "Closed"
            };

            var validationResult = new ValidationResult();
            _mockValidatorUpdate.Setup(v => v.ValidateAsync(request, CancellationToken.None))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.Update(request);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            _mockSupportCaseService.Verify(s => s.UpdateAsync(It.IsAny<SupportCaseUpdateRequest>()), Times.Never);
        }

        [Test]
        public async Task Update_WithValidationErrors_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new SupportCaseUpdateRequest
            {
                ReferenceNumber = -1
            };

            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("Status", "Status is required")
            });

            _mockValidatorUpdate.Setup(v => v.ValidateAsync(request, CancellationToken.None))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.Update(request);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_WithServiceError_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new SupportCaseUpdateRequest
            {
                ReferenceNumber = 1001,
                Status = "Closed"
            };

            var errorResponse = new SupportCaseResponse
            {
                ErrorMessage = "Case not found"
            };

            var validationResult = new ValidationResult();
            _mockValidatorUpdate.Setup(v => v.ValidateAsync(request, CancellationToken.None))
                .ReturnsAsync(validationResult);

            _mockSupportCaseService.Setup(s => s.UpdateAsync(request))
                .ReturnsAsync(errorResponse);

            // Act
            var result = await _controller.Update(request);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        #endregion

        #region Search Filter Tests

        [Test]
        public async Task Search_WithValidFilter_ShouldReturnFilteredCases()
        {
            // Arrange
            var filterRequest = new SupportCaseFilterRequest
            {
                ReferenceNumber = 1001
            };

            var results = new List<SupportCaseResponse>
            {
                new SupportCaseResponse { ReferenceNumber = 1001, CustomerName = "John" }
            };

            _mockFilterService.Setup(s => s.Search(filterRequest))
                .ReturnsAsync(results);

            // Act
            var result = await _controller.Search(filterRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            _mockFilterService.Verify(s => s.Search(filterRequest), Times.Once);
        }

        [Test]
        public async Task Search_WithNoMatches_ShouldReturnEmptyList()
        {
            // Arrange
            var filterRequest = new SupportCaseFilterRequest
            {
                Status = "NonExistent"
            };

            _mockFilterService.Setup(s => s.Search(filterRequest))
                .ReturnsAsync(new List<SupportCaseResponse>());

            // Act
            var result = await _controller.Search(filterRequest);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        #endregion
    }
}
