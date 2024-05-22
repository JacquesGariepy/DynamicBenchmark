using BenchmarkDotNet.Attributes;
using System.Reflection;

public class BenchmarkDiscovery
{
    public static IEnumerable<MethodInfo> DiscoverBenchmarkedMethods(Assembly assembly)
    {
        return assembly.GetTypes()
            .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Where(method => method.GetCustomAttributes<BenchmarkAttribute>().Any());
    }
}