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
using Core.UnitTests.Fixtures;

namespace Core.UnitTests.ViewModels.MockedViewModels
{
    [TestFixture]
    public class SearchViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<IWeatherService> weatherMock;
        private Mock<ILocationService> locationMock;
        private Mock<IMvxNavigationService> navigationMock;
        private Mock<SearchViewModel> searchMock;
        private Mock<IAlertService> alertMock;
        private Mock<IConnectivityService> connectivityMock;

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

            weatherMock = new Mock<IWeatherService>();
            Ioc.RegisterSingleton<IWeatherService>(weatherMock.Object);

            MockFixtures.MockLocation(out locationMock);
            Ioc.RegisterSingleton<ILocationService>(locationMock.Object);

            alertMock = new Mock<IAlertService>();
            Ioc.RegisterSingleton<IAlertService>(alertMock.Object);

            MockFixtures.MockConnectivity(out connectivityMock);
            Ioc.RegisterSingleton<IConnectivityService>(connectivityMock.Object);

            MockSearch();
        }

        #region Tests

        [Test]
        public async Task CheckWeatherCommand_Should_Call_CheckWeather()
        {
            //Arrange
            Setup();
            searchMock.Protected()
                .Setup<Task>("CheckWeather")
                .Returns(Task.FromResult("Some Result"));
            var vm = searchMock.Object;

            //Act
            vm.CityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;
            await vm.CheckWeatherCommand.ExecuteAsync();

            //Assert
            searchMock.Protected().Verify("CheckWeather",
                Times.Once());
            searchMock.Protected().Verify("GetLocationCityName",
                Times.Never());
        }

        [Test]
        public void CheckWeather_Should_Call_GetWeather_And_NavigateToWeatherDetails()
        {
            //Arrange
            Setup();
            searchMock.Protected()
                .Setup<Task<CurrentWeatherResponse>>("GetWeather")
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather)
                .Verifiable();
            var vm = searchMock.Object;
            var checkWeather = vm.GetType().GetMethod("CheckWeather",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            vm.CityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;
            checkWeather.Invoke(vm, null);

            //Assert
            searchMock.Protected().Verify("GetWeather",
                Times.Once());
            searchMock.Protected().Verify("NavigateToWeatherDetails",
                Times.Once(), ItExpr.IsAny<CurrentWeatherResponse>());
        }

        [TestCase("London2")]
        [TestCase("asdasd")]
        public void CheckWeather_Should_Call_Only_GetWeather(string cityName)
        {
            //Arrange
            Setup();
            searchMock.Protected()
                .Setup<Task<CurrentWeatherResponse>>("GetWeather")
                .ReturnsAsync((CurrentWeatherResponse)null)
                .Verifiable();
            var vm = searchMock.Object;
            var checkWeather = vm.GetType().GetMethod("CheckWeather",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            vm.CityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;
            checkWeather.Invoke(vm, null);

            //Assert
            searchMock.Protected().Verify("GetWeather",
                Times.Once());
            searchMock.Protected().Verify("NavigateToWeatherDetails",
                Times.Never(), ItExpr.IsAny<CurrentWeatherResponse>());
        }

        [Test]
        public async Task GetLocationCityNameCommand_Should_Call_GetLocationCityName()
        {
            //Arrange
            Setup();
            var vm = searchMock.Object;

            //Act
            await vm.GetLocationCityNameCommand.ExecuteAsync();

            //Assert
            searchMock.Protected().Verify("GetLocationCityName",
                Times.Once());
        }

        [Test]
        public void GetLocationCityName_Should_Call_GetLocationCityNameAsync_And_ActivityIndicators_And_Update_CityName()
        {
            //Arrange
            Setup();
            searchMock.Protected()
                .Setup<Task>("GetLocationCityName")
                .CallBase();
            var vm = searchMock.Object;
            var getLocationCityName = vm.GetType().GetMethod("GetLocationCityName",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            vm.CityName = string.Empty;
            getLocationCityName.Invoke(vm, null);

            //Assert
            vm.CityName.ShouldBe(WeatherDetailsTestData.FakeWeatherDetails.CityName);
            searchMock.Protected().Verify("ShowActivityIndicator",
                Times.Once());
            searchMock.Protected().Verify("HideActivityIndicator",
                Times.Once());
            locationMock.Verify(l => l.GetLocationCityNameAsync(),
                Times.Once());
        }

        [Test]
        public void ShowActivityIndicator_Should_Set_IsLoading_To_True()
        {
            //Arrange
            Setup();
            searchMock.Protected()
                .Setup("ShowActivityIndicator")
                .CallBase();
            var vm = searchMock.Object;
            var showActivityIndicator = vm.GetType().GetMethod("ShowActivityIndicator",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            vm.IsLoading = false;
            showActivityIndicator.Invoke(vm, null);

            //Assert
            vm.IsLoading.ShouldBeTrue();
            searchMock.Protected().Verify("GetLocationCityName",
                Times.Never());
            searchMock.Protected().Verify("HideActivityIndicator",
                Times.Never());
            searchMock.Protected().Verify("GetWeather",
                Times.Never());
            searchMock.Protected().Verify("NavigateToWeatherDetails",
                Times.Never(), ItExpr.IsAny<CurrentWeatherResponse>());
        }

        [Test]
        public void HideActivityIndicator_Should_Set_IsLoading_To_False()
        {
            //Arrange
            Setup();
            searchMock.Protected()
                .Setup("HideActivityIndicator")
                .CallBase();
            var vm = searchMock.Object;
            var hideActivityIndicator = vm.GetType().GetMethod("HideActivityIndicator",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            vm.IsLoading = true;
            hideActivityIndicator.Invoke(vm, null);

            //Assert
            vm.IsLoading.ShouldBeFalse();
            searchMock.Protected().Verify("GetLocationCityName",
                Times.Never());
            searchMock.Protected().Verify("ShowActivityIndicator",
                Times.Never());
            searchMock.Protected().Verify("GetWeather",
                Times.Never());
            searchMock.Protected().Verify("NavigateToWeatherDetails",
                Times.Never(), ItExpr.IsAny<CurrentWeatherResponse>());
        }

        #endregion

        #region Mocks

        private void MockSearch()
        {
            searchMock = new Mock<SearchViewModel>(MockBehavior.Loose,
                mapperMock.Object, navigationMock.Object, weatherMock.Object,
                locationMock.Object, connectivityMock.Object, alertMock.Object)
            {
                CallBase = true
            };
            searchMock.Protected()
                .Setup("NavigateToWeatherDetails", ItExpr.IsAny<CurrentWeatherResponse>())
                .Verifiable();
            searchMock.Protected()
                .Setup<Task>("GetLocationCityName")
                .Returns(Task.FromResult("Some Result"))
                .Verifiable();
        }

        #endregion
    }
}
