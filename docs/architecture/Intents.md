# Intents

**Intents** serve as an abstraction for user-triggered actions, utilizing a Request-Handler-Mediator pattern (similar to CQRS).

## Core Components
- **Intent:** A named collection of arguments and the expected type of result.
- **Handler:** The specific logic responsible for processing a given Intent.
- **Dispatcher:** Resolves the appropriate processor at runtime, dispatches the intent, and returns the result to the caller.
