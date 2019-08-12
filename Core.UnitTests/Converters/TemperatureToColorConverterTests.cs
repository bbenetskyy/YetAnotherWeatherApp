using MvvmCross.Tests;
using NUnit.Framework;
using Core.Constants;
using Core.UnitTests.Stubs;
using Core.UnitTests.TestData;
using MvvmCross.UI;
using Shouldly;

namespace Core.UnitTests.Converters
{
    public class TemperatureToColorConverterTests : MvxIoCSupportingTest
    {
        public TemperatureToColorConverterTests()
        {
            base.Setup();
        }

        [Theory]
        [TestCaseSource(typeof(TemperatureConverterTestData), nameof(TemperatureConverterTestData.CorrectTemperatureToColorData))]
        public void GetColor_WithCorrectTemperatureString_ExpectedColorReturned(string temperature, MvxColor expectedColor)
        {
            //Arrange
            var iocRegistrar = new TemperatureToColorConverterStub();

            //Act
            var color = iocRegistrar.GetColor(temperature);

            //Assert
            color.ShouldBe(expectedColor);
        }

        [Theory]
        [TestCaseSource(typeof(TemperatureConverterTestData), nameof(TemperatureConverterTestData.IncorrectTemperatureToColorData))]
        public void GetColor_WithInCorrectTemperatureString_DefaultColorReturned(string temperature)
        {
            //Arrange
            var iocRegistrar = new TemperatureToColorConverterStub();

            //Act
            var color = iocRegistrar.GetColor(temperature);

            //Assert
            color.ShouldBe(Colors.Default);
        }
    }
}
