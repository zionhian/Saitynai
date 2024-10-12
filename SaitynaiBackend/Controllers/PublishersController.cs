using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaitynaiBackend.Data.Models;
using SaitynaiBackend.Data;

[Route("api/publishers")]
[ApiController]
public class PublishersController : ControllerBase
{
    private readonly StoreDbContext _context;

    public PublishersController(StoreDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<List<Publisher.PublisherGetDto>>> GetPublishers()
    {
        var publishers = (await _context.Publishers.ToListAsync()).Select(pub => pub.ToDto());
        return Ok(publishers);
    }

    [HttpPost]
    public async Task<ActionResult<Publisher.PublisherGetDto>> CreatePublisher([FromBody]Publisher.PublisherPostDto publisherDto)
    {
        var publisher = new Publisher
        {
            Name = publisherDto.Name,
            Description = publisherDto.Description
        };

        _context.Publishers.Add(publisher);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPublisher), new { id = publisher.Id }, publisher.ToDto());
    }

    // GET: Get a specific publisher
    [HttpGet("{id}")]
    public async Task<ActionResult<Publisher.PublisherGetDto>> GetPublisher(int id)
    {
        var publisher = await _context.Publishers.FindAsync(id);
        if (publisher == null)
        {
            return NotFound();
        }

        return Ok(publisher.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePublisher(int id, [FromBody] Publisher.PublisherPutDto publisherDto)
    {
        var publisher = await _context.Publishers.FindAsync(id);
        if (publisher == null)
        {
            return NotFound();
        }

        publisher.Name = publisherDto.Name;
        publisher.Description = publisherDto.Description;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePublisher(int id)
    {
        var publisher = await _context.Publishers.FindAsync(id);
        if (publisher == null)
        {
            return NotFound();
        }

        _context.Publishers.Remove(publisher);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
