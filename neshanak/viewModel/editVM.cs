using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
  
   

   
    public class editVM
    {
        public List<Land> lands { get; set; }
        public List<City> cities { get; set; }
        public List<Cat> cat { get; set; }
       
        public int id { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string mobile1 { get; set; }
        public string mobile2 { get; set; }
        public string fax { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string instagram { get; set; }
        public string desc { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string catid { get; set; }
        public string subcatid { get; set; }
        public string subcatid2 { get; set; }
    }
 
}