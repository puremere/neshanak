using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
    public class teachingVM
    {

    }
   

    public class ItemList
    {
        public int id { get; set; }
        public string title { get; set; }
        public string image { get; set; }
    }
    public class Forum
    {
        public string  title { get; set; }
        public string url { get; set; }
    }
    public class lanVM
    {
        public List<Sld> sld { get; set; }
        public List<Forum> forum { get; set; }
        public List<ItemList> itemList { get; set; }
    }
   
   

    public class vocabVM
    {
        public List<Sld> sld { get; set; }
        public List<vocabList> VocabList { get; set; }

    }
    public class vocabList
    {
        public int id { get; set; }
        public string title { get; set; }
        public string translation { get; set; }
        public string pronun { get; set; }
    }


}