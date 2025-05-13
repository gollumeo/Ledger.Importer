namespace Ledger.Importer.Application.Commands;

public sealed class ImportTransactions
{
    public Stream Csv { get; }
    
    public ImportTransactions(Stream csv)
    {
        Csv = csv ?? throw new ArgumentNullException(nameof(csv));
    }
}