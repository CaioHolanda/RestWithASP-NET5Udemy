using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNETUdemy.Model;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Data.VO;

namespace RestWithASPNETUdemy.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class BookController : ControllerBase
    {
        public readonly ILogger<BookController> _logger;
        private IBookBusiness _bookService;
        public BookController(ILogger<BookController> logger, IBookBusiness bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_bookService.FindAll());
        }
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var book=_bookService.FindById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
        [HttpPost]
        public IActionResult Post([FromBody] BookVO book)
        {
            
            if (book == null)
            {
                return BadRequest();
            }
            return Ok(_bookService.Create(book));
        }   
        [HttpPut]
        public IActionResult Put([FromBody] BookVO book)
        {
            
            if (book == null)
            {
                return BadRequest();
            }
            return Ok(_bookService.Update(book));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _bookService.Delete(id);
            return NoContent();
        }

    }
}
