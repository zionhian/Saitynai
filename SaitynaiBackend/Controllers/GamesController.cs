using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaitynaiBackend.Data.Models;
using SaitynaiBackend.Data;

[Route("api/publishers/{publisherId}/games")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly StoreDbContext _context;

    public GamesController(StoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game.GameGetDto>>> GetGames(int publisherId)
    {
        var publisherExists = await _context.Publishers.AnyAsync(p => p.Id == publisherId);
        if (!publisherExists)
        {
            return NotFound($"Publisher with ID {publisherId} not found.");
        }

        var games = await _context.Games
            .Where(g => g.Publisher.Id == publisherId)
            .Select(g => g.ToDto())
            .ToListAsync();

        return Ok(games);
    }

    // GET: api/publishers/{publisherId}/games/{id} (Get game by ID)
    [HttpGet("{id}")]
    public async Task<ActionResult<Game.GameGetDto>> GetGame(int publisherId, int id)
    {
        var publisherExists = await _context.Publishers.AnyAsync(p => p.Id == publisherId);
        if (!publisherExists)
        {
            return NotFound($"Publisher with ID {publisherId} not found.");
        }

        var game = await _context.Games
            .Where(g => g.Publisher.Id == publisherId && g.Id == id)
            .Select(g => g.ToDto())
            .FirstOrDefaultAsync();

        if (game == null)
        {
            return NotFound($"Game with ID {id} not found for publisher with ID {publisherId}.");
        }

        return Ok(game);
    }

    // POST: api/publishers/{publisherId}/games (Create a new game for a publisher)
    [HttpPost]
    public async Task<ActionResult<Game.GameGetDto>> CreateGame(int publisherId,[FromBody] Game.GamePostDto gameDto)
    {
        var publisher = await _context.Publishers.FindAsync(publisherId);
        if (publisher == null)
        {
            return NotFound($"Publisher with ID {publisherId} not found.");
        }

        var game = new Game
        {
            Title = gameDto.Title,
            Description = gameDto.Description,
            Publisher = publisher
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGame), new { publisherId = publisherId, id = game.Id }, game.ToDto());
    }

    // PUT: api/publishers/{publisherId}/games/{id} (Update game by ID)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGame(int publisherId, int id,[FromBody] Game.GamePutDto gameDto)
    {
        var publisherExists = await _context.Publishers.AnyAsync(p => p.Id == publisherId);
        if (!publisherExists)
        {
            return NotFound($"Publisher with ID {publisherId} not found.");
        }

        var game = await _context.Games
            .Where(g => g.Publisher.Id == publisherId && g.Id == id)
            .FirstOrDefaultAsync();

        if (game == null)
        {
            return NotFound($"Game with ID {id} not found for publisher with ID {publisherId}.");
        }

        game.Title = gameDto.Title;
        game.Description = gameDto.Description;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/publishers/{publisherId}/games/{id} (Delete game by ID)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int publisherId, int id)
    {
        var publisherExists = await _context.Publishers.AnyAsync(p => p.Id == publisherId);
        if (!publisherExists)
        {
            return NotFound($"Publisher with ID {publisherId} not found.");
        }

        var game = await _context.Games
            .Where(g => g.Publisher.Id == publisherId && g.Id == id)
            .FirstOrDefaultAsync();

        if (game == null)
        {
            return NotFound($"Game with ID {id} not found for publisher with ID {publisherId}.");
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
