public class CreateMemberRequest
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
}

public class UpdateMemberRequest
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
}

public class MemberResponse
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public DateTime MembershipDate { get; set; }
}