using Jilo.App.Domain.Entities;

namespace Jilo.App.Domain.GameEntity
{
    public class Game
    {
        public Guid Id { get; set; }

        public string GameName { get; set; }

        public string Description { get; set; }

        public List<string> Roles { get; set; }

        public List<string> Ranks { get; set; }

        public string CoverImageUrl { get; private set; }

        public ICollection<UserGame> UserGames { get; set; } = null!;

        public Game(Guid id, string gameName, string coverImageUrl, string description, List<string> roles, List<string> ranks)
        {
            Id = id;
            GameName = gameName;
            Description = description;
            CoverImageUrl = coverImageUrl;
            Roles = roles;
            Ranks = ranks;
        }

    }
}
