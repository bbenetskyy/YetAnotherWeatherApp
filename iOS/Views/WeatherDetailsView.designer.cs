// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace iOS.Views
{
    [Register ("WeatherDetailsView")]
    partial class WeatherDetailsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem backButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel cityNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel currentTemperatureLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel descriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView loadingIndicator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel maxTemperatureLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel minTemperatureLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton refreshButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (backButton != null) {
                backButton.Dispose ();
                backButton = null;
            }

            if (cityNameLabel != null) {
                cityNameLabel.Dispose ();
                cityNameLabel = null;
            }

            if (currentTemperatureLabel != null) {
                currentTemperatureLabel.Dispose ();
                currentTemperatureLabel = null;
            }

            if (descriptionLabel != null) {
                descriptionLabel.Dispose ();
                descriptionLabel = null;
            }

            if (loadingIndicator != null) {
                loadingIndicator.Dispose ();
                loadingIndicator = null;
            }

            if (maxTemperatureLabel != null) {
                maxTemperatureLabel.Dispose ();
                maxTemperatureLabel = null;
            }

            if (minTemperatureLabel != null) {
                minTemperatureLabel.Dispose ();
                minTemperatureLabel = null;
            }

            if (refreshButton != null) {
                refreshButton.Dispose ();
                refreshButton = null;
            }
        }
    }
}