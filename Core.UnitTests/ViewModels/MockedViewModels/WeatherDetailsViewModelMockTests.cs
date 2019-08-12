using AutoMapper;
using Core.Services.Interfaces;
using Core.UnitTests.TestData;
using Core.ViewModels;
using Moq;
using Moq.Protected;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using NUnit.Framework;
using OpenWeatherMap;
using Shouldly;
using System.Reflection;
using System.Threading.Tasks;
using Plugin.Connectivity.Abstractions;

namespace Core.UnitTests.ViewModels.MockedViewModels
{
    [TestFixture]
    public class WeatherDetailsViewModelMockTests : MvxIoCSupportingTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<IWeatherService> weatherServiceMock;
        private Mock<IMvxNavigationService> navigationMock;
        private Mock<WeatherDetailsViewModel> weatherMock;
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
            Ioc.RegisterSingleton<IMvxNavigationService>(navigationMock.Object);

            weatherServiceMock = new Mock<IWeatherService>();
            Ioc.RegisterSingleton<IWeatherService>(weatherServiceMock.Object);

            alertMock = new Mock<IAlertService>();
            Ioc.RegisterSingleton<IAlertService>(alertMock.Object);

            MockConnectivity();

            MockWeather();
        }

        #region Tests

        [Test]
        public void RefreshWeather_Should_Call_GetWeather_And_MapWeatherToProperties_And_ActivityIndicators()
        {
            //Arrange
            base.Setup();
            var vm = weatherMock.Object;
            var refreshWeather = vm.GetType().GetMethod("RefreshWeather",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            vm.Prepare(WeatherDetailsTestData.FakeWeatherDetails);
            refreshWeather.Invoke(vm, null);

            //Assert
            weatherMock.Protected().Verify("GetWeather",
                Times.Once());
            weatherMock.Protected().Verify("MapWeatherToProperties",
                Times.Once(), ItExpr.IsAny<CurrentWeatherResponse>());
            weatherMock.Protected().Verify("NavigateToSearch",
                Times.Never());
        }

        [Test]
        public void RefreshWeather_Should_Call_GetWeather_And_NavigateToSearch_And_ActivityIndicators()
        {
            //Arrange
            base.Setup();
            weatherMock.Protected()
                .Setup<Task<CurrentWeatherResponse>>("GetWeather")
                .ReturnsAsync((CurrentWeatherResponse)null)
                .Verifiable();
            var vm = weatherMock.Object;
            var refreshWeather = vm.GetType().GetMethod("RefreshWeather",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            refreshWeather.Invoke(vm, null);

            //Assert
            weatherMock.Protected().Verify("GetWeather",
                Times.Once());
            weatherMock.Protected().Verify("MapWeatherToProperties",
                Times.Never(), ItExpr.IsAny<CurrentWeatherResponse>());
            weatherMock.Protected().Verify("NavigateToSearch",
                Times.Once());
        }

        [Test]
        public async Task BackCommand_Should_Call_NavigateToSearch()
        {
            //Arrange
            base.Setup();
            var vm = weatherMock.Object;

            //Act
            await vm.BackCommand.ExecuteAsync();

            //Assert
            weatherMock.Protected().Verify("GetWeather",
                Times.Never());
            weatherMock.Protected().Verify("MapWeatherToProperties",
                Times.Never(), ItExpr.IsAny<CurrentWeatherResponse>());
            weatherMock.Protected().Verify("NavigateToSearch",
                Times.Once());
            weatherMock.Protected().Verify("ShowActivityIndicator",
                Times.Never());
            weatherMock.Protected().Verify("HideActivityIndicator",
                Times.Never());
        }

        [Test]
        public void ShowActivityIndicator_Should_Set_IsLoading_To_True()
        {
            //Arrange
            base.Setup();
            weatherMock.Protected()
                .Setup("ShowActivityIndicator")
                .CallBase();
            var vm = weatherMock.Object;
            var showActivityIndicator = vm.GetType().GetMethod("ShowActivityIndicator",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            vm.IsLoading = false;
            showActivityIndicator.Invoke(vm, null);

            //Assert
            vm.IsLoading.ShouldBeTrue();
            weatherMock.Protected().Verify("GetWeather",
                Times.Never());
            weatherMock.Protected().Verify("MapWeatherToProperties",
                Times.Never(), ItExpr.IsAny<CurrentWeatherResponse>());
            weatherMock.Protected().Verify("NavigateToSearch",
                Times.Never());
            weatherMock.Protected().Verify("HideActivityIndicator",
                Times.Never());
        }

        [Test]
        public void HideActivityIndicator_Should_Set_IsLoading_To_False()
        {
            //Arrange
            base.Setup();
            weatherMock.Protected()
                .Setup("HideActivityIndicator")
                .CallBase();
            var vm = weatherMock.Object;
            var hideActivityIndicator = vm.GetType().GetMethod("HideActivityIndicator",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            vm.IsLoading = true;
            hideActivityIndicator.Invoke(vm, null);

            //Assert
            vm.IsLoading.ShouldBeFalse();
            weatherMock.Protected().Verify("GetWeather",
                Times.Never());
            weatherMock.Protected().Verify("MapWeatherToProperties",
                Times.Never(), ItExpr.IsAny<CurrentWeatherResponse>());
            weatherMock.Protected().Verify("NavigateToSearch",
                Times.Never());
            weatherMock.Protected().Verify("ShowActivityIndicator",
                Times.Never());
        }

        [Test]
        public async Task RefreshWeatherCommand_Should_Call_RefreshWeather()
        {
            //Arrange
            base.Setup();
            weatherMock.Protected()
                .Setup<Task>("RefreshWeather")
                .Returns(Task.FromResult("Some Result"))
                .Verifiable();
            var vm = weatherMock.Object;

            //Act
            await vm.RefreshWeatherCommand.ExecuteAsync();

            //Assert
            weatherMock.Protected().Verify("RefreshWeather",
                Times.Once());
            weatherMock.Protected().Verify("NavigateToSearch",
                Times.Never());
        }

        #endregion

        #region Mocks

        private void MockWeather()
        {
            weatherMock = new Mock<WeatherDetailsViewModel>(MockBehavior.Loose,
                mapperMock.Object, navigationMock.Object, weatherServiceMock.Object,
                connectivityMock.Object, alertMock.Object)
            {
                CallBase = true
            };
            weatherMock.Protected()
                .Setup<Task>("NavigateToSearch")
                .Returns(Task.FromResult("Some Result"))
                .Verifiable();
            weatherMock.Protected()
                .Setup<Task>("MapWeatherToProperties", ItExpr.IsAny<CurrentWeatherResponse>())
                .Returns(Task.FromResult("Some Result"))
                .Verifiable();
            weatherMock.Protected()
                .Setup<Task<CurrentWeatherResponse>>("GetWeather")
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather)
                .Verifiable();
            weatherMock.Protected()
                .Setup("ShowActivityIndicator")
                .Verifiable();
            weatherMock.Protected()
                .Setup("HideActivityIndicator")
                .Verifiable();
        }

        private void MockConnectivity()
        {
            connectivityMock = new Mock<IConnectivity>();
            connectivityMock.Setup(a => a.IsConnected)
                .Returns(true);
            Ioc.RegisterSingleton<IConnectivity>(connectivityMock.Object);
        }

        #endregion
    }
}
