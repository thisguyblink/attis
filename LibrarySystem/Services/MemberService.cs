public class MemberService : IMemberService
{
    private readonly IMemberRepository _repo;

    public MemberService(IMemberRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<MemberResponse>> GetAllAsync()
    {
        var members = await _repo.GetAllAsync();
        return members.Select(m => new MemberResponse
        {
            Id = m.Id,
            FullName = m.FullName,
            Email = m.Email,
            MembershipDate = m.MembershipDate
        }).ToList();
    }

    public async Task<MemberResponse?> GetByIdAsync(int id)
    {
        var member = await _repo.GetByIdAsync(id);
        if (member == null) return null;

        return new MemberResponse
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            MembershipDate = member.MembershipDate
        };
    }

    public async Task<MemberResponse> CreateAsync(CreateMemberRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new Exception("FullName is required");

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
            throw new Exception("A valid Email is required");

        if (await _repo.EmailExistsAsync(request.Email))
            throw new Exception("A member with this email already exists");

        var member = new Member
        {
            FullName = request.FullName,
            Email = request.Email,
            MembershipDate = DateTime.Now
        };

        await _repo.AddAsync(member);

        return new MemberResponse
        {
            Id = member.Id,
            FullName = member.FullName,
            Email = member.Email,
            MembershipDate = member.MembershipDate
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateMemberRequest request)
    {
        var member = await _repo.GetByIdAsync(id);
        if (member == null) return false;

        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new Exception("FullName is required");

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
            throw new Exception("A valid Email is required");

        member.FullName = request.FullName;
        member.Email = request.Email;

        await _repo.UpdateAsync(member);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var member = await _repo.GetByIdAsync(id);
        if (member == null) return false;

        await _repo.DeleteAsync(member);
        return true;
    }
}