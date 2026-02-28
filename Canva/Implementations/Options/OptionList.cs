namespace RpgCompanion.Canva;

public class OptionList
{
   public IOption Choice { get; set; } = default!;
}
public class OptionList<T>
{
   public T Choice { get; set; } = default!;
}
public class TargetList : OptionList<ITarget>
{

}