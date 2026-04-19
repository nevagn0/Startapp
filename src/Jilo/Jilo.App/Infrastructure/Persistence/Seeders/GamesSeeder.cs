using Jilo.App.Domain.GameEntity;
namespace Jilo.App.Infrastructure.Persistence.Seeders;

public static class GamesSeeder
{
    public static async Task InitializeAsync(ServiceContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // Создаем игры только если их нет
        if (!context.Games.Any())
        {
            var games = new List<Game>
            {
                new Game(
                    Guid.NewGuid(),
                    "Counter-Strike 2",
                    "Legendary tactical shooter game",
                    "/images/games/cs2.jpg"
                ),
                new Game(
                    Guid.NewGuid(),
                    "Dota 2",
                    "Multiplayer online battle arena game",
                    "/images/games/dota2.jpg"
                ),
                new Game(
                    Guid.NewGuid(),
                    "Valorant",
                    "Tactical hero shooter game",
                    "/images/games/valorant.jpg"
                ),
                new Game(
                    Guid.NewGuid(),
                    "League of Legends",
                    "MOBA game with strategic gameplay",
                    "/images/games/lol.jpg"
                ),
                new Game(
                    Guid.NewGuid(),
                    "Apex Legends",
                    "Battle royale hero shooter",
                    "/images/games/apex.jpg"
                )
            };

            await context.Games.AddRangeAsync(games);
            await context.SaveChangesAsync();
        }
    }
}