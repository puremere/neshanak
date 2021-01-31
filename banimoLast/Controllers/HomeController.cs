using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using neshanak.ViewModel;
using Newtonsoft.Json;


namespace neshanak.Controllers
{
    public class HomeController : Controller
    {
        static readonly string serverName = "http://supectco.com/neshanak/api/v1/";
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
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
        private void SetCookie(string Value)
        {
            var encodedCookie = new HttpCookie("token", Value);
            encodedCookie.HttpOnly = true;
            encodedCookie.Expires = DateTime.Now.AddDays(1);
            System.Web.HttpContext.Current.Response.Cookies.Add(encodedCookie);


        }
        private CookieVM getCookie()
        {
            if (System.Web.HttpContext.Current.Request.Cookies["token"] != null)
            {
                string value = string.Empty;
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["token"];

                if (cookie != null)
                {
                    // For security purpose, we need to encrypt the value.
                    HttpCookie decodedCookie = cookie;
                    value = decodedCookie.Value;
                }

                return JsonConvert.DeserializeObject<CookieVM>(Decrypt(value));
            }
            else
            {
                CookieVM model = new CookieVM();
                string srt = JsonConvert.SerializeObject(model);
                SetCookie(Encrypt(srt));
                return model;
                //if (Request.Cookies["token"] == null )
                //{

                //}
                //else
                //{
                //    return JsonConvert.DeserializeObject<CookieVM>(Decrypt(Request.Cookies["token"].Value));
                //}

            }


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
        public ActionResult Index()
        {
            string device = RandomString();
            string mobile = "09194594505";
            string city = "teh";
           string code = MD5Hash(mobile+ city+device + "_banimo_hasdi1237e_");
            var client = new HttpClient();
           
            string html = string.Empty;
            string url = serverName + @"GetMain.php?code=" + code + "&device=" + device+ "&mobile="+mobile + "&city="+city;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            IndexVM model = JsonConvert.DeserializeObject<IndexVM>(html);
            return View(model);
        }
        public ActionResult GetSubCat(string result)
        {
            string device = RandomString();
            string mobile = "09194594505";
            string city = "teh";
            string code = MD5Hash(mobile + city + device + "_banimo_hasdi1237e_");
            var client = new HttpClient();

            string html = string.Empty;
            string url = @"http://supectco.com/app/api/v1/GetCatsPage.php?code=" + code + "&device=" + device + "&mobile=" + mobile + "&city=" + city+"&ID="+result.Split('-')[0]+"&catLevel="+ result.Split('-')[1];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            IndexVM model = JsonConvert.DeserializeObject<IndexVM>(html);
            return View(model);

            //return View();
        }
        public ActionResult searchResult(string result,string mallID, string floorID)
        {
            string device = RandomString();
            string mobile = "09194594505";
            string city = "teh";
            string code = MD5Hash(mobile + city + device + "_banimo_hasdi1237e_");
            var client = new HttpClient();

            string html = string.Empty;
            string url = @"http://supectco.com/app/api/v1/GetSearchResult.php?code=" + code + "&device=" + device + "&mobile=" + mobile + "&city=" + city+ "&subcat="+result.Split('-')[0]+ "&catLevel="+ result.Split('-')[1]+"&mallID="+mallID + "&floorID="+floorID;

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
            return View(model);
           // return View();
        }
        public ActionResult searchResultMap(string result, string mallID, string floorID)
        {
            string device = RandomString();
            string mobile = "09194594505";
            string city = "teh";
            string code = MD5Hash(mobile + city + device + "_banimo_hasdi1237e_");
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
            if(mallID != "0")
            {
                ViewBag.zoom = 18;
            }
            return View(model);
           // return View();
        }
        public ActionResult profile (string id)
        {
            string device = RandomString();
            string mobile = "09194594505";
            string city = "teh";
            string code = MD5Hash(mobile + city + device + "_banimo_hasdi1237e_");
            var client = new HttpClient();

            string html = string.Empty;
            string url = @"http://supectco.com/app/api/v1/GetProfile.php?id=" + id;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            
IndexVM model = JsonConvert.DeserializeObject<IndexVM>(html);
            return View(model);
        }

        public ActionResult Register(string message)
        {
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
            CookieVM cookieModel = getCookie();

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
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}