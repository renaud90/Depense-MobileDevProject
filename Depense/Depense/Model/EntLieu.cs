using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DepenseCompletBD.Model
{
    public class EntLieu
    {
        public Response response { get; set; }
    }

    public class Location
    {
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public int distance { get; set; }
        public string postalCode { get; set; }
        public string cc { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public IList<string> formattedAddress { get; set; }
        public string crossStreet { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
        public string pluralName { get; set; }
        public string shortName { get; set; }
        public bool primary { get; set; }
    }

    public class Venue
    {
        public Venue()
        {
            location = new Location();
            categories = new List<Category>();
        }

        public string id { get; set; }
        public string name { get; set; }
        public Location location { get; set; }
        public List<Category> categories { get; set; }
    }

    public class Response
    {
        public IList<Venue> venues { get; set; }
    }
}
