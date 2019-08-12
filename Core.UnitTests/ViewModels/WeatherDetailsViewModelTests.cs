using AutoMapper;
using Core.Services;
using Core.Services.Interfaces;
using Core.UnitTests.TestData;
using Core.ViewModels;
using Moq;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using NUnit.Framework;
using OpenWeatherMap;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Core.Models;
using Core.Resources;

namespace Core.UnitTests.ViewModels
{
    [TestFixture]
    public class WeatherDetailsViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IWeatherService> weatherMock;
        private Mock<IMvxNavigationService> navigationMock;
        private Mock<IConnectivityService> connectivityMock;
        private Mock<IAlertService> alertMock;

        protected override void AdditionalSetup()
        {
            MvxSingletonCache.Instance
                .Settings
                .AlwaysRaiseInpcOnUserInterfaceThread = false;

            var helper = new MvxUnitTestCommandHelper();
            Ioc.RegisterSingleton<IMvxCommandHelper>(helper);

            MockWeather();

            MockNavigation();

            alertMock = new Mock<IAlertService>();
            Ioc.RegisterSingleton<IAlertService>(alertMock.Object);

            MockConnectivity();

            Ioc.RegisterSingleton<IMapper>(MapService.ConfigureMapper);
        }

        #region Tests

        [Test]
        public void ViewModel_Should_Show_WeatherDetails_Correctly()
        {
            //Arrange
            Setup();
            var vm = Ioc.IoCConstruct<WeatherDetailsViewModel>();

            //Act
            vm.Prepare(WeatherDetailsTestData.FakeWeatherDetails);

            //Assert
            vm.CityName.ShouldBe(WeatherDetailsTestData.FakeWeatherDetails.CityName);
            vm.CurrentTemperature.ShouldBe(WeatherDetailsTestData.FakeWeatherDetails.CurrentTemperature);
            vm.MaxTemperature.ShouldBe(WeatherDetailsTestData.FakeWeatherDetails.MaxTemperature);
            vm.MinTemperature.ShouldBe(WeatherDetailsTestData.FakeWeatherDetails.MinTemperature);
            vm.Description.ShouldBe(WeatherDetailsTestData.FakeWeatherDetails.Description);
        }

        [Test]
        public async Task RefreshWeatherCommand_Should_Call_Api_With_Same_CityName()
        {
            //Arrange
            Setup();
            var vm = Ioc.IoCConstruct<WeatherDetailsViewModel>();
            vm.Prepare(WeatherDetailsTestData.FakeWeatherDetails);

            //Act
            await vm.RefreshWeatherCommand.ExecuteAsync();

            //Assert
            alertMock.Verify(a => a.Show(It.IsAny<string>(), AlertType.Warning), Times.Never);
            weatherMock.Verify(a => a.GetWeatherAsync(vm.CityName), Times.Once);
            navigationMock.Verify(n => n.Navigate<SearchViewModel>(null, default(CancellationToken)),
                Times.Never);
        }

        [Test]
        public async Task RefreshWeatherCommand_Should_Navigate_To_SearchViewModel_If_Api_Throw_An_Error()
        {
            //Arrange
            Setup();
            var vm = Ioc.IoCConstruct<WeatherDetailsViewModel>();

            //Act
            await vm.RefreshWeatherCommand.ExecuteAsync();

            //Assert
            alertMock.Verify(a => a.Show(It.IsAny<string>(), AlertType.Warning), Times.Never);
            weatherMock.Verify(a => a.GetWeatherAsync(vm.CityName), Times.Once);
            navigationMock.Verify(n => n.Navigate<SearchViewModel>(null, default(CancellationToken)),
                Times.Once);
        }

        [Test]
        public async Task RefreshWeatherCommand_Should_Check_Internet_Connection()
        {
            //Arrange
            Setup();
            var vm = Ioc.IoCConstruct<WeatherDetailsViewModel>();

            //Act
            await vm.RefreshWeatherCommand.ExecuteAsync();

            //Assert
            connectivityMock.Verify(a => a.IsConnected, Times.Once);
        }

        [Test]
        public async Task RefreshWeatherCommand_Should_Navigate_To_SearchViewModel_If_No_Internet()
        {
            //Arrange
            Setup();
            connectivityMock.Setup(a => a.IsConnected)
                .Returns(false);
            var vm = Ioc.IoCConstruct<WeatherDetailsViewModel>();

            //Act
            await vm.RefreshWeatherCommand.ExecuteAsync();

            //Assert
            alertMock.Verify(a => a.Show(AppResources.CheckInternetConnection, AlertType.Warning), Times.Once);
            weatherMock.Verify(a => a.GetWeatherAsync(vm.CityName), Times.Never);
            navigationMock.Verify(n => n.Navigate<SearchViewModel>(null, default(CancellationToken)),
                Times.Once);
        }

        [Test]
        public async Task BackCommand_Should_Navigate_To_SearchViewModel()
        {
            //Arrange
            Setup();
            var vm = Ioc.IoCConstruct<WeatherDetailsViewModel>();

            //Act
            await vm.BackCommand.ExecuteAsync();

            //Assert
            alertMock.Verify(a => a.Show(It.IsAny<string>(), AlertType.Warning), Times.Never);
            weatherMock.Verify(a => a.GetWeatherAsync(vm.CityName), Times.Never);
            navigationMock.Verify(n => n.Navigate<SearchViewModel>(null, default(CancellationToken)),
                Times.Once);
        }

        #endregion

        #region Mocks

        private void MockConnectivity()
        {
            connectivityMock = new Mock<IConnectivityService>();
            connectivityMock.Setup(a => a.IsConnected)
                .Returns(true);
            Ioc.RegisterSingleton<IConnectivityService>(connectivityMock.Object);
        }

        private void MockNavigation()
        {
            navigationMock = new Mock<IMvxNavigationService>();
            navigationMock.Setup(n => n.Navigate<SearchViewModel>(null, default(CancellationToken)))
                .ReturnsAsync(true);
            Ioc.RegisterSingleton<IMvxNavigationService>(navigationMock.Object);
        }

        private void MockWeather()
        {
            weatherMock = new Mock<IWeatherService>();
            weatherMock.Setup(a => a.GetWeatherAsync(It.IsAny<string>()))
                .ReturnsAsync((CurrentWeatherResponse)null);
            weatherMock.Setup(a => a.GetWeatherAsync(CurrentWeatherTestData.FakeCurrentWeather.City.Name))
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather);
            Ioc.RegisterSingleton<IWeatherService>(weatherMock.Object);
        }

        #endregion
    }
}
