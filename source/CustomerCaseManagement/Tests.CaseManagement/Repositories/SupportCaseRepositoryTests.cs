using Domain.CaseManagement.Entity;
using Infra.CaseManagement.Repositories;
using NUnit.Framework;
using Tests.CaseManagement.Utilities;

namespace Tests.CaseManagement.Repositories
{
    [TestFixture]
    public class SupportCaseRepositoryTests
    {
        private CaseDbContext _context;
        private SupportCaseRepository _repository;

        [SetUp]
        public void Setup()
        {
            _context = TestDbContextFactory.CreateInMemoryContext();
            _repository = new SupportCaseRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        #region AddAsync Tests

        [Test]
        public async Task AddAsync_WithValidSupportCase_ShouldAddAndReturnWithIncrementedReferenceNumber()
        {
            // Arrange
            var newCase = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(0)
                .WithReferenceNumber(0)
                .Build();

            // Act
            var result = await _repository.AddAsync(newCase);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ReferenceNumber, Is.GreaterThan(0));
            Assert.That(result.Id, Is.GreaterThan(0));
        }

        [Test]
        public async Task AddAsync_WithMultipleCases_ShouldIncrementReferenceNumberSequentially()
        {
            // Arrange
            var case1 = SupportCaseTestDataBuilder.CreateDefault().Build();
            var case2 = SupportCaseTestDataBuilder.CreateDefault()
                .WithCustomerName("Second Customer")
                .Build();

            // Act
            var result1 = await _repository.AddAsync(case1);
            var result2 = await _repository.AddAsync(case2);

            // Assert
            Assert.That(result2.ReferenceNumber, Is.GreaterThan(result1.ReferenceNumber));
        }

        [Test]
        public async Task AddAsync_WithNullCase_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.AddAsync(null));
        }

        [Test]
        public async Task AddAsync_ShouldPersistToDatabase()
        {
            // Arrange
            var newCase = SupportCaseTestDataBuilder.CreateDefault().Build();

            // Act
            var addedCase = await _repository.AddAsync(newCase);
            var retrievedCase = await _repository.GetByIdAsync(addedCase.Id);

            // Assert
            Assert.That(retrievedCase, Is.Not.Null);
            Assert.That(retrievedCase.CustomerEmail, Is.EqualTo(newCase.CustomerEmail));
        }

        #endregion

        #region GetAllAsync Tests

        [Test]
        public async Task GetAllAsync_WithNoCases_ShouldReturnEmptyList()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllAsync_WithMultipleCases_ShouldReturnAllCasesOrderedByReferenceNumber()
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

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].ReferenceNumber, Is.LessThan(result[1].ReferenceNumber));
        }

        #endregion

        #region GetByIdAsync Tests

        [Test]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCorrectCase()
        {
            // Arrange
            var caseToAdd = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .Build();
            _context.SupportCase.Add(caseToAdd);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.CustomerEmail, Is.EqualTo(caseToAdd.CustomerEmail));
        }

        [Test]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        #endregion

        #region GetByRefIdAsync Tests

        [Test]
        public async Task GetByRefIdAsync_WithValidReferenceNumber_ShouldReturnCorrectCase()
        {
            // Arrange
            var caseToAdd = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithReferenceNumber(1001)
                .Build();
            _context.SupportCase.Add(caseToAdd);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByRefIdAsync(1001);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ReferenceNumber, Is.EqualTo(1001));
        }

        [Test]
        public async Task GetByRefIdAsync_WithInvalidReferenceNumber_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByRefIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        #endregion

        #region Search Tests

        [Test]
        public async Task Search_WithValidId_ShouldReturnMatchingCases()
        {
            // Arrange
            var caseToAdd = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .Build();
            _context.SupportCase.Add(caseToAdd);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.Search(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result[0].Id, Is.EqualTo(1));
        }

        [Test]
        public async Task Search_WithNonExistentId_ShouldReturnEmptyList()
        {
            // Act
            var result = await _repository.Search(999);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        #endregion

        #region UpdateAsync Tests

        [Test]
        public async Task UpdateAsync_WithValidChanges_ShouldUpdateCase()
        {
            // Arrange
            var caseToAdd = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithStatus("Open")
                .Build();
            _context.SupportCase.Add(caseToAdd);
            await _context.SaveChangesAsync();

            caseToAdd.Status = "In Progress";

            // Act
            var result = await _repository.UpdateAsync(caseToAdd);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo("In Progress"));
        }

        [Test]
        public async Task UpdateAsync_ShouldPersistChanges()
        {
            // Arrange
            var caseToAdd = SupportCaseTestDataBuilder.CreateDefault()
                .WithId(1)
                .WithStatus("Open")
                .Build();
            _context.SupportCase.Add(caseToAdd);
            await _context.SaveChangesAsync();

            caseToAdd.Status = "Closed";
            await _repository.UpdateAsync(caseToAdd);

            // Act
            var retrievedCase = await _repository.GetByIdAsync(1);

            // Assert
            Assert.That(retrievedCase.Status, Is.EqualTo("Closed"));
        }

        [Test]
        public async Task UpdateAsync_WithNullCase_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.UpdateAsync(null));
        }

        #endregion
    }
}
