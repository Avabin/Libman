using Orleans;
using Server.GrainInterfaces;
using Server.GrainInterfaces.Book;

#pragma warning disable CS1998

namespace Server.Grains.BookStore;

public class BookStore : Grain<BookStoreState>, IBookStore
{
    private Guid _streamId => State.Id;
    public async Task AddBookAsync(BookDto book)
    {
        State = State with
        {
            Books = State.Books.Add(book)
        };

        await WriteStateAsync();
        await GetStreamProvider("default").GetStream<string>(_streamId, "BookStore").OnNextAsync("Book added");
    }

    public async Task RemoveBookAsync(string isbn)
    {
        State = State with
        {
            Books = State.Books.RemoveAll(x => x.Isbn == isbn)
        };
        
        await WriteStateAsync();
        
        await GetStreamProvider("default").GetStream<string>(_streamId, "BookStore").OnNextAsync("Book removed");
    }

    public async Task BorrowBookAsync(Guid clientId, string isbn)
    {
        await ReadStateAsync();
        var client = GrainFactory.GetGrain<IClient>(clientId);

        var oldValue = State.Books.First(x => x.Isbn == isbn);
        oldValue.LastBorrowedBy = clientId;
        oldValue.LastBorrowedOn = DateTimeOffset.Now;
        var books = State.Books.RemoveAll(x => x.Isbn == isbn).Add(oldValue);
        
        State = State with
        {
            Books = books
        };

        await client.BorrowAsync(isbn);
        
        await WriteStateAsync();
        
        await GetStreamProvider("default").GetStream<string>(_streamId, "BookStore").OnNextAsync("Book borrowed");
    }

    public async Task ReturnBookAsync(Guid clientId, string isbn)
    {
        var client = GrainFactory.GetGrain<IClient>(clientId);
        
        var oldValue = State.Books.First(x => x.Isbn == isbn);
        var books = State.Books.RemoveAll(x => x.Isbn == isbn).Add(oldValue);
        
        State = State with
        {
            Books = books
        };

        await client.ReturnAsync(isbn);
        
        await WriteStateAsync();
        
        await GetStreamProvider("default").GetStream<string>(_streamId, "BookStore").OnNextAsync("Book returned");
    }

    public async Task<BookDto?> GetBookAsync(string isbn) => State.Books.FirstOrDefault(x => x.Isbn == isbn);
    public async Task<Guid>     GetStreamIdAsync()        => _streamId;
}