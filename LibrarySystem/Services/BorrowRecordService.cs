using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Net;
using Microsoft.Extensions.Caching.Memory;

public class BorrowRecordService : IBorrowRecordService
{
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IMemoryCache _cache;
    private const string AllBorrowRecordsCacheKey = "AllBorrowRecords";

    public BorrowRecordService(IBorrowRecordRepository borrowRecordRepository, IMemoryCache cache)
    {
        _borrowRecordRepository = borrowRecordRepository;
        _cache = cache;
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

        _cache.Remove(AllBorrowRecordsCacheKey);
        
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

        _cache.Remove(AllBorrowRecordsCacheKey);

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
        if (_cache.TryGetValue(AllBorrowRecordsCacheKey, out List<BorrowRecordResponse> cachedRecords)) {
            Console.WriteLine("Cache hit, cache being returned");
            return cachedRecords;
        }
        var borrowRecords = await _borrowRecordRepository.GetAllBorrowRecordAsync();
        
        var result = borrowRecords.Select(record => new BorrowRecordResponse 
        {
            Id = record.Id,
            BookId = record.BookId,
            MemberId = record.MemberId,
            BorrowDate = record.BorrowDate,
            ReturnDate = record.ReturnDate,
            Status = record.Status

        }).ToList();

        _cache.Set(AllBorrowRecordsCacheKey, result, TimeSpan.FromMinutes(10));
        Console.WriteLine("Cache not hit, result saved to cache");

        return result;
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


    
    
    