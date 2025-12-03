-- Christmas Tic-Tac-Toe Database Initialization
-- This script creates the initial database structure

USE ChristmasTicTacToe;

-- Create Games table
CREATE TABLE IF NOT EXISTS Games (
    Id CHAR(36) PRIMARY KEY,
    BoardState JSON NOT NULL,
    CurrentPlayer CHAR(1) NOT NULL CHECK (CurrentPlayer IN ('X', 'O')),
    FirstPlayer CHAR(1) NOT NULL CHECK (FirstPlayer IN ('X', 'O')),
    IsGameOver BOOLEAN NOT NULL DEFAULT FALSE,
    Winner CHAR(1) NULL CHECK (Winner IN ('X', 'O') OR Winner IS NULL),
    WinningCells JSON NULL,
    Difficulty ENUM('Easy', 'Average', 'Hard') NOT NULL DEFAULT 'Easy',
    Message TEXT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_created_at (CreatedAt),
    INDEX idx_difficulty (Difficulty),
    INDEX idx_game_over (IsGameOver)
);

-- Add some sample data for testing (optional)
-- INSERT INTO Games (Id, BoardState, CurrentPlayer, FirstPlayer, IsGameOver, Winner, WinningCells, Difficulty, Message) 
-- VALUES (
--     UUID(),
--     '[[null,null,null,null],[null,null,null,null],[null,null,null,null],[null,null,null,null]]',
--     'X',
--     'X',
--     FALSE,
--     NULL,
--     NULL,
--     'Easy',
--     'Welcome to Christmas Tic-Tac-Toe!'
-- );

-- Performance optimization: Add indexes for common queries
-- These indexes will help with game lookup and filtering operations

-- Create a stored procedure for cleanup old games (optional)
DELIMITER //
CREATE PROCEDURE CleanupOldGames()
BEGIN
    -- Delete games older than 7 days
    DELETE FROM Games 
    WHERE CreatedAt < DATE_SUB(NOW(), INTERVAL 7 DAY)
    AND IsGameOver = TRUE;
END //
DELIMITER ;

-- Create an event scheduler to run cleanup daily (optional)
-- Note: This requires the event scheduler to be enabled
-- SET GLOBAL event_scheduler = ON;
-- 
-- CREATE EVENT IF NOT EXISTS daily_cleanup
-- ON SCHEDULE EVERY 1 DAY
-- STARTS '2023-12-01 02:00:00'
-- DO
--   CALL CleanupOldGames();

-- Grant permissions to the application user
GRANT SELECT, INSERT, UPDATE, DELETE ON ChristmasTicTacToe.* TO 'gameuser'@'%';
FLUSH PRIVILEGES;

-- Display success message
SELECT 'Christmas Tic-Tac-Toe database initialized successfully! 🎄🎅' AS Status;