using Ledger.Importer.Application.Contracts;
using Ledger.Importer.Domain.Entities;

namespace Ledger.Importer.Tests.Fakes;

public class FakeNarrateTransactionsImportLive : INarrateTransactionsImportLive
{
    public List<Transaction> Imported { get; } = [];
    public List<(int Line, string Reason)> Failed { get; } = [];
    public (int Total, int Failed)? Completed { get; private set; }
    
    public Task NotifyTransactionImported(Transaction transaction)
    {
        Imported.Add(transaction);
        return Task.CompletedTask;
    }

    public Task NotifyTransactionFailed(int lineNumber, string reason)
    {
        Failed.Add((lineNumber, reason));
        return Task.CompletedTask;
    }

    public Task NotifyImportCompleted(int total, int failed)
    {
        Completed = (total, failed);
        return Task.CompletedTask;
    }
}