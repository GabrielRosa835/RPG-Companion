# Events API

**Events** represent the core abstraction for applying TTRPG rules and sequences within the RPG-Companion ecosystem. Designed around the **State Machine** pattern, the Events API captures the step-by-step nature of game rules, where each `Event` acts as a discrete "State" and the `EventEngine` serves as the managing "Machine".

## Surface: The Core Abstractions (PDK)

Plugin developers interact primarily with the abstractions provided by the Plugin Development Kit (PDK). 

### The `Event` Base Class
An `Event` defines three overridable lifecycle methods that dictate its behavior during a pipeline:
- **`Setup`**: Initialization logic, parameter validation, or starting resources (e.g., a stopwatch).
- **`Execute`**: The core logic phase. It runs repeatedly in a loop until the event transitions. 
- **`Teardown`**: Cleanup operations (e.g., disposing of a timer) that safely run even if the pipeline is cancelled.

All methods return a `ValueTask` to allow asynchronous operations seamlessly. 

### The `IEventContext`
Passed into every lifecycle method, the `IEventContext` is the event's bridge to ambient data and DI scoped services (via `IRegistry` and `IStorage`). It provides the control flow methods to determine the pipeline's **resolution**:
- **`Continue(Event)` / `Continue<TEvent>()`**: Transitions the state machine to the next event. Developers can instantiate the next event manually or delegate it to the `IEventFactory` abstraction. While plugin developers can implement their own factory logic, the default factory provided by the host automatically resolves events from the DI container.
- **`Exit(EventResult)`**: Breaks the pipeline gracefully and returns a specific payload.
- **`Halt(bool throwException)`**: Immediately terminates the pipeline by triggering the `CancellationTokenSource`.
- **Wait**: If no resolution is invoked, the `Execute` method naturally yields and will be invoked again in the next tick.

## Depths: Host Implementation

The host server implements the robust engine running these states.

### `EventEngine` & Pipelines
When an event is raised, the `EventEngine` spins up an awaitable pipeline managed by an `EventExecutionContext`. This context holds the pipeline's DI scope, state flags, and the cancellation token. 
- To prevent plugins from freezing the host with infinite synchronous loops, the engine enforces a `Task.Delay` yield between `Execute` iterations. 
- Events can declare a custom `ExecutionInterval`. If omitted, a fallback of **100ms** is used, and a hard minimum limit of **10ms** ensures host stability.
- *(Planned)* To ensure fair resource distribution among plugins, each plugin's singleton `EventEngine` will cap active concurrent pipelines to a maximum limit (e.g., 10).

#### Cancellation and Safety
The `EventEngine` employs specific safety mechanisms regarding pipeline cancellation:
- **Immediate Halting:** Every execution phase (`Setup`, `Execute`, `Teardown`) is individually wrapped in a `try/catch` block that specifically catches `OperationCanceledException`. This allows a developer to invoke `Halt(throwException: true)`, which throws the exception and immediately breaks out of the current phase, halting the pipeline in place without crashing the host.
- **Safe Teardown:** If the pipeline is cancelled or halted before reaching the `Teardown` phase, the engine ensures that an uncancellable token (`CancellationToken.None`) is passed down to `Teardown`. This guarantees that the event has a safe, uninterrupted window to dispose of resources, clear timers, or finalize states.

### Handling `EventResult`
Awaiting a pipeline returns an `EventResult`. This record type acts as a discriminated union encapsulating the pipeline's ending state. The caller is responsible for pattern-matching this result and handling edge cases gracefully. The core framework provides the following built-in results:
- **`None`**: Represents an empty or default outcome.
- **`Halted`**: Indicates the pipeline was explicitly terminated (e.g., cancelled) before reaching a natural conclusion.
- **`Stopped`**: Indicates the pipeline was explicitly exited for termination .
- **`Completed<TResult>`**: Indicates the event exited successfully, providing a custom result payload of type `TResult`.
- **`Faulted`**: Indicates an unhandled exception crashed the pipeline, encapsulating the thrown `Exception`.

*(Extensibility)*: Since `EventResult` is an abstract record, plugin developers are free to define their own custom result types by extending it, giving them complete flexibility in how they communicate pipeline outcomes back to the caller.
