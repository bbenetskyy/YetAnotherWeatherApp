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
    public class SearchViewModelTests : MvxIoCSupportingTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<IAlertService> alertMock;
        private Mock<IMvxNavigationService> navigationMock;
        private Mock<SearchViewModel> searchMock;

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

            searchMock = new Mock<SearchViewModel>(MockBehavior.Loose,
                mapperMock.Object, navigationMock.Object, alertMock.Object)
            {
                CallBase = true
            };
            searchMock.Protected()
                .Setup("NavigateToWeatherDetails", ItExpr.IsAny<CurrentWeatherResponse>())
                .Verifiable();
        }

        [Test]
        public async Task CheckWeatherCommand_Should_Call_GetWeather_And_NavigateToWeatherDetails()
        {
            //Arrange
            base.Setup();
            searchMock.Protected()
                .Setup<Task<CurrentWeatherResponse>>("GetWeather")
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather)
                .Verifiable();
            var vm = searchMock.Object;

            //Act
            vm.CityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;
            await vm.CheckWeatherCommand.ExecuteAsync();

            //Assert
            searchMock.Protected().Verify("GetWeather", Times.Once());
            searchMock.Protected().Verify("NavigateToWeatherDetails",
                Times.Once(), ItExpr.IsAny<CurrentWeatherResponse>());
        }

        [TestCase("London2")]
        [TestCase("asdasd")]
        public async Task CheckWeatherCommand_Should_Call_Only_GetWeather(string cityName)
        {
            //Arrange
            base.Setup();
            searchMock.Protected()
                .Setup<Task<CurrentWeatherResponse>>("GetWeather")
                .ReturnsAsync((CurrentWeatherResponse)null)
                .Verifiable();
            var vm = searchMock.Object;

            //Act
            vm.CityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;
            await vm.CheckWeatherCommand.ExecuteAsync();

            //Assert
            searchMock.Protected().Verify("GetWeather", Times.Once());
            searchMock.Protected().Verify("NavigateToWeatherDetails",
                Times.Never(), ItExpr.IsAny<CurrentWeatherResponse>());
        }
    }
}
