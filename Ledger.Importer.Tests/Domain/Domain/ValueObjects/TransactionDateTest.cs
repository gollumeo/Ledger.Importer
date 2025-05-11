using FluentAssertions;
using Ledger.Importer.Domain.Exceptions;
using Ledger.Importer.Domain.ValueObjects;

namespace Ledger.Importer.Tests.Domain.Domain.ValueObjects;

public class TransactionDateTest
{
    [Fact]
    public void TransactionDateIsCreatedSuccessfullyWithValidData()
    {
        var now = DateTime.UtcNow;
        var transactionDate = new TransactionDate(now);
        
        transactionDate.Date.Should().Be(now);
    }
    
    [Fact]
    public void TransactionDateIsCreatedFromIsoFormat()
    {
        var date = TransactionDate.From("2025-05-10T12:45:00Z");
        
        date.Should().Be(new TransactionDate(new DateTime(2025, 05, 10, 12, 45, 00)));
    }
    
    [Fact]
    public void TransactionDateIsCreatedFromSimplifiedString()
    {
        var date = TransactionDate.From("2025-05-10");
        
        date.Should().Be(new TransactionDate(new DateTime(2025, 05, 10, 12, 45, 00)));
    }
    
    [Fact]
    public void ExceptionIsThrownWhenDatStringIsInvalid()
    {
        var transactionDateConstruction = () => TransactionDate.From("INVALID_DATE");
        
        transactionDateConstruction.Should().Throw<InvalidTransactionData>().WithMessage("Invalid date format: INVALID_DATE.");   
    }
    
    [Fact]
    public void ExceptionIsThrowWhenDateStringIsEmpty()
    {
        var transactionDateConstruction = () => TransactionDate.From("    ");
        
        transactionDateConstruction.Should().Throw<InvalidTransactionData>().WithMessage("Date cannot be empty.");
    }
}