using Api.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameController> _logger;

        public GameController(IGameService gameService, ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpPost("start")]
        public async Task<ActionResult<GameResponse>> StartGame([FromBody] CreateGameRequest request)
        {
            try
            {
                var game = await _gameService.StartNewGameAsync(request.Difficulty);
                var gameDto = MapToDto(game);
                
                _logger.LogInformation("New game started with ID: {GameId}, Difficulty: {Difficulty}", 
                    game.Id, request.Difficulty);

                return Ok(new GameResponse
                {
                    Success = true,
                    Message = "Game started successfully!",
                    Game = gameDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting new game");
                return BadRequest(new GameResponse
                {
                    Success = false,
                    Message = "Failed to start game: " + ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameResponse>> GetGame(Guid id)
        {
            try
            {
                var game = await _gameService.GetGameAsync(id);
                if (game == null)
                {
                    return NotFound(new GameResponse
                    {
                        Success = false,
                        Message = "Game not found"
                    });
                }

                var gameDto = MapToDto(game);
                return Ok(new GameResponse
                {
                    Success = true,
                    Message = "Game retrieved successfully",
                    Game = gameDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving game {GameId}", id);
                return BadRequest(new GameResponse
                {
                    Success = false,
                    Message = "Failed to retrieve game: " + ex.Message
                });
            }
        }

        [HttpPost("{id}/move")]
        public async Task<ActionResult<GameResponse>> MakeMove(Guid id, [FromBody] MakeMoveRequest request)
        {
            try
            {
                var game = await _gameService.MakeMoveAsync(id, request.Position);
                var gameDto = MapToDto(game);
                
                _logger.LogInformation("Move made in game {GameId} at position {Position}", id, request.Position);

                return Ok(new GameResponse
                {
                    Success = true,
                    Message = GetMoveResultMessage(game.Status),
                    Game = gameDto
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new GameResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new GameResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error making move in game {GameId}", id);
                return BadRequest(new GameResponse
                {
                    Success = false,
                    Message = "Failed to make move: " + ex.Message
                });
            }
        }

        [HttpPost("{id}/restart")]
        public async Task<ActionResult<GameResponse>> RestartGame(Guid id)
        {
            try
            {
                var game = await _gameService.RestartGameAsync(id);
                var gameDto = MapToDto(game);
                
                _logger.LogInformation("Game {GameId} restarted", id);

                return Ok(new GameResponse
                {
                    Success = true,
                    Message = "Game restarted successfully!",
                    Game = gameDto
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new GameResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restarting game {GameId}", id);
                return BadRequest(new GameResponse
                {
                    Success = false,
                    Message = "Failed to restart game: " + ex.Message
                });
            }
        }

        private GameDto MapToDto(Game game)
        {
            // Determine if it's currently the player's turn
            bool isPlayerTurn;
            if (game.Status != GameStatus.InProgress)
            {
                isPlayerTurn = false;
            }
            else if (game.PlayerStartsFirst)
            {
                // Player is X, so it's player's turn when current player is X
                isPlayerTurn = game.CurrentPlayer == 'X';
            }
            else
            {
                // AI is X, so it's player's turn when current player is O
                isPlayerTurn = game.CurrentPlayer == 'O';
            }

            return new GameDto
            {
                Id = game.Id,
                BoardState = game.BoardState,
                CurrentPlayer = game.CurrentPlayer,
                Status = game.Status,
                Difficulty = game.Difficulty,
                Winner = game.Winner,
                WinningLine = game.WinningLine,
                PlayerStartsFirst = game.PlayerStartsFirst,
                CreatedAt = game.CreatedAt,
                CompletedAt = game.CompletedAt,
                IsPlayerTurn = isPlayerTurn
            };
        }

        private string GetMoveResultMessage(GameStatus status)
        {
            return status switch
            {
                GameStatus.PlayerWins => "🎄 Congratulations! You won! 🎄",
                GameStatus.ComputerWins => "🤖 Computer wins this round! Try again!",
                GameStatus.Draw => "🎁 It's a draw! Great game!",
                GameStatus.InProgress => "Move made successfully!",
                _ => "Move completed"
            };
        }
    }
}