using System.Collections.Concurrent;

public interface IBorrowRecordRepository
{
    Task<BorrowRecord> CheckoutBookAsync(BorrowRecord borrowRecord);
    Task<BorrowRecord> ReturnBookAsync(BorrowRecord borrowRecord);
    Task<List<BorrowRecord>> GetAllBorrowRecordAsync();
    Task<List<BorrowRecord>> GetMemberBorrowRecordAsync(int memberId);
    Task<BorrowRecord?> GetRecordByIds(int bookId, int memeberId);

    Task<bool> CheckBookAvaliable(int bookId);
}