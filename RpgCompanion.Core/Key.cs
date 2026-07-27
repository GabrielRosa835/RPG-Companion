namespace RpgCompanion.Core;

public abstract record Key(string Content);
public abstract record Key<T>(string Content) : Key(Content);
