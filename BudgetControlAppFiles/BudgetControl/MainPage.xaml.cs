using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BudgetControl
{
    public partial class MainPage : ContentPage
    {
        double number;
        double remain;
        double amt_spent;
        public MainPage()
        {
            InitializeComponent();
        }
        private void BudgetEntry_Completed(object sender, EventArgs e)
        {
            string text = Budget.Text;
            number = Convert.ToDouble(Budget.Text.ToString());
            remain = number;
            Budget_print.Text = "Your Budget is $" + text;
            amt_spent = 0.00;

        }
        private void CostEntry_Completed(object sender, EventArgs e)
        {
            string text = cost.Text;
            double cost_number = Convert.ToDouble(cost.Text.ToString());
            remain = remain - cost_number;
            amt_spent = amt_spent + cost_number;
            BudgetLeft.Text = "You spent $" + amt_spent.ToString() + " and have $" + remain.ToString() + " left.";

        }
        async void loadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }
            await DisplayAlert("camera clicked", "Take a photo of your receipt", "okay");
            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });
            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });
            await MakePredictionRequest(file);

        }
        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePredictionRequest(MediaFile file)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Prediction-Key", "cd81c739b3af4637b617921c4bd82cf9");

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/cf7c8598-5391-4cb1-91fe-9bf57cf73fc0/image?iterationId=0741adce-be34-4952-bf31-166d2c8ec9c5";
            HttpResponseMessage response;

            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    EvaluationModel responseModel = JsonConvert.DeserializeObject<EvaluationModel>(responseString);

                    double max = responseModel.Predictions.Max(m => m.Probability);

                    TagLabel.Text = (max >= 0.5) ? "receipt received" : "not receipt: please take the photo again";
                    //PredictionLabel.Text = "probability:" + Math.Round(max, 2).ToString();

                }
                file.Dispose();
            }
        }

        async void doneButtonClicked(object sender, EventArgs e)
                {
                    await postBudgetAsync();

                    async Task postBudgetAsync()
                    {

                        BC001 model = new BC001()
                        {
                            Budget = (float)number,
                            Remain = (float)remain

                        };

                        await AzureManager.AzureManagerInstance.PostBudgetInformation(model);
                    }
                    await DisplayAlert("", "transaction complete", "okay");

        }
    }
 }


