// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Blank
{
    [Register ("WeatherDetailsView")]
    partial class WeatherDetailsView
    {
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
        UIKit.UILabel maxLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel minLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
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

            if (maxLabel != null) {
                maxLabel.Dispose ();
                maxLabel = null;
            }

            if (minLabel != null) {
                minLabel.Dispose ();
                minLabel = null;
            }
        }
    }
}