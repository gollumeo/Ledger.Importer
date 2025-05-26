using Ledger.Importer.Domain.Entities;

namespace Ledger.Importer.Application.ReadModels;

public sealed class ImportedTransactions
{
    public required IReadOnlyCollection<Transaction> Items { get; init; }

    public int Count => Items.Count;
    public bool IsEmpty => Items.Count == 0;
}