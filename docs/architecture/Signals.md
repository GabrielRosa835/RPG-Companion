# Signal API Architecture

## Overview

The Signal API provides a simplified, non-blocking form of server-to-client communication within the RPG-Companion ecosystem. It contrasts with the existing `Question` and `Intent` APIs by focusing on lightweight, one-way messaging (push notifications or reactive events) from the server to connected clients. 

## Key Design Principles

1. **Lightweight Payloads**: Signals can be parameterless (acting simply as triggers that prompt the client to fetch state via Intents) or carry an optional payload (DTO). Including an optional payload reduces round-trips for simple updates, like HP changes, while maintaining a lightweight footprint.
2. **Configurable Delivery & Monitoring**: To preserve an explicitly non-blocking developer experience, sending a signal does not return an awaitable `Task` directly. Instead, `ISignalSender.Send` returns an `ISignalTracker`.
    - **Fire-and-Forget**: The developer can safely ignore the return value.
    - **Guaranteed Delivery**: If edge cases need handling, the developer can await the signal using `tracker.WaitAllAsync()`.
3. **Flexible Targeting & Routing**: Signal target policies (defining which clients receive the signal) are built using an `ISignalBuilder`, analogous to the `Question` API. This unifies the domain language while enabling runtime configuration overrides when sending.
4. **Native SignalR Integration**: Signals leverage native SignalR topics (prefixed with `PluginId.SignalName`) rather than relying on a complex custom unified wrapper stream. This simplifies client-side consumption, allowing it to work cleanly with out-of-the-box SignalR/WebSockets without requiring a heavy custom SDK, and prevents naming collisions between plugins.

## Core Interfaces

The API abstractions are located in `RpgCompanion.Core/_Components/_Signals`:

- **`ISignal` / `ISignal<TPayload>`**: Defines the base signal contract. Implementing classes define their builder and context requirements.
- **`ISignalBuilder` / `ISignalBuilder<TPayload>`**: Used to construct and configure the signal, most notably defining target policies via `WithTargets()` and associating payloads.
- **`ISignalContext`**: Provides execution context, such as access to the `ISessionContext`.
- **`ISignalTargetPolicy`**: Determines the set of `ClientId`s that should receive the signal given a context.
- **`ISignalSender`**: The core service used to dispatch a signal. It returns an `ISignalTracker`.
- **`ISignalTracker`**: An observer/handle returned when a signal is sent. Provides `WaitAllAsync(CancellationToken)` to monitor the delivery status of the signal.
