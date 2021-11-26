using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Depense.Model
{
    public class Categorie
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nom { get; set; }

        //public decimal Pourcentage { get; set; }

        public decimal Pourcentage
        {
            get
            {
                using (var conn = new SQLiteConnection(App.CheminBD))
                {
                    var totaleDepense = conn.Table<EntDepense>().ToList().Sum(x => x.Montant);

                    if (totaleDepense == 0)
                    {
                        return 0;
                    }

                    var montantCategorie = conn.Table<EntDepense>().ToList().Where(x => x.CategorieId == Id).Sum(x => x.Montant);
                    return montantCategorie / totaleDepense * 100;
                }

            }
        }
    }
}
