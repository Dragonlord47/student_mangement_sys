using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BiometricIdentificationSystem.Classes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BiometricIdentificationSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class View_Students_Records : Page
    {
        public View_Students_Records()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                processing_status.Visibility = Visibility.Visible;
            });
            StudentList.ItemsSource = await Operations_Handler.GetStudents();

            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                processing_status.Visibility = Visibility.Collapsed;
            });
        }

        private async void Search_options_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectionBox = sender as ComboBox;
            if (search_parameter != null)
            {
                if (selectionBox.SelectedIndex > 0)
                    search_parameter.IsEnabled = true;
                else
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                    {
                        processing_status.Visibility = Visibility.Visible;
                    });
                    StudentList.ItemsSource = await Operations_Handler.GetUsers();

                    await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                    {
                        processing_status.Visibility = Visibility.Collapsed;
                    });

                    search_parameter.IsEnabled = false;
                }

            }

        }

        private async void Search_parameter_TextChanged(object sender, TextChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                processing_status.Visibility = Visibility.Visible;
            });
            
                StudentList.ItemsSource = await Operations_Handler.GetStudents(search_options.SelectedIndex, search_parameter.Text);
           

            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                processing_status.Visibility = Visibility.Collapsed;
            });
        }
    }
    
}
