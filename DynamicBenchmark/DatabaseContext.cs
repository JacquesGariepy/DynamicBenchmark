using BenchmarkDotNet.Reports;

public class DatabaseContext
{
    private readonly IDatabaseStrategy _databaseStrategy;

    public DatabaseContext(IDatabaseStrategy databaseStrategy)
    {
        _databaseStrategy = databaseStrategy;
    }

    public void SaveResults(Summary summary)
    {
        _databaseStrategy.SaveResults(summary);
    }
}