namespace RpgCompanion.Core;

public delegate T Rule<T>(T t);
public delegate U Rule<in T, out U>(T t);
