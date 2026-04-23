using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]

public class BorrowRecordController : ControllerBase
{
    private readonly IBorrowRecordService _borrowRecordService;

    public BorrowRecordController(IBorrowRecordService borrowRecordService)
    {
        _borrowRecordService = borrowRecordService;
    }

    [HttpGet] 
    public async Task<IActionResult> GetAllBorrowRecords()
    {
        var borrowRecords = await _borrowRecordService.AllBorrowRecords();
        return Ok(borrowRecords);
    }

    [HttpGet("{id}")]
    public async  Task<IActionResult>GetMemberBorrowRecords(int memberId)
    {
        try {
            var memberBorrowRecords = await _borrowRecordService.MemberBorrowRecords(memberId);
            return Ok(memberBorrowRecords);

        } catch (Exception e) {
            return BadRequest(new { error = e.Message });
        }

    }

    [HttpPost]

    public async Task<IActionResult> CheckoutBook(CheckoutBookRequest checkoutBookRequest)
    {
        try
        {
            var BorrowRecordResponse = await _borrowRecordService.CheckoutBook(checkoutBookRequest);
            return Ok(BorrowRecordResponse);

        } catch (Exception e ) {
            return BadRequest(new { error = e.Message });
        }
        
    }

    [HttpPost]
    public async Task<IActionResult> ReturnBook(ReturnBookRequest returnBookRequest)
    {
        try
        {
            var BorrowRecordResponse = await _borrowRecordService.ReturnBook(returnBookRequest);
            return Ok(BorrowRecordResponse);
            
        } catch (Exception e) {
            return BadRequest(new { error = e.Message });
        }
    }

}