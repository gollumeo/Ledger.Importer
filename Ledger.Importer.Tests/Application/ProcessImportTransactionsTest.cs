using System.Text;
using FluentAssertions;
using Ledger.Importer.Application.Commands;
using Ledger.Importer.Application.Handlers;
using Xunit.Abstractions;

namespace Ledger.Importer.Tests.Application;

public class ProcessImportTransactionsTest
{

    [Fact]
    public void ExecutionWithValidCsvStreamReturnsTransactions()
    {
        const string csv = "description,amount,date\nUber Eats Paris,29.90,2025-05-13";
        var csvStream = CreateCsvStream(csv);
        var command = new ImportTransactions(csvStream);
        
        var transactions = ProcessTransactionsImport.Execute(command);
        
        transactions.Should().HaveCount(1);
    }
    
    private static MemoryStream CreateCsvStream(string csvContent)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
    }
}