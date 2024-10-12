using System.ComponentModel.DataAnnotations;

namespace SaitynaiBackend.Data.Models
{
    public class Game
    {
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required Publisher Publisher { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public GameGetDto ToDto()
        {
            return new GameGetDto
            {
                Id = Id,
                Title = Title,
                Description = Description
            };
        }
        public class GameGetDto()
        {
            public int Id { get; set; }
            public required string Title { get; set; }
            public required string Description { get; set; }
        }
        public class GamePostDto()
        {
            public required string Title { get; set; }
            public required string Description { get; set; }
        }
        public class GamePutDto
        {
            public required string Title { get; set; }
            public required string Description { get; set; }
        }
    }
}

