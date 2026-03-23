namespace RpgCompanion.Application.Services;

using System.Reflection;
using Core.Contexts;
using Core.Events;
using Reflection;

internal class BundlerDescriptor : ComponentDescriptor
{
    public Type EventType { get; set; } = default!;
    public Delegate? Action { get; set; }

    public void Apply(Reflect reflect, IEvent e, IEditableContext context) =>
        InvokeMethod(reflect, nameof(IBundler<>.Bundle), this.Action, e, context);
}
