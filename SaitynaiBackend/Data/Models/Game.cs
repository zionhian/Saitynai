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
        public List<StoreUser> Owners { get; set; } = new List<StoreUser>();

        public GameGetDto ToDto()
        {
            return new GameGetDto
            {
                Id = Id,
                Title = Title,
                Description = Description,
                PublisherUserId = Publisher.OwnerId
            };
        }
        public class GameGetDto()
        {
            public int Id { get; set; }
            [Required]
            public  string Title { get; set; }
            [Required]
            public string Description { get; set; }
            public bool userOwnsGame { get; set; }
            public string PublisherUserId { get; set; }
        }
        public class GamePostDto()
        {
            [Required]
            public required string Title { get; set; }
            [Required]
            public required string Description { get; set; }
        }
        public class GamePutDto
        {
            [Required]
            public required string Title { get; set; }
            [Required]
            public required string Description { get; set; }
        }
    }
}

