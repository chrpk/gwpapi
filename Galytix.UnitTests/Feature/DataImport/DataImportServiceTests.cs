using System.IO.Abstractions;
using System.Linq.Expressions;
using System.Text;
using FluentAssertions;
using Galytix.WebApi.Domain.Entities;
using Galytix.WebApi.Feature.DataImport.Services;
using LiteDB.Async;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Galytix.UnitTests.Feature.DataImport;

public class DataImportServiceTests
{
    private string CsvHeaderAndRow =
        "country,lineOfBusiness,Y2000" + Environment.NewLine +
        "DE,Life,1000";

    [Fact]
    public async Task LoadAsync_WhenCollectionIsEmptyAndFileExists_InsertsRecordsAndLogsInformation()
    {
        // Arrange
        var fakeDb = Substitute.For<ILiteDatabaseAsync>();
        var fakeColl = Substitute.For<ILiteCollectionAsync<GrossWrittenPremiumRecord>>();
        var fakeFsFile = Substitute.For<IFile>();
        var fakeFs = Substitute.For<IFileSystem>();
        var fakeLogger = Substitute.For<ILogger<DataImportService>>();

        // Db setup
        fakeDb.GetCollection<GrossWrittenPremiumRecord>()
            .Returns(fakeColl);
        fakeColl.CountAsync().Returns(Task.FromResult(0));

        // FileSystem setup
        fakeFs.File.Returns(fakeFsFile);
        fakeFsFile.Exists(Arg.Any<string>()).Returns(true);
        fakeFsFile.OpenText(Arg.Any<string>())
            .Returns(ci =>
            {
                var bytes = Encoding.UTF8.GetBytes(CsvHeaderAndRow);
                var ms = new MemoryStream(bytes);
                return new StreamReader(ms, Encoding.UTF8);
            });

        var svc = new DataImportService(fakeDb, fakeFs, fakeLogger);

        // Act
        await svc.LoadAsync("some/path.csv");

        // Assert
        // It should have called InsertBulkAsync with at least one record
        await fakeColl.Received(1)
            .InsertBulkAsync(Arg.Is<GrossWrittenPremiumRecord[]>(arr => arr.Length == 1));
    }

    [Fact]
    public async Task LoadAsync_WhenCollectionAlreadyExists_LogsAndReturnsImmediately()
    {
        // Arrange
        var fakeDb = Substitute.For<ILiteDatabaseAsync>();
        var fakeColl = Substitute.For<ILiteCollectionAsync<GrossWrittenPremiumRecord>>();
        var fakeFs = Substitute.For<IFileSystem>();
        var fakeLogger = Substitute.For<ILogger<DataImportService>>();

        fakeDb.GetCollection<GrossWrittenPremiumRecord>().Returns(fakeColl);
        fakeColl.CountAsync().Returns(Task.FromResult(42)); // non‐zero

        var svc = new DataImportService(fakeDb, fakeFs, fakeLogger);

        // Act
        await svc.LoadAsync("ignored.csv");

        // Assert
        // Should never attempt to read files or write to the DB
        await fakeColl.DidNotReceive().EnsureIndexAsync(Arg.Any<string>(),
            Arg.Any<Expression<Func<GrossWrittenPremiumRecord, object>>>());
        await fakeColl.DidNotReceive().InsertBulkAsync(Arg.Any<GrossWrittenPremiumRecord[]>());

        // Should log the early‐exit message
        fakeLogger.Received(1).LogInformation(
            "The collection already exists. No need to load the data again."
        );
    }

    [Xunit.Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task LoadAsync_WithNullOrWhitespaceFilePath_ThrowsArgumentNullException(string path)
    {
        // Arrange
        var svc = new DataImportService(
            Substitute.For<ILiteDatabaseAsync>(),
            Substitute.For<IFileSystem>(),
            Substitute.For<ILogger<DataImportService>>()
        );

        // Act
        var act = () => svc.LoadAsync(path);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("filePath");
    }

    [Fact]
    public async Task LoadAsync_WhenFileDoesNotExist_ThrowsFileNotFoundException()
    {
        // Arrange
        var fakeDb = Substitute.For<ILiteDatabaseAsync>();
        var fakeColl = Substitute.For<ILiteCollectionAsync<GrossWrittenPremiumRecord>>();
        var fakeFsFile = Substitute.For<IFile>();
        var fakeFs = Substitute.For<IFileSystem>();
        var fakeLogger = Substitute.For<ILogger<DataImportService>>();

        fakeDb.GetCollection<GrossWrittenPremiumRecord>().Returns(fakeColl);
        fakeColl.CountAsync().Returns(Task.FromResult(0));

        fakeFs.File.Returns(fakeFsFile);
        fakeFsFile.Exists(Arg.Any<string>()).Returns(false);

        var svc = new DataImportService(fakeDb, fakeFs, fakeLogger);

        // Act
        var act = () => svc.LoadAsync("nonexistent.csv");

        // Assert
        await act.Should().ThrowAsync<FileNotFoundException>()
            .WithMessage("The file 'nonexistent.csv' does not exist.");
    }
}