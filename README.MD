# Scripts Overview

This project is a Unity-based game that utilizes various programming and object-oriented programming (OOP) concepts to manage game mechanics, user interactions, and data persistence. Below is an overview of the key scripts and the concepts they demonstrate. **To try the game, check release [link](https://github.com/den-dimas/Ball-Bouncer/releases/tag/Release)**

## Key Scripts and Concepts

### 1. Singleton Pattern
The Singleton pattern ensures that a class has only one instance and provides a global point of access to it. This pattern is used in several manager classes to maintain a single instance throughout the game.

- **SaveManager**: Manages user data, saving, and loading game states.
  - File: [SaveManager.cs](Assets/Scripts/Managers/SaveManager.cs)
- **AchievementManager**: Manages achievements and updates them based on user actions.
  - File: [AchievementManager.cs](Assets/Scripts/Managers/AchievementManager.cs)
- **GameManager**: Manages the overall game state and transitions between different game states.
  - File: [GameManager.cs](Assets/Scripts/Managers/GameManager.cs)

### 2. Event-Driven Programming
Event-driven programming is used to handle various game events and user interactions. The `EventBus` is used to publish and subscribe to events. [EventBus.cs](Assets/Scripts/Utils/Events/EventBus.cs)

- **TutorialUI**: Subscribes to game state events to show or hide the tutorial UI.
  - File: [TutorialUI.cs](Assets/Scripts/UI/TutorialUI.cs)
- **UserOptionsUI**: Handles user account management and updates the UI based on user actions.
  - File: [UserOptionsUI.cs](Assets/Scripts/UI/UserOptionsUI.cs)

### 3. Coroutines
Coroutines are used to perform actions over time without blocking the main thread. This is useful for animations, waiting for a certain condition, or delaying actions.

- **UserOptionsUI**: Uses coroutines to show error messages for a limited time.
  - File: [UserOptionsUI.cs](Assets/Scripts/UI/UserOptionsUI.cs)

### 4. Input Handling
Unity's new Input System is used to handle player inputs, such as movement and actions.

- **Platform**: Handles player movement and input actions.
  - File: [Platform.cs](Assets/Scripts/Player/Platform.cs)
- **Player**: Manages player-specific actions and interactions.
  - File: [Player.cs](Assets/Scripts/Player/Player.cs)

### 5. Data Persistence
Data persistence is achieved using the MessagePack library to serialize and deserialize game data. This ensures that user progress and game states are saved and loaded efficiently.

- **SaveManager**: Handles saving and loading of user data using MessagePack.
  - File: [SaveManager.cs](Assets/Scripts/Managers/SaveManager.cs)

### 6. Object Pooling
Object pooling is used to manage the creation and reuse of game objects, such as enemies, to improve performance and reduce memory usage.

- **EnemySpawner**: Manages the pooling of enemy objects.
  - File: [EnemySpawner.cs](Assets/Scripts/Enemy/EnemySpawner.cs)

## Conclusion

This project demonstrates various programming and OOP concepts to create a well-structured and efficient game. By utilizing patterns like Singleton, event-driven programming, coroutines, input handling, data persistence, and object pooling, the project ensures maintainability, scalability, and performance.
