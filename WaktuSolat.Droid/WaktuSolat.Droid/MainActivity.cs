using Android.App;
using Android.OS;
using System.Net;
using System.IO;
using System;
using System.Json;

using System.Threading.Tasks;

using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WaktuSolat.Droid
{
    [Activity(Label = "Waktu Solat", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            Button button = FindViewById<Button>(Resource.Id.button1);

            button.Click += async (sender, e) => {

                //thanks to siapa yang buat API ni.. kawan pinjam untuk belajar ye... Tq
                string url = "http://kayrules.com/times/today.json?zone=JHR02&format=12-hour";

                JsonValue json = await FetchWeatherAsync(url);
                ParseAndDisplay(json);
            };

        }

        private async Task<JsonValue> FetchWeatherAsync(string url)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                }
            }
        }

        private void ParseAndDisplay(JsonValue json)
        {
            TextView lblSubuhWaktu = FindViewById<TextView>(Resource.Id.lblSubuhWaktu);
            TextView lblZohorWaktu = FindViewById<TextView>(Resource.Id.lblZohorWaktu);
            TextView lblAsarWaktu = FindViewById<TextView>(Resource.Id.lblAsarWaktu);
            TextView lblMaghribWaktu = FindViewById<TextView>(Resource.Id.lblMaghribWaktu);
            TextView lblIsyakWaktu = FindViewById<TextView>(Resource.Id.lblIsyakWaktu);

            JsonValue prayer_times = json["prayer_times"];
            lblSubuhWaktu.Text = prayer_times["subuh"];
            lblZohorWaktu.Text = prayer_times["zohor"];
            lblAsarWaktu.Text = prayer_times["asar"];
            lblMaghribWaktu.Text = prayer_times["maghrib"];
            lblIsyakWaktu.Text = prayer_times["isyak"];


        }
    }
}

