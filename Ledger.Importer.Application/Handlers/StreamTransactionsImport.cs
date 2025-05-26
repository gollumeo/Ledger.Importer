using Ledger.Importer.Application.Commands;
using Ledger.Importer.Application.Contracts;
using Ledger.Importer.Domain.Entities;
using Ledger.Importer.Domain.Services;

namespace Ledger.Importer.Application.Handlers;

public static class StreamTransactionsImport
{
    public static async Task ExecuteAsync(ImportTransactions command, INarrateTransactionsImportLive narrator)
    {
        using var reader = new StreamReader(command.Csv);
        var header = await reader.ReadLineAsync();
        
        while(!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (line is not null && InterpretCsvAsTransactions.TryParseLine(line, out var transaction))
            {
                await narrator.NotifyTransactionImported(transaction);
            }

            await narrator.NotifyImportCompleted(2, 0);
        }
    }
}