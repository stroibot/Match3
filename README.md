# Project Architecture Overview

While working on this project, I focused on creating a well-structured and maintainable architecture, which can be easily expanded in the future. I adhered to industry best practices and followed several key principles:

## Key Principles Followed
- **SOLID** Ensured the design is modular, flexible, and maintainable.
- **DRY** Minimized code duplication to increase efficiency.
- **KISS** Focused on simplicity to improve readability and maintainability.

## Code and Style Practices
- **Naming Conventions** Followed standard C# naming conventions to enhance readability and ease of use for all developers.
- **Indentation** Used tabs instead of spaces for consistency, in alignment with standard style practices.

Although these style choices are generally preferred, I can adapt to company-specific style guides when necessary.

## Structural Changes
- **Restructured the Project** Organized the project to improve maintainability and scalability.
- **Assembly Definitions** Introduced assembly definitions to separate the code into logical modules.

## Design Patterns Implemented
- **Dependency Injection** Applied the DI pattern using the VContainer DI container to facilitate Inversion of Control.
- **State Machine** Utilized the State Machine pattern to track the game flow, where each state encapsulates specific game logic.
- **Factories and Pooling Systems** Introduced factories and pooling mechanisms, applicable to both plain C# objects and Unity's GameObjects, for efficient resource management.

## Architecture and Logic
- **Separation of Concerns**:
  - **Models** Plain C# classes that hold data and game logic.
  - **Views** MonoBehaviours that observe the models and react to changes, updating the view accordingly.
- **Single Responsibility Principle** Each part of the game is well-defined, with clear responsibilities.
- **Efficient Algorithms** Focused on optimizing the game logic to minimize unintended matches and improve performance.

## Unit Testing
- **Clean Architecture** Because of the clean architecture and adherence to design patterns and best principles, it was possible to create comprehensive unit tests.
- **Unit Tests Example** The modular design facilitated the development of unit tests for critical game components, ensuring robustness and reliability.
