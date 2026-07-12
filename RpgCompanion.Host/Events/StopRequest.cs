namespace RpgCompanion.Host.Events;

using Core;

internal abstract record StopRequest
{
    internal sealed record Exiting(EventResult Reason) : StopRequest;
    internal sealed record Halting(EventResult Reason) : StopRequest;
    internal sealed record Terminating(EventResult Reason) : StopRequest;
    internal sealed record None : StopRequest;

    public static implicit operator bool(StopRequest request) => request is not None;
}
