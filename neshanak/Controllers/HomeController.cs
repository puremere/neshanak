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

namespace neshanak.Controllers
{
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
                string srt = JsonConvert.SerializeObject(cookieModel);
                SetCookie(srt, "token");
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

            if (Session["lang"] == null)
            {
                Session["lang"] = "en";
            }
            string lang = Session["lang"] as string;
            getMainVM mainModel = new getMainVM()
            {
                code = device,
                device = code,
                city = "",
                lan = lang,
                mobile = "",
                nodeID = ""

            };
            string contactpayload = JsonConvert.SerializeObject(mainModel);
            string resu = await wb.doPostData(server + "/GetMain.php", contactpayload);
            IndexVM model = JsonConvert.DeserializeObject<IndexVM>(resu);
            return View(model);
        }

        public async Task <ActionResult> GetSubCat(string result)
        {
            string lang = Session["lang"] as string;

            getSubCatVM VMmodel = new getSubCatVM() {
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
            string lang = Session["lang"] as string ;

            searchResultVM VMmodel = new searchResultVM()
            {
                city ="",
                mobile = "",
                subcat = model.result.Split('-')[0],
                catLevel = model.result.Split('-')[1],
                code = HomeController.code,
                device = HomeController.device,
                lan = lang,
                floorID = model.floorID,
                mallID = model.mallID

            };
            string searchResultPayload = JsonConvert.SerializeObject(VMmodel);
            string resu = await wb.doPostData(server + "/GetSearchResult.php", searchResultPayload);
            BzListVM  viewmodel = JsonConvert.DeserializeObject<BzListVM>(resu);
            ViewBag.result = model.result;
            ViewBag.mallID = model.mallID;
            ViewBag.floorID = model.floorID;
            return View(viewmodel);
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
            if (mallID != "0")
            {
                ViewBag.zoom = 18;
            }
            return View(model);
            // return View();
        }
       
        public ActionResult ChangeLanguage(string lang, string actionstring)
        {
            if (actionstring == null || actionstring == "")
            {
                actionstring = "index";
            }
            Session["lang"] = lang;
            return RedirectToAction(actionstring, "Home");
            
        }


    }
}