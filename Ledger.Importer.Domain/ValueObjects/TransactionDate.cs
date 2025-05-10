namespace Ledger.Importer.Domain.ValueObjects;

public readonly struct TransactionDate(DateTime date)
{
    public DateTime Date { get; } = date;
}