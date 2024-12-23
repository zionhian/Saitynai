using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaitynaiBackend.Data.Models;
using SaitynaiBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SaitynaiBackend.Auth;
using System.Security.Claims;

[Route("api/publishers")]
[ApiController]
public class PublishersController : ControllerBase
{
    private readonly StoreDbContext _context;
    private readonly UserManager<StoreUser> _userManager;

    public PublishersController(StoreDbContext context, UserManager<StoreUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [HttpGet]
    public async Task<ActionResult<List<Publisher.PublisherGetDto>>> GetPublishers()
    {
        var publishers = (await _context.Publishers.ToListAsync()).Select(pub => pub.ToDto());
        return Ok(publishers);
    }

    [HttpPost]
    [Authorize(Roles = UserStoreRoles.Admin)]
    public async Task<ActionResult<Publisher.PublisherGetDto>> CreatePublisher([FromBody] Publisher.PublisherPostDto publisherDto)
    {
        var userId = User.FindFirstValue("sub");
        if (userId == null)
        {
            return BadRequest("User is already a publisher");
        }
        await Console.Out.WriteLineAsync(userId);
        var user = await _context.Users
            .Include(u => u.OwnedGames)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return BadRequest("User not found");
        }

        var publisher = new Publisher
        {
            Name = publisherDto.Name,
            Description = publisherDto.Description,
            OwnerId = userId
        };
        await _userManager.AddToRoleAsync(user, "Publisher");

        _context.Publishers.Add(publisher);
        await _context.SaveChangesAsync();

        return Ok();
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
    [Authorize(Roles = UserStoreRoles.Admin)]
    public async Task<IActionResult> UpdatePublisher(int id, [FromBody] Publisher.PublisherPutDto publisherDto)
    {
        var userId = User.FindFirstValue("sub");
        var publisher = await _context.Publishers
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (publisher == null)
        {
            return Forbid();
        }

        publisher.Name = publisherDto.Name;
        publisher.Description = publisherDto.Description;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserStoreRoles.Admin)]
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
    /*[HttpPost("{id}/assign")]
    [Authorize(Roles = UserStoreRoles.Admin)]
    public async Task<IActionResult> AssignUserAsPublisher(int id, [FromBody] string userId)
    {
        var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (user.PublishCompany != null)
        {
            return BadRequest("This user is already a publisher for another company.");
        }

        var publisher = await _context.Publishers.FindAsync(id);
        if (publisher == null)
        {
            return NotFound("Publisher not found.");
        }

        user.PublishCompany = publisher;
        await _userManager.AddToRoleAsync(user, "Publisher");
        await _context.SaveChangesAsync();

        return NoContent();*/
    //}
}
