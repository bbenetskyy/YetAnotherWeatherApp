using API;
using AutoMapper;
using Core.IoC;
using Core.Services.Interfaces;
using MvvmCross;
using MvvmCross.Tests;
using NUnit.Framework;
using Shouldly;

namespace Core.UnitTests.IoC
{
    public class IoCRegistrarTests : MvxIoCSupportingTest
    {
        public IoCRegistrarTests()
        {
            Setup();
        }

        [Test]
        public void RegisterServices_ServicesRegisteredAsInterfaces_IoCCanResolveEachServiceInterface()
        {
            //Arrange
            var iocRegistrar = new IoCRegistrar();

            //Act
            iocRegistrar.RegisterServices();

            //Assert
            Mvx.IoCProvider.ShouldSatisfyAllConditions(
                () => Mvx.IoCProvider.CanResolve<IAlertService>().ShouldBeTrue(),
                () => Mvx.IoCProvider.CanResolve<IApiClient>().ShouldBeTrue(),
                () => Mvx.IoCProvider.CanResolve<IWeatherService>().ShouldBeTrue(),
                () => Mvx.IoCProvider.CanResolve<ILocationService>().ShouldBeTrue(),
                () => Mvx.IoCProvider.CanResolve<IGeolocationService>().ShouldBeTrue(),
                () => Mvx.IoCProvider.CanResolve<IGeocodingService>().ShouldBeTrue(),
                () => Mvx.IoCProvider.CanResolve<IConnectivityService>().ShouldBeTrue()
                );
        }

        [Test]
        public void RegisterSingletons_SingletonsRegisteredAsInterfaces_IoCCanResolveEachSingletonInterface()
        {
            //Arrange
            var iocRegistrar = new IoCRegistrar();

            //Act
            iocRegistrar.RegisterSingletons();

            //Assert
            Mvx.IoCProvider.CanResolve<IMapper>().ShouldBeTrue();
        }
    }
}
