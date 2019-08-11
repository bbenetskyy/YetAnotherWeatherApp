using System;
using System.Collections.Generic;
using System.Text;
using Core.Constants;

namespace Core.UnitTests.TestData
{
    public static class TemperatureConverterTestData
    {
        public static IEnumerable<object[]> CorrectTemperatureToColorData = new List<object[]>
        {
            new object[] {"-2  °C", Colors.Cold},
            new object[] {"-1  °C", Colors.Cold},
            new object[] {"0  °C", Colors.Cold},
            new object[] {"1  °C", Colors.Chilly},
            new object[] {"2  °C", Colors.Chilly},
            new object[] {"8  °C", Colors.Chilly},
            new object[] {"9  °C", Colors.Chilly},
            new object[] {"10  °C", Colors.Chilly},
            new object[] {"11  °C", Colors.Warm},
            new object[] {"12  °C", Colors.Warm},
            new object[] {"18  °C", Colors.Warm},
            new object[] {"19  °C", Colors.Warm},
            new object[] {"20  °C", Colors.Warm},
            new object[] {"21  °C", Colors.Hotly},
            new object[] {"22  °C", Colors.Hotly},
        };
        public static IEnumerable<object[]> IncorrectTemperatureToColorData = new List<object[]>
        {
            new object[] {"-2°C"},
            new object[] {"a-1  °C"},
            new object[] {"0d  °C"},
            new object[] {"°C 1"},
            new object[] {string.Empty},
            new object[] {null} ,
        };
    }
}
