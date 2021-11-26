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
    public partial class CategorieDepense : ContentPage
    {
        public CategorieDepense()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //CalculerPourcentage();
            using (var conn = new SQLiteConnection(App.CheminBD))
            {
                lstCategories.ItemsSource = null;
                lstCategories.ItemsSource = conn.Table<Categorie>().ToList();
            }
        }

        //private void CalculerPourcentage()
        //{
        //    using (var conn = new SQLiteConnection(App.CheminBD))
        //    {
        //        var totaleDepenses = conn.Table<EntDepense>().ToList().Sum(x => x.Montant);

        //        foreach (var categorie in conn.Table<Categorie>().ToList())
        //        {
        //            if (totaleDepenses == 0)
        //            {
        //                categorie.Pourcentage = 0;
        //                return;
        //            }

        //            var montantCategorie = conn.Table<EntDepense>().ToList().Where(x => x.CategorieId == categorie.Id).Sum(x => x.Montant);
        //            categorie.Pourcentage = montantCategorie / totaleDepenses * 100;
        //            conn.Update(categorie);
        //        }
        //    }
        //}

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NouvelleCategorie());
        }

        private void lstCategories_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var categorie = lstCategories.SelectedItem as Categorie;

            if(categorie != null)
            {
                Navigation.PushAsync(new NouvelleCategorie(categorie));
                lstCategories.SelectedItem = null;
            }
        }
    }
}