using System.Collections.Immutable;
using Orleans.Concurrency;

namespace Server.Grains.Client;

[Immutable]
public record ClientState(string BookStoreKey, ImmutableList<BorrowedBook> BorrowedBooks)
{
    public ClientState() : this("", ImmutableList<BorrowedBook>.Empty)
    {
        
    }
}