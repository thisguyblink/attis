using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Net;

public class BorrowRecordService : IBorrowRecordService
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public BorrowRecordService(IBorrowRecordRepository borrowRecordRepository)
    {
        _borrowRecordRepository = borrowRecordRepository;
    } 

    public async Task<BorrowRecordResponse> CheckoutBook(CheckoutBookRequest checkoutBookRequest)
    {
        if (!await _borrowRecordRepository.CheckBookAvaliable(checkoutBookRequest.BookId))
        {
            throw new InvalidOperationException("Book is already checked out.");
        }

        var date = DateTime.Now;
        var borrowRecord = new BorrowRecord
        {
            BookId = checkoutBookRequest.BookId,
            MemberId = checkoutBookRequest.MemberId,
            BorrowDate = date,
            ReturnDate = null,
            Status = "Checked Out"
        };
        
        await _borrowRecordRepository.CheckoutBookAsync(borrowRecord);

        return new BorrowRecordResponse
        {
            Id = borrowRecord.Id, 
            BookId = checkoutBookRequest.BookId,
            MemberId = checkoutBookRequest.MemberId,
            BorrowDate = date,
            ReturnDate = null,
            Status = "Checked Out"
        };

    }

    public async Task<BorrowRecordResponse> ReturnBook(ReturnBookRequest returnBookRequest)
    {
        var record = await _borrowRecordRepository.GetRecordByIds(returnBookRequest.BookId, returnBookRequest.MemberId);
        
        if (record == null)
            throw new KeyNotFoundException("Borrow record not found.");

        record.ReturnDate = DateTime.Now;
        record.Status = "Returned";   

        await _borrowRecordRepository.ReturnBookAsync(record);

        return new BorrowRecordResponse
        {
            Id = record.Id,
            BookId = record.BookId,
            MemberId = record.MemberId,
            BorrowDate = record.BorrowDate,
            ReturnDate = record.ReturnDate,
            Status = record.Status
        };
    }

    public async Task<List<BorrowRecordResponse>> AllBorrowRecords()
    {
        var borrowRecords = await _borrowRecordRepository.GetAllBorrowRecordAsync();

        return borrowRecords.Select(record => new BorrowRecordResponse 
        {
            Id = record.Id,
            BookId = record.BookId,
            MemberId = record.MemberId,
            BorrowDate = record.BorrowDate,
            ReturnDate = record.ReturnDate,
            Status = record.Status

        }).ToList();
    }

    public async Task<List<BorrowRecordResponse>> MemberBorrowRecords(int MemberId)
    {
        var memberBorrowRecords = await _borrowRecordRepository.GetMemberBorrowRecordAsync(MemberId);

        return memberBorrowRecords.Select(record => new BorrowRecordResponse 
        {
            Id = record.Id,
            BookId = record.BookId,
            MemberId = record.MemberId,
            BorrowDate = record.BorrowDate,
            ReturnDate = record.ReturnDate,
            Status = record.Status

        }).ToList();
    }
}


    
    
    