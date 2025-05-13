using System.Text;
using FluentAssertions;
using Ledger.Importer.Domain.Exceptions;
using Ledger.Importer.Domain.Services;
using Xunit.Abstractions;

namespace Ledger.Importer.Tests.Domain.Services;

public class InterpretCsvAsTransactionTest
{

    [Fact]
    public void EmptyCsvReturnsEmptyTransactionList()
    {
        const string csv = "description,amount,date";
        var csvStream = CreateCsvStream(csv);
        
        var transactions = InterpretCsvAsTransactions.From(csvStream);

        transactions.Should().BeEmpty();
    }
    
    [Fact]
    public void CsvWithOnlyHeadersReturnsEmptyTransactionList()
    {
        const string csv = "description,amount,date";
        
        var csvStream = CreateCsvStream(csv);
        
        var transactions = InterpretCsvAsTransactions.From(csvStream);
        
        transactions.Should().BeEmpty();
    }
    
    [Fact]
    public void CsvWithSingleValidLineReturnsAProperlyFormattedTransaction()
    {
        const string csv = "description,amount,date\nUber Eats Paris,29.90,2025-05-08T12:45:00Z";

        var csvStream = CreateCsvStream(csv);
        
        var transactionList = InterpretCsvAsTransactions.From(csvStream);

        var transactions = transactionList.ToList();
        transactions.Should().HaveCount(1);

        var transaction = transactions.First();
        transaction.Description.Should().Be("Uber Eats Paris");
        transaction.Amount.Should().Be(29.90m);
        var expectedDate = DateTime.SpecifyKind(
            DateTime.Parse("2025-05-08T12:45:00"), DateTimeKind.Utc);

        transaction.Date.Value.Should().Be(expectedDate);
    }
    
    [Fact]
    public void CsvWithSingleValidLineAndIncorrectLineReturnsAProperlyFormattedTransaction()
    {
        const string csv = "description,amount,date\nUber Eats Paris,29.90,2025-05-08T12:45:00Z\nUber,";
        
        var csvStream = CreateCsvStream(csv);
        
        var transactions = InterpretCsvAsTransactions.From(csvStream).ToList();
        transactions.Should().HaveCount(1);        
    }
    
    [Fact]
    public void CsvWithInvalidHeadersThrows()
    {
        const string csv = "desc,amount,date";

        var csvStream = CreateCsvStream(csv);
        
        var csvParsingAction = () => InterpretCsvAsTransactions.From(csvStream);

        csvParsingAction.Should().Throw<InvalidCsvFormat>();
    }
    
    private static MemoryStream CreateCsvStream(string csvContent)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
    }
}