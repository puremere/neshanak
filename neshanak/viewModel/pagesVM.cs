﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
    public class pagesVM
    {
        public List<Item> items { get; set; }

    }
    public class Item
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }

    
}