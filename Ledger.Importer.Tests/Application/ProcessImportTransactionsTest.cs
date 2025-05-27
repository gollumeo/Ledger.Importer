using System.Text;
using FluentAssertions;
using Ledger.Importer.Application.Commands;
using Ledger.Importer.Application.Handlers;
using Ledger.Importer.Domain.Exceptions;

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
    
    [Fact]
    public void ExecutionWithEmptyStreamReturnsEmptyList()
    {
        const string csv = "";
        var csvStream = CreateCsvStream(csv);
        var command = new ImportTransactions(csvStream);
        
        var transactions = ProcessTransactionsImport.Execute(command);

        transactions.Should().BeEmpty();
    }
    
    [Fact]
    public void ExecutionWithNullStreamThrows()
    {
        var commandConstruction = () => new ImportTransactions(null!);
        
        commandConstruction.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void ExecutionWithInvalidCsvStreamThrows()
    {
        const string csv = "desc,amnt, dte\nUber Eats Paris,29.90,2025-05-13";
        var csvStream = CreateCsvStream(csv);
        var command = new ImportTransactions(csvStream);
        
        var commandExecution = () => ProcessTransactionsImport.Execute(command);

        commandExecution.Should().Throw<InvalidCsvFormat>("Invalid CSV headers.");
    }
    
    private static MemoryStream CreateCsvStream(string csvContent)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
    }
}