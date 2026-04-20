using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _service;

    public BooksController(IBookService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var books = await _service.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _service.GetByIdAsync(id);
        if (book == null) return NotFound(new { error = "Book not found" });

        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookRequest request)
    {
        try
        {
            var book = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateBookRequest request)
    {
        try
        {
            var success = await _service.UpdateAsync(id, request);
            if (!success) return NotFound(new { error = "Book not found" });

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound(new { error = "Book not found" });

        return Ok();
    }
}