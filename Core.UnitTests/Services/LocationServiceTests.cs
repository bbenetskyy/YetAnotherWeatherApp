using Core.Services;
using Core.Services.Interfaces;
using Core.UnitTests.TestData;
using InteractiveAlert;
using Moq;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Tests;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Core.UnitTests.Services
{
    [TestFixture]
    public class LocationServiceTests : MvxIoCSupportingTest
    {
        private Mock<IGeolocationService> geolocationMock;
        private Mock<IGeocodingService> geocodingMock;
        private Mock<IInteractiveAlerts> interactiveMock;

        protected override void AdditionalSetup()
        {
            MvxSingletonCache.Instance
                .Settings
                .AlwaysRaiseInpcOnUserInterfaceThread = false;

            var helper = new MvxUnitTestCommandHelper();
            Ioc.RegisterSingleton<IMvxCommandHelper>(helper);

            geolocationMock = new Mock<IGeolocationService>();
            Ioc.RegisterSingleton<IGeolocationService>(geolocationMock.Object);

            geocodingMock = new Mock<IGeocodingService>();
            Ioc.RegisterSingleton<IGeocodingService>(geocodingMock.Object);

            interactiveMock = new Mock<IInteractiveAlerts>();
            Ioc.RegisterSingleton<IInteractiveAlerts>(interactiveMock.Object);
        }

        [Test]
        public async Task GetLocationCityNameAsync_Should_Return_City_Name()
        {
            //Arrange
            base.Setup();
            geolocationMock
                .Setup(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()))
                .ReturnsAsync(new Location(1, 1));
            geocodingMock
                .Setup(g => g.GetPlacemarksAsync(1, 1))
                .ReturnsAsync(new List<Placemark>
                {
                    new Placemark
                    {
                        Locality = WeatherDetailsTestData.FakeWeatherDetails.CityName
                    }
                });
            var locationService = Ioc.IoCConstruct<LocationService>();

            //Act
            var cityName = await locationService.GetLocationCityNameAsync();

            //Assert
            cityName.ShouldNotBeNull();
            cityName.ShouldBe(WeatherDetailsTestData.FakeWeatherDetails.CityName);
            geolocationMock.Verify(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()),
                Times.Once);
            geocodingMock.Verify(g => g.GetPlacemarksAsync(1, 1),
                Times.Once);
            interactiveMock.Verify(i => i.ShowAlert(It.IsAny<InteractiveAlertConfig>()),
                Times.Never);
        }

        [Test]
        public async Task GetLocationCityNameAsync_Should_Return_Null_And_Allert_If_Geolocation_Return_Null()
        {
            //Arrange
            base.Setup();
            geolocationMock
                .Setup(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()))
                .ReturnsAsync((Location)null);
            var locationService = Ioc.IoCConstruct<LocationService>();

            //Act
            var cityName = await locationService.GetLocationCityNameAsync();

            //Assert
            cityName.ShouldBeNull();
            geolocationMock.Verify(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()),
                Times.Once);
            geocodingMock.Verify(g => g.GetPlacemarksAsync(It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
            interactiveMock.Verify(i => i.ShowAlert(It.IsAny<InteractiveAlertConfig>()),
                Times.Once);
        }

        [Test]
        public async Task GetLocationCityNameAsync_Should_Return_Null_And_Allert_If_Geolocation_Throw_Exception()
        {
            //Arrange
            base.Setup();
            geolocationMock
                .Setup(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()))
                .Throws<Exception>();
            var locationService = Ioc.IoCConstruct<LocationService>();

            //Act
            var cityName = await locationService.GetLocationCityNameAsync();

            //Assert
            cityName.ShouldBeNull();
            geolocationMock.Verify(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()),
                Times.Once);
            geocodingMock.Verify(g => g.GetPlacemarksAsync(It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
            interactiveMock.Verify(i => i.ShowAlert(It.IsAny<InteractiveAlertConfig>()),
                Times.Once);
        }

        [Test]
        public async Task GetLocationCityNameAsync_Should_Return_Null_And_Allert_If_Geocoding_Return_Null()
        {
            //Arrange
            base.Setup();
            geolocationMock
                .Setup(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()))
                .ReturnsAsync(new Location(1, 1));
            geocodingMock
                .Setup(g => g.GetPlacemarksAsync(1, 1))
                .ReturnsAsync((IEnumerable<Placemark>)null);
            var locationService = Ioc.IoCConstruct<LocationService>();

            //Act
            var cityName = await locationService.GetLocationCityNameAsync();

            //Assert
            cityName.ShouldBeNull();
            geolocationMock.Verify(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()),
                Times.Once);
            geocodingMock.Verify(g => g.GetPlacemarksAsync(1, 1),
                Times.Once);
            interactiveMock.Verify(i => i.ShowAlert(It.IsAny<InteractiveAlertConfig>()),
                Times.Once);
        }

        [Test]
        public async Task GetLocationCityNameAsync_Should_Return_Null_And_Allert_If_Geocoding_Throw_Exception()
        {
            //Arrange
            base.Setup();
            geolocationMock
                .Setup(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()))
                .ReturnsAsync(new Location(1, 1));
            geocodingMock
                .Setup(g => g.GetPlacemarksAsync(1, 1))
                .Throws<Exception>();
            var locationService = Ioc.IoCConstruct<LocationService>();

            //Act
            var cityName = await locationService.GetLocationCityNameAsync();

            //Assert
            cityName.ShouldBeNull();
            geolocationMock.Verify(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()),
                Times.Once);
            geocodingMock.Verify(g => g.GetPlacemarksAsync(1, 1),
                Times.Once);
            interactiveMock.Verify(i => i.ShowAlert(It.IsAny<InteractiveAlertConfig>()),
                Times.Once);
        }
    }
}
