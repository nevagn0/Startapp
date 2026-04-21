using Jilo.App.Domain.Entities;
using Jilo.App.Domain.LobbyEntity;

namespace Jilo.App.Domain.GameEntity
{
    public class Game
    {
        public Guid Id { get; set; }

        public string GameName { get; set; }

        public string Description { get; set; }

        public string CoverImageUrl { get; private set; }

        public ICollection<UserGame> UserGames { get; set; } = null!;

        public ICollection<Lobby> Lobbies { get; private set; }

        public Game(Guid id, string gameName, string coverImageUrl, string description)
        {
            Id = id;
            GameName = gameName;
            Description = description;
            CoverImageUrl = coverImageUrl;
            Lobbies = [];
        }

    }
}
