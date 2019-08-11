using Core.Converters;
using MvvmCross.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UnitTests.Converters
{
    public class TemperatureToColorConverterTests : MvxIoCSupportingTest
    {
        public TemperatureToColorConverterTests()
        {
            base.Setup();
        }

        [Test]
        public void RegisterServices_ServicesRegisteredAsInterfaces_IoCCanResolveEachServiceInterface()
        {
            //Arrange
            var iocRegistrar = new TemperatureToColorConverter();

            //Act
            iocRegistrar.GetColor();

            //Assert
            Mvx.IoCProvider.CanResolve<IAlertService>().ShouldBeTrue();
            Mvx.IoCProvider.CanResolve<IApiClient>().ShouldBeTrue();
            Mvx.IoCProvider.CanResolve<IWeatherService>().ShouldBeTrue();
            Mvx.IoCProvider.CanResolve<ILocationService>().ShouldBeTrue();
            Mvx.IoCProvider.CanResolve<IGeolocationService>().ShouldBeTrue();
            Mvx.IoCProvider.CanResolve<IGeocodingService>().ShouldBeTrue();
        }
    }
}
