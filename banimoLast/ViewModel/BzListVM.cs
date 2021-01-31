using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.ViewModel
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class ShoppingCenterFloor
    {
        public int ID { get; set; }
        public string title { get; set; }

    }

    public class Bzlist
    {
        public int id { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public object cat { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public string desc { get; set; }
        public bool isShoppingCenter { get; set; }
        public List<ShoppingCenterFloor> ShoppingCenterFloors { get; set; }
        public string dist { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }

    }

    public class BzListVM
    {
        public List<Bzlist> bzlist { get; set; }

    }


}