# Christmas Tic-Tac-Toe 🎄🎅

A festive 4x4 tic-tac-toe game built with ASP.NET Core, Vue.js 3, and MySQL. Challenge Santa's helpers in this holiday-themed game with three difficulty levels!

## Features ✨

- **4x4 Tic-Tac-Toe**: Traditional rules on a larger grid - get 4 in a row to win!
- **Christmas Theme**: Festive holiday design with Christmas colors and emojis
- **AI Opponents**: Three difficulty levels:
  - 🧝 **Easy**: Playful elf (random moves)
  - 🦌 **Average**: Smart reindeer (basic strategy)
  - 🎅 **Hard**: Santa himself (advanced AI)
- **Random Start**: System randomly decides who goes first
- **Win Animation**: Celebratory effects when someone wins
- **Responsive Design**: Works great on all device sizes
- **Game Restart**: Start a new game anytime

## Technology Stack 🛠️

- **Backend**: ASP.NET Core 8.0 with C#
- **Frontend**: Vue.js 3 with Composition API
- **Database**: MySQL 8.0 with Entity Framework Core
- **Containerization**: Docker & Docker Compose
- **Styling**: Modern CSS with Christmas theme

## Game Rules 🎮

1. Play on a 4x4 grid instead of the traditional 3x3
2. You play as Christmas trees (🎄), Santa's helper plays as Santa faces (🎅)
3. Get **4 in a row** (horizontal, vertical, or diagonal) to win
4. Who goes first is randomly determined each game
5. Choose your difficulty level before starting

## Quick Start 🚀

### Prerequisites
- Docker and Docker Compose installed
- Ports 3000, 8080, and 3306 available

### Run the Application

1. **Clone the repository**
   ```bash
   git clone https://github.com/davidavc/4x4-tick-tac-toe-20251203143521.git
   cd 4x4-tick-tac-toe-20251203143521
   ```

2. **Start all services**
   ```bash
   docker-compose up -d
   ```

3. **Wait for services to be ready** (about 30-60 seconds)
   ```bash
   docker-compose logs -f
   ```

4. **Open your browser**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:8080
   - API Documentation: http://localhost:8080/swagger

### Stop the Application
```bash
docker-compose down
```

## Development 👩‍💻

### Project Structure
```
├── backend/                 # ASP.NET Core API
│   ├── src/
│   │   ├── Api/            # Controllers, DTOs, Program.cs
│   │   ├── Domain/         # Entities, Interfaces
│   │   └── Infrastructure/ # Data, Repositories, Services
│   └── Dockerfile
├── frontend/               # Vue.js 3 Application
│   ├── src/
│   │   ├── components/     # Vue components
│   │   ├── services/       # API services
│   │   └── stores/         # State management
│   ├── Dockerfile
│   └── nginx.conf
├── database/
│   └── init/              # Database initialization
└── docker-compose.yml
```

### API Endpoints

- `POST /api/game` - Create a new game
- `GET /api/game/{id}` - Get game state
- `POST /api/game/{id}/move` - Make a player move
- `POST /api/game/{id}/restart` - Restart the game
- `GET /api/health` - Health check

### Game State Management

The game uses a reactive store pattern with Vue 3's Composition API:
- `gameStore.js` - Central state management
- `gameService.js` - API communication layer
- Real-time UI updates based on game state changes

## Game AI Logic 🤖

### Easy Mode (🧝 Playful Elf)
- Makes completely random valid moves
- No strategy, just fun and unpredictable

### Average Mode (🦌 Smart Reindeer)
- Tries to win when possible (completes 4 in a row)
- Blocks player from winning (prevents player's 4 in a row)
- Otherwise makes random moves

### Hard Mode (🎅 Santa Himself)
- Uses minimax algorithm with alpha-beta pruning
- Evaluates multiple moves ahead
- Creates strategic setups and combinations
- Nearly unbeatable when playing optimally

## Christmas Theme Elements 🎄

- **Colors**: Traditional Christmas red, green, gold, and white
- **Game Pieces**: 🎄 (Player) vs 🎅 (AI)
- **Animations**: Snowfall effects, win celebrations, floating decorations
- **Messages**: Holiday-themed status messages and responses
- **Icons**: Christmas emojis throughout the interface

## Performance Features ⚡

- **Database Optimization**: Indexed columns for fast queries
- **Caching**: Static asset caching with proper headers
- **Compression**: Gzip compression for faster loading
- **Health Checks**: Service health monitoring
- **Responsive Images**: Optimized for different screen sizes

## Security Features 🔒

- **Input Validation**: All moves validated server-side
- **CORS Configuration**: Proper cross-origin setup
- **SQL Injection Prevention**: Entity Framework parameterized queries
- **Security Headers**: XSS protection, frame options, etc.
- **Non-root Containers**: Docker containers run as non-privileged users

## Troubleshooting 🔧

### Common Issues

**Services not starting:**
```bash
docker-compose down
docker-compose pull
docker-compose up -d --force-recreate
```

**Database connection issues:**
```bash
docker-compose exec database mysql -u gameuser -p
# Password: gamepassword
```

**Frontend not loading:**
- Check that port 3000 is available
- Verify backend is running on port 8080
- Check browser console for errors

**API not responding:**
- Check backend logs: `docker-compose logs backend`
- Verify database connection is healthy
- Ensure port 8080 is available

### Viewing Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f frontend
docker-compose logs -f backend
docker-compose logs -f database
```

## Contributing 🤝

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License 📄

This project is open source and available under the [MIT License](LICENSE).

## Acknowledgments 🙏

- Built with modern web technologies
- Inspired by the holiday spirit of Christmas
- Designed for casual gamers of all ages

---

**Happy Holidays and Enjoy Playing! 🎄🎅❄️**

*Made with Christmas spirit and modern web technologies*