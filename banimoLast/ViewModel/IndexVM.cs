using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.ViewModel
{
    public class IndexVM
    {
        public List<Sld> sld { get; set; }
        public List<Cat> cat { get; set; }
        public List<Spc> spc { get; set; }
        public List<Rec> rec { get; set; }
        public List<Bnr> bnr { get; set; }
        public List<Dplc> dplc { get; set; }
        public List<Rand> rand { get; set; }
    }
    public class Sld
    {
        public int ID { get; set; }
        public string dest { get; set; }
        public string act { get; set; }
        public string img { get; set; }
        public string Ptitle { get; set; }
        public string title { get; set; }
        public string name { get; set; }

    }

    public class Cat
    {
        public int id { get; set; }
        public string title { get; set; }
        public int IsFinal { get; set; }
        public int catLevel { get; set; }
        public string img { get; set; }

    }

    public class Spc
    {
        public string img { get; set; }
        public string txt1 { get; set; }
        public string txt2 { get; set; }
        public int act { get; set; }
        public string dest { get; set; }
        public string bg { get; set; }

    }

    public class Rec
    {
        public int id { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public object cat { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public string desc { get; set; }

    }

    public class Bnr
    {
        public int ID { get; set; }
        public string dest { get; set; }
        public string act { get; set; }
        public string img { get; set; }
        public string Ptitle { get; set; }
        public string title { get; set; }
        public string name { get; set; }

    }

    public class Dplc
    {
        public int id { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public object cat { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public string desc { get; set; }

    }

    public class Rand
    {
        public int id { get; set; }
        public string img { get; set; }
        public string title { get; set; }
        public object cat { get; set; }
        public int disc { get; set; }
        public int eshop { get; set; }
        public int verif { get; set; }
        public string desc { get; set; }

    }

    
}