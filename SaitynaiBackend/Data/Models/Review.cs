using System.ComponentModel.DataAnnotations;

namespace SaitynaiBackend.Data.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        public required int Rating { get; set; }
        public string? Comment { get; set; }
        [Required]
        public required Game Game { get; set; }

        public ReviewGetDto ToGetDto()
        {
            return new ReviewGetDto
            {
                Id = Id,
                Rating = Rating,
                Comment = Comment,
            };
        }
        public class ReviewGetDto()
        {
            public int Id { get; set; }
            public int Rating { get; set; }
            public string? Comment { get; set; }

        }
        public class ReviewPutDto()
        {
            [Required]
            public required int Rating { get; set; }
            public string? Comment { get; set; }
        }
        public
          class ReviewPostDto()
        {
            [Required]
            public required int Rating { get; set; }
            public string? Comment { get; set; }

        }

    }

}
