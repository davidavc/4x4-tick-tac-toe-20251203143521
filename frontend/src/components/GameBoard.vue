<template>
  <div class="game-board">
    <!-- Game Status -->
    <div class="game-status" :class="{ 'celebration': game?.isGameOver && game?.winner }">
      <h2>{{ gameStatusMessage }}</h2>
      <div v-if="game?.isGameOver && game?.winner" class="snowflakes">
        <div class="snowflake" v-for="n in 20" :key="n">❄️</div>
      </div>
    </div>

    <!-- Game Grid -->
    <div class="grid-container">
      <div 
        class="game-grid" 
        :class="{ 'game-over': game?.isGameOver, 'winning-animation': showWinAnimation }"
      >
        <div
          v-for="(row, rowIndex) in game?.board || []"
          :key="rowIndex"
          class="board-row"
        >
          <div
            v-for="(cell, colIndex) in row"
            :key="`${rowIndex}-${colIndex}`"
            class="board-cell"
            :class="{
              'player-x': cell === 'X',
              'player-o': cell === 'O',
              'empty': cell === null,
              'clickable': canMakeMove(rowIndex, colIndex),
              'winning-cell': isWinningCell(rowIndex, colIndex)
            }"
            @click="handleCellClick(rowIndex, colIndex)"
          >
            <span v-if="cell === 'X'" class="piece piece-x">🎄</span>
            <span v-else-if="cell === 'O'" class="piece piece-o">🎅</span>
            <span v-else class="empty-cell">+</span>
          </div>
        </div>
      </div>
    </div>

    <!-- Loading Overlay -->
    <div v-if="loading" class="loading-overlay">
      <div class="loading-spinner">🎅</div>
      <p>{{ game?.currentPlayer === 'O' ? 'Santa is thinking...' : 'Processing move...' }}</p>
    </div>
  </div>
</template>

<script setup>
import { computed, watch, ref } from 'vue'
import { useGameStore } from '../stores/gameStore'

const gameStore = useGameStore()
const { game, loading, isPlayerTurn, gameStatusMessage, makeMove } = gameStore

const showWinAnimation = ref(false)

// Watch for game over to trigger win animation
watch(() => game.value?.isGameOver, (isGameOver) => {
  if (isGameOver && game.value?.winner) {
    showWinAnimation.value = true
    setTimeout(() => {
      showWinAnimation.value = false
    }, 3000)
  }
})

const canMakeMove = (row, col) => {
  if (!game.value || game.value.isGameOver || !isPlayerTurn.value || loading.value) {
    return false
  }
  return game.value.board[row][col] === null
}

const isWinningCell = (row, col) => {
  if (!game.value?.winningCells) return false
  return game.value.winningCells.some(cell => cell.row === row && cell.col === col)
}

const handleCellClick = (row, col) => {
  if (canMakeMove(row, col)) {
    makeMove(row, col)
  }
}
</script>

<style scoped>
.game-board {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
  padding: 2rem;
}

.game-status {
  text-align: center;
  position: relative;
  padding: 1rem;
  border-radius: 15px;
  background: linear-gradient(135deg, #e8f5e8, #f0f8ff);
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
  min-height: 60px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.game-status h2 {
  color: #2d5a2d;
  font-size: 1.5rem;
  margin: 0;
  font-weight: 600;
}

.game-status.celebration {
  background: linear-gradient(135deg, #ff6b6b, #4ecdc4, #45b7d1);
  animation: celebration 2s ease-in-out infinite alternate;
}

@keyframes celebration {
  0% { transform: scale(1); }
  100% { transform: scale(1.05); }
}

.snowflakes {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  pointer-events: none;
  overflow: hidden;
}

.snowflake {
  position: absolute;
  font-size: 1rem;
  animation: snowfall 3s linear infinite;
  opacity: 0.8;
}

.snowflake:nth-child(odd) {
  animation-duration: 4s;
  font-size: 0.8rem;
}

.snowflake:nth-child(even) {
  animation-duration: 2.5s;
  font-size: 1.2rem;
}

@keyframes snowfall {
  0% {
    top: -10px;
    transform: translateX(0) rotate(0deg);
  }
  100% {
    top: 110%;
    transform: translateX(100px) rotate(360deg);
  }
}

.grid-container {
  position: relative;
}

.game-grid {
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding: 20px;
  background: linear-gradient(135deg, #2d5a2d, #1e3a1e);
  border-radius: 20px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.3);
  border: 4px solid #c9a55a;
}

.game-grid.winning-animation {
  animation: winPulse 1s ease-in-out 3;
}

@keyframes winPulse {
  0%, 100% { transform: scale(1); }
  50% { transform: scale(1.05); }
}

.board-row {
  display: flex;
  gap: 8px;
}

.board-cell {
  width: 80px;
  height: 80px;
  background: linear-gradient(135deg, #f8f9fa, #e9ecef);
  border: 2px solid #c9a55a;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.board-cell.clickable:hover {
  background: linear-gradient(135deg, #e8f5e8, #d4edda);
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
}

.board-cell.empty .empty-cell {
  color: #6c757d;
  font-size: 2rem;
  opacity: 0.3;
  transition: opacity 0.3s ease;
}

.board-cell.clickable:hover .empty-cell {
  opacity: 0.6;
}

.piece {
  font-size: 3rem;
  animation: pieceAppear 0.5s ease-out;
}

@keyframes pieceAppear {
  0% {
    transform: scale(0) rotate(180deg);
    opacity: 0;
  }
  50% {
    transform: scale(1.2) rotate(90deg);
  }
  100% {
    transform: scale(1) rotate(0deg);
    opacity: 1;
  }
}

.winning-cell {
  background: linear-gradient(135deg, #ffd700, #ffed4e) !important;
  animation: winningCellGlow 1s ease-in-out infinite alternate;
}

@keyframes winningCellGlow {
  0% { box-shadow: 0 0 10px #ffd700; }
  100% { box-shadow: 0 0 20px #ffd700, 0 0 30px #ffd700; }
}

.loading-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(255, 255, 255, 0.9);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  border-radius: 20px;
  gap: 1rem;
}

.loading-spinner {
  font-size: 3rem;
  animation: spin 2s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.loading-overlay p {
  color: #2d5a2d;
  font-weight: 600;
  font-size: 1.1rem;
}

/* Responsive Design */
@media (max-width: 768px) {
  .game-board {
    padding: 1rem;
    gap: 1.5rem;
  }
  
  .board-cell {
    width: 60px;
    height: 60px;
  }
  
  .piece {
    font-size: 2.5rem;
  }
  
  .game-status h2 {
    font-size: 1.2rem;
  }
}

@media (max-width: 480px) {
  .board-cell {
    width: 50px;
    height: 50px;
  }
  
  .piece {
    font-size: 2rem;
  }
  
  .game-grid {
    gap: 4px;
    padding: 15px;
  }
}
</style>