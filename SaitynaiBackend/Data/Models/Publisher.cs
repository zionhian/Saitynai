using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaitynaiBackend.Data.Models;
public class Publisher
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Description { get; set; }
    public ICollection<Game> Games { get; set; } = new List<Game>();
    public string OwnerId { get; set; }
    [Required]

    [ForeignKey("OwnerId")]
    public StoreUser Owner { get; set; }

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
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
    }
    public class PublisherPostDto()
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required string UserId { get; set; }
    }
    public class PublisherPutDto()
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
    }
}


