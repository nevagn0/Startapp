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
                    Guid.Parse("475a01ab-e832-4563-bd28-b84861470517"),
                    "Counter-Strike 2",
                    "Legendary tactical shooter game",
                    "/images/games/cs2.jpg"
                ),
                new Game(
                    Guid.Parse("ac3315c0-2592-48bf-9a5f-83d19071efd2"),
                    "Dota 2",
                    "Multiplayer online battle arena game",
                    "/images/games/dota2.jpg"
                ),
                new Game(
                    Guid.Parse("9a01c5f8-e26b-4e9c-a98d-b2097db55f32"),
                    "Valorant",
                    "Tactical hero shooter game",
                    "/images/games/valorant.jpg"
                ),
                new Game(
                    Guid.Parse("411393c4-55cd-448d-9da1-612b0142d9d5"),
                    "League of Legends",
                    "MOBA game with strategic gameplay",
                    "/images/games/lol.jpg"
                ),
                new Game(
                    Guid.Parse("acdff89d-6f7d-41b1-ba78-0c765e64326e"),
                    "Apex Legends",
                    "Battle royale hero shooter",
                    "/images/games/apex.jpg"
                ),
                new Game(
                    Guid.NewGuid(),
                    "PUBG",
                    "126RUS",
                    "/images/games/pubg.jpg"
                )
            };

            await context.Games.AddRangeAsync(games);
            await context.SaveChangesAsync();
        }
    }
}