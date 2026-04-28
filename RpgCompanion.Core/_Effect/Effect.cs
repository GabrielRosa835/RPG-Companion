namespace RpgCompanion.Core;

public delegate TResult Effect<in T, out TResult>(T current);
