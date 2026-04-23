using System.ComponentModel.DataAnnotations;

public class Member
{
    public int Id { get; set; }

    [Required]
    public required string FullName { get; set; }

    [Required]
    public required string Email { get; set; }

    public DateTime MembershipDate { get; set; }
}