using System;
using System.Collections.Generic;
using System.Text;
using API;
using Core.IoC;
using Core.Models;
using Core.Services.Interfaces;
using Moq;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Tests;
using NUnit.Framework;
using Shouldly;

namespace Core.UnitTests.IoC
{
    public class IoCRegistrarTests : MvxIoCSupportingTest
    {
        public IoCRegistrarTests()
        {
            base.Setup();
        }

        [Test]
        public void RegisterServices_ServicesRegisteredAsInterfaces_IoCCanResolveEachServiceInterface()
        {
            //Arrange
            var iocRegistrar = new IoCRegistrar();

            //Act
            iocRegistrar.RegisterServices();

            //Assert
            Mvx.IoCProvider.CanResolve<IApiClient>().ShouldBeTrue();
            Mvx.IoCProvider.CanResolve<IWeatherService>().ShouldBeTrue();
            Mvx.IoCProvider.CanResolve<ILocationService>().ShouldBeTrue();
            Mvx.IoCProvider.CanResolve<IGeolocationService>().ShouldBeTrue();
            Mvx.IoCProvider.CanResolve<IGeocodingService>().ShouldBeTrue();
        }
    }
}
