using AutoMapper;
using Core.Models;
using Core.Services.Interfaces;
using Core.UnitTests.TestData;
using Core.ViewModels;
using Moq;
using MvvmCross.Base;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using NUnit.Framework;
using OpenWeatherMap;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Core.Resources;
using Plugin.Connectivity.Abstractions;
using IMvxCommandHelper = MvvmCross.Commands.IMvxCommandHelper;

namespace Core.UnitTests.ViewModels
{
    [TestFixture]
    public class SearchViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<IWeatherService> weatherMock;
        private Mock<IMvxNavigationService> navigationMock;
        private Mock<ILocationService> locationMock;
        private Mock<IAlertService> alertMock;
        private Mock<IConnectivity> connectivityMock;

        protected override void AdditionalSetup()
        {
            MvxSingletonCache.Instance
                .Settings
                .AlwaysRaiseInpcOnUserInterfaceThread = false;

            var helper = new MvxUnitTestCommandHelper();
            Ioc.RegisterSingleton<IMvxCommandHelper>(helper);

            mapperMock = new Mock<IMapper>();
            Ioc.RegisterSingleton<IMapper>(mapperMock.Object);

            navigationMock = new Mock<IMvxNavigationService>();
            navigationMock.Setup(n => n.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                        It.IsAny<WeatherDetails>(), null, default(CancellationToken)))
                .ReturnsAsync(true);
            Ioc.RegisterSingleton<IMvxNavigationService>(navigationMock.Object);

            weatherMock = new Mock<IWeatherService>();
            weatherMock.Setup(a => a.GetWeatherAsync(It.IsAny<string>()))
                .ReturnsAsync((CurrentWeatherResponse)null);
            weatherMock.Setup(a => a.GetWeatherAsync(CurrentWeatherTestData.FakeCurrentWeather.City.Name))
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather);
            Ioc.RegisterSingleton<IWeatherService>(weatherMock.Object);

            alertMock = new Mock<IAlertService>();
            Ioc.RegisterSingleton<IAlertService>(alertMock.Object);

            connectivityMock = new Mock<IConnectivity>();
            connectivityMock.Setup(a => a.IsConnected)
                .Returns(true);
            Ioc.RegisterSingleton<IConnectivity>(connectivityMock.Object);

            locationMock = new Mock<ILocationService>();
            locationMock.Setup(l => l.GetLocationCityNameAsync())
                .ReturnsAsync(WeatherDetailsTestData.FakeWeatherDetails.CityName);
            Ioc.RegisterSingleton<ILocationService>(locationMock.Object);
        }

        [Test]
        public void CheckWeatherCommand_Should_RaisedCanExecuteChanged_When_CityName_Changed()
        {
            //Arrange
            base.Setup();
            var vm = Ioc.IoCConstruct<SearchViewModel>();
            vm.CheckWeatherCommand.ListenForRaiseCanExecuteChanged();

            //Act
            vm.CityName = "Some City Name";

            //Assert
            vm.CheckWeatherCommand.RaisedCanExecuteChanged().ShouldBeTrue();
        }

        [Test]
        public async Task CheckWeatherCommand_Should_Call_Api_And_Navigate_To_WeatherDetailsViewModel()
        {
            //Arrange
            base.Setup();
            var vm = Ioc.IoCConstruct<SearchViewModel>();

            //Act
            vm.CityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;
            await vm.CheckWeatherCommand.ExecuteAsync();

            //Assert
            alertMock.Verify(a => a.Show(It.IsAny<string>(), AlertType.Warning), Times.Never);
            weatherMock.Verify(a => a.GetWeatherAsync(vm.CityName), Times.Once);
            navigationMock.Verify(n => n.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                It.IsAny<WeatherDetails>(), null, default(CancellationToken)),
                Times.Once);
        }


        [Test]
        public async Task CheckWeatherCommand_Should_Check_Internet_Connection()
        {
            //Arrange
            base.Setup();
            var vm = Ioc.IoCConstruct<SearchViewModel>();

            //Act
            vm.CityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;
            await vm.CheckWeatherCommand.ExecuteAsync();

            //Assert
            connectivityMock.Verify(a => a.IsConnected, Times.Once);
        }

        [TestCase("London2")]
        [TestCase("asdasd")]
        public async Task CheckWeatherCommand_Should_Call_Api_And_Show_Error_Alert(string cityName)
        {
            //Arrange
            base.Setup();
            var vm = Ioc.IoCConstruct<SearchViewModel>();

            //Act
            vm.CityName = cityName;
            await vm.CheckWeatherCommand.ExecuteAsync();

            //Assert
            alertMock.Verify(a => a.Show(It.IsAny<string>(), AlertType.Warning), Times.Never);
            weatherMock.Verify(a => a.GetWeatherAsync(vm.CityName), Times.Once);
            navigationMock.Verify(n => n.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                    It.IsAny<WeatherDetails>(), null, default(CancellationToken)),
                    Times.Never);
        }

        [Test]
        public async Task CheckWeatherCommand_Should_Call_Api_And_If_No_Internet_Show_Warning_Alert()
        {
            //Arrange
            base.Setup();
            connectivityMock.Setup(a => a.IsConnected)
                .Returns(false);
            var vm = Ioc.IoCConstruct<SearchViewModel>();

            //Act
            vm.CityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;
            await vm.CheckWeatherCommand.ExecuteAsync();

            //Assert
            alertMock.Verify(a => a.Show(AppResources.CheckInternetConnection, AlertType.Warning), Times.Once);
            weatherMock.Verify(a => a.GetWeatherAsync(vm.CityName), Times.Never);
            navigationMock.Verify(n => n.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                    It.IsAny<WeatherDetails>(), null, default(CancellationToken)), Times.Never);
        }

        [Test]
        public async Task GetLocationCityNameCommand_Should_Call_Api_And_Update_City_Name()
        {
            //Arrange
            base.Setup();
            var vm = Ioc.IoCConstruct<SearchViewModel>();

            //Act
            vm.CityName = string.Empty;
            await vm.GetLocationCityNameCommand.ExecuteAsync();

            //Assert
            vm.CityName.ShouldBe(WeatherDetailsTestData.FakeWeatherDetails.CityName);
            locationMock.Verify(l => l.GetLocationCityNameAsync(),
                Times.Once);
        }
    }
}
