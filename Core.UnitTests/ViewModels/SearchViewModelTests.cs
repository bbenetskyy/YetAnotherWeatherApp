using AutoMapper;
using Core.Models;
using Core.Services;
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
using IMvxCommandHelper = MvvmCross.Commands.IMvxCommandHelper;

namespace Core.UnitTests.ViewModels
{
    [TestFixture]
    public class SearchViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<IAlertService> alertMock;
        private Mock<IMvxNavigationService> navigationMock;

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

            alertMock = new Mock<IAlertService>();
            alertMock.Setup(a => a.GetWeatherAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((CurrentWeatherResponse)null);
            alertMock.Setup(a => a.IsInternetConnection())
                .Returns(true);
            alertMock.Setup(a => a.GetWeatherAsync(CurrentWeatherTestData.FakeCurrentWeather.City.Name, It.IsAny<string>()))
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather);
            Ioc.RegisterSingleton<IAlertService>(alertMock.Object);
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
            alertMock.Verify(a => a.IsInternetConnection(), Times.Once);
            alertMock.Verify(a => a.GetWeatherAsync(vm.CityName, It.IsAny<string>()), Times.Once);
            navigationMock.Verify(n => n.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                It.IsAny<WeatherDetails>(), null, default(CancellationToken)),
                Times.Once);
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
            alertMock.Verify(a => a.IsInternetConnection(), Times.Once);
            alertMock.Verify(a => a.GetWeatherAsync(vm.CityName, It.IsAny<string>()), Times.Once);
            navigationMock.Verify(n => n.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                    It.IsAny<WeatherDetails>(), null, default(CancellationToken)),
                    Times.Never);
        }


        [Test]
        public async Task CheckWeatherCommand_Should_Call_Api_And_If_No_Internet_Show_Warning_Alert()
        {
            //Arrange
            base.Setup();
            alertMock.Setup(a => a.IsInternetConnection())
                .Returns(false);
            var vm = Ioc.IoCConstruct<SearchViewModel>();

            //Act
            vm.CityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;
            await vm.CheckWeatherCommand.ExecuteAsync();

            //Assert
            alertMock.Verify(a => a.IsInternetConnection(), Times.Once);
            alertMock.Verify(a => a.GetWeatherAsync(vm.CityName, It.IsAny<string>()), Times.Never);
            navigationMock.Verify(n => n.Navigate<WeatherDetailsViewModel, WeatherDetails>(
                    It.IsAny<WeatherDetails>(), null, default(CancellationToken)),
                Times.Never);
        }
    }
}
