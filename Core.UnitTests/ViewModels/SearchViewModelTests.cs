using API;
using AutoMapper;
using Core.ViewModels;
using Moq;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Tests;
using NUnit.Framework;
using OpenWeatherMap;
using Shouldly;

namespace Core.UnitTests.ViewModels
{
    [TestFixture]
    public class SearchViewModelTests : MvxIoCSupportingTest
    {
        protected override void AdditionalSetup()
        {
            MvxSingletonCache.Instance
                .Settings
                .AlwaysRaiseInpcOnUserInterfaceThread = false;

            var helper = new MvxUnitTestCommandHelper();
            Ioc.RegisterSingleton<IMvxCommandHelper>(helper);

            var currentWeather = new CurrentWeatherResponse
            {
                Weather = new Weather
                {
                    Value = "CityName"
                }
            };
            var apiMock = new Mock<IApiClient>();
            apiMock.Setup(a => a.GetWeatherByCityNameAsync(It.IsAny<string>()))
                .ReturnsAsync(currentWeather);
            Ioc.RegisterSingleton<IApiClient>(apiMock.Object);

            var mapperMock = new Mock<IMapper>();
            Ioc.RegisterSingleton<IMapper>(mapperMock.Object);

            var navigationMock = new Mock<IMvxNavigationService>();
            Ioc.RegisterSingleton<IMvxNavigationService>(navigationMock.Object);
        }

        [Test]
        public void SomeTest()
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
    }
}
