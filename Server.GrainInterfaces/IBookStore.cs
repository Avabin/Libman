using Orleans;
using Server.GrainInterfaces.Book;

namespace Server.GrainInterfaces;

public interface IBookStore : IGrainWithStringKey
{
    public static string                       CreateKey(string       street, string city) => $"{street}-{city}";
    public static (string street, string city) ParseKey(string        key) => (key.Split('-')[0], key.Split('-')[1]);
    Task                                       AddBookAsync(BookDto   book);
    Task                                       RemoveBookAsync(string isbn);
    Task                                       BorrowBookAsync(Guid   clientId, string isbn);
    Task                                       ReturnBookAsync(Guid   clientId, string isbn);
    Task<BookDto?>                                     GetBookAsync(string isbn);

    Task<Guid> GetStreamIdAsync();
}