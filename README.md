# DynamicBenchmark

This project is a proof of concept (POC) designed to perform benchmarks on different methods of a concrete class using the BenchmarkDotNet library. It uses reflection to automatically discover the methods to be tested, which avoids the need to decorate each method with benchmark attributes. This makes the code cleaner and easier to maintain.

## Project Structure

The project contains several key classes:

- `AutoBenchmark`: This class is the entry point for executing the benchmarks. It uses reflection to discover benchmarks in the specified assemblies, executes them, and saves the results.

- `BenchmarkDiscovery`: This class is responsible for discovering the methods to be tested using reflection.

- `DatabaseContext`: This class is used to save the benchmark results in a database.

- `PostgreSQLStrategy`: This class implements the `IDatabaseStrategy` interface to save the benchmark results in a PostgreSQL database. It is easy to include a strategy for another type of database by implementing the same interface.

## Running the Benchmarks

To run the benchmarks, you can execute the project from the command line with the command `dotnet run -c Release`.

## Benchmark Results

The benchmark results will be displayed in the console. They are also saved in a PostgreSQL database for further analysis.

## Dependencies

This project uses the BenchmarkDotNet library to perform the benchmarks. You can install this library with the command `dotnet add package BenchmarkDotNet`.

## Contribution

Contributions to this project are welcome. If you wish to contribute, please create a pull request with your modifications.
