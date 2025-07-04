﻿using System.Globalization;
using Ledger.Importer.Domain.Entities;
using Ledger.Importer.Domain.Exceptions;
using Ledger.Importer.Domain.Validation;
using Ledger.Importer.Domain.ValueObjects;

namespace Ledger.Importer.Domain.Services;

public static class InterpretCsvAsTransactions
{
    public static IEnumerable<Transaction> From(Stream csvStream)
    {
        using var reader = new StreamReader(csvStream);
        var headerLine = reader.ReadLine();

        if (!CsvHeaderValidation.IsStandardTransactionHeader(headerLine))
            throw new InvalidCsvFormat("Invalid CSV headers.");

        var transactions = new List<Transaction>();

        while (!reader.EndOfStream)
        {
            var dataLine = reader.ReadLine();

            if (dataLine != null && TryParseLine(dataLine, out var transaction))
            {
                transactions.Add(transaction);
            }
        }

        return transactions;
    }

    public static bool TryParseLine(string dataLine, out Transaction transaction)
    {
        transaction = null!;

        if (string.IsNullOrWhiteSpace(dataLine)) return false;

        var parts = dataLine.Split(',');
        if (parts.Length != 3) return false;

        var description = parts[0].Trim();
        var amountString = parts[1].Trim();
        var dateString = parts[2].Trim();

        if (string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(amountString) ||
            string.IsNullOrWhiteSpace(dateString))
            return false;

        if (!decimal.TryParse(amountString, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
            return false;

        try
        {
            var date = TransactionDate.From(dateString);
            transaction = new Transaction(description, amount, date);
            return true;
        }
        catch
        {
            return false;
        }
    }
}