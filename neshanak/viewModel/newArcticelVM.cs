using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace neshanak.viewModel
{
    public class newArcticelVM
    {
        public string image { get; set; }
        public string catList { get; set; }
        public string title { get; set; }
        [AllowHtml]
        public string description { get; set; }
        public string tag { get; set; }
    }
    public class updatePagesVM
    {
        [AllowHtml]
        public string content { get; set; }
        
        
        public string name { get; set; }
        public string update { get; set; }
        public string type { get; set; }
        public string newName { get; set; }
        public string dropdown { get; set; }

    }
    public class updateArticleVM
    {
        public string IDupdate { get; set; }
        public string catListupdate { get; set; }
        public string titleupdate { get; set; }
        [AllowHtml]
        public string descriptionupdate { get; set; }
        public string tagupdate { get; set; }
    }
    
}