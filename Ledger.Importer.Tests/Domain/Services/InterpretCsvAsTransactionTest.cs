using System.Text;
using FluentAssertions;
using Ledger.Importer.Domain.Exceptions;
using Ledger.Importer.Domain.Services;

namespace Ledger.Importer.Tests.Domain.Services;

public class InterpretCsvAsTransactionTest
{
    [Fact]
    public void EmptyCsvReturnsEmptyTransactionList()
    {
        var csvInterpret = new InterpretCsvAsTransactions();
        
        var transactions = csvInterpret.From(new MemoryStream());

        transactions.Should().BeEmpty();
    }
    
    [Fact]
    public void CsvWithOnlyHeadersReturnsEmptyTransactionList()
    {
        var csv = "description,amount,date";
        
        var csvStream = CreateCsvStream(csv);
        
        var csvInterpret = new InterpretCsvAsTransactions();
        
        var transactions = csvInterpret.From(csvStream);
        
        transactions.Should().BeEmpty();
    }
    
    [Fact]
    public void CsvWithSingleValidLineReturnsAProperlyFormattedTransaction()
    {
        var csv = "description,amount,date\nUber Eats Paris,29.90,2025-05-08T12:45:00Z";

        var csvStream = CreateCsvStream(csv);
        
        var csvInterpret = new InterpretCsvAsTransactions();
        
        var transactionList = csvInterpret.From(csvStream);

        var transactions = transactionList.ToList();
        transactions.Should().HaveCount(1);

        var transaction = transactions.First();
        transaction.Description.Should().Be("Uber Eats Paris");
        transaction.Amount.Should().Be(29.90m);
        transaction.Date.Should().Be(DateTime.Parse("2025-05-08T12:45:00Z"));
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
        var csv = "description,amount,date\nUber Eats Paris,29.90,2025-05-08T12:45:00Z";

        var csvStream = CreateCsvStream(csv);
        
        var csvInterpret = new InterpretCsvAsTransactions();
        
        var csvParsingAction = () => csvInterpret.From(csvStream);

        csvParsingAction.Should().Throw<InvalidCsvFormat>();
    }
    
    private static MemoryStream CreateCsvStream(string csvContent)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
    }
}