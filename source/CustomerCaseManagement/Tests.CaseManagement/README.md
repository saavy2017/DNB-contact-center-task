# NUnit Test Project for Customer Case Management

## Overview
This test project provides comprehensive unit test coverage for the Customer Case Management application using:
- **NUnit 4.1.0** - Testing framework
- **Moq 4.20.70** - Mocking framework
- **AutoFixture 4.18.1** - Test data builder
- **.NET 8.0** - Target framework

## Test Structure

### 1. Utilities
- **TestDbContextFactory.cs** - Creates in-memory test databases
- **SupportCaseTestDataBuilder.cs** - Fluent builder for test data

### 2. Repository Tests
- **SupportCaseRepositoryTests.cs** - Tests CRUD operations, sequencing, persistence
- **SupportCaseFilterRepositoryTests.cs** - Tests filtering, sorting, and complex queries

### 3. Service Tests
- **SupportCaseServiceTests.cs** - Tests business logic, mapping, error handling
- Mocks ISupportCaseRepository for isolation

### 4. Controller Tests
- **SupportCaseControllerTests.cs** - Tests HTTP endpoints, validation, response codes
- Mocks ISupportCaseService and validators

## Running Tests

### Visual Studio Test Explorer
1. Build the solution
2. Open Test Explorer (Tests > Test Explorer)
3. All tests in Tests.CaseManagement project should be visible
4. Click "Run All" or select specific tests

### Command Line (PowerShell)
```powershell
# Run all tests
dotnet test source/CustomerCaseManagement/Tests.CaseManagement/Tests.CaseManagement.csproj

# Run specific test class
dotnet test --filter "FullyQualifiedName~Tests.CaseManagement.Repositories.SupportCaseRepositoryTests"

# Run with verbose output
dotnet test -v n
```

### Test Naming Convention
Tests follow the pattern: `MethodName_Scenario_ExpectedResult`

Example: 
- `AddAsync_WithValidSupportCase_ShouldAddAndReturnWithIncrementedReferenceNumber`
- `Search_FilterByPriority_ShouldReturnMatchingCases`

## Test Coverage Summary

| Layer | Test Class | Test Count | Coverage |
|-------|-----------|-----------|----------|
| **Repository** | SupportCaseRepositoryTests | 14 | CRUD + sequencing |
| **Repository** | SupportCaseFilterRepositoryTests | 9 | Filtering + sorting |
| **Service** | SupportCaseServiceTests | 13 | Business logic + errors |
| **Controller** | SupportCaseControllerTests | 15 | HTTP + validation |
| **TOTAL** | | **51** | Comprehensive |

## Key Test Scenarios

### Repository Layer
✅ Adding cases with reference number generation
✅ Retrieving all cases with proper ordering
✅ Getting single cases by ID and reference number
✅ Updating cases with persistence verification
✅ Filtering with multiple criteria
✅ Null/invalid input handling

### Service Layer
✅ Mapping DTOs to entities
✅ Setting default descriptions
✅ Error logging and propagation
✅ Missing case error handling
✅ Business rule enforcement

### Controller Layer
✅ HTTP 200 (OK) responses
✅ HTTP 400 (BadRequest) for validation errors
✅ Input validation via FluentValidation
✅ Service method invocation verification
✅ Large dataset handling

## AAA Pattern
All tests follow the Arrange-Act-Assert pattern:

```csharp
[Test]
public async Task ExampleTest()
{
	// Arrange - Set up test data and mocks
	var testData = SupportCaseTestDataBuilder.CreateDefault().Build();

	// Act - Execute the method under test
	var result = await _service.AddAsync(testData);

	// Assert - Verify the results
	Assert.That(result, Is.Not.Null);
}
```

## Best Practices Used

1. **Isolation** - Services mocked, only testing single layer
2. **Clear Naming** - Test names describe scenario and expectation
3. **Single Assertion Focus** - Each test validates one behavior
4. **Setup/Teardown** - Proper resource cleanup in TearDown
5. **FluentAssertions** - Readable assertion format with NUnit
6. **Moq Verification** - Confirms methods called expected times

## Adding New Tests

When adding new functionality:

1. Create test method in appropriate fixture
2. Follow naming: `MethodName_Scenario_Expected`
3. Use test builders for data (SupportCaseTestDataBuilder)
4. Mock external dependencies (repositories, services)
5. Follow Arrange-Act-Assert pattern
6. Verify mocks if needed

Example:
```csharp
[Test]
public async Task NewMethod_WithValidInput_ShouldReturnExpectedResult()
{
	// Arrange
	var input = SupportCaseTestDataBuilder.CreateDefault().Build();

	// Act
	var result = await _service.NewMethod(input);

	// Assert
	Assert.That(result, Is.EqualTo(expectedValue));
	_mockDependency.Verify(m => m.Method(It.IsAny<T>()), Times.Once);
}
```

## Debugging Tests

### In Visual Studio
1. Set breakpoint in test
2. Right-click test in Test Explorer
3. Select "Debug Selected Tests"
4. Step through code normally

### InMemory Database Issues
- Each test gets unique database instance
- Data from one test doesn't affect another
- Use TestDbContextFactory.CreateInMemoryContext()

## CI/CD Integration

To integrate in GitHub Actions or other CI/CD:

```yaml
- name: Run NUnit Tests
  run: dotnet test source/CustomerCaseManagement/Tests.CaseManagement/Tests.CaseManagement.csproj --logger trx --results-directory TestResults
```

## Troubleshooting

**Tests not discovered:**
- Ensure test class has [TestFixture] attribute
- Ensure test methods have [Test] attribute
- Rebuild solution
- Clear Test Explorer cache

**Mock not working:**
- Verify Setup() is called before Act
- Check that method signature matches Setup
- Ensure Moq namespace is imported

**InMemory database errors:**
- Each test gets unique context instance
- Check for proper Setup/TearDown
- Verify entities are added before querying

## Performance Notes

- InMemory database is fast but doesn't validate constraints
- All tests run in < 1 second typically
- Mocking eliminates database calls
- Test isolation ensures good test performance
