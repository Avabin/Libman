using Orleans;
using Server.GrainInterfaces;

namespace Server.Grains.Client;

public class Client : Grain<ClientState>, IClient
{
    public async Task BorrowAsync(string isbn)
    {
        State = State with
        {
            BorrowedBooks = State.BorrowedBooks.Add(new BorrowedBook(this.GetPrimaryKey(), isbn, DateTimeOffset.Now)) 
        };
    }

    public async Task ReturnAsync(string isbn)
    {
        State = State with
        {
            BorrowedBooks = State.BorrowedBooks.RemoveAll(b => b.Isbn == isbn)
        };
    }

    public async Task<IReadOnlyList<string>> GetBorrowedBooksAsync() => State.BorrowedBooks.Select(b => b.Isbn).ToList();
}