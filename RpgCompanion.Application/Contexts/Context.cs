namespace RpgCompanion.Application;

using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;

internal class Context(ComponentProvider componentProvider) : IEditableContext, IContext
{
    private readonly ContextData _data = new();
    private readonly ComponentProvider _registry = componentProvider;

    IEditableContextData IEditableContext.Data => _data;
    IContextData IContext.Data => _data;
    public IRegistry Registry => _registry;
}
