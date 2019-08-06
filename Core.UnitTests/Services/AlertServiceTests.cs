using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Resources;
using Core.Services;
using Core.UnitTests.TestData;
using InteractiveAlert;
using Moq;
using MvvmCross.Tests;
using NUnit.Framework;
using Shouldly;
using Xamarin.Essentials;

namespace Core.UnitTests.Services
{
    public class AlertServiceTests : MvxIoCSupportingTest
    {
        private Mock<IInteractiveAlerts> interactiveMock;

        public AlertServiceTests()
        {
            base.Setup();
        }

        protected override void AdditionalSetup()
        {
            interactiveMock = new Mock<IInteractiveAlerts>();
            Ioc.RegisterSingleton<IInteractiveAlerts>(interactiveMock.Object);
        }

        [Test]
        public void Show_CreateInteractiveAlertConfig_ShowAlertCalled()
        {
            //Arrange
            var alertService = Ioc.IoCConstruct<AlertService>();

            //Act
            alertService.Show(It.IsAny<string>(), It.IsAny<AlertType>());

            //Assert
            interactiveMock.Verify(x => x.ShowAlert(It.IsAny<InteractiveAlertConfig>()), Times.Once);
        }
    }
}
