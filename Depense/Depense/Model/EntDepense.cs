using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Depense.Model
{
    public class EntDepense
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Montant { get; set; }
        public DateTime Date { get; set; }
        public int CategorieId { get; set; }
        public string LieuNom { get; set; }
        public string LieuCategorieId { get; set; }
        public string LieuAddress { get; set; }
        public double LieuLatitude { get; set; }
        public double LieuLongitude { get; set; }


        public string CategorieNom
        {
            get
            {
                using (var conn = new SQLiteConnection(App.CheminBD))
                {
                    return conn.Table<Categorie>().ToList().FirstOrDefault(x => x.Id == CategorieId).Nom;
                }
            }
        }
    }
}
