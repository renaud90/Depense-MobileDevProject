using Depense.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace Depense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Carte : ContentPage
    {
        IGeolocator locator = CrossGeolocator.Current;
        public Carte()
        {
            InitializeComponent();
            ObtenirLocalisation();
            locator.PositionChanged += Locator_PositionChanged;
        }

        private async void ObtenirLocalisation()
        {
            var statut = await App.ValiderEtDemanderLocalisation();
            if (statut == PermissionStatus.Granted)
            {
                // Nous allons utiliser cela un peu plus tard
                var localisation = await Geolocation.GetLocationAsync();
                CentrerCarte(localisation.Latitude, localisation.Longitude);
            }
            
        }

        private void CentrerCarte(double latitude, double longitude) 
        {
            var centre = new Xamarin.Forms.Maps.Position(latitude, longitude);
            var span = new MapSpan(centre, 0.01, 0.01);
            carteLocalisation.MoveToRegion(span);
        }

        private async void DemarrerLocalisation()
        {
            await locator.StartListeningAsync(new TimeSpan(0, 0, 15), 100);
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            CentrerCarte(e.Position.Latitude, e.Position.Longitude);
        }

        private async void ArreterLocalisation()
        {
            await locator.StopListeningAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DemarrerLocalisation();
            ObtenirLieux();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ArreterLocalisation();
        }

        private void ObtenirLieux()
        {
            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var depenses = conn.Table<EntDepense>().ToList();
                foreach (var depense in depenses)
                {
                    try
                    {
                        var positionPin = new Xamarin.Forms.Maps.Position(depense.LieuLatitude, depense.LieuLongitude);
                        var pin = new Pin()
                        {
                            Position = positionPin,
                            Label = depense.LieuNom,
                            Address = depense.LieuAddress,
                            Type = PinType.SavedPin
                        };
                        carteLocalisation.Pins.Add(pin);
                    }
                    catch (Exception ex) { }
                }
            }
        }


    }
}