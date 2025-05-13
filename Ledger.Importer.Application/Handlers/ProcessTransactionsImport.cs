using Ledger.Importer.Application.Commands;
using Ledger.Importer.Domain.Entities;
using Ledger.Importer.Domain.Services;

namespace Ledger.Importer.Application.Handlers;

public class ProcessTransactionsImport
{
   public static IEnumerable<Transaction> Execute(ImportTransactions command)
    {
        return InterpretCsvAsTransactions.From(command.Csv);
    }
}