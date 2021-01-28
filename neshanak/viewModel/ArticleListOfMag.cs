using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
    public class Articlemag
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }

        public string image { get; set; }
        public string catName { get; set; }
        public string catNameEn { get; set; }

        public string writer { get; set; }
        public string hashtags { get; set; }
        public string link { get; set; }
        public int view { get; set; }
        public string date { get; set; }
    }

    public class ArticleListOfMag
    {
        public List<Articlemag> articles { get; set; }
    }

    public class articleDetailVM
    {
        public List<Comment> comment { get; set; }
        public List<Articlemag> articles { get; set; }
    }
    public class Comment
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string time { get; set; }
        public string rtime { get; set; }
        public string comment { get; set; }
        public string title { get; set; }
        public string IsOwner { get; set; }
        public string ispublish { get; set; }
        public string Answer { get; set; }
    }
    public class Articlecomment
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string time { get; set; }
        public string rtime { get; set; }
        public string comment { get; set; }
        public string title { get; set; }
        public string IsOwner { get; set; }
        public string ispublish { get; set; }
        public string Answer { get; set; }

    }
    public class Comments
    {
        public List<Comment> comment { get; set; }
        public List<Articlecomment> Articlecomment { get; set; }

    }
    public class Articles
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string image { get; set; }
        public string catName { get; set; }
        public string writer { get; set; }
        public string hashtags { get; set; }
        public string link { get; set; }
        public int view { get; set; }
        public string date { get; set; }

    }
}