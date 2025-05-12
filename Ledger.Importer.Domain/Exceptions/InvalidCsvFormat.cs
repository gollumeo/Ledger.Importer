namespace Ledger.Importer.Domain.Exceptions;

public class InvalidCsvFormat(string message) : Exception(message);