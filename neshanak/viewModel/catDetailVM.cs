using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
    public class Datum
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
    }

    public class catDetailVM
    {
        public List<Datum> data { get; set; }
    }
    
}