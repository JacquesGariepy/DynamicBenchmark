public class BenchmarkResult
{
    public string Benchmark { get; set; }
    public double Mean { get; set; }
    public double Median { get; set; }
    public double StdDev { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public double OperationsPerSecond { get; set; }
    public double AllocatedMemory { get; set; }
    public DateTime Timestamp { get; set; }
    public string AssemblyName { get; set; }
    public string AssemblyVersion { get; set; }
}