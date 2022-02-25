using neshanak.viewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace neshanak.Controllers
{
    public class languageController : Controller
    {
        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
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
        // GET: language
        public ActionResult index()
        {
            
            CookieVM model = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            model.currentpage = "index";
            model.controller = "language";
            SetCookie(JsonConvert.SerializeObject(model), "token");

            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            if (Session["lang"] == null)
            {
                Session["lang"] = "en";
            }
            string lan = Session["lang"] as string;
            
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("page", "1001");
                collection.Add("lan", lan);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(server + "/getLanguageList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }

            lanVM model0 = JsonConvert.DeserializeObject<lanVM>(result);
            return View(model0);
        }
        public ActionResult section(string lanID)
        {
            CookieVM model = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            model.currentpage = "index";
            model.controller = "language";
            SetCookie(JsonConvert.SerializeObject(model), "token");
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("lan", lan);
                collection.Add("lanID", lanID);


                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(server + "/getSectionList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            lanVM model0 = JsonConvert.DeserializeObject<lanVM>(result);

            return View(model0);
        }
        public ActionResult course(string sectionID)
        {
            CookieVM model0 = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            model0.currentpage = "index";
            model0.controller = "language";
            SetCookie(JsonConvert.SerializeObject(model0), "token");
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("lan", lan);
                collection.Add("sectionID", sectionID);
                

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(server + "/getCourseList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            lanVM model = JsonConvert.DeserializeObject<lanVM>(result);
            return View(model);
        }
        public ActionResult book(string courseID)
        {
            CookieVM model0 = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            model0.currentpage = "index";
            model0.controller = "language";
            SetCookie(JsonConvert.SerializeObject(model0), "token");
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("lan", lan);
                collection.Add("courseID", courseID);
                

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response =
                client.UploadValues(server + "/getBookList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            lanVM model = JsonConvert.DeserializeObject<lanVM>(result);
            return View(model);
        }
        public ActionResult lession(string bookID)
        {
            CookieVM model0 = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            model0.currentpage = "index";
            model0.controller = "language";
            SetCookie(JsonConvert.SerializeObject(model0), "token");
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("lan", lan);
                collection.Add("bookID", bookID);


                byte[] response =
                client.UploadValues(server + "/getLessonList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            lanVM model = JsonConvert.DeserializeObject<lanVM>(result);
            return View(model);
        }
        public ActionResult vocab(string lessonID)
        {
            CookieVM model0 = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            model0.currentpage = "index";
            model0.controller = "language";
            SetCookie(JsonConvert.SerializeObject(model0), "token");
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");

            string result = "";
            string lan = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("lessonID", lessonID);
                collection.Add("lan", lan);


                byte[] response =
                client.UploadValues(server + "/getVocabList.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            vocabVM model = JsonConvert.DeserializeObject<vocabVM>(result);
            return View(model);
        }
    }
}