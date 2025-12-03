<template>
  <div class="game-controls">
    <!-- Difficulty Selection -->
    <div class="control-section">
      <h3>🎅 Choose Santa's Helper Difficulty</h3>
      <div class="difficulty-buttons">
        <button
          v-for="difficulty in difficulties"
          :key="difficulty.value"
          :class="['difficulty-btn', difficulty.class, { active: selectedDifficulty === difficulty.value }]"
          @click="setDifficulty(difficulty.value)"
          :disabled="loading"
        >
          <span class="difficulty-icon">{{ difficulty.icon }}</span>
          <span class="difficulty-label">{{ difficulty.label }}</span>
          <span class="difficulty-desc">{{ difficulty.description }}</span>
        </button>
      </div>
    </div>

    <!-- Game Actions -->
    <div class="control-section">
      <div class="action-buttons">
        <button
          v-if="!game"
          class="action-btn start-btn"
          @click="createGame()"
          :disabled="loading"
        >
          <span class="btn-icon">🎄</span>
          Start Christmas Game
        </button>

        <button
          v-if="game"
          class="action-btn restart-btn"
          @click="restartGame()"
          :disabled="loading"
        >
          <span class="btn-icon">🔄</span>
          New Christmas Game
        </button>
      </div>
    </div>

    <!-- Error Display -->
    <div v-if="error" class="error-message">
      <span class="error-icon">⚠️</span>
      <span>{{ error }}</span>
      <button class="error-close" @click="clearError">×</button>
    </div>

    <!-- Game Info -->
    <div v-if="game" class="game-info">
      <div class="info-item">
        <span class="info-label">🎯 Difficulty:</span>
        <span class="info-value">{{ selectedDifficulty }}</span>
      </div>
      <div class="info-item">
        <span class="info-label">🎮 First Player:</span>
        <span class="info-value">{{ game.firstPlayer === 'X' ? 'You 🎄' : 'Santa\'s Helper 🎅' }}</span>
      </div>
      <div v-if="game.isGameOver" class="info-item">
        <span class="info-label">🏆 Result:</span>
        <span class="info-value">
          <span v-if="game.winner === 'X'">You Won! 🎉</span>
          <span v-else-if="game.winner === 'O'">Santa's Helper Won! 🎅</span>
          <span v-else>Christmas Tie! 🎁</span>
        </span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useGameStore } from '../stores/gameStore'

const gameStore = useGameStore()
const { 
  game, 
  loading, 
  error, 
  selectedDifficulty, 
  setDifficulty, 
  createGame, 
  restartGame, 
  clearError 
} = gameStore

const difficulties = [
  {
    value: 'Easy',
    label: 'Easy',
    description: 'Playful elf',
    icon: '🧝',
    class: 'easy'
  },
  {
    value: 'Average',
    label: 'Average', 
    description: 'Smart reindeer',
    icon: '🦌',
    class: 'average'
  },
  {
    value: 'Hard',
    label: 'Hard',
    description: 'Santa himself!',
    icon: '🎅',
    class: 'hard'
  }
]
</script>

<style scoped>
.game-controls {
  display: flex;
  flex-direction: column;
  gap: 2rem;
  padding: 2rem;
  background: linear-gradient(135deg, #f8f9fa, #e9ecef);
  border-radius: 20px;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.1);
  max-width: 600px;
  margin: 0 auto;
}

.control-section h3 {
  color: #2d5a2d;
  text-align: center;
  margin-bottom: 1.5rem;
  font-size: 1.4rem;
  font-weight: 600;
}

.difficulty-buttons {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(160px, 1fr));
  gap: 1rem;
}

.difficulty-btn {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
  padding: 1.5rem 1rem;
  border: 3px solid transparent;
  border-radius: 15px;
  background: linear-gradient(135deg, #ffffff, #f8f9fa);
  cursor: pointer;
  transition: all 0.3s ease;
  font-family: inherit;
  min-height: 120px;
}

.difficulty-btn:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
}

.difficulty-btn.active {
  transform: translateY(-3px);
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.2);
}

.difficulty-btn.easy {
  border-color: #28a745;
}

.difficulty-btn.easy.active {
  background: linear-gradient(135deg, #d4edda, #c3e6cb);
  border-color: #28a745;
}

.difficulty-btn.average {
  border-color: #ffc107;
}

.difficulty-btn.average.active {
  background: linear-gradient(135deg, #fff3cd, #ffeaa7);
  border-color: #ffc107;
}

.difficulty-btn.hard {
  border-color: #dc3545;
}

.difficulty-btn.hard.active {
  background: linear-gradient(135deg, #f8d7da, #f5c6cb);
  border-color: #dc3545;
}

.difficulty-icon {
  font-size: 2rem;
  margin-bottom: 0.5rem;
}

.difficulty-label {
  font-weight: 600;
  color: #2d5a2d;
  font-size: 1.1rem;
}

.difficulty-desc {
  font-size: 0.9rem;
  color: #6c757d;
  text-align: center;
}

.action-buttons {
  display: flex;
  justify-content: center;
  gap: 1rem;
}

.action-btn {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1rem 2rem;
  border: none;
  border-radius: 25px;
  font-size: 1.1rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  font-family: inherit;
  min-width: 200px;
  justify-content: center;
}

.start-btn {
  background: linear-gradient(135deg, #28a745, #20c997);
  color: white;
}

.start-btn:hover {
  background: linear-gradient(135deg, #218838, #1ea080);
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(40, 167, 69, 0.3);
}

.restart-btn {
  background: linear-gradient(135deg, #17a2b8, #138496);
  color: white;
}

.restart-btn:hover {
  background: linear-gradient(135deg, #138496, #117a8b);
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(23, 162, 184, 0.3);
}

.action-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
  box-shadow: none;
}

.btn-icon {
  font-size: 1.2rem;
}

.error-message {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 1rem;
  background: linear-gradient(135deg, #f8d7da, #f5c6cb);
  border: 2px solid #dc3545;
  border-radius: 10px;
  color: #721c24;
}

.error-icon {
  font-size: 1.2rem;
}

.error-close {
  margin-left: auto;
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
  color: #721c24;
  padding: 0;
  width: 24px;
  height: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  transition: background-color 0.2s ease;
}

.error-close:hover {
  background-color: rgba(114, 28, 36, 0.1);
}

.game-info {
  background: linear-gradient(135deg, #e8f5e8, #d4edda);
  border-radius: 15px;
  padding: 1.5rem;
  border: 2px solid #28a745;
}

.info-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem 0;
  border-bottom: 1px solid rgba(40, 167, 69, 0.2);
}

.info-item:last-child {
  border-bottom: none;
}

.info-label {
  font-weight: 600;
  color: #2d5a2d;
}

.info-value {
  color: #155724;
  font-weight: 500;
}

/* Responsive Design */
@media (max-width: 768px) {
  .game-controls {
    padding: 1.5rem;
    gap: 1.5rem;
  }

  .difficulty-buttons {
    grid-template-columns: 1fr;
  }

  .action-buttons {
    flex-direction: column;
  }

  .action-btn {
    min-width: auto;
    width: 100%;
  }

  .info-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 0.25rem;
  }
}

@media (max-width: 480px) {
  .game-controls {
    padding: 1rem;
  }

  .control-section h3 {
    font-size: 1.2rem;
  }

  .difficulty-btn {
    padding: 1rem;
    min-height: 100px;
  }

  .difficulty-icon {
    font-size: 1.5rem;
  }
}
</style>