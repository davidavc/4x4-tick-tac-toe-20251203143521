<template>
  <div id="app">
    <!-- Header -->
    <header class="app-header">
      <div class="header-content">
        <h1 class="app-title">
          <span class="title-icon">🎄</span>
          Christmas Tic-Tac-Toe
          <span class="title-icon">🎅</span>
        </h1>
        <p class="app-subtitle">
          ❄️ Get 4 in a row to win this festive game! ⭐
        </p>
      </div>
      <div class="header-decoration">
        <div class="decoration-item" v-for="n in 5" :key="n">🎁</div>
      </div>
    </header>

    <!-- Main Game Area -->
    <main class="app-main">
      <div class="game-container">
        <!-- Game Controls -->
        <div class="controls-section">
          <GameControls />
        </div>

        <!-- Game Board -->
        <div class="board-section" v-if="game">
          <GameBoard />
        </div>

        <!-- Welcome Message -->
        <div class="welcome-section" v-if="!game">
          <div class="welcome-card">
            <div class="welcome-icons">
              <span class="welcome-icon">🎄</span>
              <span class="welcome-icon">🎅</span>
              <span class="welcome-icon">❄️</span>
            </div>
            <h2>Welcome to Christmas Tic-Tac-Toe!</h2>
            <p>Challenge Santa's helpers in this festive 4x4 tic-tac-toe game.</p>
            <ul class="game-rules">
              <li>🎯 Get 4 in a row to win (horizontal, vertical, or diagonal)</li>
              <li>🎄 You play as Christmas trees</li>
              <li>🎅 Santa's helpers play as Santa faces</li>
              <li>🎲 Who goes first is decided randomly</li>
              <li>🎊 Choose your difficulty level and start playing!</li>
            </ul>
          </div>
        </div>
      </div>
    </main>

    <!-- Footer -->
    <footer class="app-footer">
      <div class="footer-content">
        <p>🎄 Made with Christmas spirit 🎅</p>
        <div class="footer-decoration">
          <span v-for="n in 3" :key="n" class="footer-star">⭐</span>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup>
import { useGameStore } from './stores/gameStore'
import GameControls from './components/GameControls.vue'
import GameBoard from './components/GameBoard.vue'

const gameStore = useGameStore()
const { game } = gameStore
</script>

<style>
/* Global Styles */
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  background: linear-gradient(135deg, #0f4c3a, #1a5f4a, #2d8659);
  background-attachment: fixed;
  min-height: 100vh;
  overflow-x: hidden;
}

#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* Header Styles */
.app-header {
  background: linear-gradient(135deg, #c41e3a, #8b0000);
  color: white;
  padding: 2rem 1rem;
  text-align: center;
  position: relative;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
  border-bottom: 4px solid #c9a55a;
}

.header-content {
  max-width: 800px;
  margin: 0 auto;
}

.app-title {
  font-size: 3rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
}

.title-icon {
  font-size: 2.5rem;
  animation: bounce 2s infinite;
}

.title-icon:nth-child(3) {
  animation-delay: 0.5s;
}

@keyframes bounce {
  0%, 20%, 50%, 80%, 100% {
    transform: translateY(0);
  }
  40% {
    transform: translateY(-10px);
  }
  60% {
    transform: translateY(-5px);
  }
}

.app-subtitle {
  font-size: 1.3rem;
  opacity: 0.9;
  margin-bottom: 1rem;
}

.header-decoration {
  display: flex;
  justify-content: center;
  gap: 2rem;
  margin-top: 1rem;
}

.decoration-item {
  font-size: 1.5rem;
  animation: float 3s ease-in-out infinite;
}

.decoration-item:nth-child(even) {
  animation-delay: 1s;
}

@keyframes float {
  0%, 100% {
    transform: translateY(0px);
  }
  50% {
    transform: translateY(-10px);
  }
}

/* Main Content */
.app-main {
  flex: 1;
  padding: 2rem 1rem;
  display: flex;
  justify-content: center;
}

.game-container {
  width: 100%;
  max-width: 1200px;
  display: flex;
  flex-direction: column;
  gap: 3rem;
}

.controls-section,
.board-section {
  display: flex;
  justify-content: center;
}

/* Welcome Section */
.welcome-section {
  display: flex;
  justify-content: center;
  padding: 2rem 0;
}

.welcome-card {
  background: linear-gradient(135deg, #ffffff, #f8f9fa);
  border-radius: 25px;
  padding: 3rem 2rem;
  text-align: center;
  box-shadow: 0 15px 35px rgba(0, 0, 0, 0.2);
  border: 3px solid #c9a55a;
  max-width: 600px;
}

.welcome-icons {
  display: flex;
  justify-content: center;
  gap: 1rem;
  margin-bottom: 2rem;
}

.welcome-icon {
  font-size: 3rem;
  animation: pulse 2s infinite;
}

.welcome-icon:nth-child(2) {
  animation-delay: 0.5s;
}

.welcome-icon:nth-child(3) {
  animation-delay: 1s;
}

@keyframes pulse {
  0%, 100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
}

.welcome-card h2 {
  color: #2d5a2d;
  font-size: 2rem;
  margin-bottom: 1rem;
  font-weight: 600;
}

.welcome-card p {
  color: #6c757d;
  font-size: 1.2rem;
  margin-bottom: 2rem;
  line-height: 1.6;
}

.game-rules {
  list-style: none;
  text-align: left;
  color: #495057;
  font-size: 1.1rem;
  line-height: 1.8;
}

.game-rules li {
  margin-bottom: 0.5rem;
  padding-left: 1rem;
}

/* Footer */
.app-footer {
  background: linear-gradient(135deg, #2d5a2d, #1e3a1e);
  color: white;
  padding: 2rem 1rem;
  text-align: center;
  margin-top: auto;
  border-top: 3px solid #c9a55a;
}

.footer-content {
  max-width: 800px;
  margin: 0 auto;
}

.footer-content p {
  font-size: 1.1rem;
  margin-bottom: 1rem;
}

.footer-decoration {
  display: flex;
  justify-content: center;
  gap: 1rem;
}

.footer-star {
  font-size: 1.5rem;
  animation: twinkle 2s infinite;
}

.footer-star:nth-child(2) {
  animation-delay: 0.5s;
}

.footer-star:nth-child(3) {
  animation-delay: 1s;
}

@keyframes twinkle {
  0%, 50%, 100% {
    opacity: 1;
    transform: scale(1);
  }
  25% {
    opacity: 0.5;
    transform: scale(0.8);
  }
  75% {
    opacity: 0.8;
    transform: scale(1.1);
  }
}

/* Responsive Design */
@media (max-width: 768px) {
  .app-title {
    font-size: 2.2rem;
    flex-direction: column;
    gap: 0.5rem;
  }

  .title-icon {
    font-size: 2rem;
  }

  .app-subtitle {
    font-size: 1.1rem;
  }

  .game-container {
    gap: 2rem;
  }

  .welcome-card {
    padding: 2rem 1rem;
    margin: 0 1rem;
  }

  .welcome-card h2 {
    font-size: 1.6rem;
  }

  .welcome-card p {
    font-size: 1rem;
  }

  .game-rules {
    font-size: 1rem;
  }
}

@media (max-width: 480px) {
  .app-header {
    padding: 1.5rem 0.5rem;
  }

  .app-title {
    font-size: 1.8rem;
  }

  .title-icon {
    font-size: 1.5rem;
  }

  .app-subtitle {
    font-size: 1rem;
  }

  .app-main {
    padding: 1rem 0.5rem;
  }

  .welcome-card {
    padding: 1.5rem 1rem;
  }

  .welcome-icons .welcome-icon {
    font-size: 2rem;
  }

  .header-decoration .decoration-item {
    font-size: 1.2rem;
  }
}

/* Custom scrollbar for webkit browsers */
::-webkit-scrollbar {
  width: 12px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 10px;
}

::-webkit-scrollbar-thumb {
  background: linear-gradient(135deg, #c41e3a, #8b0000);
  border-radius: 10px;
  border: 2px solid #f1f1f1;
}

::-webkit-scrollbar-thumb:hover {
  background: linear-gradient(135deg, #8b0000, #c41e3a);
}
</style>