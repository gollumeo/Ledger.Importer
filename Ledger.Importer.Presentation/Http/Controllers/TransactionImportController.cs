using Ledger.Importer.Application.Commands;
using Ledger.Importer.Application.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Importer.Presentation.Http.Controllers;

[ApiController]
[Route("transactions")]
public sealed class TransactionImportController : ControllerBase
{
    [HttpPost("import")]
    public IActionResult Import([FromForm] IFormFile file)
    {
        if (file.Length == 0)
        {
            return BadRequest("No file provided.");
        }
        
        using var stream = file.OpenReadStream();

        var command = new ImportTransactions(stream);

        var transactions = ProcessTransactionsImport.Execute(command);

        return Ok(transactions);
    }
}