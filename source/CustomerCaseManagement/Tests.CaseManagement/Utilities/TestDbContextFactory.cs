using Domain.CaseManagement.Entity;
using Microsoft.EntityFrameworkCore;

namespace Tests.CaseManagement.Utilities
{
    /// <summary>
    /// Factory for creating in-memory test databases
    /// </summary>
    public static class TestDbContextFactory
    {
        public static CaseDbContext CreateInMemoryContext(string databaseName = "TestDb")
        {
            var options = new DbContextOptionsBuilder<CaseDbContext>()
                .UseInMemoryDatabase(databaseName + Guid.NewGuid())
                .Options;

            var context = new CaseDbContext(options);
            return context;
        }
    }

    /// <summary>
    /// Builder for creating test SupportCase entities
    /// </summary>
    public class SupportCaseTestDataBuilder
    {
        private int _id = 1;
        private int _referenceNumber = 1001;
        private string _customerName = "Test Customer";
        private string _customerEmail = "test@example.com";
        private string _subject = "Test Subject";
        private string _description = "Test Description";
        private string _priority = "High";
        private string _status = "Open";
        private DateTime _createdOn = DateTime.UtcNow;
        private DateTime? _modifiedOn = null;

        public SupportCaseTestDataBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public SupportCaseTestDataBuilder WithReferenceNumber(int refNumber)
        {
            _referenceNumber = refNumber;
            return this;
        }

        public SupportCaseTestDataBuilder WithCustomerName(string name)
        {
            _customerName = name;
            return this;
        }

        public SupportCaseTestDataBuilder WithCustomerEmail(string email)
        {
            _customerEmail = email;
            return this;
        }

        public SupportCaseTestDataBuilder WithSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public SupportCaseTestDataBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public SupportCaseTestDataBuilder WithPriority(string priority)
        {
            _priority = priority;
            return this;
        }

        public SupportCaseTestDataBuilder WithStatus(string status)
        {
            _status = status;
            return this;
        }

        public SupportCaseTestDataBuilder WithCreatedOn(DateTime createdOn)
        {
            _createdOn = createdOn;
            return this;
        }

        public SupportCaseTestDataBuilder WithModifiedOn(DateTime? modifiedOn)
        {
            _modifiedOn = modifiedOn;
            return this;
        }

        public SupportCase Build()
        {
            return new SupportCase
            {
                Id = _id,
                ReferenceNumber = _referenceNumber,
                CustomerName = _customerName,
                CustomerEmail = _customerEmail,
                Subject = _subject,
                Description = _description,
                Priority = _priority,
                Status = _status,
                CreatedOn = _createdOn,
                ModifiedOn = _modifiedOn
            };
        }

        public static SupportCaseTestDataBuilder CreateDefault()
        {
            return new SupportCaseTestDataBuilder();
        }
    }
}
