using BenchmarkDotNet.Reports;
using System.Data.Common;

public interface IDatabaseStrategy
{
    void SaveResults(Summary summary);
}