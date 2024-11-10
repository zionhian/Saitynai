using Microsoft.AspNetCore.Identity;

namespace SaitynaiBackend.Data.Models
{
    public class StoreUser : IdentityUser
    {
        public List<Game> OwnedGames { get; set; } = new List<Game>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public Publisher PublishCompany { get; set; }

    }
}
