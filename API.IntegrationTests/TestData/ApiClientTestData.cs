using System.Collections.Generic;

namespace API.IntegrationTests.TestData
{
    public static class ApiClientTestData
    {
        public static IEnumerable<object[]> ValidCityNames => new[]
        {
            new[] {"London"},
            new[] {"Rzeszow"},
            new[] {"Kalush"}
        };

        public static IEnumerable<object[]> InValidCityNames => new[]
        {
            new[] {"London2"},
            new[] {"asdasdasd"},
            new[] {""}
        };
    }
}
