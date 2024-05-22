using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Configuration;
using System.Reflection;

public class AutoBenchmark
{
    private readonly IConfig _iConfig;
    private readonly string[] _args;
    private readonly IConfigurationRoot _configurationRoot;
    private DatabaseContext _databaseContext;
    private string _baseDirectory;

    public AutoBenchmark(IConfig iConfig, string[] args, IConfigurationRoot configurationRoot)
    {
        _iConfig = iConfig;
        _args = args;
        _configurationRoot = configurationRoot;
        Setup();
    }

    [GlobalSetup]
    public void Setup()
    {
        _databaseContext = new DatabaseContext(new PostgreSQLStrategy(_configurationRoot));

        // Get the base directory from the configuration
        _baseDirectory = _configurationRoot["BaseDirectory"];

        // If the base directory is not set in the configuration, use the current assembly location
        if (string.IsNullOrEmpty(_baseDirectory))
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            _baseDirectory = Path.GetDirectoryName(currentAssembly.Location);
        }
    }

    [Benchmark]
    public void BenchmarkAllMethods()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var baseDirectory = Path.GetDirectoryName(currentAssembly.Location);
        var directoryInfo = new DirectoryInfo(baseDirectory);
        foreach (var file in directoryInfo.GetFiles("*.dll"))
        {
            RunBenchmarksInAssembly(file.FullName);
        }
    }

    private void RunBenchmarksInAssembly(string assemblyPath)
    {
        var assembly = Assembly.LoadFile(assemblyPath);
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            RunBenchmarksInType(type);
        }
    }

    private void RunBenchmarksInType(Type type)
    {
        var methods = type.GetMethods();
        foreach (var method in methods)
        {
            RunBenchmarkIfPresent(method);
        }
    }

    private void RunBenchmarkIfPresent(MethodInfo method)
    {
        var benchmarkAttribute = method.GetCustomAttribute<BenchmarkAttribute>();
        if (benchmarkAttribute != null)
        {
            var summary = BenchmarkRunner.Run(method.DeclaringType, _iConfig, _args);
            _databaseContext.SaveResults(summary);
        }
    }
}
