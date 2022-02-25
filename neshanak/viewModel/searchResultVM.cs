using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
    public class searchResultVM
    {
        public string contract { get; set; }
        public string code { get; set; }
        public string device { get; set; }
        public string mobile { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string subcat { get; set; }
        public string catLevel { get; set; }
        public string mallID = "0";
        public string floorID = "0";
        public string lan { get; set; }
        public string page = "0";
        public string  searchq { get; set; }

    }
    
}