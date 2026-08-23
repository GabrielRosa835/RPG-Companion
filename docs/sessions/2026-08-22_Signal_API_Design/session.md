# Session: Signal API Design
**Date:** 2026-08-22

## Purpose
Brainstorming and defining the abstraction for the new `Signal` API. The Signal API acts as a simplified, non-blocking form of server-to-client communication, contrasting with the existing `Question` and `Intent` APIs.

## Decisions Made

1. **Payloads**
   - **Decision:** Signals will support an optional payload (DTO).
   - **Rationale:** While purely parameterless triggers could work by forcing the client to fetch state via Intents, including an optional payload reduces round-trips for simple updates (e.g., HP changes) while remaining lightweight.

2. **Delivery Guarantees & Monitoring**
   - **Decision:** Delivery will be configurable. The `Send` method will remain non-blocking (not returning a `Task` to be awaited), but will return an `ISignalTracker` (Handle/Observer pattern).
   - **Rationale:** This preserves the explicitly non-blocking DX. The developer can ignore the return value for fire-and-forget, or optionally attach callbacks (`tracker.OnFailed()`) or await it later (`tracker.WaitForAllAsync()`) if they need to handle edge cases or guarantee delivery.

3. **Targeting & Routing**
   - **Decision:** Targeting policies will be defined at the Signal level via an `ISignalBuilder` (similar to Questions), but the `Send` method will accept an `Action<ISignalBuilder>? overrides = null` to allow runtime configuration.
   - **Rationale:** This unifies the framework's domain language with Questions while providing necessary runtime flexibility for reactive events.

4. **Client Consumption & Topic Routing**
   - **Decision:** We lean towards native SignalR topics (prefixed with `PluginId.SignalName`) rather than a unified wrapper stream.
   - **Rationale:** This keeps the client-side implementation simple and native to out-of-the-box SignalR/WebSockets, avoiding the need for a custom client-side parsing SDK, while still preventing naming collisions between plugins on the server.

## Next Steps
- Draft the core interfaces (`ISignal`, `ISignalPublisher`, `ISignalTracker`, `ISignalBuilder`) in `RpgCompanion.Canva/_Signals` based on these design decisions.
- Implement the internal SignalR dispatching mechanism honoring the tracking and targeting policies.
