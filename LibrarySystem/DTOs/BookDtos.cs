public class CreateBookRequest
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string ISBN { get; set; }
    public int TotalCopies { get; set; }
}

public class UpdateBookRequest
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public int TotalCopies { get; set; }
}

public class BookResponse
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string ISBN { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}