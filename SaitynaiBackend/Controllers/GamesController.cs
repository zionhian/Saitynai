using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaitynaiBackend.Data.Models;
using SaitynaiBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

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
            .Where(g => g.Publisher.Id == publisherId).Include(a => a.Publisher)
            .Select(g => g.ToDto())
            .ToListAsync();
        var userId = User.FindFirstValue("sub");
        var user = await _context.Users
            .Include(u => u.OwnedGames)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            foreach (var game in games) { 
                if (user.OwnedGames.Exists(o => o.Id == game.Id))
                        game.userOwnsGame = true;
            }
        }


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
            .Where(g => g.Publisher.Id == publisherId && g.Id == id).Include(a => a.Publisher)
            .Select(g => g.ToDto())
            .FirstOrDefaultAsync();

        if (game == null)
        {
            return NotFound($"Game with ID {id} not found for publisher with ID {publisherId}.");
        }
        var userId = User.FindFirstValue("sub");
        await Console.Out.WriteLineAsync(userId);
        var user = await _context.Users
            .Include(u => u.OwnedGames)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            if (user.OwnedGames.Exists(g => g.Id == game.Id))
            {
                game.userOwnsGame = true;

            }
        }
        return Ok(game);
    }

    // POST: api/publishers/{publisherId}/games (Create a new game for a publisher)
    [HttpPost]
    [Authorize(Roles = "Publisher")]
    public async Task<ActionResult<Game.GameGetDto>> CreateGame(int publisherId,[FromBody] Game.GamePostDto gameDto)
    {
        var userId = User.FindFirstValue("sub");
        var publisher = await _context.Publishers
            .Where(p => p.Id == publisherId && p.OwnerId == userId)
            .FirstOrDefaultAsync();
        if (publisher == null)
        {
            return Forbid();
        }

        var game = new Game
        {
            Title = gameDto.Title,
            Description = gameDto.Description,
            Publisher = publisher
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // PUT: api/publishers/{publisherId}/games/{id} (Update game by ID)
    [HttpPut("{id}")]
    [Authorize(Roles = "Publisher")]
    public async Task<IActionResult> UpdateGame(int publisherId, int id,[FromBody] Game.GamePutDto gameDto)
    {
        var userId = User.FindFirstValue("sub");
        var game = await _context.Games
            .Where(g => g.Publisher.Id == publisherId && g.Id == id && g.Publisher.OwnerId == userId)
            .FirstOrDefaultAsync();

        if (game == null)
        {
            return Forbid();
        }

        game.Title = gameDto.Title;
        game.Description = gameDto.Description;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/publishers/{publisherId}/games/{id} (Delete game by ID)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Publisher")]
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
        if (game.Owners.Any())
        {
            return BadRequest("Cannot delete the game because it has owners");
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpPost("{id}/buy")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> BuyGame(int id)
    {
        var userId = User.FindFirstValue("sub");
        var user = await _context.Users
            .Include(u => u.OwnedGames)
            .FirstOrDefaultAsync(u => u.Id == userId);



        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound("Game not found.");
        }

        if (user.OwnedGames.Contains(game))
        {
            return BadRequest("Game already owned by the user.");
        }

        user.OwnedGames.Add(game);
        await _context.SaveChangesAsync();

        return Ok("Game purchased successfully.");
    }
}
