using Depense.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Depense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NouveauCompte : ContentPage
    {
        public NouveauCompte()
        {
            InitializeComponent();
        }

        private void btnCreerCompte_Clicked(object sender, EventArgs e)
        {
            var adresseCourriel = txtAdresseCourriel.Text;
            var motDePasse = txtMotDePasse.Text;
            var confirmMotDePasse = txtConfirmationMotDePasse.Text;

            if (string.IsNullOrEmpty(adresseCourriel))
            {
                DisplayAlert("Alert", "Veuillez saisir une adresse courriel", "Fermer");
                return;
            }

            if (string.IsNullOrEmpty(motDePasse))
            {
                DisplayAlert("Alert", "Veuillez saisir un mot de passe", "Fermer");
                return;
            }

            if (string.IsNullOrEmpty(confirmMotDePasse))
            {
                DisplayAlert("Alert", "Veuillez comfirmer le mot de passe", "Fermer");
                return;
            }

            if(motDePasse != confirmMotDePasse)
            {
                DisplayAlert("Alert", "La confirmation du mot de passe ne correspond pas", "Fermer");
                return;
            }

            //valider l'addresse courriel est exacte

            var nouveauUtilisateur = new Utilisateur() { AdresseCourriel = adresseCourriel, MotDePasse = motDePasse };

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                var exist = conn.Table<Utilisateur>().ToList().Exists(x => x.AdresseCourriel == adresseCourriel);

                if (exist)
                {
                    DisplayAlert("Alert", "Il existe déjà un utilisateur avec cette addresse courriel.", "Fermer");
                    return;
                }

                conn.Insert(nouveauUtilisateur);
            }

            DisplayAlert("Message", "L'utilisateur a été créé avec succès", "Fermer");

            Navigation.PopAsync();

        }
    }
}