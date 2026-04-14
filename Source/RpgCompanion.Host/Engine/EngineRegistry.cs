namespace RpgCompanion.Application;

using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using Core.Engine;
using Core.Engine.Contexts;
using Engine;
using Engines;
using Microsoft.Extensions.DependencyInjection;
using Reflection;
using RpgCompanion.Engine.Components;
using Services;

internal static class AppServices
{
    internal static void AddPluginServices(
        this IServiceCollection services,
        ComponentCollection components)
    {

    }
}
