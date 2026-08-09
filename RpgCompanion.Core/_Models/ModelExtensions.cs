namespace RpgCompanion.Core;

public static class ModelExtensions
{
    extension<TModel>(TModel subject) where TModel : IModel
    {
        public ModelContent Get(string fieldName)
        {
            ArgumentNullException.ThrowIfNull(subject);
            ArgumentNullException.ThrowIfNull(fieldName);
            var extensions = Model.Extensions(subject);
            if (!extensions.Properties.TryGetValue(fieldName, out var content))
            {
                return new ModelContent.None();
            }
            return content;
        }

        public void Set(string fieldName, ModelContent content)
        {
            ArgumentNullException.ThrowIfNull(subject);
            ArgumentNullException.ThrowIfNull(fieldName);
            var extensions = Model.Extensions(subject);
            extensions.Properties[fieldName] = content;
        }

        public DatabaseId Id
        {
            get =>
                Model.Get(subject, "Id") switch
                {
                    ModelContent.Generic<DatabaseId> g => g.Value,
                    ModelContent.String s => new DatabaseId(s.Value),
                    _ => throw new InvalidOperationException()
                };
            set => Model.Set(subject, "Id", new ModelContent.Generic<DatabaseId>(DatabaseId.Create(value.Value)));
        }
    }
}
