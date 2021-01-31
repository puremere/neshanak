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
                string srt = JsonConvert.SerializeObject(cookieModel);
                SetCookie(srt, "token");
                return srt;
            }
            else if (name ==  "vari" && req2 == "")
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
            cookiemodel.city = "1";
            cookiemodel.country = "1";
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
                country = cookiemodel.country == null ? "": cookiemodel.country,
                lan = lang,
                mobile = "",
                nodeID = ""

            };
            string contactpayload = JsonConvert.SerializeObject(mainModel);
            string resu = await wb.doPostData(server + "/GetMain.php", contactpayload);
            IndexVM model = JsonConvert.DeserializeObject<IndexVM>(resu);
            return View(model);
        }

        public async Task<ActionResult> GetSubCat(string result)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "GetSubCat";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
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
                city = "",
                mobile = "",
                lan = lang,

            };
            string subCatload = JsonConvert.SerializeObject(VMmodel);
            string resu = await wb.doPostData(server + "/GetCatsPage.php", subCatload);
            IndexVM model = JsonConvert.DeserializeObject<IndexVM>(resu);
            return View(model);

            //return View();
        }
        public async Task<ActionResult> GetSubCatjson(string result)
        {
            string lang = Session["lang"] as string;

            getSubCatVM VMmodel = new getSubCatVM()
            {
                catLevel = result.Split('-')[1],
                ID = result.Split('-')[0],
                code = HomeController.code,
                device = HomeController.device,
                city = "",
                mobile = "",
                lan = lang,

            };
            string subCatload = JsonConvert.SerializeObject(VMmodel);
            string resu = await wb.doPostData(server + "/GetCatsPage.php", subCatload);
            IndexVM model = JsonConvert.DeserializeObject<IndexVM>(resu);
            if (result.Split('-')[1] == "1")
            {
                return PartialView("/Views/Shared/_subcat.cshtml", model);

            }
            else
            {
                return PartialView("/Views/Shared/_subcat2.cshtml", model);
            }

            //return View();
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

            //return View();
        }
        
        public void setCity (string id)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.city = id;
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
        }
        public void setValues(string result, string mallID)
        {
            string srt = getCookie("token");
            CookieVM cookieVM = JsonConvert.DeserializeObject<CookieVM>(srt);
            cookieVM.result = result;
            cookieVM.mallID = mallID;
            SetCookie(JsonConvert.SerializeObject(cookieVM), "token");
        }
        public async Task<ActionResult> searchResult()
        {
            // string result, string mallID, string floorID
            CookieVM model = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            model.currentpage = "searchResult";
            string lang = Session["lang"] as string;
            SetCookie(JsonConvert.SerializeObject(model), "token");
            searchResultVM VMmodel = new searchResultVM()
            {
                city = model.city == null ? "": model.city ,
                country = model.country == null ? "" : model.country,
                mobile = "",
                subcat = model.result.Split('-')[0],
                catLevel = model.result.Split('-')[1],
                code = HomeController.code,
                device = HomeController.device,
                lan = lang,
                floorID = model.floorID,
                mallID = model.mallID,
               
            };
            string searchResultPayload = JsonConvert.SerializeObject(VMmodel);
            string resu = await wb.doPostData(server + "/GetSearchResult.php", searchResultPayload);
            BzListVM viewmodel = JsonConvert.DeserializeObject<BzListVM>(resu);
            ViewBag.result = model.result;
            ViewBag.mallID = model.mallID;
            ViewBag.floorID = model.floorID;
            return View(viewmodel);
            // return View();
        }
        public ActionResult searchResultMap(string result, string mallID, string floorID)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "searchResultMap";
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
            string current = JsonConvert.DeserializeObject<CookieVM>(getCookie("token")).currentpage;

            Session["lang"] = lang;
            return RedirectToAction(current);

        }




        public async Task<ActionResult> createItem()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "createItem";
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
            countryCityCatVM viewmodel = JsonConvert.DeserializeObject<countryCityCatVM>(resu);

            return View(viewmodel);

        }
        [HttpPost]
        public async Task<ActionResult> addITem(createItem model)
        {
            CookieVM cookie = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            string lang = Session["lang"] as string;
            createItemToSend modelTosend = new createItemToSend()
            {
                title = JsonConvert.SerializeObject(new languageVM() { en = model.titleEn != null ? model.titleEn : "", fa = model.titleFa != null ? model.titleFa : "", de = model.titleDe != null ? model.titleDe : "" }),
                desc = JsonConvert.SerializeObject(new languageVM() { en = model.descEn != null ? model.descEn : "", fa = model.descFa != null ? model.descFa : "", de = model.descDe != null ? model.descDe : "" }),
                aboutus = JsonConvert.SerializeObject(new languageVM() { en = model.aboutusEn != null ? model.aboutusEn : "", fa = model.aboutusFa != null ? model.aboutusFa : "", de = model.aboutusDe != null ? model.aboutusDe : "" }),
                address = JsonConvert.SerializeObject(new languageVM() { en = model.addressEn != null ? model.addressEn : "", fa = model.addressFa != null ? model.addressFa : "", de = model.addressDe != null ? model.addressDe : "" }),
                country = model.country,
                city = model.city,
                email = model.email,
                instagram = model.instagram,
                lat = model.lat,
                lng = model.lng,
                mobile = model.mobile,
                phone = model.phone,
                telegram = model.telegram,
                website = model.website,
                whatsapp = model.whatsapp,
                image = cookie.images.Trim(','),
                code = code,
                device = device,
                lan = lang


            };
            string searchResultPayload = JsonConvert.SerializeObject(modelTosend);
            string resu = await wb.doPostData(server + "/addItem.php", searchResultPayload);
            countryCityCatVM viewmodel = JsonConvert.DeserializeObject<countryCityCatVM>(resu);
            return RedirectToAction("createItem");

        }
        public ContentResult sendToServer()
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
                filename = RandomString() + "_" + hpf.FileName; ;
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
            model.images = model.images.Replace(id + ",", "");
            SetCookie(JsonConvert.SerializeObject(model), "token");
        }



        public ActionResult Register(string message)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "message";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
            if (message == "1")
            {
                ViewBag.message = "عبارت امنیتی نادرست است";
            }
            else if (message == "2")
            {
                ViewBag.message = "این شماره قبلاً ثبت شده است";
            }
            else if (message == "3")
            {
                ViewBag.message = "خطا ! لطفاً مجددا تلاش کنید";
            }
            return View();
        }
        public ActionResult login(string message)
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "login";
            SetCookie(JsonConvert.SerializeObject(cookiemodel), "token");
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
                        ViewBag.message = "عبارت امنیتی نادرست است";

                    }
                    else if (message == null)
                    {
                        ViewBag.message = "";

                    }
                    else
                    {

                        ViewBag.message = "نام کاربری یا رمز عبور اشتباه است";
                    }
                }
            }

            return View();
        }
        public ActionResult confirm()
        {
            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "confirm";
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
        public async Task<ActionResult> itemDetail(string id) {

            CookieVM cookiemodel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            cookiemodel.currentpage = "itemDetail";
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