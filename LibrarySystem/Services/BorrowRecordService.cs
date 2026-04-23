using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;

public class BorrowRecordService : IBorrowRecordService
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public BorrowRecordService(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
    } 

    public async Task<BorrowRecordResponse> CheckoutBook(CheckoutBookRequest checkoutBookRequest)
    {
        var bookId = Guid.NewGuid();
        var borrowRecord = new BorrowRecord()

    }

    public async Task<BorrowRecordResponse> ReturnBook(ReturnBookRequest returnBookRequest)
    {
        
    }

    public async Task<List<BorrowRecordResponse>> AllBorrowRecords()
    {
        var borrowRecords = await _borrowRecordRepository.GetAllAsync();

        return borrowRecords.Select(record => BorrowRecordResponse{
            Id = record.Id,
            BookId = record.BookId,
            MemberId = record.MemberId,
            BorrowDate = record.BorrowDate,
            Status = record.status

        }).ToList();
    }

    public async Task<List<BorrowRecordResponse>> MemberBorrowRecords(int MemberId)
    {
        
    }
}


    
    
    