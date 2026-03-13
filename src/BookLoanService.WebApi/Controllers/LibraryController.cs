namespace BookLoanService.WebApi.Controllers;

[ApiController]
[Route("api/library")]
public class LibraryController : ControllerBase
{
    private readonly ILibraryService _bookService;
    private readonly IErrorToHttpMapper _errorMapper;

    public LibraryController(ILibraryService bookService, IErrorToHttpMapper errorMapper)
    {
        _bookService = bookService;
        _errorMapper = errorMapper;
    }

    [HttpGet("getAllBooks")]
    public IActionResult GetAllBooks()
    {
        var books = _bookService.GetAllBooks();

        return Ok(books);
    }

    [HttpPost("addBook")]
    public IActionResult AddBook([FromBody] AddBookRequest request)
    {
        _bookService.AddBook(request.Title, request.Author);

        return Ok();
    }

    [HttpPost("borrowBooks")]
    [ProducesResponseType(typeof(BorrowBookResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public IActionResult BorrowBooks([FromBody] BorrowBooksRequest request)
    {
        var command = new BorrowBooksCommand(request.CustomerId, request.BookIds);

        var result = _bookService.BorrowBooks(command);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return _errorMapper.Map(result);
    }
}
