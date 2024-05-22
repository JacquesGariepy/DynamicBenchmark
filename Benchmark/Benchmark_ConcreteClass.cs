using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Benchmark
{
    [SimpleJob(RunStrategy.ColdStart, iterationCount: 15)]
    [AllStatisticsColumn]
    public class Benchmark_ConcreteClass
    {
        private Concrete.ConcreteClass concreteClass = new Concrete.ConcreteClass();
        [Benchmark]
        public void TestDto()
        {
            concreteClass.TestDto();
        }

        [Benchmark]
        public void TestDto2()
        {
            concreteClass.TestDto2();
        }

        [Benchmark]
        public void TestSortedSetWithUserComparer()
        {
            concreteClass.TestSortedSetWithUserComparer();
        }

        [Benchmark]
        public void TestListWithSort()
        {
            concreteClass.TestListWithSort();
        }

        [Benchmark]
        public void TestListWithLinqSort()
        {
            concreteClass.TestListWithLinqSort();
        }

        [Benchmark]
        public void TestToString()
        {
            concreteClass.TestToString();
        }

        [Benchmark]
        public void TestNameOf()
        {
            concreteClass.TestNameOf();
        }
    }
}
