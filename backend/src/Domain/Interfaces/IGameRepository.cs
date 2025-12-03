using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> CreateGameAsync(DifficultyLevel difficulty);
        Task<Game?> GetGameAsync(Guid id);
        Task<Game> UpdateGameAsync(Game game);
        Task DeleteGameAsync(Guid id);
    }
}