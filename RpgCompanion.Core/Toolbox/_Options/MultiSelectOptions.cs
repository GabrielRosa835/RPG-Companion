namespace RpgCompanion.Toolbox;

public sealed class MultiSelectOptions<T> : OptionGroup<T>
{
    private MultiSelectOptions(
        IEnumerable<T> options,
        Func<T, T, bool>? funcComparer,
        IEqualityComparer<T>? objComparer)
        : base(options, funcComparer, objComparer)
    {
    }

    public static MultiSelectOptions<T> Create(params T[] options) => new(options, null, null);

    public static MultiSelectOptions<T> Create(IEnumerable<T> options, Func<T, T, bool>? funcComparer) =>
        new(options, funcComparer, null);

    public static MultiSelectOptions<T> Create(IEnumerable<T> options, IEqualityComparer<T>? comparer) =>
        new(options, null, comparer);

    public override void Select(int index)
    {
        if (index < 0 || index >= _options.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        var targetValue = _options[index];

        if (!Contains(targetValue))
        {
            _selection.Add(targetValue);
        }
        else
        {
            var itemToRemove = _selection.First(t => Equals(t, targetValue));
            _selection.Remove(itemToRemove);
        }
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
