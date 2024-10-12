using System.ComponentModel.DataAnnotations;

namespace SaitynaiBackend.Data.Models;
public class Publisher
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Description { get; set; }
    public ICollection<Game> Games { get; set; } = new List<Game>();

    public PublisherGetDto ToDto()
    {
        return new PublisherGetDto
        {
            Id = Id,
            Name = Name,
            Description = Description
        };
    }
    public class PublisherGetDto()
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
    public class PublisherPostDto()
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
    public class PublisherPutDto()
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}


