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
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using Windows.Web.Http.Filters;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BiometricIdentificationSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Add_Student_Page : Page
    {
        string m_studentID;
        public Add_Student_Page()
        {
            this.InitializeComponent();
        }
        public void clear()
        {
            m_surname.Text = "";
            m_firstname.Text = "";
            m_department.SelectedIndex = -1;
            m_level.SelectedIndex = -1;
            m_matric_number.Text = "";
            m_phone_type.Text = "";
            m_phone_model.Text = "";
            m_phone_imei.Text = "";
            m_phone_number.Text = "";
            m_laptop.Text = "";
            m_laptop_model.Text = "";
            m_laptop_serial_number.Text = "";
        }

       

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(!checkStudentData())
            {
                
                return;
            }
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                SaveButton.IsEnabled = false;
                processing_status.Visibility = Visibility.Visible;
            });

            string surname = m_surname.Text;
            string firstname = m_firstname.Text;
            string department = m_department.SelectedItem.ToString();
            string level = m_level.SelectedItem.ToString();
            string matricNumber = m_matric_number.Text;
            string phoneType = m_phone_type.Text;
            string phoneModel = m_phone_model.Text;
            string phoneIMEI = m_phone_imei.Text;
            string phoneNumber = m_phone_number.Text;
            string laptopType = m_laptop.Text;
            string laptopModel = m_laptop_model.Text;
            string laptopSerialNumber = m_laptop_serial_number.Text;
            m_studentID = StudentIDGen(firstname, surname);
           var result = await Operations_Handler.CreateStudent(m_studentID, matricNumber, surname, firstname, department, level, phoneNumber, phoneType, phoneModel, phoneIMEI, laptopType, laptopModel, laptopSerialNumber);
            if (result == true)
            {
                Operations_Handler.DisplayMessageDialog("Operation Successful", "Student data saved");
                clear();
            }
            else
            {
                Operations_Handler.DisplayMessageDialog("Error Message", Operations_Handler.Error);
            }

            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                SaveButton.IsEnabled = true;
                processing_status.Visibility = Visibility.Collapsed;
                pivotPage.SelectedItem = student_photo_pivot;
            });
        }

        private string StudentIDGen(string firstname,string lastname)
        {
            string str1 = firstname.Substring(0, 1);
            string str2 = lastname.Substring(0, 1);
            var date = DateTime.Now;
            string seconds = date.Second.ToString();
            string minutes = date.Minute.ToString();
            string hour = date.Hour.ToString();
            string day = date.Hour.ToString();
            string month = date.Month.ToString();
            string year = date.Year.ToString();
            return str1 + str2 + year + month + day + hour + minutes + seconds;
        }



        private bool matricNumberVerify(string matricNo)
        {
            //format1 year/number
            List<int> position = new List<int>();

            for(int i = 0; i < matricNo.Length; i++)
            {
                if (matricNo.ElementAt(i) == '/')
                {
                    position.Add(i);
                }
            }

            if(position.Count == 1)
            {
                try
                {
                    string str1 = matricNo.Substring(0,position.ElementAt(0)-1);
                    string str2 = matricNo.Substring(position.ElementAt(0)+1);

                    if (str1.Length > 3 && str2.Length < 2)
                        return false;
                    if (str2.Length < 4)
                        return false;

                    int a = int.Parse(str1);
                    int b = int.Parse(str2);

                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else if(position.Count == 2)
            {
                return false;
            }
            else if(position.Count == 3)
            {
                    return false;
            }
            else
            {
                return false;
            }


        }

        private bool checkStudentData()
        {
            if(m_surname.Text.Length > 1 && m_firstname.Text.Length > 1 && Operations_Handler.checkIfAllAlpha(m_surname.Text) && Operations_Handler.checkIfAllAlpha(m_firstname.Text))
            {
                if(m_department.SelectedIndex < 0 )
                {
                    Operations_Handler.DisplayMessageDialog("Error", "Please Select Department");
                    return false;
                }
                else
                {
                    if(m_level.SelectedIndex < 0)
                    {
                        Operations_Handler.DisplayMessageDialog("Error", "Please Select Level");
                        return false;
                    }
                    else
                    {
                        if(matricNumberVerify(m_matric_number.Text))
                        {
                           try
                            {
                                int.Parse(m_phone_number.Text);
                                if(m_phone_type.Text.Length > 1)
                                {
                                    if(m_phone_model.Text.Length > 1)
                                    {
                                        try
                                        {
                                            int.Parse(m_phone_imei.Text);
                                            if (Operations_Handler.checkIfAllAlpha(m_laptop.Text))
                                            {
                                                if (m_laptop_model.Text.Length > 1)
                                                {
                                                    if(m_laptop_serial_number.Text.Length > 4)
                                                    {
                                                        return true;
                                                    }
                                                    else
                                                    {
                                                        Operations_Handler.DisplayMessageDialog("Error", "Invalid laptop serial number");
                                                        return false;
                                                        
                                                    }
                                                }
                                                else
                                                {
                                                    Operations_Handler.DisplayMessageDialog("Error", "Invalid laptop model");
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                Operations_Handler.DisplayMessageDialog("Error", "Invalid laptop name");
                                                return false;
                                            }
                                        }
                                        catch(Exception ex)
                                        {
                                            Operations_Handler.DisplayMessageDialog("Error", "Invalid IMEI number");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Operations_Handler.DisplayMessageDialog("Error", "invalid model name");
                                        return false;
                                    }
                                     }
                                else
                                {
                                    Operations_Handler.DisplayMessageDialog("Error", "invalid phone Typ");
                                    return false;
                                }
                            }
                            catch(Exception ex)
                            {
                                Operations_Handler.DisplayMessageDialog("Error", "Invalid phone number");
                                return false;
                                
                            }
                   }
                        else
                        {
                            Operations_Handler.DisplayMessageDialog("Error", "Invalid matric number");
                            return false;
                        }
                    }
                  
                }
                
            }
            else
            {
                Operations_Handler.DisplayMessageDialog("Error", "Invalid name");
                return false;
            }
        }

        private async void Camera_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
             {
                 processing_status.Visibility = Visibility.Visible;
                 camera.IsEnabled = false;
                 upload.IsEnabled = false;
             });

            string name = "test";
            try
            {
                var file = await Operations_Handler.captureImage(name);
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read); 
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
                await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

                StudentPhoto_View.Source = bitmapSource;

                var response = await Operations_Handler.SendImage(file);

                if(response)
                {
                    Operations_Handler.DisplayMessageDialog("Operation status", "Photo uploaded");
                }
                else
                {
                    Operations_Handler.DisplayMessageDialog("ERROR", "An Error occured, please try again");
                }
                
                 await file.DeleteAsync();
            }
            catch(Exception ex)
            {

            }
           

            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                processing_status.Visibility = Visibility.Collapsed;
                camera.IsEnabled = true;
                upload.IsEnabled = true;
            });
        }
    }
}
