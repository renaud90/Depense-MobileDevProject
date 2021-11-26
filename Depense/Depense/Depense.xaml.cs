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
    public partial class Depense : ContentPage
    {
        public Depense()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using(var conn = new SQLiteConnection(App.CheminBD))
            {
                lstDepenses.ItemsSource = null;
                lstDepenses.ItemsSource = conn.Table<EntDepense>().ToList();
            }
            
        }


        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NouvelleDepense());
        }

        private void lstDepenses_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var depense = lstDepenses.SelectedItem as EntDepense;
            if(depense != null)
            {
                Navigation.PushAsync(new NouvelleDepense(depense));
                lstDepenses.SelectedItem = null;
            }
        }
    }
}