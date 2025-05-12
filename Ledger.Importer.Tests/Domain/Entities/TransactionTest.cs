
using FluentAssertions;
using Ledger.Importer.Domain.Entities;
using Ledger.Importer.Domain.Exceptions;
using Ledger.Importer.Domain.ValueObjects;

namespace Ledger.Importer.Tests.Domain.Domain.Entities;

public class TransactionTest
{
    [Fact]
    public void TransactionIsCreatedSuccessfullyWithValidData()
    {
        var date = new TransactionDate(DateTime.UtcNow);
        var transaction = new Transaction("Uber Eats Paris", 29.90m, date);
        
        transaction.Description.Should().Be("Uber Eats Paris");
        transaction.Amount.Should().Be(29.90m);
        transaction.Date.Should().Be(date);
    }
    
    [Fact]
    public void ExceptionIsThrownWhenDescriptionIsEmpty()
    {
        var transactionConstruction = () => new Transaction("   ", 29.90m, new TransactionDate(DateTime.UtcNow));

        transactionConstruction.Should().Throw<InvalidTransactionData>();
    }
    
    [Fact]
    public void ExceptionIsThrownWhenAmountIsZero()
    {
        var transactionConstruction = () => new Transaction("Uber Eats Paris", 0m, new TransactionDate(DateTime.UtcNow));
        
        transactionConstruction.Should().Throw<InvalidTransactionData>();
    }
    
    [Fact]
    public void TransactionIsCreatedWIthNegativeAmount()
    {
        var date = new TransactionDate(DateTime.UtcNow);
        
        var transaction = new Transaction("Uber Eats Paris", -29.90m, date);
        
        transaction.Description.Should().Be("Uber Eats Paris");
        transaction.Amount.Should().Be(-29.90m);
        transaction.Date.Should().Be(date);
    }
}