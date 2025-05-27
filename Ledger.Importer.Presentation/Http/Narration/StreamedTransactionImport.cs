using System.Text.Json;
using Ledger.Importer.Application.Contracts;
using Ledger.Importer.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Ledger.Importer.Presentation.Http.Narration;

public class StreamedTransactionImport(HttpResponse response) : INarrateTransactionsImportLive
{
    public async Task NotifyTransactionImported(Transaction transaction)
    {
        var json = JsonSerializer.Serialize(new
        {
            transaction.Description,
            transaction.Amount,
            Date = transaction.Date.Value.ToString("O"),
        });

        await response.WriteAsync("event: TransactionImported\n");
        await response.WriteAsync($"data: {json}\n\n");
        await response.Body.FlushAsync();
    }

    public async Task NotifyTransactionFailed(int lineNumber, string reason)
    {
        var json = JsonSerializer.Serialize(new
        {
            line = lineNumber,
            reason,
        });
        
        await response.WriteAsync("event: TransactionFailed\n");
        await response.WriteAsync($"data: {json}\n\n");
        await response.Body.FlushAsync();
    }

    public async Task NotifyImportCompleted(int total, int failed)
    {
        var json = JsonSerializer.Serialize(new
        {
            total,
            failed,
        });
        
        await response.WriteAsync("event: ImportCompleted\n");
        await response.WriteAsync($"data: {json}\n\n");
        await response.Body.FlushAsync();
    }
}