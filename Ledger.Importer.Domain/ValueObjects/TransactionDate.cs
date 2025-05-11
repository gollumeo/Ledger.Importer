using System.Globalization;
using Ledger.Importer.Domain.Exceptions;

namespace Ledger.Importer.Domain.ValueObjects;

public readonly struct TransactionDate
{
    public DateTime Date { get; }

    public TransactionDate(DateTime date)
    {
        Date = date;
    }

    public static TransactionDate From(string isoDate)
    {
        if (string.IsNullOrWhiteSpace(isoDate))
        {
            throw new InvalidTransactionData("Date cannot be empty.");
        }
        
        if (!DateTime.TryParse(isoDate, null, DateTimeStyles.RoundtripKind, out var parsed))
        {
            throw new InvalidTransactionData($"Invalid date format: {isoDate}.");
        } 
        
        return new TransactionDate(parsed);
    }
    
    public override string ToString() => Date.ToString("O");

    public static implicit operator DateTime(TransactionDate date) => date.Date;
    public static implicit operator TransactionDate(DateTime date) => new(date);
}