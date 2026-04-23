using System.ComponentModel.DataAnnotations;

public class BorrowRecord
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int BookId { get; set; }

    [Required]
    public int MemberId { get; set; }

    [Required]
    public string BorrowDate { get; set; }

    public string ReturnDate { get; set; }
    
    [Required]
    public string Status { get; set; }


}
