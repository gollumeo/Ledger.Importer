using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Ledger.Importer.Tests.Integration;

public class TransactionImportIntegrationTest(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ImportWithRealCsvFileWorks()
    {
        var csvPath = Path.Combine(AppContext.BaseDirectory, "sample-transactions.csv");

        var bytes = await File.ReadAllBytesAsync(csvPath);
        var content = new MultipartFormDataContent
        {
            {
                new ByteArrayContent(bytes)
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("text/csv") }
                },
                "file",
                "sample-transactions.csv"
            }
        };

        var response = await _client.PostAsync("/transactions/import", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<dynamic>>();
        
        result!.Count.Should().Be(5);
    }
}