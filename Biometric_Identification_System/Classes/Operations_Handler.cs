using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace BiometricIdentificationSystem.Classes
{
    class Operations_Handler
    {
        static string url = "http://localhost:58295/";
        public static string Error = "";
        public static String EncryptedPassword(string pass)
        {
            SHA256 sha = SHA256.Create();

            byte[] en_data = sha.ComputeHash(Encoding.UTF8.GetBytes(pass));
            return BitConverter.ToString(en_data).Replace("-", "").ToLower();
        }

        public static async void DisplayMessageDialog(string title, string message)
        {

            ContentDialog msg = new ContentDialog()
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK"
            };

            await msg.ShowAsync();
        }

        public static bool checkIfSentenceAllAlpha(string word)
        {
            char[] alphabets = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',' ' };
            word = word.ToLower();
            int count = 0;
            for (int i = 0; i < word.Length; i++)
            {
                foreach (char c in alphabets)
                {
                    char letter = word.ElementAt(i);
                    if (letter == c)
                    {
                        count++;
                        break;
                    }

                }

            }
            if (count == word.Length)
                return true;
            else
                return false;
        }

        public static bool checkIfAllAlpha(string word)
        {
            char[] alphabets = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            word = word.ToLower();
            int count = 0;
            for (int i = 0; i < word.Length; i++)
            {
                foreach (char c in alphabets)
                {
                    char letter = word.ElementAt(i);
                    if (letter == c)
                    {
                        count++;
                        break;
                    }

                }

            }
            if (count == word.Length)
                return true;
            else
                return false;
        }

        public async static Task<User> CheckLogin(string username, string password)
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = App.cookie;
                using (var client = new HttpClient(handler))
                {
                    LoginVM login = new LoginVM
                    {
                        LoginName = username,
                        Password = EncryptedPassword(password)
                    };
                    StringContent data = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync(new Uri(url + "api/login"), data))
                    {
                        string rs = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<User>(rs);
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async static Task<List<User>> GetUsers()
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = App.cookie;
                using (var client = new HttpClient(handler))
                {
                    using (var respond = await client.GetAsync(url + "api/Admin"))
                    {
                        string rs = await respond.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<User>>(rs);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessageDialog("Error", ex.Message);
            }
            return null;
        }

        public async static Task<List<Student>> GetStudents()
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = App.cookie;
                using (var client = new HttpClient(handler))
                {
                    using (var respond = await client.GetAsync(url + "api/Students"))
                    {
                        string rs = await respond.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<Student>>(rs);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessageDialog("Error", ex.Message);
            }
            return null;
        }



        public async static Task<List<User>> GetUsers(int index, string value)
        {
            try
            {

                UserSearchFormat search = new UserSearchFormat
                {
                    columnNumber = index,
                    columnValue = value
                };
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = App.cookie;
                using (var client = new HttpClient(handler))
                {

                    StringContent data = new StringContent(JsonConvert.SerializeObject(search), Encoding.UTF8, "application/json");
                    using (var respond = await client.PostAsync(new Uri(url + "api/Admin/Search"), data))
                    {
                        string rs = await respond.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<User>>(rs);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessageDialog("Error", ex.Message);
            }
            return null;
        }

        public async static Task<List<Student>> GetStudents(int index, string value)
        {
            try
            {

                UserSearchFormat search = new UserSearchFormat
                {
                    columnNumber = index,
                    columnValue = value
                };
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = App.cookie;
                using (var client = new HttpClient(handler))
                {

                    StringContent data = new StringContent(JsonConvert.SerializeObject(search), Encoding.UTF8, "application/json");
                    using (var respond = await client.PostAsync(new Uri(url + "api/Student/Search"), data))
                    {
                        string rs = await respond.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<List<Student>>(rs);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessageDialog("Error", ex.Message);
            }
            return null;
        }

        public async static Task<bool> CreateUser(string id, string firstname, string lastname, string adminType, string username, string password)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = App.cookie;
            using (var client = new HttpClient(handler))
            {
                User user = new User
                {
                    SN = 1,
                    UserID = id,
                    Firstname = firstname,
                    Lastname = lastname,
                    AdminType = adminType,
                    Username = username,
                    Password = EncryptedPassword(password)
                };
                StringContent data = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync(new Uri(url + "api/Admin"), data))
                {
                    bool status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        return true;
                    }
                    else
                    {
                        string rs = await response.Content.ReadAsStringAsync();
                        Error = rs;
                        return false;
                    }

                }
            }

        }

        public async static Task<StorageFile> captureImage(string filename)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            StorageFolder destinationFolder =
               await ApplicationData.Current.LocalFolder.CreateFolderAsync(filename,
               CreationCollisionOption.OpenIfExists);
            await photo.CopyAsync(destinationFolder, filename + ".jpg", NameCollisionOption.ReplaceExisting);

            return photo;

        }

        public async static Task<bool> SendImage(StorageFile imageFile)
        {
            
            string webServer = url + "api/Student/Image";
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = App.cookie;
            var myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
            myFilter.AllowUI = false;

            using (var client = new Windows.Web.Http.HttpClient(myFilter))
            {

                using (Windows.Web.Http.HttpMultipartFormDataContent formData = new Windows.Web.Http.HttpMultipartFormDataContent())
                {
                    IBuffer buffer = await FileIO.ReadBufferAsync(imageFile);
                    Windows.Web.Http.HttpBufferContent WebHttpContent = new Windows.Web.Http.HttpBufferContent(buffer);
                    formData.Add(WebHttpContent, "UploadedImage", imageFile.Name);
                    using (Windows.Web.Http.HttpResponseMessage response = await client.PostAsync(new Uri(webServer), formData))
                    {
                        if (response.IsSuccessStatusCode)
                            return true;
                        else
                            return false;
                    }

                }

            }
                
            
        }

        public async static Task<bool> CreateStudent(string studentID, string matricNumber, string surname, string firstname, string department, string level, string phoneNumber, string phoneType, string phoneModel, string phoneIMEI, string laptopType, string laptopModel, string laptopSerialNumber)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = App.cookie;
            using (var client = new HttpClient(handler))
            {
                Student student = new Student
                {
                    SN = 1,
                    StudentID = studentID,
                    MatricNumber = matricNumber,
                    Surname = surname,
                    Firstname = firstname,
                    Department = department,
                    StudentLevel = level,
                    PhoneNumber = phoneNumber,
                    PhoneType = phoneType,
                    PhoneModel = phoneModel,
                    PhoneIMEI = phoneIMEI,
                    LaptopType = laptopType,
                    LaptopModel = laptopModel,
                    LaptopSerialNumber = laptopSerialNumber

                };
                StringContent data = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync(new Uri(url + "api/Students"), data))
                {
                    bool status = response.IsSuccessStatusCode;
                    if (status == true)
                    {
                        return true;
                    }
                    else
                    {
                        string rs = await response.Content.ReadAsStringAsync();
                        Error = rs;
                        return false;
                    }

                }
            }
        }

    }

}
                   
                   
                   
                   
                   
                   
                   