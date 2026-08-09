namespace RpgCompanion.Canva.Models;

using Core;

public static class Model1
{
    public class Model : IModel
    {
        public int NumberValue { get; set; }
        public string TextValue { get; set; } = string.Empty;
    }

    extension(Model model)
    {
        public string Display()
        {
            return $"{nameof(Model1)}: {model.NumberValue} - {model.TextValue}";
        }
    }
}
