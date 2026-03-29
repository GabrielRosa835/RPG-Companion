namespace Utils.UnionTypes;

public static class Catchers
{
    public static Attempt<TS, TF> Try<TS, TF>(Func<TS> function, TF onException)
    {
        try
        {
            return function();
        }
        catch (Exception)
        {
            return onException;
        }
    }
    public static Attempt<TS, TF> Try<TS, TF, TException>(Func<TS> function, TF onException) where TException : Exception
    {
        try
        {
            return function();
        }
        catch (TException)
        {
            return onException;
        }
    }
    public static Attempt<TS, TF> Try<TS, TF>(Func<TS> function, Func<Exception, TF> onException)
    {
        try
        {
            return function();
        }
        catch (Exception e)
        {
            return onException(e);
        }
    }
    public static Attempt<TS, TF> Try<TS, TF, TException>(Func<TS> function, Func<TException, TF> onException) where TException : Exception
    {
        try
        {
            return function();
        }
        catch (TException e)
        {
            return onException(e);
        }
    }

    public static Attempt<TS> Try<TS>(Func<TS> function)
    {
        try
        {
            return function();
        }
        catch (Exception e)
        {
            return e;
        }
    }
    public static Attempt<TS> Try<TS>(Func<TS> function, TS recovery)
    {
        try
        {
            return function();
        }
        catch
        {
            return recovery;
        }
    }
    public static Attempt<TS> Try<TS>(Func<TS> function, Func<TS> recovery)
    {
        try
        {
            return function();
        }
        catch
        {
            return recovery();
        }
    }
    public static Attempt<TS> Try<TS>(Func<TS> function, Func<Exception, TS> recovery)
    {
        try
        {
            return function();
        }
        catch (Exception e)
        {
            return recovery(e);
        }
    }
    public static Attempt<TS> Try<TS, TException>(Func<TS> function, Func<TException, TS> recovery) where TException : Exception
    {
        try
        {
            return function();
        }
        catch (TException e)
        {
            return recovery(e);
        }
    }
    public static Attempt<TS> Try<TS>(Func<TS> function, Action<Exception> onException)
    {
        try
        {
            return function();
        }
        catch (Exception e)
        {
            onException(e);
            return e;
        }
    }
    public static Attempt<TS> Try<TS, TException>(Func<TS> function, Action<TException> onException) where TException : Exception
    {
        try
        {
            return function();
        }
        catch (TException e)
        {
            onException(e);
            return e;
        }
    }

    public static Attempt Try(Action action)
    {
        try
        {
            action();
            return Attempt.Success();
        }
        catch (Exception e)
        {
            return e;
        }
    }
    public static Attempt Try(Action action, Action<Exception> onException)
    {
        try
        {
            action();
            return Attempt.Success();
        }
        catch (Exception e)
        {
            onException(e);
            return e;
        }
    }
    public static Attempt Try<TException>(Action action, Action<TException> onException) where TException : Exception
    {
        try
        {
            action();
            return Attempt.Success();
        }
        catch (TException e)
        {
            onException(e);
            return e;
        }
    }
}