using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IGameService
    {
        Task<Game> StartNewGameAsync(DifficultyLevel difficulty);
        Task<Game> MakeMoveAsync(Guid gameId, int position);
        Task<Game> RestartGameAsync(Guid gameId);
        Task<Game?> GetGameAsync(Guid gameId);
        bool IsValidMove(Game game, int position);
        GameStatus CheckGameStatus(string boardState);
        string? GetWinningLine(string boardState);
        int GetComputerMove(Game game);
    }
}