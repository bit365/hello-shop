// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Android.App;
using Android.Runtime;

namespace HelloShop.HybridApp
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
