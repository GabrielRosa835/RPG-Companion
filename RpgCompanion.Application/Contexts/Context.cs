namespace RpgCompanion.Application;

using RpgCompanion.Core.Contexts;
using RpgCompanion.Core.Engine;

internal class Context(Engine engine, PluginDescriptor plugin) : IEditableContext, IContext
{
    private readonly Engine _engine = engine;
    private readonly ContextData _data = new();
    private readonly ComponentProvider _registry = plugin.Registry;

    IEditableContextData IEditableContext.Data => _data;
    IContextData IContext.Data => _data;
    public IRegistry Registry => _registry;
}
