using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
    public class bankInterestVM
    {
        public List<Report> report { get; set; }
        public string totalBenefit { get; set; }
    }
    public class Report
    {
        public string ID { get; set; }
        public string bardashtShode { get; set; }
        public string x1 { get; set; }
        public string ghabelPardakht { get; set; }
        public string ghabelBardasht { get; set; }
        public string x2 { get; set; }
        public string sood { get; set; }
        public string mande { get; set; }
    }

   
}