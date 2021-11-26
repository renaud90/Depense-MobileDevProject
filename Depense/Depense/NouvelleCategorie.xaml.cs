using Depense.Model;
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
    public partial class NouvelleCategorie : ContentPage
    {
        private Categorie _categorie;
        public NouvelleCategorie()
        {
            InitializeComponent();

            _categorie = null;

            btnSupprimer.IsVisible = false;
        }

        public NouvelleCategorie(Categorie categorie)
        {
            InitializeComponent();

            _categorie = categorie;

            txtCategorie.Text = categorie.Nom;
        }

        private void btnEnregistrer_Clicked(object sender, EventArgs e)
        {
            var categorie = txtCategorie.Text;

            //Valider si l'utilisateur a saisi une categories   
            if (string.IsNullOrEmpty(categorie))
            {
                DisplayAlert("Alert", "Veuillez saisir le nom de la catégorie", "Fermer");
                return;
            }

            if (_categorie == null)
            {
                using (var conn = new SQLiteConnection(App.CheminBD))
                {
                    //valider si une catégore existe déjà dans la liste avcec le même nom
                    var nomExist = conn.Table<Categorie>().ToList().Exists(x => x.Nom == categorie);
                    if (nomExist)
                    {
                        DisplayAlert("Alert", "Il existe déjà une catégprie avec ce nom", "Fermer");
                        return;
                    }

                    conn.Insert(new Categorie() { Nom = categorie });
                }
            }
            else
            {
                using (var conn = new SQLiteConnection(App.CheminBD))
                {
                    //valider si une catégore existe déjà dans la liste avec le même nom et un Id different
                    var nomExist = conn.Table<Categorie>().ToList().Exists(x => x.Nom == categorie && x.Id != _categorie.Id);
                    if (nomExist)
                    {
                        DisplayAlert("Alert", "Il existe déjà une catégprie avec ce nom", "Fermer");
                        return;
                    }

                    var exist = conn.Table<Categorie>().ToList().Exists(x => x.Id == _categorie.Id);

                    if (exist)
                    {
                        var categorieEdit = conn.Table<Categorie>().ToList().FirstOrDefault(x => x.Id == _categorie.Id);
                        categorieEdit.Nom = txtCategorie.Text;
                        conn.Update(categorieEdit);
                    }
                    else
                    {
                        DisplayAlert("Alert", "La catégorie n'existe pas", "Fermer");
                        return;
                    }
                }
            }

            DisplayAlert("Message", "La catégorie a été enregistrée avec succès", "Fermer");
        }

        private async void btnSupprimer_Clicked(object sender, EventArgs e)
        {

            var reponse = await DisplayAlert("Message", "Voulez vous supprimer cette catégorie?", "Oui", "Non");
            if (reponse)
            {
                using (var conn = new SQLiteConnection(App.CheminBD))
                {
                    var existe = conn.Table<EntDepense>().ToList().Exists(x => x.CategorieId == _categorie.Id);
                    if (existe)
                    {
                        DisplayAlert("Message", "Impossible de supprimer la catégorie car elle est utilisée dans une dépnse", "Fermer");
                        return;
                    }

                    //Supprimer la catégorie
                    var categorieDelete = conn.Table<Categorie>().ToList().Where(x => x.Id == _categorie.Id);
                    conn.Delete(categorieDelete);
                    DisplayAlert("Message", "La catégorie a été supprimée avec succès", "Fermer");
                    Navigation.PopAsync();
                }
            }
        }
    }
}