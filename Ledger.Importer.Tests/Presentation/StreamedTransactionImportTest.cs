using FluentAssertions;
using Ledger.Importer.Domain.Entities;
using Ledger.Importer.Domain.ValueObjects;
using Ledger.Importer.Presentation.Http.Narration;
using Microsoft.AspNetCore.Http;

namespace Ledger.Importer.Tests.Presentation;

public class StreamedTransactionImportTest
{
    [Fact]
    public async Task WritesTransactionImportedEventToHttpResponse()
    {
        var body = new MemoryStream();
        var response = new DefaultHttpContext().Response;
        response.Body = body;

        var narrator = new StreamedTransactionImport(response);

        var transaction = new Transaction("Uber Eats Paris", 29.90m, TransactionDate.From("2025-05-27"));

        await narrator.NotifyTransactionImported(transaction);

        body.Position = 0;
        using var reader = new StreamReader(body);
        var output = await reader.ReadToEndAsync();

        output.Should().Contain("event: TransactionImported");
        output.Should().Contain("data:");
        output.Should().Contain("Uber Eats Paris");
        output.Should().Contain("29.90");
        output.Should().Contain("2025-05-27");
        output.Should().EndWith("\n\n");
    }
}