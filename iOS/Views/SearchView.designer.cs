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
    [Register ("SearchView")]
    partial class SearchView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton getLocationButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView loadingIndicator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton searchButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField searchTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (getLocationButton != null) {
                getLocationButton.Dispose ();
                getLocationButton = null;
            }

            if (loadingIndicator != null) {
                loadingIndicator.Dispose ();
                loadingIndicator = null;
            }

            if (searchButton != null) {
                searchButton.Dispose ();
                searchButton = null;
            }

            if (searchTextField != null) {
                searchTextField.Dispose ();
                searchTextField = null;
            }
        }
    }
}