using Orleans;

namespace Server.GrainInterfaces;

/// <summary>
/// Bookstore client
/// </summary>
public interface IClient : IGrainWithGuidKey
{
    /// <summary>
    /// Borrow the book from the library
    /// </summary>
    /// <param name="isbn">The book to borrow</param>
    /// <returns>A task that represents the return operation</returns>
    Task BorrowAsync(string isbn);

    /// <summary>
    /// Return the book to the library
    /// </summary>
    /// <param name="isbn">The book to return</param>
    /// <returns>A task that represents the return operation</returns>
    Task ReturnAsync(string isbn);

    Task<IReadOnlyList<string>> GetBorrowedBooksAsync();
}