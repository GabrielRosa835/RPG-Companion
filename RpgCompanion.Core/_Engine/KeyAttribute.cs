namespace RpgCompanion.Core;

using Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Parameter)]
public class KeyAttribute(string key) : FromKeyedServicesAttribute(key);
