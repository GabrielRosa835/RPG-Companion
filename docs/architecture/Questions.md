# Questions & Delegation API

The `Question` abstraction is the core mechanism by which the RPG-Companion host server (or its plugins) delegates decisions and requests information from human players via connected clients (Web, Mobile, CLI). It bridges the gap between the server's fast, synchronous/asynchronous logic and the slower, non-deterministic nature of human input.

## Purpose: Bridging Logic and Human Intent
In a TTRPG, many actions require human input (e.g., choosing a target, deciding to use a reaction, voting). The `Question` API safely pauses the server's execution state, delegates a choice to the connected clients, and resumes execution once a valid answer is provided. It achieves this strictly without the server ever knowing about UI elements, adhering strictly to the "UI-Agnostic" principle.

## The Abstractions

### 1. Policies: Managing the Social Dynamics of a Table
Questions are composed of policies that dictate the flow and visibility of information:
- **`ITargetPolicy` (Who answers?)**: Routes the question to specific targets (e.g., a specific player, a group like "all elves", or the Game Master).
- **`ISecrecyPolicy` (Who sees what?)**: Determines the visibility of the question and its answers. Essential for TTRPGs where secrets are common (e.g., hidden questions, blind votes).
- **`IBlockingPolicy` (Who waits?)**: Dictates whose UI should be blocked (e.g., showing a loading spinner) while waiting for the response, allowing non-involved players to continue interacting with the app.

### 2. `IResponseSchema<T>`: The UI-Agnostic Lynchpin
Instead of sending HTML or UI components, the server sends a strongly-typed schema (e.g., a `NumberSchema` for a range, or `SelectionSchema` for options). The client is responsible for interpreting this schema and rendering the appropriate UI component (slider, dropdown, radial menu) based on user preference and device form factor.

### 3. Core Definition: `IQuestion` & `IQuestionBuilder`
Following a declarative, fluent design (similar to EF Core's `IEntityTypeConfiguration`), developers define questions by injecting policies and schemas via a builder:
```csharp
public interface IQuestion<TResponse>
{
    void Define(IQuestionBuilder<TResponse> builder, IQuestionContext context);
}
```

### 4. Contexts: `IQuestionContext` & `IResponseContext`
Context objects provide scoped access to ambient data and the DI container via the `IRegistry` wrapper. 
- **`IQuestionContext`**: Provides access to the `ISessionContext` (current players, session state) when building the question. 
- **Extensibility**: These contexts also act as a forward-compatible mechanism to add properties or methods as the framework evolves without breaking the interface contract.

## Developer Experience (DX)
The DX for plugin creators is linear, robust, and highly readable. Developers use `IQuestionPublisher` to ask questions and handle the `ResponseResult` discriminated union. They do not write WebSocket event listeners or manage state.

```csharp
var result = await publisher.AskAsync(new ChooseTargetQuestion(availableTargets));

switch(result)
{
    case ResponseResult.Success<Target> success:
        ApplyDamage(success.Value);
        break;
    case ResponseResult.Timeout timeout:
        // Handle AFK player
        break;
}
```
By forcing the handling of edge cases (like `Timeout` or disconnects) via pattern matching, plugins remain stable and predictable.

## Integration with Core Principles
- **"Give them the tools..."**: The core framework provides the coordination engine and robust policies. Plugin developers compose these tools to create complex interactions.
- **Security**: The server orchestrates the expected `IResponseSchema` and `ITargetPolicy`. The `QuestionCoordinator` strictly validates incoming responses against these rules before resuming execution, preventing clients from spoofing answers or answering on behalf of others.
