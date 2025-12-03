using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(16)]
        public string BoardState { get; set; } = new string(' ', 16); // 4x4 = 16 cells
        
        [Required]
        public char CurrentPlayer { get; set; } = 'X'; // X or O
        
        [Required]
        public GameStatus Status { get; set; } = GameStatus.InProgress;
        
        [Required]
        public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Easy;
        
        public char? Winner { get; set; }
        
        public string? WinningLine { get; set; } // For animation - stores winning positions
        
        [Required]
        public bool PlayerStartsFirst { get; set; } = true;
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? CompletedAt { get; set; }
    }
    
    public enum GameStatus
    {
        InProgress,
        PlayerWins,
        ComputerWins,
        Draw
    }
    
    public enum DifficultyLevel
    {
        Easy,
        Average, 
        Hard
    }
}