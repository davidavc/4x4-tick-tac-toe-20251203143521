using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly Random _random;

        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task<Game> CreateGameAsync(DifficultyLevel difficulty)
        {
            var game = new Game
            {
                Difficulty = difficulty,
                PlayerStartsFirst = _random.Next(2) == 0, // Random true/false
                CurrentPlayer = 'X' // X always starts, but PlayerStartsFirst determines if that's human or AI
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<Game?> GetGameAsync(Guid id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<Game> UpdateGameAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task DeleteGameAsync(Guid id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
            }
        }
    }
}