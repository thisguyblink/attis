using System.Collections.Concurrent;

public interface IBorrowRecordRepository
{
    Task<BorrowRecord> CheckoutBookAsync(BorrowRecord borrowRecord);
    Task<BorrowRecord> ReturnBookAsync(BorrowRecord borrowRecord);
    Task<List<BorrowRecord>> GetAllBorrowRecordAsync();
    Task<List<BorrowRecord>> GetMemberBorrowRecordAsync(int memberId);
}