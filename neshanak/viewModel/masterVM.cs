using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel.master
{
    public class masterVM
    {
        public List<Report> report { get; set; }
        public string totalBenefit { get; set; }
    }
    public class Report
    {
        public string monthCount { get; set; }
        public string bardasht { get; set; }
        public string sood { get; set; }
        public string ghest { get; set; }
        public string etebar { get; set; }
        public string mojudi { get; set; }
    }

   
}