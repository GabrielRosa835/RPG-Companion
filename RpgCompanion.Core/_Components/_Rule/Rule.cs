namespace RpgCompanion.Core;

public delegate T Rule<T>(T subject, RuleContext context);

public delegate U Rule<in T, out U>(T subject, RuleContext context);

//====================================================================

// internal delegate IEvent Action<T>(T subject, RuleContext<T> context);
//
// internal delegate T Effect<T>(T subject, RuleContext<T> context);
//
// internal delegate bool Condition<T>(T subject, RuleContext<T> context);
