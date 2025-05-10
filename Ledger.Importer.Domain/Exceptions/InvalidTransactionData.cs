namespace Ledger.Importer.Domain.Exceptions;

public class InvalidTransactionData(string message) : Exception(message);