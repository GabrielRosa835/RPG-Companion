namespace RpgCompanion.Core.Toolbox;

using System.ComponentModel;

public abstract class OptionGroup<T> : INotifyPropertyChanged
{
    protected readonly IEqualityComparer<T>? _objComparer;
    protected readonly Func<T, T, bool>? _funcComparer;
    protected readonly Func<T, T, bool> _defaultComparer;

    protected readonly T[] _options;
    protected readonly List<T> _selection = [];

    public IReadOnlyList<T> Options => _options;
    public IReadOnlyList<T> Selection => _selection;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void NotifySelectionChanged() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selection)));

    protected OptionGroup(IEnumerable<T> options, Func<T, T, bool>? funcComparer, IEqualityComparer<T>? objComparer)
    {
        _options = ValidateOptions(options);
        _objComparer = objComparer;
        _funcComparer = funcComparer;
        _defaultComparer = (x, y) => (x, y) switch
        {
            (null, null) => true,
            (null, _) or (_, null) => false,
            _ => x.Equals(y),
        };
    }

    public abstract void Select(int index);
    public abstract void Select(T value);

    protected int IndexOf(T value)
    {
        if (_objComparer is not null)
        {
            return _options.IndexOf(value, _objComparer);
        }
        for (var i = 0; i < _options.Length; i++)
        {
            if (Equals(value, _options[i])) return i;
        }
        return -1;
    }

    protected bool Contains(T value)
    {
        return _objComparer is not null
            ? _selection.Contains(value, _objComparer)
            : _selection.Any(t => Equals(value, t));
    }

    protected bool Equals(T x, T y)
    {
        if (_objComparer is not null) return _objComparer.Equals(x, y);
        if (_funcComparer is not null) return _funcComparer.Invoke(x, y);
        return _defaultComparer.Invoke(x, y);
    }

    private T[] ValidateOptions(IEnumerable<T> options)
    {
        var array = options.ToArray();
        if (_objComparer is not null)
        {
            if (array.Distinct(_objComparer).Count() != array.Length)
            {
                throw new ArgumentException("Options must be unique");
            }
        }
        else if (_funcComparer is not null)
        {
            if (array.Distinct(_funcComparer).Count() != array.Length)
            {
                throw new ArgumentException("Options must be unique");
            }
        }
        else
        {
            if (array.Distinct(_defaultComparer).Count() != array.Length)
            {
                throw new ArgumentException("Options must be unique");
            }
        }
        return array;
    }
}
