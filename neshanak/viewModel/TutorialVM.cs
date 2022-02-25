using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
    public class Book
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string level { get; set; }
    }

    public class Cours
    {
        public string ID { get; set; }
        public string title { get; set; }
        public List<Book> books { get; set; }
        public string level { get; set; }
    }

    public class Section
    {
        public string ID { get; set; }
        public object title { get; set; }
        public List<Cours> courses { get; set; }
        public string level { get; set; }
    }

    public class TutorialAll
    {
        public string ID { get; set; }
        public string title { get; set; }
        public List<Section> section { get; set; }
        public string level { get; set; }
    }

    public class TutorialVM
    {
        public List<TutorialAll> tutorialAll { get; set; }
    }
  
}