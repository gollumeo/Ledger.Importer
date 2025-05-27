using System.Text;
using FluentAssertions;
using Ledger.Importer.Application.Commands;
using Ledger.Importer.Application.Handlers;
using Ledger.Importer.Domain.Exceptions;
using Ledger.Importer.Tests.Fakes;

namespace Ledger.Importer.Tests.Application;

public class StreamTransactionsImportTest
{
    [Fact]
    public async Task NotifiesNarratorForEachValidTransaction()
    {
        const string csv = """
                           description,amount,date
                           Uber Eats Paris,29.90,2025-05-13
                           Uber Mons,200,2025-05-13
                           """;
        
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
        var command = new ImportTransactions(stream);
        var narrator = new FakeNarrateTransactionsImportLive();

        await StreamTransactionsImport.ExecuteAsync(command, narrator);

        narrator.Imported.Should().HaveCount(2);
        narrator.Failed.Should().BeEmpty();
        narrator.Completed.Should().Be((2, 0));       
    }
    
    [Fact]
    public void ThrowsIfHeadersAreInvalid()
    {
        const string csv = """
                           desc,amnt,dt
                           Uber Eats Paris,29.90,2025-05-13
                           Uber Mons,200,2025-05-13
                           """;
        
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
        var command = new ImportTransactions(stream);
        var narrator = new FakeNarrateTransactionsImportLive();

        var action = async () => await StreamTransactionsImport.ExecuteAsync(command, narrator);

        action.Should().ThrowAsync<InvalidCsvFormat>();
    }
}