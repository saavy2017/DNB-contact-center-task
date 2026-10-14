using Domain.CaseManagement.Entity;
using Infra.CaseManagement.Repositories;
using Infra.CaseManagement;
using NUnit.Framework;
using Tests.CaseManagement.Utilities;

namespace Tests.CaseManagement.Repositories
{
    [TestFixture]
    public class SupportCaseFilterRepositoryTests
    {
        private CaseDbContext _context;
        private SupportCaseFilterRepository _repository;

        [SetUp]
        public void Setup()
        {
            _context = TestDbContextFactory.CreateInMemoryContext();
            _repository = new SupportCaseFilterRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        #region Search Filter Tests

        [Test]
        public async Task Search_WithNoFilters_ShouldReturnAllCases()
        {
            // Arrange
            var case1 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithReferenceNumber(1001)
                .Build();
            var case2 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(2)
                .WithReferenceNumber(1002)
                .Build();

            _context.SupportCase.Add(case1);
            _context.SupportCase.Add(case2);
            await _context.SaveChangesAsync();

            var filterRequest = new SupportCase { ReferenceNumber = 0, CustomerEmail = null, Priority = null, Status = null };

            // Act
            var result = await _repository.Search(filterRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Search_FilterByReferenceNumber_ShouldReturnMatchingCase()
        {
            // Arrange
            var case1 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithReferenceNumber(1001)
                .Build();
            var case2 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(2)
                .WithReferenceNumber(1002)
                .Build();

            _context.SupportCase.Add(case1);
            _context.SupportCase.Add(case2);
            await _context.SaveChangesAsync();

            var filterRequest = new SupportCase { ReferenceNumber = 1001 };

            // Act
            var result = await _repository.Search(filterRequest);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].ReferenceNumber, Is.EqualTo(1001));
        }

        [Test]
        public async Task Search_FilterByEmail_ShouldReturnMatchingCases()
        {
            // Arrange
            var case1 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithCustomerEmail("john@example.com")
                .Build();
            var case2 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(2)
                .WithCustomerEmail("jane@example.com")
                .Build();

            _context.SupportCase.Add(case1);
            _context.SupportCase.Add(case2);
            await _context.SaveChangesAsync();

            var filterRequest = new SupportCase { ReferenceNumber = 0, CustomerEmail = "john" };

            // Act
            var result = await _repository.Search(filterRequest);

            // Assert
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result[0].CustomerEmail, Does.Contain("john"));
        }

        [Test]
        public async Task Search_FilterByPriority_ShouldReturnMatchingCases()
        {
            // Arrange
            var case1 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithPriority("High")
                .Build();
            var case2 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(2)
                .WithPriority("Low")
                .Build();

            _context.SupportCase.Add(case1);
            _context.SupportCase.Add(case2);
            await _context.SaveChangesAsync();

            var filterRequest = new SupportCase { ReferenceNumber = 0, Priority = "High" };

            // Act
            var result = await _repository.Search(filterRequest);

            // Assert
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result[0].Priority, Is.EqualTo("High"));
        }

        [Test]
        public async Task Search_FilterByStatus_ShouldReturnMatchingCases()
        {
            // Arrange
            var case1 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithStatus("Open")
                .Build();
            var case2 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(2)
                .WithStatus("Closed")
                .Build();

            _context.SupportCase.Add(case1);
            _context.SupportCase.Add(case2);
            await _context.SaveChangesAsync();

            var filterRequest = new SupportCase { ReferenceNumber = 0, Status = "Open" };

            // Act
            var result = await _repository.Search(filterRequest);

            // Assert
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result[0].Status, Is.EqualTo("Open"));
        }

        [Test]
        public async Task Search_WithMultipleFilters_ShouldReturnMatchingCases()
        {
            // Arrange
            var case1 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithReferenceNumber(1001)
                .WithPriority("High")
                .WithStatus("Open")
                .Build();
            var case2 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(2)
                .WithReferenceNumber(1002)
                .WithPriority("Low")
                .WithStatus("Closed")
                .Build();

            _context.SupportCase.Add(case1);
            _context.SupportCase.Add(case2);
            await _context.SaveChangesAsync();

            var filterRequest = new SupportCase 
            { 
                ReferenceNumber = 1001, 
                Priority = "High", 
                Status = "Open" 
            };

            // Act
            var result = await _repository.Search(filterRequest);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].ReferenceNumber, Is.EqualTo(1001));
            Assert.That(result[0].Priority, Is.EqualTo("High"));
            Assert.That(result[0].Status, Is.EqualTo("Open"));
        }

        [Test]
        public async Task Search_WithNoMatches_ShouldReturnEmptyList()
        {
            // Arrange
            var case1 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithStatus("Open")
                .Build();

            _context.SupportCase.Add(case1);
            await _context.SaveChangesAsync();

            var filterRequest = new SupportCase { ReferenceNumber = 0, Status = "NonExistent" };

            // Act
            var result = await _repository.Search(filterRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task Search_ShouldReturnResultsOrderedByReferenceNumber()
        {
            // Arrange
            var case1 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithReferenceNumber(1003)
                .Build();
            var case2 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(2)
                .WithReferenceNumber(1001)
                .Build();
            var case3 = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(3)
                .WithReferenceNumber(1002)
                .Build();

            _context.SupportCase.Add(case1);
            _context.SupportCase.Add(case2);
            _context.SupportCase.Add(case3);
            await _context.SaveChangesAsync();

            var filterRequest = new SupportCase { ReferenceNumber = 0 };

            // Act
            var result = await _repository.Search(filterRequest);

            // Assert
            Assert.That(result[0].ReferenceNumber, Is.LessThan(result[1].ReferenceNumber));
            Assert.That(result[1].ReferenceNumber, Is.LessThan(result[2].ReferenceNumber));
        }

        [Test]
        public async Task Search_WithNullRequest_ShouldHandleGracefully()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.Search(null));
        }

        #endregion
    }
}
