namespace RpgCompanion.Application.Reflection;

using System.Reflection;

internal class Reflect : IDisposable
{
    private readonly Dictionary<Type, Dictionary<string, MethodInfo>> _methods = [];
    private readonly Timer _cleanupTimer;

    public Reflect() => _cleanupTimer = new Timer(Clear, this, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));

    public MethodInfo? GetMethod(Type genericType, string methodName)
    {
        MethodInfo? method = default;
        if (!_methods.TryGetValue(genericType, out var methodsInfos))
        {
            methodsInfos = new Dictionary<string, MethodInfo>();
            _methods.Add(genericType, methodsInfos);
        }
        if (!methodsInfos.TryGetValue(methodName, out method))
        {
            method = genericType.GetMethod(methodName);
            if (method is not null)
            {
                methodsInfos.Add(methodName, method);
            }
        }
        return method;
    }
    private void Clear(object? state)
    {
        var r = (Reflect) state!;
        _methods.Clear();
    }

    public void Dispose()
    {
        _cleanupTimer?.Dispose();
    }
}
