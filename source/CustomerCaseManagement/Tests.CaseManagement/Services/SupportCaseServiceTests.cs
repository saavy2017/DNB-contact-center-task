using Application.CaseManagement.DTO;
using Application.CaseManagement.Interface;
using Application.CaseManagement.Service;
using Domain.CaseManagement.Entity;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Tests.CaseManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.CaseManagement.Services
{
    [TestFixture]
    public class SupportCaseServiceTests
    {
        private Mock<ISupportCaseRepository> _mockRepository;
        private Mock<ILogger<SupportCaseService>> _mockLogger;
        private SupportCaseService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ISupportCaseRepository>();
            _mockLogger = new Mock<ILogger<SupportCaseService>>();
            _service = new SupportCaseService(_mockRepository.Object, _mockLogger.Object);
        }

        #region AddAsync Tests

        [Test]
        public async Task AddAsync_WithValidRequest_ShouldReturnSupportCaseResponse()
        {
            // Arrange
            var request = new SupportCaseRequest
            {
                CustomerName = "John Doe",
                CustomerEmail = "john@example.com",
                Subject = "Test Subject",
                Description = "Test Description",
                Priority = "High"
            };

            var supportCase = SupportCaseTestDataBuilder.CreateDefault()
                .WithCustomerName(request.CustomerName)
                .Build();

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<SupportCase>()))
                .ReturnsAsync(supportCase);

            // Act
            var result = await _service.AddAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CustomerName, Is.EqualTo(request.CustomerName));
            Assert.That(result.CustomerEmail, Is.EqualTo(request.CustomerEmail));
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<SupportCase>()), Times.Once);
        }

        [Test]
        public async Task AddAsync_WithNullDescription_ShouldSetDefaultDescription()
        {
            // Arrange
            var request = new SupportCaseRequest
            {
                CustomerName = "John Doe",
                CustomerEmail = "john@example.com",
                Subject = "Test Subject",
                Description = null,
                Priority = "High"
            };

            var supportCase = SupportCaseTestDataBuilder.CreateDefault().Build();
            _mockRepository.Setup(r => r.AddAsync(It.IsAny<SupportCase>()))
                .ReturnsAsync(supportCase);

            // Act
            var result = await _service.AddAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            _mockRepository.Verify(r => r.AddAsync(It.Is<SupportCase>(
                c => c.Description == "Description not provided")), Times.Once);
        }

        [Test]
        public async Task AddAsync_WithException_ShouldLogAndThrow()
        {
            // Arrange
            var request = new SupportCaseRequest
            {
                CustomerName = "John Doe",
                CustomerEmail = "john@example.com",
                Subject = "Test Subject",
                Priority = "High"
            };

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<SupportCase>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _service.AddAsync(request));
            _mockLogger.Verify(
                l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        #endregion

        #region GetByIdAsync Tests

        [Test]
        public async Task GetByIdAsync_WithValidId_ShouldReturnSupportCaseResponse()
        {
            // Arrange
            var supportCase = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .Build();

            _mockRepository.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(supportCase);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ReferenceNumber, Is.EqualTo(supportCase.ReferenceNumber));
            _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnResponseWithErrorMessage()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999))
                .ReturnsAsync((SupportCase)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public async Task GetByIdAsync_WithException_ShouldLogAndThrow()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _service.GetByIdAsync(1));
        }

        #endregion

        #region GetAllAsync Tests

        [Test]
        public async Task GetAllAsync_WithMultipleCases_ShouldReturnAllCases()
        {
            // Arrange
            var case1 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .Build();
            var case2 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(2)
                .Build();

            _mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<SupportCase> { case1, case2 });

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllAsync_WithNoCases_ShouldReturnEmptyList()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<SupportCase>());

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        #endregion

        #region UpdateAsync Tests

        [Test]
        public async Task UpdateAsync_WithValidRequest_ShouldUpdateCase()
        {
            // Arrange
            var updateRequest = new SupportCaseUpdateRequest
            {
                ReferenceNumber = 1001,
                CustomerName = "Updated Name",
                Status = "In Progress",
                Priority = "Medium",
                Subject = "Updated Subject",
                Description = "Updated Description"
            };

            var supportCase = SupportCaseTestDataBuilder.CreateDefault()
                .WithReferenceNumber(1001)
                .WithStatus("In Progress")
                .Build();

            _mockRepository.Setup(r => r.GetByRefIdAsync(1001))
                .ReturnsAsync(supportCase);

            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<SupportCase>()))
                .ReturnsAsync(supportCase);

            // Act
            var result = await _service.UpdateAsync(updateRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<SupportCase>()), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_WithNonExistentReference_ShouldReturnErrorMessage()
        {
            // Arrange
            var updateRequest = new SupportCaseUpdateRequest
            {
                ReferenceNumber = 9999,
                Status = "Closed"
            };

            _mockRepository.Setup(r => r.GetByRefIdAsync(9999))
                .ReturnsAsync((SupportCase)null);

            // Act
            var result = await _service.UpdateAsync(updateRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Null.And.Not.Empty);
        }

        #endregion

        #region GetByRefIdAsync Tests

        [Test]
        public async Task GetByRefIdAsync_WithValidRefId_ShouldReturnCase()
        {
            // Arrange
            var supportCase = SupportCaseTestDataBuilder.CreateDefault()
                .WithReferenceNumber(1001)
                .Build();

            _mockRepository.Setup(r => r.GetByRefIdAsync(1001))
                .ReturnsAsync(supportCase);

            // Act
            var result = await _service.GetByRefIdAsync(1001);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ReferenceNumber, Is.EqualTo(1001));
        }

        [Test]
        public async Task GetByRefIdAsync_WithInvalidRefId_ShouldReturnErrorMessage()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByRefIdAsync(9999))
                .ReturnsAsync((SupportCase)null);

            // Act
            var result = await _service.GetByRefIdAsync(9999);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.Not.Null.And.Not.Empty);
        }

        #endregion
    }
}
