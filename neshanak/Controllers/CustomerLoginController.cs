using neshanak.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json;
using neshanak.Classes;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using BotDetect.Web.Mvc;
using System.IO;
using System.Web.Routing;


namespace neshanak.Controllers
{
    public class CustomerLoginController : Controller
    {

        static string server = "http://www.supectco.com/neshanak/api/v1";
        
        //
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
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "RegistrationCaptcha", "Incorrect CAPTCHA Code!")]
        public ActionResult CustomerVerification(string registeremail, string email, string registerpassword, string CaptchaCode, string registerMoaref)
        {
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState.Values)
                {
                    if (item.Errors.Count() > 0)
                    {
                        if (item.Errors.First().ErrorMessage == "Incorrect CAPTCHA Code!")
                        {
                            return RedirectToAction("Register", "Home", new { message = "1" });
                        }
                    }

                }

            }
            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("user", registeremail);
                collection.Add("password", registerpassword);
                collection.Add("moaref", registerMoaref);
                byte[] response = client.UploadValues(server + "/doSignUp.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            signeupviewModel mymodel = JsonConvert.DeserializeObject<signeupviewModel>(result);


            if (mymodel.status == 200)
            {
                cookieModel.id = registeremail;
                cookieModel.pass = registerpassword;
                SetCookie(JsonConvert.SerializeObject(cookieModel), "token");
                return RedirectToAction("confirm", "Home");

            }
            else if (mymodel.status == 300)
            {
                return RedirectToAction("Register", "Home", new { message = "2" });

            }
            else
            {
                return RedirectToAction("Register", "Home", new { message = "3" });

            }





        }
        public ContentResult CustomerRegister(string email, string pass, string phone, string val)
        {
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));

            string currentpage = cookieModel.currentpage;


            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", phone);
                collection.Add("pass", pass);
                collection.Add("lan", lang);
                collection.Add("verifyCode", val);
               
                byte[] response = client.UploadValues(server + "/getCode.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            userdata log = JsonConvert.DeserializeObject<userdata>(result);
            userdata user = new userdata();
            
            Session["token"] = log.token;
            return Content(log.status + "*" + currentpage);


        }
        [ValidateAntiForgeryToken]
        [CaptchaValidationActionFilter("CaptchaCode", "RegistrationCaptcha", "Incorrect CAPTCHA Code!")]
        public ActionResult CustomerLogInWC(string registerpassword, string registeremail, string register, string CaptchaCode)
        {
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            if (!ModelState.IsValid)
            {
                if (ModelState.Values.Last().Errors.Count > 0)
                {
                    if (Convert.ToInt32(Session["LoginTime"]) > 2)
                    {
                        return RedirectToAction("login", "Home", new { message = "1" });
                    }
                    else
                    {
                        Session["LoginTime"] = Convert.ToInt32(Session["LoginTime"]) + 1;
                    }


                }
                else
                {
                    return RedirectToAction("login", "home");
                }

            }


            string currentpage = cookieModel.currentpage;


            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("email", registeremail);
                collection.Add("password", registerpassword);
                collection.Add("lan", lang);


                byte[] response = client.UploadValues(server + "/doSignIn.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<userdata>(result);

            if (log.status == "300")
            {
                cookieModel.id = registeremail;
                cookieModel.pass = registerpassword;
                SetCookie(JsonConvert.SerializeObject(cookieModel), "token");
                return RedirectToAction("confirm", "Home");
            }
            else if (log.status == "400")
            {

                return RedirectToAction("login", "Home", new { message = "error" });
            }
            else
            {
                

                Session["LogedInUser"] = log;
                Session["token"] = log.token;


                if(currentpage != null)
                {

                }
                else
                {
                    currentpage = "index";
                }
                if (currentpage.Contains("?"))
                {
                    RouteValueDictionary obj = new RouteValueDictionary();

                    List<string> lst = currentpage.Split('?').ToList();
                    List<string> argList = lst[1].Split('&').ToList();
                    foreach (var valueitem in argList)
                    {
                        List<string> finalLst = valueitem.Split('=').ToList();
                        obj[finalLst[0]] = finalLst[1];
                    }

                    return RedirectToAction(lst[0], "Home", obj);

                }
                else
                {
                    return RedirectToAction(currentpage, "Home");
                }

            }


        }
        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }
      
        public ActionResult CustomerLogIn(string registerpassword, string registeremail, string register)
        {
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            Session["LoginTime"] = Convert.ToInt32(Session["LoginTime"]) + 1;
            if (!ModelState.IsValid)
            {

                return RedirectToAction("login", "Home");
            }


            string currentpage = cookieModel.currentpage;


            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", registeremail);
                collection.Add("password", registerpassword);
               
                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(server + "/doSignIn.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<userdata>(result);

            if (log.status != "200")
            {
                return RedirectToAction("login", "Home");
            }
            else
            {

                Session["LogedInUser"] = log;
                //  Session["UserLogedIn"] = log;
                Session["token"] = log.token;
                return RedirectToAction("Index", "Home");

            }






        }
        public ContentResult checkConfirmCode(string phone, string register)
        {
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            string currentpage = cookieModel.currentpage;


            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", phone);
                collection.Add("verifyCode", register);
              

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                string address = server + "/checkUserTokenForPass.php";
                byte[] response = client.UploadValues(address, collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<signeInviewModel>(result);

            // Session["LogedInUser"] = log;
            Session["token"] = log.token;
            return Content(log.status);


        }
        public ActionResult ResendCode(string phone)
        {
            string json;
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";

            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("mobile", phone);
              
                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(server + "/sendCodeAgain.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<signeupviewModel>(result);
            return Content(log.status + "");
            //return Content("");
        }
        public ContentResult ForgetPass(string phone)
        {
            CookieVM cookieModel = JsonConvert.DeserializeObject<CookieVM>(getCookie("token"));
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string result = "";
            string lang = Session["lang"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("phone", phone);
                collection.Add("lan", lang);
               

                //foreach (var myvalucollection in imaglist) {
                //    collection.Add("imaglist[]", myvalucollection);
                //}
                byte[] response = client.UploadValues(server + "/checkUserForPass.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<userdata>(result);
            cookieModel.id = phone;
            cookieModel.pass = "";
            SetCookie(JsonConvert.SerializeObject(cookieModel), "token");
            //Session["LogedInUser"] = log;
            // Session["UserLogedIn"] = log;
            Session["token"] = log.token;
            return Content(log.status + "");


        }
        public ContentResult ChangePassForLogedInUser(string pass)
        {
            if (Session["token"] != null)
            {
                RedirectToAction("index", "home");
            }
            string result = "";
            string device = RandomString();
            string code = MD5Hash(device + "ncase8934f49909");
            string token = Session["token"] as string;
            using (WebClient client = new WebClient())
            {

                var collection = new NameValueCollection();
                collection.Add("device", device);
                collection.Add("code", code);
                collection.Add("token", token);
                collection.Add("password", pass);
                byte[] response = client.UploadValues(server+ "/changePass.php", collection);

                result = System.Text.Encoding.UTF8.GetString(response);
            }
            var log = JsonConvert.DeserializeObject<userdata>(result);

            Session["LogedInUser"] = log;
            // Session["UserLogedIn"] = log;
            Session["token"] = log.token;
            return Content(log.status + "");
        }
        public ActionResult LogOff()
        {
           
            Session["LogedInUser"] = null;
            Session["token"] = null;
            return RedirectToAction("Index", "Home");
        }

       


    }
}