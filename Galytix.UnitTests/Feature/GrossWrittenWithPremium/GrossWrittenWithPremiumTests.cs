using FluentAssertions;
using Galytix.WebApi.Domain.Entities;
using Galytix.WebApi.Feature.GrossWrittenPremium;
using LiteDB.Async;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Galytix.UnitTests.Feature.GrossWrittenWithPremium;

public class GrossWrittenPremiumHandlerTests
{
    private ILiteDatabaseAsync _db;
    private readonly ILogger<GrossWrittenPremiumHandler> _logger;

    public GrossWrittenPremiumHandlerTests()
    {
        _logger = Substitute.For<ILogger<GrossWrittenPremiumHandler>>();
        SetupLiteDb();
    }

    private void SetupLiteDb()
    {
        _db = new LiteDatabaseAsync(new MemoryStream());
        var collection = _db.GetCollection<GrossWrittenPremiumRecord>();
        collection.InsertBulkAsync(new[]
        {
            new GrossWrittenPremiumRecord
            {
                Country = "US",
                LineOfBusiness = "Auto",
                Sum = 1000m
            },
            new GrossWrittenPremiumRecord
            {
                Country = "UK",
                LineOfBusiness = "Home",
                Sum = 2000m
            },
            new GrossWrittenPremiumRecord
            {
                Country = "UK",
                LineOfBusiness = "Auto",
                Sum = 3000m
            }
        }).GetAwaiter().GetResult();
    }

    [Fact]
    public async Task Handle_ReturnsEmptyDictionary_WhenNoRecordsExist()
    {
        // Arrange
        var handler = new GrossWrittenPremiumHandler(_db, _logger);
        var request = new GetAverageRequest { Country = "ES", Lob = [] };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ReturnsDictionaryOfSums_WhenRecordsExist_WithoutLobFilter()
    {
        // Arrange
        var handler = new GrossWrittenPremiumHandler(_db, _logger);
        var request = new GetAverageRequest { Country = "UK", Lob = Array.Empty<string>() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(new KeyValuePair<string, decimal>("Auto", 3000m));
        result.Should().Contain(new KeyValuePair<string, decimal>("Home", 2000m));
    }

    [Fact]
    public async Task Handle_ReturnsDictionaryOfSums_WhenRecordsExist_WithLobFilter()
    {
        // Arrange
        var handler = new GrossWrittenPremiumHandler(_db, _logger);
        var request = new GetAverageRequest { Country = "US", Lob = new[] { "Auto" } };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(new KeyValuePair<string, decimal>("Auto", 1000m));
    }
}