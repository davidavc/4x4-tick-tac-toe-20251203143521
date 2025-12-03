using Domain.Entities;

namespace Api.DTOs
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public string BoardState { get; set; } = string.Empty;
        public char CurrentPlayer { get; set; }
        public GameStatus Status { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public char? Winner { get; set; }
        public string? WinningLine { get; set; }
        public bool PlayerStartsFirst { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsPlayerTurn { get; set; }
    }

    public class CreateGameRequest
    {
        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Easy;
    }

    public class MakeMoveRequest
    {
        public int Position { get; set; }
    }

    public class GameResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public GameDto? Game { get; set; }
    }
}