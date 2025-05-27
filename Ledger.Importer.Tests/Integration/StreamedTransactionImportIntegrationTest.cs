using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Ledger.Importer.Tests.Integration;

public class StreamedTransactionImportIntegrationTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    [Fact]
    public async Task StreamedImportEmitsTransactionEvents()
    {
        var response = await _client.GetAsync("/transactions/import/stream", HttpCompletionOption.ResponseHeadersRead); // continuous stream

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("text/event-stream");

        var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        var output = await reader.ReadToEndAsync();
        
        output.Should().Contain("event: TransactionImported");
        output.Should().Contain("event: ImportCompleted");
    }
}