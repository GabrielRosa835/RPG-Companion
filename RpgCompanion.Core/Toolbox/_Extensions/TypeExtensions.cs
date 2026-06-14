namespace RpgCompanion.Core.Toolbox;

public static class TypeExtensions
{
    extension(Type type)
    {
        /// <summary>
        /// Checks if the type is allowed to have inner fields
        /// </summary>
        public bool IsComplex => type.IsClass || type is { IsValueType: true, IsPrimitive: false, IsEnum: false };
    }
}
