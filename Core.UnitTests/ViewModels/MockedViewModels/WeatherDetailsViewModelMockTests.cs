using AutoMapper;
using Core.Constants;
using Core.Services.Interfaces;
using Core.UnitTests.TestData;
using Core.ViewModels;
using Moq;
using Moq.Protected;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using MvvmCross.UI;
using NUnit.Framework;
using OpenWeatherMap;
using Shouldly;
using System.Reflection;
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

        [TestCase(null)]
        [TestCase("")]
        [TestCase("°C")]
        [TestCase(" °C")]
        [TestCase("text °C")]
        public async Task GetColorByTemperature_Should_Return_Default_Color(string temperature)
        {
            //Arrange
            base.Setup();
            var vm = weatherMock.Object;
            var getColorByTemperature = vm.GetType().GetMethod("GetColorByTemperature",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            var color = getColorByTemperature.Invoke(vm, new object[] { temperature });

            //Assert
            color.ShouldBe(Colors.Default);
        }

        [TestCase("-1 °C")]
        [TestCase("-0.56 °C")]
        [TestCase("0 °C")]
        public async Task GetColorByTemperature_Should_Return_Cold_Color(string temperature)
        {
            //Arrange
            base.Setup();
            var vm = weatherMock.Object;
            var getColorByTemperature = vm.GetType().GetMethod("GetColorByTemperature",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            var color = getColorByTemperature.Invoke(vm, new object[] { temperature });

            //Assert
            color.ShouldBe(Colors.Cold);
        }

        [TestCase("1 °C")]
        [TestCase("0.56 °C")]
        [TestCase("5 °C")]
        [TestCase("10 °C")]
        public async Task GetColorByTemperature_Should_Return_Chilly_Color(string temperature)
        {
            //Arrange
            base.Setup();
            var vm = weatherMock.Object;
            var getColorByTemperature = vm.GetType().GetMethod("GetColorByTemperature",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            var color = getColorByTemperature.Invoke(vm, new object[] { temperature });

            //Assert
            color.ShouldBe(Colors.Chilly);
        }

        [TestCase("10.5 °C")]
        [TestCase("15.6 °C")]
        [TestCase("18 °C")]
        [TestCase("20 °C")]
        public async Task GetColorByTemperature_Should_Return_Warm_Color(string temperature)
        {
            //Arrange
            base.Setup();
            var vm = weatherMock.Object;
            var getColorByTemperature = vm.GetType().GetMethod("GetColorByTemperature",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            var color = getColorByTemperature.Invoke(vm, new object[] { temperature });

            //Assert
            color.ShouldBe(Colors.Warm);
        }

        [TestCase("20.1 °C")]
        [TestCase("35 °C")]
        [TestCase("40 °C")]
        public async Task GetColorByTemperature_Should_Return_Hotly_Color(string temperature)
        {
            //Arrange
            base.Setup();
            var vm = weatherMock.Object;
            var getColorByTemperature = vm.GetType().GetMethod("GetColorByTemperature",
                BindingFlags.NonPublic | BindingFlags.Instance);

            //Act
            var color = getColorByTemperature.Invoke(vm, new object[] { temperature });

            //Assert
            color.ShouldBe(Colors.Hotly);
        }

        [Test]
        public void CurrentTemperatureColor_Should_Return_Warn_Color()
        {
            //Arrange
            base.Setup();
            weatherMock.Protected()
                .Setup<MvxColor>("GetColorByTemperature", ItExpr.IsAny<string>())
                .Returns(Colors.Warm)
                .Verifiable();
            var vm = weatherMock.Object;

            //Act
            vm.Prepare(WeatherDetailsTestData.FakeWeatherDetails);

            //Assert
            vm.CurrentTemperatureColor.ShouldBe(Colors.Warm);
            weatherMock.Protected().Verify("GetColorByTemperature",
                Times.Once(), ItExpr.IsAny<string>());
        }

        [Test]
        public void MinTemperatureColor_Should_Return_Warn_Color()
        {
            //Arrange
            base.Setup();
            weatherMock.Protected()
                .Setup<MvxColor>("GetColorByTemperature", ItExpr.IsAny<string>())
                .Returns(Colors.Warm)
                .Verifiable();
            var vm = weatherMock.Object;

            //Act
            vm.Prepare(WeatherDetailsTestData.FakeWeatherDetails);

            //Assert
            vm.MinTemperatureColor.ShouldBe(Colors.Warm);
            weatherMock.Protected().Verify("GetColorByTemperature",
                Times.Once(), ItExpr.IsAny<string>());
        }

        [Test]
        public void MaxTemperatureColor_Should_Return_Warn_Color()
        {
            //Arrange
            base.Setup();
            weatherMock.Protected()
                .Setup<MvxColor>("GetColorByTemperature", ItExpr.IsAny<string>())
                .Returns(Colors.Warm)
                .Verifiable();
            var vm = weatherMock.Object;

            //Act
            vm.Prepare(WeatherDetailsTestData.FakeWeatherDetails);

            //Assert
            vm.MaxTemperatureColor.ShouldBe(Colors.Warm);
            weatherMock.Protected().Verify("GetColorByTemperature",
                Times.Once(), ItExpr.IsAny<string>());
        }
    }
}
