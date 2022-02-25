using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using neshanak.viewModel;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net;
using System.IO;
using neshanak.Classes;
using System.Collections.Specialized;
using HtmlAgilityPack;

namespace neshanak.Controllers
{

    [SessionCheck]
    public class HomeController : Controller
    {
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        private void SetCookie(string mymodel, string name)
        {

            Response.Cookies[name].Value = Encrypt(mymodel);

        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private string getCookie(string name)
        {


            string req2 = "";
            if (Request.Cookies[name] != null)
            {
                req2 = Decrypt(Request.Cookies[name].Value);
            }
            if (name == "token" && req2 == "")
            {
                CookieVM cookieModel = new CookieVM();
                cookieModel.currentpage = "index";
                cookieModel.controller = "home";
                string srt = JsonConvert.SerializeObject(cookieModel);
                SetCookie(srt, "token");
                return srt;
            }
            else if (name == "vari" && req2 == "")
            {
                variabli cookieModel = new variabli();
                string srt = JsonConvert.SerializeObject(cookieModel);
                SetCookie(srt, "vari");
                return srt;
            }
            return req2;


        }
        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        public static string RandomString()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        static string server = "http://www.supectco.com/neshanak/api/v1";
        static string servername = "";
        webService wb = new webService();
        public static string device = RandomString();
        public static string code = MD5Hash(device + "ncase8934f49909");
        public async Task<ActionResult> Index()
        
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "index";
            cookiemodel.controller = "home";
           
            cookiemodel.city = cookiemodel.city==null?  "1" : cookiemodel.city;
            cookiemodel.country = cookiemodel.country  == null ?  "1" : cookiemodel.country;
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            if (Session["lang"] == null)

            {
                Session["lang"] = "en";
            }
            string lang = Session["lang"] as string;
            getMainVM mainModel = new getMainVM()
            {
                code = device,
                device = code,
                city = cookiemodel.city == null ? "" : cookiemodel.city,
                country = cookiemodel.country == null ? "" : cookiemodel.country,
                lan = lang,
                mobile = "",
                nodeID = ""

            };
            string contactpayload = JsonConvert.SerializeObject(mainModel);
            string resu = await wb.doPostData(server + "/GetMain.php", contactpayload);

            IndexVM model = JsonConvert.DeserializeObject<IndexVM>(resu);
            return View(model);
        }

        public async Task<ActionResult> GetSubCat(string result,string city)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "GetSubCat";
            cookiemodel.controller = "home";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            if (city == null)
            {
                city = cookiemodel.city;
            }
            else
            {
                cookiemodel.city = city;
            }
            variabli varimodel = JsonConvert.DeserializeObject<variabli>(getCookie("vari"));
            if (result == null)
            {
                result = varimodel.result;
            }
            else
            {
                varimodel.result = result;
            }
           
            SetCookie(JsonConvert.SerializeObject(varimodel), "vari");
            string lang = Session["lang"] as string;

            getSubCatVM VMmodel = new getSubCatVM()
            {
                catLevel = result.Split('-')[1],
                ID = result.Split('-')[0],
                code = HomeController.code,
                device = HomeController.device,
                city = city,
                mobile = "",
                lan = lang,

            };
            string subCatload = JsonConvert.SerializeObject(VMmodel);
            string resu = await wb.doPostData(server + "/GetCatsPage.php", subCatload);
            IndexVM model = JsonConvert.DeserializeObject<IndexVM>(resu);
            return View(model);
            
            //return View();
        }

        public async Task<ActionResult> about()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "about";
            cookiemodel.controller = "home";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            string lang = Session["lang"] as string;
            getPageVM mainModel = new getPageVM()
            {
                code = device,
                device = code,
                lan = lang,
                page = "about"

            };
            string contactpayload = JsonConvert.SerializeObject(mainModel);
            string resu = await wb.doPostData(server + "/getPage.php", contactpayload);
            ViewBag.message = resu;
            return View();

        }
        public async Task<ActionResult> contact()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "contact";
            cookiemodel.controller = "home";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            string lang = Session["lang"] as string;
            getPageVM mainModel = new getPageVM()
            {
                code = device,
                device = code,
                lan = lang,
                page = "contact"

            };
            string contactpayload = JsonConvert.SerializeObject(mainModel);
            string resu = await wb.doPostData(server + "/getPage.php", contactpayload);
            ViewBag.message = resu;
            return View();

        }
        public async Task<ActionResult> privacy()
        {

            //CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            //cookiemodel.currentpage = "contact";
            //cookiemodel.controller = "home";
            //SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            //string lang = Session["lang"] as string;
            //getPageVM mainModel = new getPageVM()
            //{
            //    code = device,
            //    device = code,
            //    lan = lang,
            //    page = "privacy"

            //};
            //string contactpayload = JsonConvert.SerializeObject(mainModel);
            //string resu = await wb.doPostData(server + "/getPage.php", contactpayload);
           // ViewBag.message = resu;
            return View();

        }
        public ActionResult setCat(string result)
        {

            variabli varimodel = JsonConvert.DeserializeObject<variabli>(getCookie("token"));
            if (result == null)
            {
                result = varimodel.result;
            }
            else
            {
                varimodel.result = result;
            }
            SetCookie(JsonConvert.SerializeObject(varimodel), "token");
            return RedirectToAction("searchResult");
        }

        public async Task<ActionResult> GetSubCatjson(string result, string level)
        {
            string lang = Session["lang"] as string;

            getSubCatVM VMmodel = new getSubCatVM()
            {
                catLevel = level,
                ID = result,
                code = HomeController.code,
                device = HomeController.device,
                city = "",
                mobile = "",
                lan = lang,

            };
            string subCatload = JsonConvert.SerializeObject(VMmodel);
            string resu = await wb.doPostData(server + "/GetCatsPageFull.php", subCatload);
            IndexVM model = JsonConvert.DeserializeObject<IndexVM>(resu);
            if (level == "1")
            {
                return PartialView("/Views/Shared/_subcat.cshtml", model);

            }
            else
            {
                return PartialView("/Views/Shared/_subcat2.cshtml", model);
            }

            //return View();
        }
        public ActionResult test()
        {


            return Content("{\"message\" : \"shashidam to retrofited\"}");
        }

        public async Task<ActionResult> getcityPartial(string result)
        {
            CookieVM cookmodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookmodel.country = result;
            SetCookie(JsonConvert.SerializeObject(cookmodel), "token");
            string lang = Session["lang"] as string;

            getLandCATVM VMmodel = new getLandCATVM()
            {

                code = HomeController.code,
                device = HomeController.device,
                country = result,
                lan = lang,

            };
            string subCatload = JsonConvert.SerializeObject(VMmodel);
            string resu = await wb.doPostData(server + "/getLandscat.php", subCatload);
            countryCityCatVM model = JsonConvert.DeserializeObject<countryCityCatVM>(resu);
           
             return PartialView("/Views/Shared/_partialCity.cshtml", model);
        }
        public async Task<ActionResult> citySection(string result)
        {

            CookieVM cookmodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookmodel.country = result;
            cookmodel.query = "";
            SetCookie(JsonConvert.SerializeObject(cookmodel), "token");
            string lang = Session["lang"] as string;

            getLandCATVM VMmodel = new getLandCATVM()
            {

                code = HomeController.code,
                device = HomeController.device,
                country = result,
                lan = lang,

            };
            string subCatload = JsonConvert.SerializeObject(VMmodel);
            string resu = await wb.doPostData(server + "/getLandscat.php", subCatload);
            countryCityCatVM model = JsonConvert.DeserializeObject<countryCityCatVM>(resu);
            return View(model);
           // return PartialView("/Views/Shared/_partialCity.cshtml", model);

            //return View();
        }

        public void setCity(string id)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.city = id;
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
        }
        public void setValues(string result, string mallID,string contract)
        {
            string srt = getCookie("token");
            CookieVM cookieVM = JsonConvert.DeserializeObject<CookieVM>(srt);
            cookieVM.result = result;
            cookieVM.mallID = mallID;
            TempData["contract"] = contract;
            SetCookie(JsonConvert.SerializeObject(cookieVM), "token");
        }
        public void setQuery(string query, string cityID, string countryID)
        {
            string srt = getCookie("token");
            CookieVM cookieVM = JsonConvert.DeserializeObject<CookieVM>(srt);
            cookieVM.city = cityID;
            cookieVM.country = countryID;
            cookieVM.query = query;
            SetCookie(JsonConvert.SerializeObject(cookieVM), "token");
        }
        public async Task<ActionResult> searchResult(string fromForm)
        {
            
            // string result, string mallID, string floorID
            CookieVM model = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            if (fromForm == null)
            {
                model.page ="1";
            }
            model.currentpage = "searchResult";
            model.controller = "home";
            string lang = Session["lang"] as string;
            SetCookie(JsonConvert.SerializeObject(model), "token");
            string subcat = model.result != null ? model.result.Split('-')[0] : "";
            string catlevel = model.result != null ? model.result.Split('-')[1] : "";
            string contract = TempData["contract"] as string;
            searchResultVM VMmodel = new searchResultVM()
            {
                searchq = model.query,
                city = model.city == null ? "" : model.city,
                country = model.country == null ? "" : model.country,
                mobile = "",
                subcat = subcat,
                catLevel = catlevel,
                code = HomeController.code,
                device = HomeController.device,
                lan = lang,
                floorID = model.floorID,
                mallID = model.mallID,
                page = model.page,
                contract = contract

                //02122517428@tct2
                //650038
            };
            string searchResultPayload = JsonConvert.SerializeObject(VMmodel);
            string resu = await wb.doPostData(server + "/GetSearchResult.php", searchResultPayload);
            BzListVM viewModel = JsonConvert.DeserializeObject<BzListVM>(resu);
            ViewBag.result = model.result;
            ViewBag.mallID = model.mallID;
            ViewBag.floorID = model.floorID;
            if (fromForm != null)
            {
                return Content(JsonConvert.SerializeObject(viewModel));
            }
            return View(viewModel);
            // return View();
        }
       
        public ActionResult getNewList (string objectstring)
        {
            BzListVM viewModel = JsonConvert.DeserializeObject<BzListVM>(objectstring);
            return PartialView("/Views/Shared/_dataSectinOfSearch.cshtml", viewModel);
        }
        public void setPage()
        {
            CookieVM model = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            int current = model.page != null ? Int32.Parse(model.page) : 0;
            model.page = (current + 1).ToString() ;
            SetCookie(JsonConvert.SerializeObject(model), "token");
        }
        public ActionResult searchResultMap(string result, string mallID, string floorID)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "searchResultMap";
            cookiemodel.controller = "home";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            string device = RandomString();
            string mobile = "09194594505";
            string city = "teh";
            string code = MD5Hash(mobile + city + device + "_neshanak_hasdi1237e_");
            var client = new HttpClient();

            string html = string.Empty;
            string url = @"http://supectco.com/app/api/v1/GetSearchResult.php?code=" + code + "&device=" + device + "&mobile=" + mobile + "&city=" + city + "&subcat=" + result.Split('-')[0] + "&catLevel=" + result.Split('-')[1] + "&mallID=" + mallID + "&floorID=" + floorID;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            BzListVM model = JsonConvert.DeserializeObject<BzListVM>(html);
            ViewBag.result = result;
            ViewBag.mallID = mallID;
            ViewBag.floorID = floorID;
            ViewBag.zoom = 14;
            if (mallID != "0")
            {
                ViewBag.zoom = 18;
            }
            return View(model);
            // return View();
        }
        public void setcountry(string id)
        {

        }
        public ActionResult ChangeLanguage(string lang)
        {

            lang = lang.ToLower();
            string current = JsonConvert.DeserializeObject<CookieVM>(getCookie("token")).currentpage;
            string controller = JsonConvert.DeserializeObject<CookieVM>(getCookie("token")).controller;
            Session["lang"] = lang;
            return RedirectToAction(current, controller);

        }
        [HomeSessionCheck]
        public async Task<ActionResult> createItem()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "createItem";
            cookiemodel.controller = "home";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            CookieVM model = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            //model.images = null;
            //SetCookie(JsonConvert.SerializeObject(model), "token");
            string lang = Session["lang"] as string;
            lang = lang == null ? "en" : lang;
            if (model.images != null)
            {
                if (model.images != "")
                    ViewBag.images = model.images.Trim(',');
            }

            getLandCATVM VMmodel = new getLandCATVM()
            {

                code = HomeController.code,
                device = HomeController.device,
                lan = lang,

            };
            string searchResultPayload = JsonConvert.SerializeObject(VMmodel);
            string resu = await wb.doPostData(server + "/getLandscat.php", searchResultPayload);
            itemDetailVM viewModel = JsonConvert.DeserializeObject<itemDetailVM>(resu);

            return View(viewModel);
        }
        [HttpPost]
        public async Task<ActionResult> addITem(itemDetailVM model)
        {
            CookieVM cookie = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            string lang = Session["lang"] as string;
            model.code = code;
            model.device = device;
            model.lan = lang;
            model.img = cookie.images;
            string searchResultPayload = JsonConvert.SerializeObject(model);
            string resu = await wb.doPostData(server + "/additem.php", searchResultPayload);
            countryCityCatVM viewModel = JsonConvert.DeserializeObject<countryCityCatVM>(resu);
            cookie.images = "";
            SetCookie(JsonConvert.SerializeObject(cookie), "token");
            return RedirectToAction("createItem");

        }
        public ContentResult sendToServerByJS()
        {
            string pathString = "~/images";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string filename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                filename = RandomString(7)+ hpf.FileName; ;
                string savedFileName = Path.Combine(Server.MapPath(pathString), filename);
                string savedFileNameThumb = Path.Combine(Server.MapPath(pathString), "0" + filename);
                hpf.SaveAs(savedFileName);

            }
            return Content(filename);
        }
        public ContentResult sendToServer(string srt)
        {
           
            string pathString = "~/images";
            if (!Directory.Exists(Server.MapPath(pathString)))
            {
                DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(pathString));
            }
            string filename = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {

                HttpPostedFileBase hpf = Request.Files[i];

                if (hpf.ContentLength == 0)
                    continue;
                filename =RandomString(7) + hpf.FileName; ;
                string savedFileName = Path.Combine(Server.MapPath(pathString), filename);
                string savedFileNameThumb = Path.Combine(Server.MapPath(pathString), "0" + filename);
                hpf.SaveAs(savedFileName);
                CookieVM model = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
                model.images += filename + ",";
                SetCookie(JsonConvert.SerializeObject(model), "token");



            }
            return Content(filename);

        }
        public void removeImage(string id)
        {
            string pathString = "~/images";
            string savedFileName = Path.Combine(Server.MapPath(pathString), id);
            System.IO.File.Delete(savedFileName);
            CookieVM model = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            if (model.images != null)
            {
                model.images = model.images.Replace(id + ",", "");
            }


            SetCookie(JsonConvert.SerializeObject(model), "token");
        }
        public void deleteImage(string id)
        {
            string pathString = "~/images";
            string savedFileName = Path.Combine(Server.MapPath(pathString), id);
            //System.IO.File.Delete(savedFileName);

        }


        public ActionResult Register(string message)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "message";
            cookiemodel.controller = "home";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            ViewBag.message = message;
            
            return View();
        }
        public ActionResult login(string message)
        {
            
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            ViewBag.username = cookieModel.Username;
            ViewBag.password = cookieModel.Password;
            if (Session["LoginTime"] == null)
            {
                Session["LoginTime"] = 0;
            }
            else
            {
                if (Convert.ToInt32(Session["LoginTime"]) > 0)
                {
                    if (message == "1")
                    {
                        ViewBag.message = "1" ;

                    }
                    else if (message == null)
                    {
                        ViewBag.message = "";
                    }
                    else
                    {
                        ViewBag.message = "2";
                    }
                }
            }

            return View();
        }
        public ActionResult forum(string gr)
        {
            if (Session["LogedInUser"] == null)
            {
                return RedirectToAction("login", "Home");
            }
            else
            {
                string emailsrt = Session["LogedInUser"] as string;
                string email = emailsrt + "|" + emailsrt.Split('@')[0];


                ViewBag.url = email;
                ViewBag.gr = gr;
            }
            return View();
        }
        public ActionResult live()
        {
            if (Session["LogedInUser"] == null)
            {
                return RedirectToAction("login", "Home");
            }
            else
            {
                string emailsrt = Session["LogedInUser"] as string;
                string email = emailsrt + "|" + emailsrt.Split('@')[0];


                ViewBag.url = email;
            }
            return View();
        }
        public ActionResult logout() {
            Session["token"] = null;
            Session["LogedInUser"] = null;
            return RedirectToAction("index");

        }
        public ActionResult confirm()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "confirm";
            cookiemodel.controller = "home";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            confrimVM model = new confrimVM()
            {
                id = cookieModel.id,
                pass = cookieModel.pass
            };
            return View(model);
        }
        public ActionResult forgetpass(string type)
        {

            return View();
        }
        public ActionResult ChangePass(string Token)
        {
            if (Session["token"] != null)
            {
                RedirectToAction("index", "home");
            }
            return View();
        }

     
        public ActionResult Mag()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "Mag";
            cookiemodel.controller = "home";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");

            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string id = "";
            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("lan", lan);

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(server + "/getDataCatArticlesList.php?", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }


            ArticleListOfMag log = JsonConvert.DeserializeObject<ArticleListOfMag>(result);
            if (log.articles != null)
            {
                foreach (var item in log.articles)
                {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(item.content);
                    HtmlNode node = doc.DocumentNode;
                    item.content = node.InnerText;

                }
            }


            return View(log);
        }
        public ActionResult magDetail(string id)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "magDetail";
            cookiemodel.controller = "home";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");


            variabli varimodel = JsonConvert.DeserializeObject<variabli>(getCookie("vari"));

            if (id == null)
            {
                id = varimodel.magid;
            }
            else
            {
                varimodel.magid = id;
            }
            SetCookie(JsonConvert.SerializeObject(varimodel), "vari");

            string token = "";
            if (Session["token"] != null)
            {
                token = Session["token"].ToString();

            }
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("id", id);
                collection.Add("token", token);
                collection.Add("lan", lan);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(server + "/getDataArticlesDetail.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            articleDetailVM log = JsonConvert.DeserializeObject<articleDetailVM>(result);


            return View(log);

        }
        public async Task<ActionResult> itemDetail(string id)
        {

            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "itemDetail";
            cookiemodel.controller = "home";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");


            variabli varimodel = JsonConvert.DeserializeObject<variabli>(getCookie("vari"));
            if (id == null)
            {
                id = varimodel.detailID;
            }
            else
            {
                varimodel.detailID = id;
            }
            SetCookie(JsonConvert.SerializeObject(varimodel), "vari");

            if (Session["lang"] == null)
            {
                Session["lang"] = "en";
            }
            string lang = Session["lang"] as string;
            getItemDetail mainModel = new getItemDetail()
            {
                code = device,
                device = code,
                lan = lang,
                NodeID = id

            };
            string contactpayload = JsonConvert.SerializeObject(mainModel);
            string resu = await wb.doPostData(server + "/GetNodeData.php", contactpayload);
            viewModel.itemDetailVM model = JsonConvert.DeserializeObject<viewModel.itemDetailVM>(resu);
            return View(model);
        }

     

    }
}