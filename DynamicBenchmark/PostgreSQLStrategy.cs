using BenchmarkDotNet.Reports;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class PostgreSQLStrategy : IDatabaseStrategy
{
    private readonly IConfiguration _configuration;

    public PostgreSQLStrategy(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SaveResults(Summary summary)
    {
        using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            connection.Open();

            CreateTableIfNotExists(connection);

            foreach (var report in summary.Reports)
            {
                var benchmarkResult = CreateBenchmarkResult(report);
                if (benchmarkResult != null)
                {
                    InsertBenchmarkResult(connection, benchmarkResult);
                }
            }
        }
    }

    private void CreateTableIfNotExists(NpgsqlConnection connection)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "CREATE TABLE IF NOT EXISTS Benchmarks (Id SERIAL PRIMARY KEY, Benchmark TEXT, Mean DOUBLE PRECISION, Median DOUBLE PRECISION, StdDev DOUBLE PRECISION, Min DOUBLE PRECISION, Max DOUBLE PRECISION, OperationsPerSecond DOUBLE PRECISION, AllocatedMemory DOUBLE PRECISION, Timestamp TIMESTAMPTZ, AssemblyName TEXT, AssemblyVersion TEXT)";
            command.ExecuteNonQuery();
        }
    }

    private BenchmarkResult? CreateBenchmarkResult(BenchmarkReport report)
    {
        if (report == null || report.ResultStatistics == null)
        {
            return null;
        }
        var assembly = report.BenchmarkCase?.Descriptor?.WorkloadMethod?.DeclaringType?.Assembly;
        if(assembly == null) { return null; }

        var assemblyName = assembly?.GetName().Name;
        var assemblyVersion = assembly?.GetName()?.Version?.ToString();

        var operationsPerSecond = CalculateOperationsPerSecond(report.ResultStatistics.Mean);

        return new BenchmarkResult
        {
            Benchmark = report.BenchmarkCase.Descriptor.WorkloadMethod.Name,
            Mean = report.ResultStatistics.Mean,
            Median = report.ResultStatistics.Median,
            StdDev = report.ResultStatistics.StandardDeviation,
            Min = report.ResultStatistics.Min,
            Max = report.ResultStatistics.Max,
            OperationsPerSecond = report.Metrics?.ContainsKey("OperationsPerSecond") == true ? report.Metrics["OperationsPerSecond"].Value : operationsPerSecond,
            AllocatedMemory = report.Metrics?.ContainsKey("Allocated Memory") == true ? report.Metrics["Allocated Memory"].Value : 0,
            Timestamp = DateTime.UtcNow,
            AssemblyName = assemblyName,
            AssemblyVersion = assemblyVersion
        };
    }

    private double CalculateOperationsPerSecond(double meanTime)
    {
        return 1_000_000_000 / meanTime; // Conversion ns -> s
    }

    private void InsertBenchmarkResult(NpgsqlConnection connection, BenchmarkResult benchmarkResult)
    {
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO Benchmarks (Benchmark, Mean, Median, StdDev, Min, Max, OperationsPerSecond, AllocatedMemory, Timestamp, AssemblyName, AssemblyVersion) VALUES (@Benchmark, @Mean, @Median, @StdDev, @Min, @Max, @OperationsPerSecond, @AllocatedMemory, @Timestamp, @AssemblyName, @AssemblyVersion)";
            command.Parameters.AddWithValue("Benchmark", benchmarkResult.Benchmark);
            command.Parameters.AddWithValue("Mean", benchmarkResult.Mean);
            command.Parameters.AddWithValue("Median", benchmarkResult.Median);
            command.Parameters.AddWithValue("StdDev", benchmarkResult.StdDev);
            command.Parameters.AddWithValue("Min", benchmarkResult.Min);
            command.Parameters.AddWithValue("Max", benchmarkResult.Max);
            command.Parameters.AddWithValue("OperationsPerSecond", benchmarkResult.OperationsPerSecond);
            command.Parameters.AddWithValue("AllocatedMemory", benchmarkResult.AllocatedMemory);
            command.Parameters.AddWithValue("Timestamp", benchmarkResult.Timestamp);
            command.Parameters.AddWithValue("AssemblyName", benchmarkResult.AssemblyName);
            command.Parameters.AddWithValue("AssemblyVersion", benchmarkResult.AssemblyVersion);
            command.ExecuteNonQuery();
        }
    }
}
