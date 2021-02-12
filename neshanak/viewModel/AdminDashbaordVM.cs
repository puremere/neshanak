using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
    public class LastOrder
    {
        public string orderNumber { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
        public string status { get; set; }
        public string price { get; set; }
    }

    public class LastProduct
    {
        public object title { get; set; }
        public string ID { get; set; }
    }

    public class AdminDashbaordVM
    {
     
        public List<LastProduct> lastProducts { get; set; }
        public int installNumRows { get; set; }
        public int nodeNumRows { get; set; }
        public int userNumRows { get; set; }
      
    }
    public class articlesComment
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        
    }

    public class ArticleCommentList
    {
        public List<articlesComment> articles { get; set; }
    }
    
  
}