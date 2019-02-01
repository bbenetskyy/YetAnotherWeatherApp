using API;
using AutoMapper;
using Core.Services;
using Core.UnitTests.TestData;
using Core.ViewModels;
using InteractiveAlert;
using Moq;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.UnitTests.ViewModels
{
    [TestFixture]
    public class WeatherDetailsViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IApiClient> apiMock;
        private readonly Mock<IMapper> mapperMock;
        private Mock<IMvxNavigationService> navigationMock;
        private Mock<IInteractiveAlerts> interactiveMock;

        protected override void AdditionalSetup()
        {
            MvxSingletonCache.Instance
                .Settings
                .AlwaysRaiseInpcOnUserInterfaceThread = false;

            var helper = new MvxUnitTestCommandHelper();
            Ioc.RegisterSingleton<IMvxCommandHelper>(helper);

            apiMock = new Mock<IApiClient>();
            apiMock.Setup(a => a.GetWeatherByCityNameAsync(It.IsAny<string>()))
                .Throws<AggregateException>();
            apiMock.Setup(a => a.GetWeatherByCityNameAsync(null))
                .Throws<ArgumentException>();
            apiMock.Setup(a => a.GetWeatherByCityNameAsync(CurrentWeatherTestData.FakeCurrentWeather.City.Name))
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather);
            Ioc.RegisterSingleton<IApiClient>(apiMock.Object);

            Ioc.RegisterSingleton<IMapper>(MapService.ConfigureMapper);

            navigationMock = new Mock<IMvxNavigationService>();
            navigationMock.Setup(n => n.Navigate<SearchViewModel>(null, default(CancellationToken)))
                .ReturnsAsync(true);
            Ioc.RegisterSingleton<IMvxNavigationService>(navigationMock.Object);

            interactiveMock = new Mock<IInteractiveAlerts>();
            Ioc.RegisterSingleton<IInteractiveAlerts>(interactiveMock.Object);
        }

        [Test]
        public void ViewModel_Should_Show_WeatherDetails_Correctly()
        {
            //Arrange
            base.Setup();
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
            base.Setup();
            var vm = Ioc.IoCConstruct<WeatherDetailsViewModel>();
            vm.Prepare(WeatherDetailsTestData.FakeWeatherDetails);

            //Act
            await vm.RefreshWeatherCommand.ExecuteAsync();

            //Assert
            apiMock.Verify(a => a.GetWeatherByCityNameAsync(vm.CityName), Times.Once);
            navigationMock.Verify(n => n.Navigate<SearchViewModel>(null, default(CancellationToken)),
                Times.Never);
            interactiveMock.Verify(i => i.ShowAlert(It.IsAny<InteractiveAlertConfig>()), Times.Never);
        }

        [Test]
        public async Task RefreshWeatherCommand_Should_Navigate_To_SearchViewModel_If_Api_Throw_An_Error()
        {
            //Arrange
            base.Setup();
            var vm = Ioc.IoCConstruct<WeatherDetailsViewModel>();

            //Act
            await vm.RefreshWeatherCommand.ExecuteAsync();

            //Assert
            apiMock.Verify(a => a.GetWeatherByCityNameAsync(vm.CityName), Times.Once);
            navigationMock.Verify(n => n.Navigate<SearchViewModel>(null, default(CancellationToken)),
                Times.Once);
            interactiveMock.Verify(i => i.ShowAlert(It.IsAny<InteractiveAlertConfig>()), Times.Once);
        }

        [Test]
        public async Task BackCommand_Should_Navigate_To_SearchViewModel()
        {
            //Arrange
            base.Setup();
            var vm = Ioc.IoCConstruct<WeatherDetailsViewModel>();

            //Act
            await vm.BackCommand.ExecuteAsync();

            //Assert
            apiMock.Verify(a => a.GetWeatherByCityNameAsync(vm.CityName), Times.Never);
            navigationMock.Verify(n => n.Navigate<SearchViewModel>(null, default(CancellationToken)),
                Times.Once);
            interactiveMock.Verify(i => i.ShowAlert(It.IsAny<InteractiveAlertConfig>()), Times.Never);
        }

    }
}
