using Ledger.Importer.Domain.Entities;

namespace Ledger.Importer.Application.Contracts;

public interface INarrateTransactionsImportLive
{
    Task NotifyTransactionImported(Transaction transaction);
    Task NotifyTransactionFailed(int lineNumber, string reason);
    Task NotifyImportCompleted(int total, int failed);
}