namespace RpgCompanion.Core;

public abstract record ModelContent
{
    public record None : ModelContent;

    public record String(string Value) : ModelContent
    {
        public String() : this(string.Empty)
        {
        }
    }

    public record Generic<T>(T Value) : ModelContent;

    public record Number(decimal Value) : ModelContent
    {
        public Number() : this(default(decimal))
        {
        }
    }

    public record Boolean(bool Value) : ModelContent
    {
        public Boolean() : this(default(bool))
        {
        }
    }

    public record Null : ModelContent;

    public record Object(Dictionary<string, ModelContent> Properties) : ModelContent
    {
        public Object() : this(new Dictionary<string, ModelContent>())
        {
        }
    }

    public record List(List<ModelContent> Items) : ModelContent
    {
        public List() : this([])
        {
        }
    }
}
