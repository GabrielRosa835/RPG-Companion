namespace RpgCompanion.Core;

using System.Collections;

public static class Policies
{
    public static class Signals
    {
        public static ISignalTargetPolicy MultiTarget(params IEnumerable<IPlayer> targets) => new MultiTargetPolicy(targets);
        public static ISignalTargetPolicy SingleTarget(IPlayer target) => new SingleTargetPolicy(target);
        public static ISignalTargetPolicy MasterOnly => new MasterOnlyTargetPolicy();
        public static ISignalTargetPolicy Broadcast => new BroadcastPolicy();
    }

    public static class Questions
    {
        public static IQuestionTargetPolicy MultiTarget(params IEnumerable<IPlayer> targets) => new MultiTargetPolicy(targets);
        public static IQuestionTargetPolicy SingleTarget(IPlayer target) => new SingleTargetPolicy(target);
        public static IQuestionTargetPolicy MasterOnly => new MasterOnlyTargetPolicy();
        public static IQuestionTargetPolicy Broadcast => new BroadcastPolicy();
    }

    public static IGrouping<ClientId, ClientId> GroupOf(ClientId who, IEnumerable<ClientId> canSee) => new Grouping(who, canSee);

    private readonly record struct Grouping(ClientId Key, IEnumerable<ClientId> CanSee) : IGrouping<ClientId, ClientId>
    {
        public IEnumerator<ClientId> GetEnumerator() => CanSee.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
