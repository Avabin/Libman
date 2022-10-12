using Orleans.Concurrency;

namespace Server.Grains.Client;

/// <summary>
/// Borrowed book value object
/// </summary>
/// <param name="ClientId">Id of the client who borrowed the book</param>
/// <param name="Isbn">ISBN number of the book that was borrowed</param>
/// <param name="BorrowedAt">Time when the book was borrowed</param>
[Immutable]
public record BorrowedBook(Guid ClientId, string Isbn, DateTimeOffset BorrowedAt)
{
    // parameterless constructor for serialization
    public BorrowedBook() : this(Guid.Empty, "", DateTimeOffset.MinValue)
    {
    }
}