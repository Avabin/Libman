using System.Collections.Immutable;
using Orleans.Concurrency;
using Server.GrainInterfaces.Book;

namespace Server.Grains.BookStore;

[Immutable]
public record BookStoreState(Guid Id, ImmutableList<BookDto> Books)
{
    public BookStoreState() : this(Guid.NewGuid(), ImmutableList<BookDto>.Empty)
    {
    }
}