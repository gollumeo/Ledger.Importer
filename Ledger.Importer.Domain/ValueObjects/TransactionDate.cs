using System.Globalization;
using Ledger.Importer.Domain.Exceptions;

namespace Ledger.Importer.Domain.ValueObjects;

public readonly struct TransactionDate
{
    public DateTime Value { get; }

    public TransactionDate(DateTime value)
    {
        Value = value;
    }

    public static TransactionDate From(string isoDate)
    {
        if (string.IsNullOrWhiteSpace(isoDate))
        {
            throw new InvalidTransactionData("Date cannot be empty.");
        }
        
        if (!DateTime.TryParse(isoDate, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out var parsed))
        {
            throw new InvalidTransactionData($"Invalid date format: {isoDate}.");
        } 
        
        return new TransactionDate(parsed);
    }
    
    public override string ToString() => Value.ToString("O");

    public static implicit operator DateTime(TransactionDate date) => date.Value;
    public static implicit operator TransactionDate(DateTime date) => new(date);
}