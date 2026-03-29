namespace Utils.UnionTypes;

public static class AttemptAsync
{
    public static Task<TR> Either<TR>(
        this Task<Attempt> attemptTask, Func<TR> onSuccess, Func<Exception, TR> onFailure, CancellationToken cancellationToken = default)
    {
        return attemptTask.ContinueWith(task => task.Result.Either(onSuccess, onFailure), cancellationToken);
    }

   public static Task<bool> IsFailure(this Task<Attempt> attemptTask)
   {
      return attemptTask.ContinueWith(task => task.Result.IsFailure);
   }
   public static Task<bool> IsSuccess (this Task<Attempt> attemptTask)
   {
      return attemptTask.ContinueWith(task => task.Result.IsSuccess);
   }
}