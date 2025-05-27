using Ledger.Importer.Application.Commands;
using Ledger.Importer.Application.Handlers;
using Ledger.Importer.Presentation.Http.Narration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ledger.Importer.Presentation.Http.Controllers;

[ApiController]
[Route("transactions/import")]
public sealed class TransactionImportController : ControllerBase
{
    [HttpPost("")]
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
    
    [HttpGet("stream")]
    public async Task GetStreamedImport()
    {
        Response.Headers.Add("Content-Type", "text/event-stream");

        var path = Path.Combine("storage", "sample-transactions.csv");
        
        if(!System.IO.File.Exists(path))
        {
            Response.StatusCode = 404;
            await Response.WriteAsync("event: Error\ndata: File not found.\n\n");
            await Response.Body.FlushAsync();
            return;
        }
        
        var stream = System.IO.File.OpenRead(path);
        var command = new ImportTransactions(stream);

        var narrator = new StreamedTransactionImport(Response);

        await StreamTransactionsImport.ExecuteAsync(command, narrator);
    }
}