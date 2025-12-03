using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly Random _random;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
            _random = new Random();
        }

        public async Task<Game> StartNewGameAsync(DifficultyLevel difficulty)
        {
            var game = await _gameRepository.CreateGameAsync(difficulty);
            
            // If AI starts first, make the first move
            if (!game.PlayerStartsFirst)
            {
                var computerMove = GetComputerMove(game);
                var boardArray = game.BoardState.ToCharArray();
                boardArray[computerMove] = 'X';
                game.BoardState = new string(boardArray);
                game.CurrentPlayer = 'O';
                game = await _gameRepository.UpdateGameAsync(game);
            }
            
            return game;
        }

        public async Task<Game> MakeMoveAsync(Guid gameId, int position)
        {
            var game = await _gameRepository.GetGameAsync(gameId);
            if (game == null)
                throw new ArgumentException("Game not found");

            if (game.Status != GameStatus.InProgress)
                throw new InvalidOperationException("Game is not in progress");

            if (!IsValidMove(game, position))
                throw new ArgumentException("Invalid move");

            // Make player move
            var boardArray = game.BoardState.ToCharArray();
            boardArray[position] = game.CurrentPlayer;
            game.BoardState = new string(boardArray);
            
            // Check for win/draw after player move
            game.Status = CheckGameStatus(game.BoardState);
            if (game.Status == GameStatus.PlayerWins)
            {
                game.Winner = game.CurrentPlayer;
                game.WinningLine = GetWinningLine(game.BoardState);
                game.CompletedAt = DateTime.UtcNow;
                return await _gameRepository.UpdateGameAsync(game);
            }
            else if (game.Status == GameStatus.Draw)
            {
                game.CompletedAt = DateTime.UtcNow;
                return await _gameRepository.UpdateGameAsync(game);
            }

            // Switch turns and make computer move
            game.CurrentPlayer = game.CurrentPlayer == 'X' ? 'O' : 'X';
            var computerMove = GetComputerMove(game);
            boardArray = game.BoardState.ToCharArray();
            boardArray[computerMove] = game.CurrentPlayer;
            game.BoardState = new string(boardArray);

            // Check for win/draw after computer move
            game.Status = CheckGameStatus(game.BoardState);
            if (game.Status == GameStatus.ComputerWins)
            {
                game.Winner = game.CurrentPlayer;
                game.WinningLine = GetWinningLine(game.BoardState);
                game.CompletedAt = DateTime.UtcNow;
            }
            else if (game.Status == GameStatus.Draw)
            {
                game.CompletedAt = DateTime.UtcNow;
            }
            else
            {
                // Switch back to player's turn
                game.CurrentPlayer = game.CurrentPlayer == 'X' ? 'O' : 'X';
            }

            return await _gameRepository.UpdateGameAsync(game);
        }

        public async Task<Game> RestartGameAsync(Guid gameId)
        {
            var game = await _gameRepository.GetGameAsync(gameId);
            if (game == null)
                throw new ArgumentException("Game not found");

            game.BoardState = new string(' ', 16);
            game.Status = GameStatus.InProgress;
            game.Winner = null;
            game.WinningLine = null;
            game.CompletedAt = null;
            game.CurrentPlayer = 'X';
            game.PlayerStartsFirst = _random.Next(2) == 0; // Randomize again

            // If AI starts first, make the first move
            if (!game.PlayerStartsFirst)
            {
                var computerMove = GetComputerMove(game);
                var boardArray = game.BoardState.ToCharArray();
                boardArray[computerMove] = 'X';
                game.BoardState = new string(boardArray);
                game.CurrentPlayer = 'O';
            }

            return await _gameRepository.UpdateGameAsync(game);
        }

        public async Task<Game?> GetGameAsync(Guid gameId)
        {
            return await _gameRepository.GetGameAsync(gameId);
        }

        public bool IsValidMove(Game game, int position)
        {
            return position >= 0 && position < 16 && game.BoardState[position] == ' ';
        }

        public GameStatus CheckGameStatus(string boardState)
        {
            var board = boardState.ToCharArray();
            
            // Check all possible winning combinations for 4x4 grid
            var winConditions = new int[][]
            {
                // Rows
                new[] { 0, 1, 2, 3 }, new[] { 4, 5, 6, 7 }, new[] { 8, 9, 10, 11 }, new[] { 12, 13, 14, 15 },
                // Columns  
                new[] { 0, 4, 8, 12 }, new[] { 1, 5, 9, 13 }, new[] { 2, 6, 10, 14 }, new[] { 3, 7, 11, 15 },
                // Diagonals
                new[] { 0, 5, 10, 15 }, new[] { 3, 6, 9, 12 }
            };

            foreach (var condition in winConditions)
            {
                var chars = condition.Select(pos => board[pos]).ToArray();
                if (chars.All(c => c == 'X' && c != ' '))
                    return GameStatus.PlayerWins;
                if (chars.All(c => c == 'O' && c != ' '))
                    return GameStatus.ComputerWins;
            }

            // Check for draw
            if (board.All(c => c != ' '))
                return GameStatus.Draw;

            return GameStatus.InProgress;
        }

        public string? GetWinningLine(string boardState)
        {
            var board = boardState.ToCharArray();
            
            var winConditions = new int[][]
            {
                // Rows
                new[] { 0, 1, 2, 3 }, new[] { 4, 5, 6, 7 }, new[] { 8, 9, 10, 11 }, new[] { 12, 13, 14, 15 },
                // Columns
                new[] { 0, 4, 8, 12 }, new[] { 1, 5, 9, 13 }, new[] { 2, 6, 10, 14 }, new[] { 3, 7, 11, 15 },
                // Diagonals
                new[] { 0, 5, 10, 15 }, new[] { 3, 6, 9, 12 }
            };

            foreach (var condition in winConditions)
            {
                var chars = condition.Select(pos => board[pos]).ToArray();
                if (chars.All(c => c != ' ' && c == chars[0]))
                {
                    return string.Join(",", condition);
                }
            }

            return null;
        }

        public int GetComputerMove(Game game)
        {
            var board = game.BoardState.ToCharArray();
            var availableMoves = new List<int>();

            for (int i = 0; i < 16; i++)
            {
                if (board[i] == ' ')
                    availableMoves.Add(i);
            }

            if (availableMoves.Count == 0)
                throw new InvalidOperationException("No moves available");

            switch (game.Difficulty)
            {
                case DifficultyLevel.Easy:
                    return GetEasyMove(availableMoves);
                case DifficultyLevel.Average:
                    return GetAverageMove(board, availableMoves, game.CurrentPlayer);
                case DifficultyLevel.Hard:
                    return GetHardMove(board, availableMoves, game.CurrentPlayer);
                default:
                    return GetEasyMove(availableMoves);
            }
        }

        private int GetEasyMove(List<int> availableMoves)
        {
            return availableMoves[_random.Next(availableMoves.Count)];
        }

        private int GetAverageMove(char[] board, List<int> availableMoves, char aiPlayer)
        {
            char opponent = aiPlayer == 'X' ? 'O' : 'X';

            // Try to win first
            var winMove = FindWinningMove(board, availableMoves, aiPlayer);
            if (winMove.HasValue) return winMove.Value;

            // Block opponent from winning
            var blockMove = FindWinningMove(board, availableMoves, opponent);
            if (blockMove.HasValue) return blockMove.Value;

            // Otherwise random
            return availableMoves[_random.Next(availableMoves.Count)];
        }

        private int GetHardMove(char[] board, List<int> availableMoves, char aiPlayer)
        {
            // Use minimax or similar advanced strategy
            // For now, use average strategy with some improvements
            char opponent = aiPlayer == 'X' ? 'O' : 'X';

            // Try to win
            var winMove = FindWinningMove(board, availableMoves, aiPlayer);
            if (winMove.HasValue) return winMove.Value;

            // Block opponent
            var blockMove = FindWinningMove(board, availableMoves, opponent);
            if (blockMove.HasValue) return blockMove.Value;

            // Prefer center positions
            var centerMoves = availableMoves.Where(move => move == 5 || move == 6 || move == 9 || move == 10).ToList();
            if (centerMoves.Any())
                return centerMoves[_random.Next(centerMoves.Count)];

            // Prefer corners
            var cornerMoves = availableMoves.Where(move => move == 0 || move == 3 || move == 12 || move == 15).ToList();
            if (cornerMoves.Any())
                return cornerMoves[_random.Next(cornerMoves.Count)];

            return availableMoves[_random.Next(availableMoves.Count)];
        }

        private int? FindWinningMove(char[] board, List<int> availableMoves, char player)
        {
            var winConditions = new int[][]
            {
                // Rows
                new[] { 0, 1, 2, 3 }, new[] { 4, 5, 6, 7 }, new[] { 8, 9, 10, 11 }, new[] { 12, 13, 14, 15 },
                // Columns
                new[] { 0, 4, 8, 12 }, new[] { 1, 5, 9, 13 }, new[] { 2, 6, 10, 14 }, new[] { 3, 7, 11, 15 },
                // Diagonals
                new[] { 0, 5, 10, 15 }, new[] { 3, 6, 9, 12 }
            };

            foreach (var condition in winConditions)
            {
                var playerCount = condition.Count(pos => board[pos] == player);
                var emptyCount = condition.Count(pos => board[pos] == ' ');
                
                if (playerCount == 3 && emptyCount == 1)
                {
                    var winningMove = condition.First(pos => board[pos] == ' ');
                    if (availableMoves.Contains(winningMove))
                        return winningMove;
                }
            }

            return null;
        }
    }
}