using AutoMapper;
using Core.Services;
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
using System.Threading.Tasks;

namespace Core.UnitTests.ViewModels.MockedViewModels
{
    [TestFixture]
    public class WeatherDetailsViewModelMockTests : MvxIoCSupportingTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<IAlertService> alertMock;
        private Mock<IMvxNavigationService> navigationMock;
        private Mock<WeatherDetailsViewModel> weatherMock;

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

            alertMock = new Mock<IAlertService>();
            Ioc.RegisterSingleton<IAlertService>(alertMock.Object);

            weatherMock = new Mock<WeatherDetailsViewModel>(MockBehavior.Loose,
                mapperMock.Object, navigationMock.Object, alertMock.Object)
            {
                CallBase = true
            };
            weatherMock.Protected()
                .Setup("NavigateToSearch")
                .Verifiable();
            weatherMock.Protected()
                .Setup("MapWeatherToProperties", ItExpr.IsAny<CurrentWeatherResponse>())
                .Verifiable();
            weatherMock.Protected()
                .Setup<Task<CurrentWeatherResponse>>("GetWeather")
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather)
                .Verifiable();
        }

        [Test]
        public async Task RefreshWeatherCommand_Should_Call_GetWeather_And_MapWeatherToProperties()
        {
            //Arrange
            base.Setup();
            var vm = weatherMock.Object;
            vm.Prepare(WeatherDetailsTestData.FakeWeatherDetails);

            //Act
            await vm.RefreshWeatherCommand.ExecuteAsync();

            //Assert
            weatherMock.Protected().Verify("GetWeather",
                Times.Once());
            weatherMock.Protected().Verify("MapWeatherToProperties",
                Times.Once(), ItExpr.IsAny<CurrentWeatherResponse>());
            weatherMock.Protected().Verify("NavigateToSearch",
                Times.Never());
        }

        [Test]
        public async Task RefreshWeatherCommand_Should_Call_GetWeather_And_NavigateToSearch()
        {
            //Arrange
            base.Setup();
            weatherMock.Protected()
                .Setup<Task<CurrentWeatherResponse>>("GetWeather")
                .ReturnsAsync((CurrentWeatherResponse)null)
                .Verifiable();
            var vm = weatherMock.Object;

            //Act
            await vm.RefreshWeatherCommand.ExecuteAsync();

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
        }
    }
}
