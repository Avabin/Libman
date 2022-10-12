using Microsoft.AspNetCore.Mvc;
using Orleans;
using Orleans.Streams;
using Server.GrainInterfaces;
using Server.GrainInterfaces.Book;

namespace Server.Controllers;

[ApiController, Route("[controller]")]
public class BookStoreController : ControllerBase
{
    private static readonly string                       BookStoreKey = IBookStore.CreateKey("Mroczna 420", "Kraków");
    private readonly        IClusterClient               _clusterClient;
    private readonly        ILogger<BookStoreController> _logger;

    public BookStoreController(IClusterClient clusterClient, ILogger<BookStoreController> logger)
    {
        _clusterClient = clusterClient;
        _logger   = logger;
    }

    [HttpPut]
    public async Task AddBook(BookDto book)
    {
        var bookStore = _clusterClient.GetGrain<IBookStore>(BookStoreKey);
        var streamId  = await bookStore.GetStreamIdAsync();
        var handle = await _clusterClient.GetStreamProvider("default").GetStream<string>(streamId, "BookStore").SubscribeAsync(async (s,
            token) =>
        {
            _logger.LogInformation($"Received message: {s}");
        });
        await bookStore.AddBookAsync(book);

        await handle.UnsubscribeAsync();
    }

    [HttpGet("{isbn}")]
    public async Task<BookDto?> GetBook(string isbn) => await _clusterClient.GetGrain<IBookStore>(BookStoreKey).GetBookAsync(isbn);
    
    [HttpDelete("{isbn}")]
    public async Task DeleteBook(string isbn) => await _clusterClient.GetGrain<IBookStore>(BookStoreKey).RemoveBookAsync(isbn);
    
    [HttpPut("{isbn}/borrow")]
    public async Task BorrowBook(string isbn, Guid clientId) => await _clusterClient.GetGrain<IBookStore>(BookStoreKey).BorrowBookAsync(clientId, isbn);
    
    [HttpPut("{isbn}/return")]
    public async Task ReturnBook(string isbn, Guid clientId) => await _clusterClient.GetGrain<IBookStore>(BookStoreKey).ReturnBookAsync(clientId, isbn);
}