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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BiometricIdentificationSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminLandingPage : Page
    {
        public AdminLandingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter != null)
            {
                var admin = e.Parameter as User;
                admin_name.Text = admin.Firstname.Trim() + " " + admin.Lastname.Trim(); 
            }
            else
            {
                this.Frame.Navigate(typeof(LoginPage));
            }
        }
        

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if(args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(Settings));
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;

                switch(item.Name)
                {
                    case "create_student_record":
                       
                        ContentFrame.Navigate(typeof(Add_Student_Page));
                        break;
                    case "view_students_records":
                        ContentFrame.Navigate(typeof(View_Students_Records));
                        break;
                    case "create_staff_record":
                        ContentFrame.Navigate(typeof(Create_Admin_Page));
                        break;
                    case "view_staff_record":
                        ContentFrame.Navigate(typeof(VIew_Admin_Pages));
                        break;
                    case "archive_record":
                        ContentFrame.Navigate(typeof(Archived_Records));
                        break;
                }
            }
        }
    }
}
