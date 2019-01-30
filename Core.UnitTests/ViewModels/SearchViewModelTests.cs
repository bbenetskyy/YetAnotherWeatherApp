using API;
using AutoMapper;
using Core.Models;
using Core.UnitTests.TestData;
using Core.ViewModels;
using Moq;
using MvvmCross.Base;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using NUnit.Framework;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using IMvxCommandHelper = MvvmCross.Commands.IMvxCommandHelper;

namespace Core.UnitTests.ViewModels
{
    [TestFixture]
    public class SearchViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IApiClient> apiMock;
        private Mock<IMapper> mapperMock;
        private Mock<IMvxNavigationService> navigationMock;

        protected override void AdditionalSetup()
        {
            MvxSingletonCache.Instance
                .Settings
                .AlwaysRaiseInpcOnUserInterfaceThread = false;

            var helper = new MvxUnitTestCommandHelper();
            Ioc.RegisterSingleton<IMvxCommandHelper>(helper);

            apiMock = new Mock<IApiClient>();
            apiMock.Setup(a => a.GetWeatherByCityNameAsync(CurrentWeatherTestData.FakeCurrentWeather.City.Name))
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather);
            //apiMock.Setup(a => a.GetWeatherByCityNameAsync(It.IsAny<string>()))
            //    .Throws<AggregateException>();
            //apiMock.Setup(a => a.GetWeatherByCityNameAsync(null))
            //    .Throws<ArgumentException>();
            Ioc.RegisterSingleton<IApiClient>(apiMock.Object);

            mapperMock = new Mock<IMapper>();
            Ioc.RegisterSingleton<IMapper>(mapperMock.Object);

            navigationMock = new Mock<IMvxNavigationService>();
            navigationMock.Setup(n => n.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                        It.IsAny<WeatherDetails>(), null, default(CancellationToken)))
                .ReturnsAsync(true);
            Ioc.RegisterSingleton<IMvxNavigationService>(navigationMock.Object);
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
            apiMock.Verify(a => a.GetWeatherByCityNameAsync(vm.CityName), Times.Once);
            navigationMock.Verify(n => n.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                It.IsAny<WeatherDetails>(), null, default(CancellationToken)),
                Times.Once);
        }
    }
}
