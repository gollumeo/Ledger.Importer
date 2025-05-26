using Ledger.Importer.Application.Commands;
using Ledger.Importer.Application.Contracts;
using Ledger.Importer.Domain.Exceptions;
using Ledger.Importer.Domain.Services;

namespace Ledger.Importer.Application.Handlers;

public static class StreamTransactionsImport
{
    public static async Task ExecuteAsync(ImportTransactions command, INarrateTransactionsImportLive narrator)
    {
        using var reader = new StreamReader(command.Csv);
        var header = await reader.ReadLineAsync();

        if (!CsvHeaderValidation.IsStandardTransactionHeader(header))
            throw new InvalidCsvFormat("Invalid CSV headers.");

        var total = 0;
        var imported = 0;

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            total++;

            if (line is not null && InterpretCsvAsTransactions.TryParseLine(line, out var transaction))
            {
                await narrator.NotifyTransactionImported(transaction);
                imported++;
            }
            else
            {
                await narrator.NotifyTransactionFailed(total, "Invalid or unparseable line.");
            }
        }

        var failed = total - imported;

        await narrator.NotifyImportCompleted(total, failed);
    }
}