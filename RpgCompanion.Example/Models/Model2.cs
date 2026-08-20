namespace RpgCompanion.Canva.Models;

using Core;

public class Model2 : IModel
{
    public int NumberValue { get; set; }
    public string TextValue { get; set; } = string.Empty;

    public string Display()
    {
        return $"{nameof(Model1)}: {NumberValue} - {TextValue}";
    }
}
