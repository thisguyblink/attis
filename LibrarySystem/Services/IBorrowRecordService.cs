public interface IBorrowRecordService
{
    Task<BorrowRecordResponse> CheckoutBook(CheckoutBookRequest checkoutBookRequest);
    Task<BorrowRecordResponse> ReturnBook(ReturnBookRequest returnBookRequest);
    Task<List<BorrowRecordResponse>> AllBorrowRecords();
    Task<List<BorrowRecordResponse>> MemberBorrowRecords(int MemberId);
}