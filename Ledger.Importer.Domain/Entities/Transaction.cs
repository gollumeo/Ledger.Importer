using Ledger.Importer.Domain.Exceptions;
using Ledger.Importer.Domain.ValueObjects;

namespace Ledger.Importer.Domain.Entities;

public sealed class Transaction
{
    public string Description { get; }
    public decimal Amount { get; }
    public TransactionDate Date { get; }

    public Transaction(string description, decimal amount, TransactionDate date)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidTransactionData("Description is required.");

        if (amount == 0)
            throw new InvalidTransactionData("Amount cannot be zero.");

        Description = description;
        Amount = amount;
        Date = date;
    }
}