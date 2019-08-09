using Core.Services;
using Core.Services.Interfaces;
using Core.UnitTests.TestData;
using Moq;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Tests;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Core.Resources;
using Xamarin.Essentials;

namespace Core.UnitTests.Services
{
    [TestFixture]
    public class LocationServiceTests : MvxIoCSupportingTest
    {
        private Mock<IGeolocationService> geolocationMock;
        private Mock<IGeocodingService> geocodingMock;

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
        }

        [Test]
        public async Task GetLocationCityNameAsync_WithLocationAndPlacemark_CityNameReturned()
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
            geolocationMock.Verify(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()), Times.Once);
            geocodingMock.Verify(g => g.GetPlacemarksAsync(1, 1), Times.Once);
        }

        [Test]
        public async Task GetLocationCityNameAsync_WithNullLocation_LocationExceptionThrown()
        {
            //Arrange
            base.Setup();
            geolocationMock
                .Setup(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()))
                .ReturnsAsync((Location)null);
            var locationService = Ioc.IoCConstruct<LocationService>();

            //Act
            Func<Task> action = () => locationService.GetLocationCityNameAsync();

            //Assert
            (await action.ShouldThrowAsync<LocationException>()).Message.ShouldBe(AppResources.CanNotGetCityName);
            geolocationMock.Verify(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()),
                Times.Once);
            geocodingMock.Verify(g => g.GetPlacemarksAsync(It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
        }

        [Test]
        public async Task GetLocationCityNameAsync_WithExceptionInGeolocation_LocationExceptionThrown()
        {
            //Arrange
            base.Setup();
            geolocationMock
                .Setup(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()))
                .Throws<Exception>();
            var locationService = Ioc.IoCConstruct<LocationService>();

            //Act
            Func<Task> action = () => locationService.GetLocationCityNameAsync();

            //Assert
            (await action.ShouldThrowAsync<LocationException>()).Message.ShouldBe(AppResources.CanNotGetCityName);
            geolocationMock.Verify(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()),
                Times.Once);
            geocodingMock.Verify(g => g.GetPlacemarksAsync(It.IsAny<int>(), It.IsAny<int>()),
                Times.Never);
        }

        [Test]
        public async Task GetLocationCityNameAsync_WithNullPlacemark_LocationExceptionThrown()
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
            Func<Task> action = () => locationService.GetLocationCityNameAsync();

            //Assert
            (await action.ShouldThrowAsync<LocationException>()).Message.ShouldBe(AppResources.CanNotGetCityName);
            geolocationMock.Verify(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()),
                Times.Once);
            geocodingMock.Verify(g => g.GetPlacemarksAsync(1, 1),
                Times.Once);
        }

        [Test]
        public async Task GetLocationCityNameAsync_WithExceptionInGeocoding_LocationExceptionThrown()
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
            Func<Task> action = () => locationService.GetLocationCityNameAsync();

            //Assert
            (await action.ShouldThrowAsync<LocationException>()).Message.ShouldBe(AppResources.CanNotGetCityName);
            geolocationMock.Verify(g => g.GetLocationAsync(It.IsAny<GeolocationRequest>()),
                Times.Once);
            geocodingMock.Verify(g => g.GetPlacemarksAsync(1, 1),
                Times.Once);
        }
    }
}
