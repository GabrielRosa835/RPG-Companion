using Utils.UnionTypes;

namespace Utils.Validation;

public interface IValidator<in T>
{
    Attempt Validate(T item);
}