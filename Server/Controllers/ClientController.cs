using Microsoft.AspNetCore.Mvc;
using Orleans;
using Server.GrainInterfaces;
using Server.GrainInterfaces.Book;

namespace Server.Controllers;

[ApiController, Route("[controller]/{clientId:guid}")]
public class ClientController : ControllerBase
{
    private readonly IClusterClient _client;

    public ClientController(IClusterClient client)
    {
        _client = client;
    }
    [HttpGet("borrowedBooks")]
    public async Task<IReadOnlyList<string>> GetBorrowedBooks(Guid clientId) => await _client.GetGrain<IClient>(clientId).GetBorrowedBooksAsync();
    
}