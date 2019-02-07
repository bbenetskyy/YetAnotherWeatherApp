# Yet Another Weather App

This one more Weather application written in C# with Xamarin.Native **Android** & **iOS** based on **Open Weather API**

This project use **ZenHub** as issue tracker, please use it during checking issues actual state.

## Checker Status

[![Build Status](https://dev.azure.com/bbenetskyy/Yet%20Another%20Weather%20App/_apis/build/status/bbenetskyy.YetAnotherWeatherApp?branchName=master)](https://dev.azure.com/bbenetskyy/Yet%20Another%20Weather%20App/_build/latest?definitionId=3&branchName=master)

[![CodeFactor](https://www.codefactor.io/repository/github/bbenetskyy/yetanotherweatherapp/badge)](https://www.codefactor.io/repository/github/bbenetskyy/yetanotherweatherapp)

## Used Frameworks & NuGet's:

* **AutoMapper** - A convention-based object-object mapper
* **InteractiveAlerts** - Interactvie Alerts fo Xamarin iOS/Android/PCL
* **Moq** - most popular and friendly mocking framework for .NET
* **MvvmCross** - is the .NET MVVM framework for cross-platform solutions, including Xamarin iOS, Xamarin Android, Xamarin Forms, Windows and Mac
* **MvvmCross.Droid.Support.V7.AppCompat** - This package contains Support v7 AppCompat support for MvvmCross
* **MvvmCross.Plugin.Visibility** - This package contains the 'Visibility' plugin for MvvmCross
* **MvvmCross.Tests** - This package contains the 'Test Helpers' for MvvmCross
* **NUnit** -  features a fluent assert syntax, parameterized, generic and theory tests and is user-extensible.
* **OpenWeatherMap .Net** - OpenWeatherMap-Api-Net compatible with Xamarin Platforms
* **Shouldly**- Assertion framework for .NET. The way asserting *Should* be
* **Xam.Plugin.Connectivity** - Get network connectivity information such as network type, speeds, and if connection is available
* **Xamarin.Essentials** - a kit of essential API's for your apps
* **MvvmCross.Plugin.Color** - contains the 'Color' plugin for MvvmCross

## Yet Another Weather App Solution Structure

* **Core Folder** 
    - **API** - in this project located all communication with Open Weather API through  **OpenWeatherMap .Net** NuGet.
    - **Core** - shared code between **Android** and **iOS** projects. Contains converted from Open Weather API Dbo model to local model class, mapping service and view models for native projects.
* **Native Folder**
    - **Android** - xamarin android native project.
    - **iOS** - xamarin ios native project.
* **Tests Folder**
    - **API.IntegrationTests** - integration tests with **NUnit** on real Open Weather API, here we testing how works **OpenWeatherMap .Net** NuGet.
    - **Core.UnitTests** - unit tests with **NUnit** where we check our mapper service and view models

## Code Coverage Status

![Selected Color Scheme](https://github.com/bbenetskyy/YetAnotherWeatherApp/blob/master/CodeCoverage.png)

### **Some Projects and files was excluded from coverage because they can not be tested.**
Was excluded:
* Full Tests Folder with Test Projects
* Full Native Folder with Native Projects (but later may be added if UI tests are written)
* From Core Project:
    - App.cs
    - Constants Folder
    - Geocoding & Geolocation

