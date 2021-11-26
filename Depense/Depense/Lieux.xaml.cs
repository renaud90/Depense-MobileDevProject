using DepenseCompletBD.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Depense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lieux : ContentPage
    {
        private Venue _venue;
        public Lieux(Venue venue)
        {
            InitializeComponent();
            RetournerLieux();
            _venue = venue;
        }

        public async void RetournerLieux()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Depense.foursquare_response.txt";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                var lieux = JsonConvert.DeserializeObject<EntLieu>(json);
                listeLieux.ItemsSource = lieux.response.venues;
            }
        }

        private void listeLieux_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var lieu = listeLieux.SelectedItem as Venue;
            _venue.location.address = lieu.location.address;
            _venue.categories.Add(lieu.categories[0]);
            _venue.location.lat = lieu.location.lat;
            _venue.location.lng = lieu.location.lng;
            _venue.name = lieu.name;
            Navigation.PopAsync();
        }



    }
}