using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
    public class CityList
    {
        public string ID { get; set; }
        public string title { get; set; }
    }

    public class LocationAll
    {
        public string ID { get; set; }
        public string title { get; set; }
        public List<CityList> cityList { get; set; }
    }

    public class countryAll
    {
        public List<LocationAll> locationAll { get; set; }
    }
    
}