public interface IMemberService
{
    Task<List<MemberResponse>> GetAllAsync();
    Task<MemberResponse?> GetByIdAsync(int id);
    Task<MemberResponse> CreateAsync(CreateMemberRequest request);
    Task<bool> UpdateAsync(int id, UpdateMemberRequest request);
    Task<bool> DeleteAsync(int id);
}