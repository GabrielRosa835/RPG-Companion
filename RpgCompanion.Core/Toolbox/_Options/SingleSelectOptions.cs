namespace RpgCompanion.Core.Toolbox;

public sealed class SingleSelectOptions<T> : OptionGroup<T>
{
    private SingleSelectOptions(
        IEnumerable<T> options,
        Func<T, T, bool>? funcComparer,
        IEqualityComparer<T>? objComparer)
        : base(options, funcComparer, objComparer)
    {
    }

    public T? Current => _selection.Count > 0 ? _selection[0] : default;

    public static SingleSelectOptions<T> Create(params T[] options) => new(options, null, null);

    public static SingleSelectOptions<T> Create(IEnumerable<T> options, Func<T, T, bool>? funcComparer) =>
        new(options, funcComparer, null);

    public static SingleSelectOptions<T> Create(IEnumerable<T> options, IEqualityComparer<T>? comparer) =>
        new(options, null, comparer);

    public override void Select(int index)
    {
        if (index < 0 || index >= _options.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        var targetValue = _options[index];

        if (_selection.Count == 1 && Equals(_selection[0], targetValue))
        {
            return;
        }

        _selection.Clear();
        _selection.Add(targetValue);
        NotifySelectionChanged();
    }

    public override void Select(T value)
    {
        var index = IndexOf(value);
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }
        Select(index);
    }
}
