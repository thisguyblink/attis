public interface IMemberRepository
{
    Task<List<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task<Member> AddAsync(Member member);
    Task UpdateAsync(Member member);
    Task DeleteAsync(Member member);
    Task<bool> EmailExistsAsync(string email);
}