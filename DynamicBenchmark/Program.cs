using Benchmark;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Configuration;
using System.Reflection;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        var configuration = builder.Build();

        var config = ManualConfig.Create(DefaultConfig.Instance)
            .AddColumn(StatisticColumn.OperationsPerSecond)
            .AddDiagnoser(MemoryDiagnoser.Default);

        new AutoBenchmark(config, args, configuration).BenchmarkAllMethods();
    }

    private class CustomCategoryDiscoverer : DefaultCategoryDiscoverer
    {
        public override string[] GetCategories(MethodInfo method)
        {
            var categories = new List<string>();
            categories.AddRange(base.GetCategories(method));
            categories.Add("All");
            categories.Add(method.Name.Substring(0, 1));
            return categories.ToArray();
        }
    }
}