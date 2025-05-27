using Ledger.Importer.Application.Commands;
using Ledger.Importer.Application.ReadModels;
using Ledger.Importer.Domain.Services;

namespace Ledger.Importer.Application.Handlers;

public static class ProcessTransactionsImport
{
    public static ImportedTransactions Execute(ImportTransactions command)
    {
        var items = command.Csv.Length == 0
            ? []
            : InterpretCsvAsTransactions.From(command.Csv).ToList();

        return new ImportedTransactions { Items = items };
    }
}