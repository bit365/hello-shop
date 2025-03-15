// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Android.App;
using Android.Content.PM;
using Microsoft.Maui;

namespace HelloShop.HybridApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
    }
}
