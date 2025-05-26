namespace Ledger.Importer.Domain.Services;

public static class CsvHeaderValidation
{
    public static bool IsStandardTransactionHeader(string? line)
    {
        if (string.IsNullOrWhiteSpace(line)) return false;

        
        var header = line.Split(',');
        
        if (header.Length != 3) return false;
        
        return header[0] == "description" 
               && header[1] == "amount" 
               && header[2] == "date";
    }
}