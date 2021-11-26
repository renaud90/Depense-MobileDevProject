using Depense.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Depense
{
    public partial class App : Application
    {
        public static string CheminBD;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Login());

        }

        public App(string cheminBD)
        {
            InitializeComponent();

            CheminBD = cheminBD;

            using (var conn = new SQLiteConnection(CheminBD))
            {
                conn.CreateTable<Utilisateur>();
                conn.CreateTable<EntDepense>();
                conn.CreateTable<Categorie>();
            }

            MainPage = new NavigationPage(new Login());
        }

        public static async Task<PermissionStatus> ValiderEtDemanderLocalisation()
        {
            var statut = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (statut == PermissionStatus.Granted)
            {
                return statut;
            }
            // Si la permission n'est pas donnée pour l'application IOS
            if (statut == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Puisque ce code est partagé entre Android et IOS
                // pour demander la permission dans IOS il
                // faut afficher un message qui demande d'activer la permission
                // dans les paramètres
            }

            // Si la permission n'est pas donnée pour l'application Android
            // il faut la demander de l'utilisateur
            statut = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return statut;

        }

        protected override void OnStart()
        {
            ValiderEtDemanderLocalisation();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
