using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatAndDogClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BitmapImage targetImage;
        String targetUri = "C:/Users/sonbi/source/repos/CatAndDogClient/CatAndDogClient/cat.jpg";

        public MainWindow()
        {
            InitializeComponent();
            setTargetImage(targetUri);
        }

        private void FindImageButton_Click(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    setTargetImage(openFileDialog.FileName);
                    StatusTextBlock.Text = "Press Upload Button";
                }
            }
        }

        private void setTargetImage(String fileName)
        {
            targetUri = fileName;
            targetImage = new BitmapImage();
            targetImage.BeginInit();
            targetImage.CacheOption = BitmapCacheOption.OnDemand;
            targetImage.CreateOptions = BitmapCreateOptions.DelayCreation;
            targetImage.DecodePixelWidth = 300;
            targetImage.UriSource = new Uri(targetUri);
            targetImage.EndInit();
            UploadImage.Source = targetImage;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:8000/blog/catdogapi/");
            request.Method = "POST";
            request.ContentType = "application/json";

            string base64string = ImageToBase64(targetImage);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = "{"+ "\"check_image\" : \"" + base64string + "\"}";
                JsonViewer.Text = json;
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
                try
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        StatusTextBlock.Text = result;
                    }
                }
                catch
                {
                    StatusTextBlock.Text = "err";
                }
            }

        }

        private string ImageToBase64(ImageSource imgSource)
        {
            byte[] bytes = null;
            var bitmapSource = imgSource as BitmapSource;
            var encoder = new BmpBitmapEncoder();
            if(bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                using (var  stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }
            return Convert.ToBase64String(bytes);
        }

    }
}
