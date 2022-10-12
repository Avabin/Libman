using System.ComponentModel.DataAnnotations;

namespace Server.GrainInterfaces.Book;

public class BookDto
{
    public string Isbn { get;                   set; } = "";
    public string         Title          { get; set; } = "";
    public string         Author         { get; set; } = "";
    public string         Genre          { get; set; } = "";
    public Guid           LastBorrowedBy { get; set; } = Guid.Empty;
    public DateTimeOffset LastBorrowedOn { get; set; } = DateTimeOffset.MinValue;
    
}