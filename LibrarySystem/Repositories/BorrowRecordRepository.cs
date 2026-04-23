using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;

public class BorrowRecordRepository : IBorrowRecordRepository
{
    private readonly AppDbContext _context;

    public BorrowRecordRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<BorrowRecord> CheckoutBookAsync(BorrowRecord borrowRecord)
    {
        _context.BorrowRecords.AddAsync(borrowRecord);
        await _context.SaveChangesAsync();
        return borrowRecord;
    }
    public Task<BorrowRecord> ReturnBookAsync(BorrowRecord borrowRecord)
    {
        _context.BorrowRecords.UpdateAsync(borrowRecord);
        await _context.SaveChangesAsync();
        return borrowRecord;
    }
    public async Task<List<BorrowRecord>> GetAllBorrowRecordAsync()
    {
        return await _context.BorrowRecords.GetAllAsync().ToListAsync();
    }
    public async Task<List<BorrowRecord>> GetMemberBorrowRecordAsync(int memberId)
    {
        return await _context.BorrowRecords.GetByIdAsync(memberId).ToListAsync();
    }
}