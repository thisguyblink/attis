public class BookService : IBookService
{
    private readonly IBookRepository _repo;

    public BookService(IBookRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<BookResponse>> GetAllAsync()
    {
        var books = await _repo.GetAllAsync();

        return books.Select(b => new BookResponse
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            ISBN = b.ISBN,
            TotalCopies = b.TotalCopies,
            AvailableCopies = b.AvailableCopies
        }).ToList();
    }

    public async Task<BookResponse?> GetByIdAsync(int id)
    {
        var book = await _repo.GetByIdAsync(id);
        if (book == null) return null;

        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies
        };
    }

    public async Task<BookResponse> CreateAsync(CreateBookRequest request)
    {
        if (request.TotalCopies <= 0)
            throw new Exception("TotalCopies must be greater than 0");

        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            ISBN = request.ISBN,
            TotalCopies = request.TotalCopies,
            AvailableCopies = request.TotalCopies
        };

        await _repo.AddAsync(book);

        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateBookRequest request)
    {
        var book = await _repo.GetByIdAsync(id);
        if (book == null) return false;

        if (request.TotalCopies < book.AvailableCopies)
            throw new Exception("TotalCopies cannot be less than AvailableCopies");

        book.Title = request.Title;
        book.Author = request.Author;
        book.TotalCopies = request.TotalCopies;

        await _repo.UpdateAsync(book);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _repo.GetByIdAsync(id);
        if (book == null) return false;

        await _repo.DeleteAsync(book);
        return true;
    }
}