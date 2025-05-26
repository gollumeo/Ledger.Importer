using Ledger.Importer.Application.Commands;
using Ledger.Importer.Domain.Entities;
using Ledger.Importer.Domain.Services;

namespace Ledger.Importer.Application.Handlers;

public static class ProcessTransactionsImport
{
   public static IEnumerable<Transaction> Execute(ImportTransactions command)
   {
       return command.Csv.Length == 0 ? [] : InterpretCsvAsTransactions.From(command.Csv);
   }
}