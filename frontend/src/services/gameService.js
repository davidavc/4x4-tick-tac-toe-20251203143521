import axios from 'axios'

const API_BASE_URL = '/api'

class GameService {
  async createGame(difficulty = 'Easy') {
    try {
      const response = await axios.post(`${API_BASE_URL}/game`, { difficulty })
      return response.data
    } catch (error) {
      console.error('Error creating game:', error)
      throw error
    }
  }

  async getGame(gameId) {
    try {
      const response = await axios.get(`${API_BASE_URL}/game/${gameId}`)
      return response.data
    } catch (error) {
      console.error('Error getting game:', error)
      throw error
    }
  }

  async makeMove(gameId, row, col) {
    try {
      const response = await axios.post(`${API_BASE_URL}/game/${gameId}/move`, {
        row,
        col
      })
      return response.data
    } catch (error) {
      console.error('Error making move:', error)
      throw error
    }
  }

  async restartGame(gameId, difficulty = 'Easy') {
    try {
      const response = await axios.post(`${API_BASE_URL}/game/${gameId}/restart`, {
        difficulty
      })
      return response.data
    } catch (error) {
      console.error('Error restarting game:', error)
      throw error
    }
  }
}

export default new GameService()