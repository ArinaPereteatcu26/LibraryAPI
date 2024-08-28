using AutoMapper;
using FluentValidation;
using LibraryDataAccess.NewFolder;
using LibraryDataAccess.Repository;
using LibraryWebAPI.Dtos.Books;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookDto> _createValidator;
        private readonly IValidator<UpdateBookDto> _updateValidator;

        public BooksController(
            ILogger<BooksController> logger,
            IBookRepository bookRepository,
            IMapper mapper,
            IValidator<CreateBookDto> createValidator,
            IValidator<UpdateBookDto> updateValidator)
        {
            _logger = logger;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<PaginatedList<BookDto>> Get(int page = 1, int nr = 10)
        {
            var books = await _bookRepository.GetBooksAsync(page, nr);
            return _mapper.Map<PaginatedList<BookDto>>(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookDto>(book));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookDto book)
        {
            var validationResult = await _createValidator.ValidateAsync(book);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var bookToCreate = _mapper.Map<Book>(book);
            var createResult = await _bookRepository.CreateBookAsync(bookToCreate);
            return Ok(_mapper.Map<BookDto>(createResult));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateBookDto book)
        {
            var validationResult = await _updateValidator.ValidateAsync(book);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var bookToUpdate = _mapper.Map<Book>(book);

            try
            {
                var updateResult = await _bookRepository.UpdateBookAsync(bookToUpdate);
                return Ok(_mapper.Map<BookDto>(updateResult));
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookRepository.DeleteBookAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
