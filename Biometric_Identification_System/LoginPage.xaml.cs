using System;
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
using BiometricIdentificationSystem.Classes;
using Windows.UI.Popups;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BiometricIdentificationSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private string m_username;
        private string m_password;

        public LoginPage()
        {
            this.InitializeComponent();
        }

        

        private async void Login_button_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
             {
                 Login_button.IsEnabled = false;
                 cancel_button.IsEnabled = false;
                 processing_ring.Visibility = Visibility.Visible;
                 reset_link.IsEnabled = false;
             });
            var admin = await Operations_Handler.CheckLogin(username_txtBox.Text, password_txtBox.Text);
            if (admin != null)
            {
                this.Frame.Navigate(typeof(AdminLandingPage), admin);
            }
            else
            {
                Operations_Handler.DisplayMessageDialog("Error Message", "Invalid username or password");


            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                Login_button.IsEnabled = true;
                cancel_button.IsEnabled = true;
                processing_ring.Visibility = Visibility.Collapsed;
                reset_link.IsEnabled = true;
            });


        }

        private void Cancel_button_Click(object sender, RoutedEventArgs e)
        {
            username_txtBox.Text = "";
            password_txtBox.Text = "";
        }

       
    }
}
