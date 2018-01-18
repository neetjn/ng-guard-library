using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Guard.Private;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Guard
{
    public class User
    {

        public class Shop {

            /*
             *  @function       Generates specified Tags.
             *  @param1         Specify Tag type { Guard, TyE, PyE, DyE }
             *  @return         string
             */
                private static string gen_tag(string prefix)
                {

                    string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    string res = "";
                    Random rnd = new Random();
                    int len = 7;
                    while (0 < len--)
                        res += valid[rnd.Next(valid.Length)];
                    string nTag = prefix + res;

                    return nTag;
                }

            private static void create_tag(string user, string package, string email)
            {

                //CHECK_PACKAGE_TAG
                    string prefix = null;
                    switch (package)
                    {

                        case "Guard Tag":

                            prefix = "G_";
                            break;
                        case "TyE Tag - 30 Days":

                            prefix = "T_";
                            break;
                        case "PyE Tag - 24 Hours":

                            prefix = "P_";
                            break;
                        case "DyE Tag - 24 Hours":

                            prefix = "D_";
                            break;
                    }

                //GENERATE_TAG
                    string nTag = gen_tag(prefix);

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "create_tag");
                    wParams.Add("pkg", package);
                    wParams.Add("email", email);
                    wParams.Add("tag", nTag);

                //GET_PHP_RESPONSE
                    sWeb.Post(URI, wParams);

                //WEB_VIEW_TAG
                    try
                    {

                        Process.Start("https://guard.neetgroup.name/web/tag.php?tag=" + nTag);
                    }
                    catch (Exception) { MessageBox.Show("Generated Tag: " + nTag); }
            }
            public static void buy_tag(string user, string pass, string package, string email)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "http://forum.neetgroup.net/guard/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "buy_tag");
                    wParams.Add("pkg", package);
                    wParams.Add("email", email);

                if (Forum.Login(user, pass))
                {

                    //SERVER_RESPONSE
                        Resp = sWeb.Post(URI, wParams).Trim();

                    if (Resp.Equals("True"))
                    {

                        create_tag(user, package, email);
                        MessageBox.Show("Your Tag Has Been Successfully Purchased!\nInvoice Forwarded To Your Email: " + email, "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (Resp.Equals("False"))
                        MessageBox.Show("An Error Has Occurred With Your Purchase\nThis Is Most Likely Due To Insufficient Funds", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    else
                        Util.Secure.endClient();
                }
                else
                    MessageBox.Show("Invalid Credentials", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }
        }

        public class Forum
        {

            public static bool Login(string user, string pass)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "http://forum.neetgroup.net/guard/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "login");
                    wParams.Add("pass", pass);

                //SERVER_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                //CHECK_MATCH_RESPONSE
                    try
                    {

                        return bool.Parse(Resp);
                    }
                    catch (Exception)
                    {

                        Util.Secure.endClient();
                        return false;
                    }
            }
        }

        public class Guard
        {

            public static bool Exists(string user)
            {
                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "exists");

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                //CHECK_RESPONSE
                    try
                    {

                        return bool.Parse(Resp);
                    }
                    catch (Exception)
                    {

                        Util.Secure.endClient();
                        return false;
                    }
            }
            public static bool Register(string user, string pass, string nTag, bool silent = false)
            {
                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "register"); ;
                    wParams.Add("pass", pass);
                    wParams.Add("tag", nTag);

                if (Forum.Login(user, pass))
                {

                    //GET_PHP_RESPONSE
                        Resp = sWeb.Post(URI, wParams).Trim();

                    if (!Resp.Equals("User:Created"))
                    {

                        if (Resp.Equals("Fail"))
                        {
                            if (!silent)
                                MessageBox.Show("An Uhandled Exception Has Occured", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Util.Secure.endClient();
                        }
                        if (Resp.Equals("User:Exists"))
                        {
                            if (!silent)
                                MessageBox.Show("Username Already In Use", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        if (Resp.Equals("User:Length"))
                        {
                            if (!silent)
                                MessageBox.Show("Invalid Username Length", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        if (Resp.Equals("Tag:Length"))
                        {
                            if (!silent)
                                MessageBox.Show("Invalid Tag Length", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        if (Resp.Equals("Tag:Invalid"))
                        {
                            if (!silent)
                                MessageBox.Show("Invalid Tag Input", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        return false;
                    }
                    else
                    {

                        if (!silent)
                            MessageBox.Show("Guard Registration Successful", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                }
                else
                {

                    MessageBox.Show("Invalid Forum Credentials", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public static bool Login(string user, string pass, bool silent = false)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "login");
                    wParams.Add("pass", pass);

                //SERVER_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                //LOG_USER_INVOKE
                    Log(user);

                //CHECK_MATCH_RESPONSE
                    if (!Banned(user))
                    {

                        try
                        {

                            Log(user);
                            bool logged = bool.Parse(Resp);
                            if (!silent)
                            {

                                if (logged)
                                    MessageBox.Show("Guard Login Successful", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                else
                                    MessageBox.Show("Username and Password Mismatch", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            return logged;
                        }
                        catch (Exception)
                        {

                            Util.Secure.endClient();
                            return false;
                        }
                    }
                    else
                    {

                        MessageBox.Show("User Is Banned From The Server", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }
            }
            public static void Subscribe(string user, string package, string sTag)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "subscribe");
                    wParams.Add("pkg", package);
                    wParams.Add("tag", sTag);

                //CHECK_PERSONAL_SUBSCRIPTIONS
                    string temp = package.ToLower();
                    if (temp.Equals("pye"))
                    {
                        if (!Util.Projects.Available("PyE", "secret_pye"))
                        {

                            MessageBox.Show("This Project Is Currently Not Available", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    else if (temp.Equals("dye"))
                        if (!Util.Projects.Available("DyE", "secret_dye"))
                        {

                            MessageBox.Show("This Project Is Currently Not Available", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                if (!Resp.Equals("Success"))
                {

                    if (Resp.Equals("Fail"))
                        Util.Secure.endClient();
                    if (Resp.Equals("Active"))
                        MessageBox.Show("User Already Subscribed", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (Resp.Equals("User:Invalid"))
                        MessageBox.Show("User Does Not Exist", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (Resp.Equals("Tag:Invalid"))
                        MessageBox.Show("Provided Tag Does Not Exist", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }
                else
                {

                    MessageBox.Show("Subscription Successful\nThanks For Subscribing!", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            private static bool valid_key(string key)
            {

                if (key.Equals("0"))
                    return true;

                if (key.Length >= 7)
                {

                    double i = 0;
                    bool r = double.TryParse(key, out i);
                    return r;
                }
                return false;
            }
            public static void Synchronize(string user, string pass)
            {

                if (Forum.Login(user, pass))
                {

                    if (Guard.Login(user, pass, true))
                    {

                        //USE_WEB_METHODS
                            SecureWeb sWeb = new SecureWeb();
                        //CRYPTOGRAPHY_USE
                            Cryptography cryptgraph = new Cryptography();
                        //RESPONSE_DATA
                            string Resp = "";
                            string[] keys;
                        //REQUEST_CREDENTIALS
                            string URI = "http://forum.neetgroup.net/guard/api/synchronize.php";
                            NameValueCollection wParams = new NameValueCollection();
                            wParams.Add("user", user);

                        //SERVER_RESPONSE
                            Resp = sWeb.Post(URI, wParams).Trim();

                        //FORMAT_KEY_ARRAY
                            if (!Resp.Equals("Fail"))
                            {

                                keys = Resp.Split(';');
                                for (int i = 0; i < keys.Length; i++)
                                {
                                    if (keys[i].Length >= 8)
                                    {

                                        try
                                        {

                                            keys[i] = cryptgraph.qDecode(keys[i]);
                                        }
                                        catch (Exception) { }
                                    }
                                }

                            //CHECK_SPECIFIED_KEYS
                                if (keys[0].Equals("0") && keys[1].Equals("0"))
                                {

                                    MessageBox.Show("NEET Key(s) Not Specified", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }
                            //CHECK_VALID_KEYS
                                if (!valid_key(keys[0]) || !valid_key(keys[1]))
                                {

                                    MessageBox.Show("NEET Key(s) Not Valid", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                    return;
                                }

                            //REQUEST_CREDENTIALS
                                URI = "https://www.guard.neetgroup.name/api/synchronize.php";
                                NameValueCollection sParams = new NameValueCollection();
                                sParams.Add("user", user);
                                sParams.Add("key_1", keys[0]); //FORUM: NEET_KEY
                                sParams.Add("key_2", keys[1]); //FORUM: SECONDARY_KEY

                            //GET_PHP_RESPONSE
                                Resp = sWeb.Post(URI, sParams).Trim();

                            //CHECK_RESPONSE
                            if (!Resp.Equals("Success"))
                            {
                                if (Resp.Equals("Fail"))
                                    MessageBox.Show("Guard Sync Failed", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                if (Resp.Equals("Time"))
                                    MessageBox.Show("User Has Synced Within The Past 10 Days", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                                return;
                            }
                            else
                            {

                                MessageBox.Show("Guard Sync Successfully Executed", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

                        }
                        else if (Resp.Equals("Fail"))
                        {

                            MessageBox.Show("Forum Sync Failed", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        else
                        {

                            Util.Secure.endClient();
                            return;
                        }
                    }
                    else
                        MessageBox.Show("Guard Account Validation Failure", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                    MessageBox.Show("Forum Account Validation Failure", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            public static void Link(string user, string pass)
            {

                if (Forum.Login(user, pass))
                {

                    //USE_WEB_METHODS
                        SecureWeb sWeb = new SecureWeb();
                        string Resp = "";

                    //REQUEST_CREDENTIALS
                        string URI = "https://www.guard.neetgroup.name/api/driver.php";
                        NameValueCollection wParams = new NameValueCollection();
                        wParams.Add("class", "user");
                        wParams.Add("user", user);
                        wParams.Add("pass", pass);
                        wParams.Add("call", "link");

                    //GET_PHP_RESPONSE
                        Resp = sWeb.Post(URI, wParams).Trim();

                    if (Resp.Equals("Success"))
                        MessageBox.Show("Account Link Successful", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    else if (Resp.Equals("Fail"))
                        MessageBox.Show("Account Link Failure", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    else if (Resp.Equals("Time"))
                        MessageBox.Show("User Has Linked Within The Past 24 Hours", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    else
                        Util.Secure.endClient();

                    return;
                }
                else
                    MessageBox.Show("Forum Account Validation Failure", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                return;
            }

            public static bool Banned(string user)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "banned");

                //SERVER_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                //CHECK_MATCH_RESPONSE
                    try
                    {

                        return bool.Parse(Resp);
                    }
                    catch (Exception)
                    {

                        Util.Secure.endClient();
                        return false;
                    }
            }
            public static bool BlackListed()
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                    string Resp = null;

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", null); //IF_USER_NULL_NO_RETURN
                    wParams.Add("call", "black_listed");
                    wParams.Add("client_key", Util.clientKey);

                //SERVER_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                //CHECK_MATCH_RESPONSE
                    try
                    {

                        return bool.Parse(Resp);
                    }
                    catch (Exception)
                    {

                        Util.Secure.endClient();
                        return false;
                    }
            }
            private static void Expire(string user)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "expire");

                //GET_PHP_RESPONSE
                    sWeb.Post(URI, wParams);

                return;
            }
            public static bool Access(string database, string user, bool system_check = false, bool subscription = false)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "access");
                    wParams.Add("system_check", system_check.ToString());
                    wParams.Add("db", database);
                    if (system_check)
                        wParams.Add("client_key", Util.clientKey);
                    
                //SERVER_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                //SUBSCRIPTION_EXPIRATION
                    if (subscription)
                    {

                        Log(user);
                        Expire(user);
                    }

                //CHECK_MATCH_RESPONSE
                    try
                    {

                        return bool.Parse(Resp);
                    }
                    catch (Exception)
                    {

                        Util.Secure.endClient();
                        return false;
                    }
            }
            public static bool Developer(string user)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "developer");

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                //LOG_USER_INVOKE
                    Log(user);

                //CHECK_MATCH_RESPONSE
                    try
                    {

                        return bool.Parse(Resp);
                    }
                    catch (Exception)
                    {

                        Util.Secure.endClient();
                        return false;
                    }
            }

            public static string Token(string user)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "user");
                    wParams.Add("user", user);
                    wParams.Add("call", "token");

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                //CHECK_RESPONSE
                    return Resp;
            }

            public class Admin
            {

                public static void Ban(string user)
                {

                    //USE_WEB_METHODS
                        SecureWeb sWeb = new SecureWeb();
                    //REQUEST_CREDENTIALS
                        string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                        NameValueCollection wParams = new NameValueCollection();
                        wParams.Add("class", "user");
                        wParams.Add("user", user);
                        wParams.Add("call", "ban");

                    //GET_PHP_RESPONSE
                        sWeb.Post(URI, wParams);

                    return;
                }
                public static void BlackList()
                {

                    //USE_WEB_METHODS
                        SecureWeb sWeb = new SecureWeb();
                    //REQUEST_CREDENTIALS
                        string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                        NameValueCollection wParams = new NameValueCollection();
                        wParams.Add("class", "user");
                        wParams.Add("user", null);
                        wParams.Add("call", "black_list");
                        wParams.Add("key", Util.clientKey);

                    //GET_PHP_RESPONSE
                        sWeb.Post(URI, wParams);

                    return;
                }
            }

            public class Public
            {

                private static string api_loc = "https://guard.neetgroup.name/api/public/driver.php?";
                public static bool Exists(string user)
                {

                    SimpleWeb sWeb = new SimpleWeb();
                    string param_url = api_loc + "class=user" + "&call=exists" + "&user=" + user;
                    return sWeb.bRecv(param_url);
                }
                public static bool Login(string user, string pass, bool silent = false)
                {
                    SimpleWeb sWeb = new SimpleWeb();
                    string param_url = api_loc + "class=user" + "&call=login" + "&user=" + user + "&pass=" + pass;
                    switch (sWeb.bRecv(param_url))
                    {

                        case true:
                            if (!silent)
                                MessageBox.Show("Guard Login Successful", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        case false:
                            if (!silent)
                                MessageBox.Show("Guard Login Failure", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                    }
                    return false;
                }
                public static bool Developer(string user)
                {

                    SimpleWeb sWeb = new SimpleWeb();
                    string param_url = api_loc + "class=user" + "&call=developer" + "&user=" + user;
                    return sWeb.bRecv(param_url);
                }
                public static bool Access(string database, string user)
                {

                    SimpleWeb sWeb = new SimpleWeb();
                    string param_url = api_loc + "class=user" + "&call=access" + "&user=" + user + "&db=" + database;
                    return sWeb.bRecv(param_url);
                }
                public static bool Banned(string user)
                {

                    SimpleWeb sWeb = new SimpleWeb();
                    string param_url = api_loc + "class=user" + "&call=banned" + "&user=" + user;
                    return sWeb.bRecv(param_url);
                }
            }
        }

        private static void Log(string user)
        {

            //USE_WEB_METHODS
                SecureWeb sWeb = new SecureWeb();

            //REQUEST_CREDENTIALS
                string URI = "https://www.guard.neetgroup.name/api/driver.php";
                NameValueCollection wParams = new NameValueCollection();
                wParams.Add("class", "user");
                wParams.Add("user", user);
                wParams.Add("call", "log");
                wParams.Add("key", Util.clientKey);

            //GET_PHP_RESPONSE
                sWeb.Post(URI, wParams);

            return;
        }
    }
}