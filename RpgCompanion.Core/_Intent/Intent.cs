namespace RpgCompanion.Core;

public interface IIntentBase;

public interface IIntent : IIntentBase;

public interface IIntent<out TResult> : IIntentBase;
