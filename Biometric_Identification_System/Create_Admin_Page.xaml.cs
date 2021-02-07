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
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BiometricIdentificationSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Create_Admin_Page : Page
    {
        public Create_Admin_Page()
        {
            this.InitializeComponent();
        }
        private bool verifyInput()
        {
            string surname = m_surname.Text;
            string firstname = m_firstname.Text;
            if (surname.Length > 1 && firstname.Length > 1 && Operations_Handler.checkIfAllAlpha(surname) && Operations_Handler.checkIfAllAlpha(firstname))
            {
                if(m_admin_Type.SelectedIndex >= 0)
                {
                    return true;
                }
                else
                {
                    Operations_Handler.DisplayMessageDialog("Error Message", "Please select admin Type");
                    return false;
                }
            }
            else
            {
                Operations_Handler.DisplayMessageDialog("Error Message", "Invalid input");
                return false;
            }         
        }
        private string UserNameGen(string firstname,string lastname)
        {
            string val1 = firstname.Substring(0, 2);
            string val2 = lastname.Substring(0, 2);
            var date = DateTime.Now;
            string username = val1 + val2 + date.Day.ToString() + date.Second.ToString() + date.Hour.ToString();
            return username;
        }
        private string PasswordGen()
        {
            Random random = new Random();
            int rand = random.Next(1000, 3000);
            char[] alpha = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', '$', 'L', 'U','P','Q','W','Z','V','J','&','#','X' };
            string password = "";
            for(int i = 0; i < 4; i++)
            {
                int temp_rand = random.Next(0, 12);
                password += alpha[temp_rand].ToString();
            }

            rand = rand - DateTime.Now.Second;
            password += rand.ToString();

            return password;
        }
        private string genUserId()
        {
            string id = "ADM";
            var date = DateTime.Now;
            id = id + date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Minute.ToString() + date.Second.ToString();
            return id;
        }
        private void clear()
        {
            m_surname.Text = "";
            m_firstname.Text = "";
            m_admin_Type.SelectedIndex = -1;
            m_username.Text = "";
            m_password.Text = "";
        }
        private async void Create_admin_button_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, ()=>
            {
                Create_admin_button.IsEnabled = false;
                processing_status.Visibility = Visibility.Visible;
            });

            if(verifyInput())
            {

                string id = genUserId();
                string first = m_firstname.Text;
                string last = m_surname.Text;
                string type = m_admin_Type.SelectedItem.ToString();
                string userName = UserNameGen(first, last);
                string passWord = PasswordGen();

                var result = await Operations_Handler.CreateUser(id, first, last, type, userName, passWord);
                if (result == true)
                {
                    Operations_Handler.DisplayMessageDialog("Operation Successful", "Admin has beeen created");
                    clear();

                }
                else
                {
                    Operations_Handler.DisplayMessageDialog("Error Message", Operations_Handler.Error);
                }

            }


            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                Create_admin_button.IsEnabled = true;
                processing_status.Visibility = Visibility.Collapsed;
            });
        }
    }
}
