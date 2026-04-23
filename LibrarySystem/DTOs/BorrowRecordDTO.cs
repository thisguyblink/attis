using System.Data.Common;
using System.Diagnostics.Contracts;

public class CheckoutBookRequest
{
    public required int BookId { get; set; }
    public required int MemberId { get; set; }
}

public class ReturnBookRequest
{
    public required int BookId { get; set; }
    public required int MemberId { get; set; }
}

public class BorrowRecordResponse
{
    public required int Id { get; set; }
    public required int BookId { get; set; }

    public required int MemberId { get; set; }

    public required DateTime BorrowDate { get; set; }

    public required DateTime ReturnDate { get; set; }
    public required string Status { get; set; }
}
