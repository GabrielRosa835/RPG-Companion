namespace RpgCompanion.Core.Toolbox;


public class Choice<T>
{
    private IEqualityComparer<T>? _comparer;
    private Func<T, T, bool>? _funcComparer;
    private bool _singleSelect;
    private T[] _options;
    private List<T> _selection = [];

    public bool Mode => _singleSelect;
    public IReadOnlyList<T> Options => _options;
    public IReadOnlyList<T> Selection => _selection;

    private Choice(IEnumerable<T> options, bool singleSelect, Func<T, T, bool>? funcComparer,
        IEqualityComparer<T>? comparer)
    {
        _options = options.ToArray();
        _funcComparer = funcComparer;
        _singleSelect = singleSelect;
        _comparer = comparer;
    }

    public static Choice<T> SingleSelect(params T[] options) => new(options, true, null, null);

    public static Choice<T> SingleSelect(IEnumerable<T> options, Func<T, T, bool>? funcComparer) =>
        new(options, true, funcComparer, null);

    public static Choice<T> SingleSelect(IEnumerable<T> options, IEqualityComparer<T>? comparer) =>
        new(options, true, null, comparer);

    public static Choice<T> MultiSelect(params T[] options) => new(options, false, null, null);

    public static Choice<T> MultiSelect(IEnumerable<T> options, Func<T, T, bool>? funcComparer) =>
        new(options, false, funcComparer, null);

    public static Choice<T> MultiSelect(IEnumerable<T> options, IEqualityComparer<T>? comparer) =>
        new(options, false, null, comparer);

    public T Select(int index)
    {
        if (index < 0 || index >= _options.Length)
            throw new ArgumentOutOfRangeException(nameof(index));
        if (_singleSelect) _selection.Clear();
        _selection.Add(_options[index]);
        return _selection[index];
    }

    public T Select(T value)
    {
        if (_singleSelect) _selection.Clear();
        var index = _options.IndexOf(value, _comparer);
        _selection.Add(_options[index]);
        return _selection[index];
    }

    private int IndexOf(T value)
    {
        for (var i = 0; i < _options.Length; i++)
        {
            if (Equals(value, _options[i])) return i;
        }
        throw new ArgumentException("The specified value is not included in the available options");
    }

    private bool Equals(T x, T y)
    {
        if (_comparer is not null) return _comparer.Equals(x, y);
        if (_funcComparer is not null) return _funcComparer.Invoke(x, y);
        return x?.Equals(y) == true;
    }
}
