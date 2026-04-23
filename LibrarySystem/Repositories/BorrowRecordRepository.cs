using System.Diagnostics.Contracts;
using System.Drawing;
using System.Net;
using Microsoft.EntityFrameworkCore;

public class BorrowRecordRepository : IBorrowRecordRepository
{
    private readonly AppDbContext _context;

    public BorrowRecordRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BorrowRecord> CheckoutBookAsync(BorrowRecord borrowRecord)
    {
        await _context.BorrowRecords.AddAsync(borrowRecord);
        await _context.SaveChangesAsync();
        return borrowRecord;
    }
    public async Task<BorrowRecord> ReturnBookAsync(BorrowRecord borrowRecord)
    {
        _context.BorrowRecords.Update(borrowRecord);
        await _context.SaveChangesAsync();
        return borrowRecord;
    }
    public async Task<List<BorrowRecord>> GetAllBorrowRecordAsync()
    {
        return await _context.BorrowRecords.ToListAsync();
    }
    public async Task<List<BorrowRecord>> GetMemberBorrowRecordAsync(int memberId)
    {
        return await _context.BorrowRecords
            .Where(r => r.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<BorrowRecord?> GetRecordByIds(int bookId, int memberId)
    {
        return await _context.BorrowRecords
            .FirstOrDefaultAsync(r => r.BookId == bookId && r.MemberId == memberId && r.Status == "Checked Out");
    }

    public async Task<bool> CheckBookAvaliable(int bookId)
    {
        var record = await _context.BorrowRecords.FirstOrDefaultAsync(r => r.BookId == bookId && r.Status == "Checked Out");
        if (record == null)
        {
            return true;
        } 
        return false; 
    }
}