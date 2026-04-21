using Jilo.App.Domain.GameEntity;
using Microsoft.EntityFrameworkCore;

namespace Jilo.App.Infrastructure.Persistence.Seeders;

public static class GamesSeeder
{
    public static async Task InitializeAsync(ServiceContext context)
    {
        await context.Database.EnsureCreatedAsync();

        var gamesData = new List<(Guid Id, string Name, string ImageUrl, string Description, List<string> Roles, List<string> Ranks)>
        {
            (Guid.Parse("475a01ab-e832-4563-bd28-b84861470517"), 
            "Counter-Strike 2",
            "/images/games/cs2.jpg",
            "Legendary tactical shooter game",
                new List<string> {"1", "2", "3"},
                new List<string> {"Рекрут", "Титан", "Калибровка"}
            ),

            (Guid.Parse("ac3315c0-2592-48bf-9a5f-83d19071efd2"), 
            "Dota 2",
            "/images/games/dota2.jpg", 
            "Multiplayer online battle arena game",
                new List<string> {"1", "2", "3"},
                new List<string> {"Рекрут", "Титан", "Калибровка"}
            ),
            (Guid.Parse("9a01c5f8-e26b-4e9c-a98d-b2097db55f32"),
            "Valorant", 
            "/images/games/valorant.jpg",
            "Tactical hero shooter game",
                new List<string> {"1", "2", "3"},
                new List<string> {"Рекрут", "Титан", "Калибровка"}
            ),
            (Guid.Parse("411393c4-55cd-448d-9da1-612b0142d9d5"), 
            "League of Legends",
            "/images/games/lol.jpg", 
            "MOBA game with strategic gameplay",
                new List<string> {"1", "2", "3"},
                new List<string> {"Рекрут", "Титан", "Калибровка"}
            ),
            
            (Guid.Parse("acdff89d-6f7d-41b1-ba78-0c765e64326e"),
            "Apex Legends",
            "/images/games/apex.jpg", 
            "Battle royale hero shooter",
                new List<string> {"1", "2", "3"},
                new List<string> {"Рекрут", "Титан", "Калибровка"}
            ),

            (Guid.Parse("d922ade7-9978-47f0-8a13-865bb828aa3d"),
            "PUBG",
            "/images/games/pubg.jpg",
            "Battle royale hero shooter",
                new List<string> {"1", "2", "3"},
                new List<string> {"Рекрут", "Титан", "Калибровка"}
            )
        };

        foreach (var gameData in gamesData)
        {
            var existingGame = await context.Games.FindAsync(gameData.Id);

            if (existingGame == null)
            {
                var newGame = new Game(
                    gameData.Id,
                    gameData.Name,
                    gameData.Description,
                    gameData.ImageUrl,
                    gameData.Roles,
                    gameData.Ranks
                );
                await context.Games.AddAsync(newGame);
            }
            else
            {
                existingGame.Roles = gameData.Roles;
                existingGame.Ranks = gameData.Ranks;
                context.Games.Update(existingGame);
            }
        }

        await context.SaveChangesAsync();
    }
}