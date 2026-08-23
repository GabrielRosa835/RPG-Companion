namespace RpgCompanion.Core;

public abstract record RuleResult<T>
{
    public static implicit operator RuleResult<T>(T value) => new Success(value);
    public static implicit operator RuleResult<T>(Exception exception) => new Failure(exception);

    public T Unwrap() => this switch
    {
        Success s => s.Value,
        Failure b => throw b.Exception,
        _ => throw new InvalidRuleResultException(),
    };

    public record Failure(Exception Exception) : RuleResult<T>
    {
        public static implicit operator Failure(Exception exception) => new(exception);
    }

    public record Success(T Value) : RuleResult<T>
    {
        public static implicit operator Success(T value) => new(value);
    }
}

public static class RuleResultMappers
{
    public static RuleResult<TResult> Map<T, TResult>(
        this RuleResult<T> result, Func<T, TResult> mapper) => result switch
    {
        RuleResult<T>.Success c => RuleResult.Safely(() => mapper(c.Value)),
        RuleResult<T>.Failure b => b.Exception,
        _ => throw new InvalidRuleResultException(),
    };

    public static RuleResult<TResult> FlatMap<T, TResult>(
        this RuleResult<T> result, Func<T, RuleResult<TResult>> mapper)
    {
        return result switch
        {
            RuleResult<T>.Success c => RuleResult.Try(() => mapper(c.Value)),
            RuleResult<T>.Failure b => b.Exception,
            _ => throw new InvalidRuleResultException(),
        };
    }

    public static Task<RuleResult<TResult>> MapAsync<T, TResult>(
        this RuleResult<T> result, Func<T, Task<TResult>> asyncMapper) => result switch
    {
        RuleResult<T>.Success c => RuleResult.SafelyAsync(() => asyncMapper(c.Value)),
        RuleResult<T>.Failure b => Task.FromResult<RuleResult<TResult>>(b.Exception),
        _ => throw new InvalidRuleResultException(),
    };

    public static Task<RuleResult<TResult>> FlatMapAsync<T, TResult>(
        this RuleResult<T> result, Func<T, Task<RuleResult<TResult>>> asyncMapper) => result switch
    {
        RuleResult<T>.Success c => RuleResult.TryAsync(() => asyncMapper(c.Value)),
        RuleResult<T>.Failure b => Task.FromResult<RuleResult<TResult>>(b.Exception),
        _ => throw new InvalidRuleResultException(),
    };
}

public static class RuleResultAsyncMappers
{
    public static async Task<RuleResult<TResult>> Map<T, TResult>(
        this Task<RuleResult<T>> resultTask, Func<T, TResult> mapper) => await resultTask switch
    {
        RuleResult<T>.Success c => RuleResult.Safely(() => mapper(c.Value)),
        RuleResult<T>.Failure b => b.Exception,
        _ => throw new InvalidRuleResultException(),
    };

    public static async Task<RuleResult<TResult>> MapAsync<T, TResult>(
        this Task<RuleResult<T>> resultTask, Func<T, Task<TResult>> mapper) => await resultTask switch
    {
        RuleResult<T>.Success c => await RuleResult.SafelyAsync(() => mapper(c.Value)),
        RuleResult<T>.Failure b => b.Exception,
        _ => throw new InvalidRuleResultException(),
    };

    public static async Task<RuleResult<TResult>> FlatMap<T, TResult>(
        this Task<RuleResult<T>> resultTask, Func<T, RuleResult<TResult>> mapper) => await resultTask switch
    {
        RuleResult<T>.Success c => RuleResult.Try(() => mapper(c.Value)),
        RuleResult<T>.Failure b => b.Exception,
        _ => throw new InvalidRuleResultException(),
    };

    public static async Task<RuleResult<TResult>> FlatMapAsync<T, TResult>(
        this Task<RuleResult<T>> resultTask, Func<T, Task<RuleResult<TResult>>> mapper) => await resultTask switch
    {
        RuleResult<T>.Success c => await RuleResult.TryAsync(() => mapper(c.Value)),
        RuleResult<T>.Failure b => b.Exception,
        _ => throw new InvalidRuleResultException(),
    };
}

public static class RuleResult
{
    public static RuleResult<T> Failure<T>(Exception exception) => new RuleResult<T>.Failure(exception);
    public static RuleResult<T> Success<T>(T value) => new RuleResult<T>.Success(value);

    public static RuleResult<T> Safely<T>(Func<T> supplier)
    {
        try
        {
            return supplier();
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static RuleResult<T> Try<T>(Func<RuleResult<T>> supplier)
    {
        try
        {
            return supplier();
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static async Task<RuleResult<T>> SafelyAsync<T>(Func<Task<T>> supplier)
    {
        try
        {
            return await supplier();
        }
        catch (Exception e)
        {
            return e;
        }
    }

    public static async Task<RuleResult<T>> TryAsync<T>(Func<Task<RuleResult<T>>> supplier)
    {
        try
        {
            return await supplier();
        }
        catch (Exception e)
        {
            return e;
        }
    }
}

public class EmptyException : Exception;

public class InvalidRuleResultException : Exception
{
    public InvalidRuleResultException() : base("Invalid rule result")
    {
    }
}
