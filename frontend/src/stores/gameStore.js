import { reactive, computed } from 'vue'
import gameService from '../services/gameService'

const state = reactive({
  game: null,
  loading: false,
  error: null,
  selectedDifficulty: 'Easy'
})

export const useGameStore = () => {
  const game = computed(() => state.game)
  const loading = computed(() => state.loading)
  const error = computed(() => state.error)
  const selectedDifficulty = computed(() => state.selectedDifficulty)

  const isPlayerTurn = computed(() => {
    return state.game && !state.game.isGameOver && state.game.currentPlayer === 'X'
  })

  const gameStatusMessage = computed(() => {
    if (!state.game) return ''
    
    if (state.game.isGameOver) {
      if (state.game.winner === 'X') return state.game.message || '🎄 Ho Ho Ho! You won! 🎅'
      if (state.game.winner === 'O') return state.game.message || '🎅 Santa\'s helper won! Try again! ❄️'
      return state.game.message || '🎁 It\'s a Christmas tie! Great game! ⭐'
    }
    
    return state.game.currentPlayer === 'X' 
      ? '🎄 Your turn! Make your move! 🎅'
      : '🎅 Santa\'s helper is thinking... ❄️'
  })

  const setDifficulty = (difficulty) => {
    state.selectedDifficulty = difficulty
  }

  const createGame = async (difficulty = state.selectedDifficulty) => {
    try {
      state.loading = true
      state.error = null
      const gameData = await gameService.createGame(difficulty)
      state.game = gameData
      setDifficulty(difficulty)
    } catch (error) {
      state.error = 'Failed to create game. Please try again.'
      console.error('Error creating game:', error)
    } finally {
      state.loading = false
    }
  }

  const makeMove = async (row, col) => {
    if (!state.game || state.game.isGameOver || !isPlayerTurn.value) return

    try {
      state.loading = true
      state.error = null
      const gameData = await gameService.makeMove(state.game.id, row, col)
      state.game = gameData
    } catch (error) {
      state.error = 'Failed to make move. Please try again.'
      console.error('Error making move:', error)
    } finally {
      state.loading = false
    }
  }

  const restartGame = async () => {
    if (!state.game) return

    try {
      state.loading = true
      state.error = null
      const gameData = await gameService.restartGame(state.game.id, state.selectedDifficulty)
      state.game = gameData
    } catch (error) {
      state.error = 'Failed to restart game. Please try again.'
      console.error('Error restarting game:', error)
    } finally {
      state.loading = false
    }
  }

  const clearError = () => {
    state.error = null
  }

  return {
    // State
    game,
    loading,
    error,
    selectedDifficulty,
    // Computed
    isPlayerTurn,
    gameStatusMessage,
    // Actions
    setDifficulty,
    createGame,
    makeMove,
    restartGame,
    clearError
  }
}