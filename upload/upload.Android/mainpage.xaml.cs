using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
import Servant
import Servant.Multipart

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace upload.Droid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class mainpage : ContentPage
    {
        object ImageFile;
        public object MainImage { get; private set; }

        public mainpage()
        {
            InitializeComponent();
        }

        async void TakePhoto_Clicked(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaiable", "Close");
                return;
            }

            if (ImageFile != null)
            {
                //ImageFile.Dispose();
            }

            //ImageFile = await CrossMedia.Current.PickPhotoAsync();
            ImageFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            { });

            if (ImageFile == null)
            {
                return;
            }

            /*Console.WriteLine(ImageFile.AlbumPath);

            MainImage.Source = ImageSource.FromStream(() =>
            {
                var stream = ImageFile.GetStream();
                return stream;
            });*/

        }

        async void PickPhoto_Clicked(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No Photo", ":( No Photo avaiable", "Close");
                return;
            }

            if (ImageFile != null)
            {
                //ImageFile.Dispose();
            }

            ImageFile = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions { });

            if (ImageFile == null)
            {
                return;
            }

            /*Console.WriteLine(ImageFile.AlbumPath);

            MainImage.Source = ImageSource.FromStream(() =>
            {
                var stream = ImageFile.GetStream();
                return stream;
            });*/

        }


        void UploadPhoto_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                byte[] imageByte = ReadFully(ImageFile.GetStream());
                const string imageKey = "userfile";
                const string imageName = "dummy.jpeg";

                MultipartFormDataContent content = new MultipartFormDataContent(5);
                content.Add(new ByteArrayContent(imageByte), imageKey, imageName);

                // Add content (optional)
                content.Add(new StringContent("admin"), "username");
                content.Add(new StringContent("1234"), "password");

                const string url = "http://172.17.9.49:1112/uploads";
                var result = new HttpClient().PostAsync(new Uri(url), content).Result;

                DisplayAlert("Response", result.Content.ReadAsStringAsync().Result, "CLOSE");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}