﻿using Microsoft.AspNetCore.Mvc;
using SaitynaiBackend.Data.Models;
using SaitynaiBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SaitynaiBackend.Auth;
using System.Security.Claims;

[Route("api/publishers/{publisherId}/games/{gameId}/reviews")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly StoreDbContext _context;

    public ReviewsController(StoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]

    public async Task<ActionResult<IEnumerable<Review.ReviewGetDto>>> GetReviews(int publisherId, int gameId)
    {
        var publisherExists = await _context.Publishers.AnyAsync(p => p.Id == publisherId);
        if (!publisherExists)
        {
            return NotFound($"Publisher with ID {publisherId} not found.");
        }

        var gameExists = await _context.Games.AnyAsync(g => g.Publisher.Id == publisherId && g.Id == gameId);
        if (!gameExists)
        {
            return NotFound($"Game with ID {gameId} not found for publisher with ID {publisherId}.");
        }

        var reviews = await _context.Reviews
            .Where(r => r.Game.Publisher.Id == publisherId && r.Game.Id == gameId)
            .Select(r => r.ToGetDto())
            .ToListAsync();
        var userId = User.FindFirstValue("sub");
        var user = await _context.Users
            .Include(u => u.OwnedGames)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            foreach (var review in reviews)
            {
                if (user.Reviews.Exists(r => r.Author.Equals(user)))
                {
                    review.MyRating = true;
                }
                    
            }
        }
        return Ok(reviews);
    }

    // GET: api/publishers/{publisherId}/games/{gameId}/reviews/{id} (Get review by ID)
    [HttpGet("{id}")]

    public async Task<ActionResult<Review.ReviewGetDto>> GetReview(int publisherId, int gameId, int id)
    {
        var publisherExists = await _context.Publishers.AnyAsync(p => p.Id == publisherId);
        if (!publisherExists)
        {
            return NotFound($"Publisher with ID {publisherId} not found.");
        }

        var gameExists = await _context.Games.AnyAsync(g => g.Publisher.Id == publisherId && g.Id == gameId);
        if (!gameExists)
        {
            return NotFound($"Game with ID {gameId} not found for publisher with ID {publisherId}.");
        }

        var review = await _context.Reviews
            .Where(r => r.Game.Publisher.Id == publisherId && r.Game.Id == gameId && r.Id == id)
            .Select(r => r.ToGetDto())
            .FirstOrDefaultAsync();

        if (review == null)
        {
            return NotFound($"Review with ID {id} not found for game with ID {gameId} and publisher with ID {publisherId}.");
        }
        var userId = User.FindFirstValue("sub");
        var user = await _context.Users
            .Include(u => u.OwnedGames)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (userId != null)
        {
            if (user != null && user.Reviews.Exists(r => r.Author.Equals(user)))
            {
                review.MyRating = true;
            }
        }

        return Ok(review);
    }

    // POST: api/publishers/{publisherId}/games/{gameId}/reviews (Create a new review for a game)
    [HttpPost]
    [Authorize(Roles = UserStoreRoles.User)]
    public async Task<ActionResult<Review.ReviewGetDto>> CreateReview(int publisherId, int gameId, [FromBody] Review.ReviewPostDto reviewDto)
    {
        var userId = User.FindFirstValue("sub");
        var user = await _context.Users
            .Include(u => u.OwnedGames)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return Forbid();
        }

        var game = await _context.Games.FindAsync(gameId);
        if (game == null)
        {
            return NotFound("Game not found.");
        }

        var review = new Review
        {
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            Game = game,
            UserId = userId,
            Author = user
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // PUT: api/publishers/{publisherId}/games/{gameId}/reviews/{id} (Update review by ID)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int publisherId, int gameId, int id,[FromBody] Review.ReviewPutDto reviewDto)
    {
        var publisherExists = await _context.Publishers.AnyAsync(p => p.Id == publisherId);
        if (!publisherExists)
        {
            return NotFound($"Publisher with ID {publisherId} not found.");
        }

        var gameExists = await _context.Games.AnyAsync(g => g.Publisher.Id == publisherId && g.Id == gameId);
        if (!gameExists)
        {
            return NotFound($"Game with ID {gameId} not found for publisher with ID {publisherId}.");
        }

        var review = await _context.Reviews
            .Where(r => r.Game.Publisher.Id == publisherId && r.Game.Id == gameId && r.Id == id)
            .FirstOrDefaultAsync();
        if (review == null)
        {
            return NotFound($"Review with ID {id} not found for game with ID {gameId} and publisher with ID {publisherId}.");
        }
        var userId = User.FindFirstValue("sub");
        if (review.UserId != userId)
        {
            return Forbid();
        }
        if (reviewDto.Rating < 1 || reviewDto.Rating > 10)
        {
            await Console.Out.WriteLineAsync("Rating not in bounds");
            return UnprocessableEntity();
        }

        review.Rating = reviewDto.Rating;
        review.Comment = reviewDto.Comment;

        _context.Entry(review).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = UserStoreRoles.Admin)]
    public async Task<IActionResult> DeleteReview(int publisherId, int gameId, int id)
    {
        var publisherExists = await _context.Publishers.AnyAsync(p => p.Id == publisherId);
        if (!publisherExists)
        {
            return NotFound($"Publisher with ID {publisherId} not found.");
        }

        var gameExists = await _context.Games.AnyAsync(g => g.Publisher.Id == publisherId && g.Id == gameId);
        if (!gameExists)
        {
            return NotFound($"Game with ID {gameId} not found for publisher with ID {publisherId}.");
        }

        var review = await _context.Reviews
            .Where(r => r.Game.Publisher.Id == publisherId && r.Game.Id == gameId && r.Id == id)
            .FirstOrDefaultAsync();

        if (review == null)
        {
            return NotFound($"Review with ID {id} not found for game with ID {gameId} and publisher with ID {publisherId}.");
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
