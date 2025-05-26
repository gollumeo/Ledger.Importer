using FluentAssertions;
using Ledger.Importer.Application.ReadModels;
using Ledger.Importer.Domain.Entities;
using Ledger.Importer.Domain.ValueObjects;

namespace Ledger.Importer.Tests.Application.ReadModels;

public class ImportedTransactionsTest
{
    [Fact]
    public void IsEmptyReturnsTrueWhenNoTransactionIsPresent()
    {
        var result = new ImportedTransactions { Items = [] };

        result.IsEmpty.Should().BeTrue();
    }
    
    [Fact]
    public void CountReturnsNumberOfImportedTransactions()
    {
        var result = new ImportedTransactions {
            Items = new List<Transaction>
            {
                new("Uber Eats Paris", 29.90m, TransactionDate.From("2025-05-26")),
                new("Uber Mons", 200m, TransactionDate.From("2025-05-26")),
            }
        };
        
        result.Count.Should().Be(2);   
    }
}