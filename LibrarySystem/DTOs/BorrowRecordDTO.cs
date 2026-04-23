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
    public required string Action { get; set; }
    public required bool Status { get; set; }
    public required int IdAssociated { get; set; } 
}


public class BorrowRecordResponse
{
    public required int Id { get; set; }
    public required int BookId { get; set; }

    public required int MemberId { get; set; }

    public required string BorrowDate { get; set; }

    public string ReturnDate { get; set; }
    public required string Status { get; set; }
}
