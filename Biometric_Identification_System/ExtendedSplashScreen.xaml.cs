﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using System.Threading.Tasks;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BiometricIdentificationSystem 
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplashScreen : Page
    {
        internal Rect splashImageRect;
        private SplashScreen splash;
        internal bool dismissed = false;
        internal Frame rootFrame;
        
        public ExtendedSplashScreen(SplashScreen splashscreen, bool loadState)
        {
            InitializeComponent();
            // Listen for window resize events to reposition the extended splash screen image accordingly/
            // This ensures that the extended splash screen formats properly in response to window resizing
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);
            splash = splashscreen;
            if(splash != null)
            {
                //Register an event handler to be executed when the splash screen has been  dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

                splashImageRect = splash.ImageLocation;
                PositionImage();

                //If applicable, include a method for postioning a progress control.
                PositionRing();
            }

            //Create a Frame to act as the navigation context
            rootFrame = new Frame();

            RestoreState(loadState);
            DismissedExtendedSplash();
        }

        void RestoreState(bool loadState)
        {
            if(loadState)
            {
                // TODO: code to load
            }
        }
        void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            extendedSplashImage.Height = splashImageRect.Height;
            extendedSplashImage.Width = splashImageRect.Width;
        }

        void PositionRing()
        {
            splashProgressRing.SetValue(Canvas.LeftProperty, splashImageRect.X + (splashImageRect.Width * 0.5) - (splashProgressRing.Width * 0.5));
            splashProgressRing.SetValue(Canvas.TopProperty, (splashImageRect.Y + splashImageRect.Height + splashImageRect.Height * 0.1));
        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // safely update the extended splash screen image coordinates. This Function will be excuted when a user resizes the window.
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
                PositionRing();

                //If applicable, include a method for positioning a progress control.
                // PositionRing();

            }
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (apploction's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;
            //Complete  aapp setuo operation here...
        }

        async void DismissedExtendedSplash()
        {

            await Task.Delay(TimeSpan.FromSeconds(3));
            
           
            rootFrame.Navigate(typeof(LoginPage));
            Window.Current.Content = rootFrame;
        }

        

    }

    
}