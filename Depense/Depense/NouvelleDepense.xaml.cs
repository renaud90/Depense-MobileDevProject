using Depense.Model;
using DepenseCompletBD.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Depense
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NouvelleDepense : ContentPage
    {
        private EntDepense _depense;
        private Venue _venue;
        public NouvelleDepense()
        {
            InitializeComponent();

            btnSupprimer.IsVisible = false;
        }

        public NouvelleDepense(EntDepense depense)
        {
            InitializeComponent();

            _depense = depense;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                pickCategorie.ItemsSource = conn.Table<Categorie>().ToList();
                pickCategorie.ItemDisplayBinding = new Binding("Nom");

                //Remplir les champs avec les valeurs de la dépense à modifier
                if (_depense != null)
                {
                    txtDescription.Text = _depense.Description;
                    txtMontant.Text = _depense.Montant.ToString();
                    datePick.Date = _depense.Date;
                    pickCategorie.SelectedItem = conn.Table<Categorie>().ToList().FirstOrDefault(x => x.Id == _depense.CategorieId);
                }
            }
        }

        private void btnEnregistrer_Clicked(object sender, EventArgs e)
        {
            var description = txtDescription.Text;
            decimal montant = 0;
            decimal.TryParse(txtMontant.Text, out montant);
            var date = datePick.Date;
            var categorie = pickCategorie.SelectedItem;

            if (string.IsNullOrEmpty(description))
            {
                DisplayAlert("Alert", "Veuillez saisir une description", "Fermer");
                return;
            }

            if (string.IsNullOrEmpty(txtMontant.Text))
            {
                DisplayAlert("Alert", "Veuillez saisir un montant", "Fermer");
                return;
            }

            if (date == null)
            {
                DisplayAlert("Alert", "Veuillez saisir une date", "Fermer");
                return;
            }

            if (categorie == null)
            {
                DisplayAlert("Alert", "Veuillez saisir une catégorie", "Fermer");
                return;
            }

            if (_depense == null)
            {
                using (var conn = new SQLiteConnection(App.CheminBD))
                {
                    var nouvelleDepense = new EntDepense()
                    {
                        Description = description,
                        Montant = montant,
                        Date = date,
                        CategorieId = (categorie as Categorie).Id
                    };
                    if (_venue != null)
                    {
                        nouvelleDepense.LieuAddress = _venue.location.address;
                        nouvelleDepense.LieuCategorieId = _venue.categories[0].id;
                        nouvelleDepense.LieuLatitude = _venue.location.lat;
                        nouvelleDepense.LieuLongitude = _venue.location.lng;
                        nouvelleDepense.LieuNom = _venue.name;
                    }

                    conn.Insert(nouvelleDepense);
                }
            }
            else
            {
                using (var conn = new SQLiteConnection(App.CheminBD))
                {
                    var depense = conn.Table<EntDepense>().ToList().FirstOrDefault(x => x.Id == _depense.Id);
                    if (depense != null)
                    {
                        depense.Description = description;
                        depense.Montant = montant;
                        depense.Date = date;
                        depense.CategorieId = (categorie as Categorie).Id;
                    }
                    if (_venue != null)
                    {
                        depense.LieuAddress = _venue.location.address;
                        depense.LieuCategorieId = _venue.categories[0].id;
                        depense.LieuLatitude = _venue.location.lat;
                        depense.LieuLongitude = _venue.location.lng;
                        depense.LieuNom = _venue.name;
                    }

                    conn.Update(depense);
                }
            }

            DisplayAlert("Message", "La dépense a été enregistrée avec succès", "Fermer");
            Navigation.PopAsync();
        }

        private async void btnSupprimer_Clicked(object sender, EventArgs e)
        {
            var reponse = await DisplayAlert("Message", "Voulez vous supprimer cette dépense?", "Oui", "Non");
            if (reponse)
            {
                using (var conn = new SQLiteConnection(App.CheminBD))
                {
                    var depeseDelete = conn.Table<EntDepense>().FirstOrDefault(x => x.Id == _depense.Id);
                    conn.Delete(depeseDelete);
                    Navigation.PopAsync();
                }
            }

        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var venue = new Venue();
            await Navigation.PushAsync(new Lieux(venue));
            _venue = venue;
        }
    }
}