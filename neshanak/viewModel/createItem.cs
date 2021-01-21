using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.viewModel
{
    public class createItem
    {
        public string  titleEn { get; set; }
        public string  titleFa { get; set; }
        public string  titleDe { get; set; }
        public string  descEn { get; set; }
        public string  descFa { get; set; }
        public string  descDe { get; set; }
        public string  lat { get; set; }
        public string  lng { get; set; }
        public string  addressEn { get; set; }
        public string  addressFa { get; set; }
        public string  addressDe { get; set; }
        public string aboutusEn { get; set; }
        public string aboutusFa { get; set; }
        public string aboutusDe { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string whatsapp { get; set; }
        public string telegram { get; set; }
        public string instagram { get; set; }
        public string country { get; set; }
        public string city { get; set; }

    }
    public class createItemToSend:main
    {
        public string title { get; set; }
        public string desc { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string address { get; set; }
        public string aboutus{ get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string whatsapp { get; set; }
        public string telegram { get; set; }
        public string instagram { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string image { get; set; }

    }
}