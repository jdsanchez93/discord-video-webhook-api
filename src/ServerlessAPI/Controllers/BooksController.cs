using Microsoft.AspNetCore.Mvc;
using ServerlessAPI.Entities;

namespace ServerlessAPI.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly ILogger<BooksController> logger;

    public BooksController(ILogger<BooksController> logger)
    {
        this.logger = logger;
    }

    // GET api/books
    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get([FromQuery] int limit = 10)
    {
        if (limit <= 0 || limit > 100) return BadRequest("The limit should been between [1-100]");

        return Ok(new Book());
    }

}
