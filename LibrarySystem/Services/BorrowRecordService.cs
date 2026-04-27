using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Net;
using Microsoft.Extensions.Caching.Memory;

public class BorrowRecordService : IBorrowRecordService
{
  private readonly IBorrowRecordRepository _borrowRecordRepository;
  private readonly IBookRepository _bookRepository;
  private readonly IMemoryCache _cache;
    private const string AllBorrowRecordsCacheKey = "AllBorrowRecords";

  public BorrowRecordService(
      IBorrowRecordRepository borrowRecordRepository, IBookRepository bookRepository, IMemoryCache cache) {
    _borrowRecordRepository = borrowRecordRepository;
    _bookRepository = bookRepository;
    _cache = cache;
  }

  public async Task<BorrowRecordResponse> CheckoutBook(CheckoutBookRequest checkoutBookRequest)
  {
    var book = await _bookRepository.GetByIdAsync(checkoutBookRequest.BookId);

    if (book == null)
      throw new KeyNotFoundException("Book not found.");

    if (book.AvailableCopies <= 0)
      throw new InvalidOperationException("No available copies available.");

    var date = DateTime.Now;

    book.AvailableCopies--;
    _cache.Remove(AllBorrowRecordsCacheKey);
    var borrowRecord = new BorrowRecord
    {
      BookId = checkoutBookRequest.BookId,
      MemberId = checkoutBookRequest.MemberId,
      BorrowDate = date,
      ReturnDate = null,
      Status = "Borrowed"
    };

    await _bookRepository.UpdateAsync(book);
    await _borrowRecordRepository.CheckoutBookAsync(borrowRecord);

    return new BorrowRecordResponse
    {
      Id = borrowRecord.Id,
      BookId = borrowRecord.BookId,
      MemberId = borrowRecord.MemberId,
      BorrowDate = borrowRecord.BorrowDate,
      ReturnDate = borrowRecord.ReturnDate,
      Status = borrowRecord.Status
    };
  }

  public async Task<BorrowRecordResponse> ReturnBook(ReturnBookRequest returnBookRequest)
  {
    var record = await _borrowRecordRepository.GetRecordByIds(
        returnBookRequest.BookId,
        returnBookRequest.MemberId
    );

    if (record == null)
      throw new KeyNotFoundException("Borrow record not found.");

    var book = await _bookRepository.GetByIdAsync(returnBookRequest.BookId);

    if (book == null)
      throw new KeyNotFoundException("Book not found.");

    record.ReturnDate = DateTime.Now;
    record.Status = "Returned";

    if (book.AvailableCopies < book.TotalCopies)
    {
      book.AvailableCopies++;
    }
    _cache.Remove(AllBorrowRecordsCacheKey);

    await _bookRepository.UpdateAsync(book);
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
