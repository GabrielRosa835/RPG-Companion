namespace RpgCompanion.Core.Options;

public class OptionList
{
   private readonly List<object> _values;
   public object Choice { get; set; } = default!;
   public OptionList (List<object> values) => _values = values;
   public void Select(int index)
   {
      if (index < 0 || index >= _values.Count) return;
      Choice = _values[index];
   }
}
public class OptionList<T>
{
   private readonly List<T> _values;
   public OptionList (List<T> values) => _values = values;
   public T Choice { get; set; } = default!;
   public void Select(int index)
   {
      if (index < 0 || index >= _values.Count) return;
      Choice = _values[index];
   }
}