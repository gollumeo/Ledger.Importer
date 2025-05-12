using FluentAssertions;
using Ledger.Importer.Domain.Services;

namespace Ledger.Importer.Tests.Domain.Services;

public class InterpretCsvAsTransactionTest
{
    [Fact]
    public void EmptyCsvReturnsEmptyTransactionList()
    {
        var csvInterpret = new InterpretCsvAsTransactions();
        
        var transactions = csvInterpret.From(new MemoryStream());

        transactions.Should().BeNullOrEmpty();
    }
    
    [Fact]
    public void CsvWithOnlyHeadersReturnsEmptyTransactionList()
    {
        var csvInterpret = new InterpretCsvAsTransactions();
        
        var transactions = csvInterpret.From(new MemoryStream());
        
        transactions.Should().BeNullOrEmpty();
    }
    
    [Fact]
    public void CsvWithSingleValidLineReturnsAProperlyFormattedTransaction()
    {
        var csvInterpret = new InterpretCsvAsTransactions();
        
        var transactions = csvInterpret.From(new MemoryStream());
        
        transactions.Count().Should().Be(1);
    }
    
    [Fact]
    public void CsvWithSingleValidLineAndIncorrectLineReturnsAProperlyFormattedTransaction()
    {
        var csvInterpret = new InterpretCsvAsTransactions();
        
        var transactions = csvInterpret.From(new MemoryStream());
        
        transactions.Count().Should().Be(1);
    }
    
    [Fact]
    public void CsvWithInvalidHeadersThrows()
    {
        var csvInterpret = new InterpretCsvAsTransactions();
        
        var csvParsingAction = () => csvInterpret.From(new MemoryStream());

        csvParsingAction.Should().Throw<Exception>();
    }
}