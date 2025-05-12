using System.Globalization;
using Ledger.Importer.Domain.Entities;
using Ledger.Importer.Domain.Exceptions;
using Ledger.Importer.Domain.ValueObjects;

namespace Ledger.Importer.Domain.Services;

public sealed class InterpretCsvAsTransactions
{
    public static IEnumerable<Transaction> From(Stream csvStream)
    {
        var transactions = new List<Transaction>();

        using var reader = new StreamReader(csvStream);
        var headerLine = reader.ReadLine();

        if (string.IsNullOrWhiteSpace(headerLine))
        {
            return transactions;
        }

        var header = headerLine.Split(',');

        if (header.Length != 3 ||
            header[0].Trim() != "description" ||
            header[1].Trim() != "amount" ||
            header[2].Trim() != "date")
        {
            throw new InvalidCsvFormat("Invalid CSV headers.");
        }

        while (!reader.EndOfStream)
        {
            var dataLine = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(dataLine))
                continue;

            var parts = dataLine.Split(',');
            if (parts.Length != 3)
                continue;

            var description = parts[0].Trim();
            var amountString = parts[1].Trim();
            var dateString = parts[2].Trim();

            if (string.IsNullOrWhiteSpace(description) ||
                string.IsNullOrWhiteSpace(amountString) ||
                string.IsNullOrWhiteSpace(dateString))
                continue;

            if (!decimal.TryParse(amountString, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
                continue;

            TransactionDate date;

            try
            {
                date = TransactionDate.From(dateString);
            }
            catch
            {
                continue;
            }

            transactions.Add(new Transaction(description, amount, date));
        }

        return transactions;
    }
}