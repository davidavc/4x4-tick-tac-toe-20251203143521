import { createApp } from 'vue'
import App from './App.vue'

// Create and mount the Vue application
const app = createApp(App)

// Global error handler
app.config.errorHandler = (error, instance, info) => {
  console.error('Vue Error:', error)
  console.error('Component:', instance)
  console.error('Info:', info)
}

// Mount the app
app.mount('#app')