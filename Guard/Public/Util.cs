using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Guard.Private;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Guard
{
    public class Util
    {

        public static string clientKey = NGiD.clientKey();

        public class Secure
        {

            public static void Initialize(bool isDebugger, bool isConnected, bool doHeartBeat, int HeartBeatDelay = 80)
            {
                //USE_PROTECTION_LIB
                    Protection protect = new Protection();

                if (isDebugger)
                {
                    if (protect.isDebugged())
                    {

                        User.Guard.Admin.BlackList();
                        endClient("Tampering Detected");
                    }
                }
                if (isConnected)
                {
                    if (!protect.isConnected())
                    {

                        endClient();
                    }
                }
                if (doHeartBeat)
                {

                    Heartbeat hb = new Heartbeat(HeartBeatDelay);
                    hb.Beat();
                }
            }
            public static void endClient(string mTitle = "Invalid Exception")
            {
                //INSTANTIATE_PROTECTION_CLASS
                    Protection protect = new Protection();
                
                protect.Terminate(mTitle, "Guard . Dialog");
            }
        }

        public class Projects
        {

            public static string[] List()
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string tempData = "";
                    string[] nullArray = { "null_data" };
                    string[] Resp;
                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", null);
                    wParams.Add("call", "list_projects");

                //GET_PHP_RESPONSE
                    tempData = sWeb.Post(URI, wParams).Trim();

                //SPLIT_GET_AUTHORS
                if (tempData.Contains(";"))
                {

                    Resp = tempData.Split(';');
                    Resp = Resp.Where(w => w != Resp[Resp.Length - 1]).ToArray();
                    return Resp;
                }
                else
                    return nullArray;
            }
            public static bool Exists(string project)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "exists");

                //GET_PHP_RESPONSE
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
            public static string InfoKey(string project)
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "info_key");

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                return Resp;
            }
            public static string Developer(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "developer");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                return Resp;
            }
            public static string Version(string project, string info_key = "root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "version");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                return Resp;
            }
            public static bool Update(string project, string version, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("version", version);
                    wParams.Add("call", "update");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
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
            public static string Description(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "description");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                return Resp;
            }
            public static bool Available(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "available");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
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
            public static bool Public(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "public");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
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
            public static string Download(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "download");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                return Resp;
            }
            public static string Thread(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "thread");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                return Resp;
            }
            public static string Guide(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "guide");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                return Resp;
            }
            public static string Executable(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "executable_name");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                return Resp;
            }
            public static bool Launch_able(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "launch_able");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
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
            public static bool SecureLaunch(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "secure");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
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
            public static string Feed(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "feed");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                return Resp;
            }
            public static int Access(string project, string info_key="root")
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "project");
                    wParams.Add("project", project);
                    wParams.Add("call", "access");
                    wParams.Add("key", info_key);

                //GET_PHP_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();

                //CHECK_MATCH_RESPONSE
                    try
                    {

                        return int.Parse(Resp);
                    }
                    catch (Exception)
                    {

                        Util.Secure.endClient();
                        return 0;
                    }
            }
            public static bool Online()
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //RESPONSE_DATA
                    string Resp = "";

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "util");
                    wParams.Add("call", "online");

                //GET_PHP_RESPONSE
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

            public class Manager
            {

                private bool Validated = false;
                private string dUser = null;

                /*
                 *  DEVELOPER_CONSTRUCTOR
                 *  @function        Validate project developer usage.
                 *  @return          void
                 */
                    public Manager(string user, string pass)
                    {

                        dUser = user;
                        Validated = User.Guard.Login(user, pass, true);
                        if (Validated)
                            Validated = User.Guard.Developer(user);
                    }

                public void Update(string project, string version)
                {

                    if (Validated)
                    {

                        string key = Projects.InfoKey(project);
                        if (dUser.Equals(Projects.Developer(project, key)))
                        {

                            //USE_WEB_METHODS
                                SecureWeb sWeb = new SecureWeb();

                            //REQUEST_CREDENTIALS
                                string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                                NameValueCollection wParams = new NameValueCollection();
                                wParams.Add("class", "project");
                                wParams.Add("project", project);
                                wParams.Add("call", "update");
                                wParams.Add("version", version);

                            //GET_PHP_RESPONSE
                                sWeb.Post(URI, wParams);

                            MessageBox.Show("Project Version Updated Successfully", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                            MessageBox.Show("Insufficient Privileges", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Invalid Access Level", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
                public void Available(string project, bool available)
                {

                    if (Validated)
                    {

                        string key = Projects.InfoKey(project);
                        if (dUser.Equals(Projects.Developer(project, key)))
                        {

                            //USE_WEB_METHODS
                                SecureWeb sWeb = new SecureWeb();

                            //REQUEST_CREDENTIALS
                                string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                                NameValueCollection wParams = new NameValueCollection();
                                wParams.Add("class", "project");
                                wParams.Add("project", project);
                                wParams.Add("call", "available");
                                wParams.Add("available", available.ToString());

                            //GET_PHP_RESPONSE
                                sWeb.Post(URI, wParams);

                            MessageBox.Show("Project Availability Updated Successfully", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                            MessageBox.Show("Insufficient Privileges", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Invalid Access Level", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
                public void Public(string project, bool pub)
                {

                    if (Validated)
                    {

                        string key = Projects.InfoKey(project);
                        if (dUser.Equals(Projects.Developer(project, key)))
                        {

                            //USE_WEB_METHODS
                                SecureWeb sWeb = new SecureWeb();

                            //REQUEST_CREDENTIALS
                                string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                                NameValueCollection wParams = new NameValueCollection();
                                wParams.Add("class", "project");
                                wParams.Add("call", "public");
                                wParams.Add("public", pub.ToString());

                            //GET_PHP_RESPONSE
                                sWeb.Post(URI, wParams);

                            MessageBox.Show("Project Availability Updated Successfully", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                            MessageBox.Show("Insufficient Privileges", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Invalid Access Level", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
                public void Description(string project, string description)
                {

                    if (Validated)
                    {

                        string key = Projects.InfoKey(project);
                        if (dUser.Equals(Projects.Developer(project, key)))
                        {

                            //USE_WEB_METHODS
                                SecureWeb sWeb = new SecureWeb();

                            //REQUEST_CREDENTIALS
                                string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                                NameValueCollection wParams = new NameValueCollection();
                                wParams.Add("class", "project");
                                wParams.Add("project", project);
                                wParams.Add("call", "description");
                                wParams.Add("description", description);

                            //GET_PHP_RESPONSE
                                sWeb.Post(URI, wParams);

                            MessageBox.Show("Project Description Updated Successfully", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                            MessageBox.Show("Insufficient Privileges", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Invalid Access Level", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
                public void Feed(string project, string feed)
                {

                    if (Validated)
                    {

                        string key = Projects.InfoKey(project);
                        if (dUser.Equals(Projects.Developer(project, key)))
                        {

                            //USE_WEB_METHODS
                                SecureWeb sWeb = new SecureWeb();

                            //REQUEST_CREDENTIALS
                                string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                                NameValueCollection wParams = new NameValueCollection();
                                wParams.Add("class", "project");
                                wParams.Add("project", project);
                                wParams.Add("call", "feed");
                                wParams.Add("feed", feed);

                            //GET_PHP_RESPONSE
                                sWeb.Post(URI, wParams);

                            MessageBox.Show("Project Feed Updated Successfully", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                            MessageBox.Show("Insufficient Privileges", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Invalid Access Level", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
                public void Thread(string project, string thread)
                {

                    if (Validated)
                    {

                        string key = Projects.InfoKey(project);
                        if (dUser.Equals(Projects.Developer(project, key)))
                        {

                            //USE_WEB_METHODS
                                SecureWeb sWeb = new SecureWeb();

                            //REQUEST_CREDENTIALS
                                string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                                NameValueCollection wParams = new NameValueCollection();
                                wParams.Add("class", "project");
                                wParams.Add("project", project);
                                wParams.Add("call", "thread");
                                wParams.Add("thread", thread);

                            //GET_PHP_RESPONSE
                                sWeb.Post(URI, wParams);

                            MessageBox.Show("Project Thread Updated Successfully", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                            MessageBox.Show("Insufficient Privileges", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Invalid Access Level", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
                public void Guide(string project, string guide)
                {

                    if (Validated)
                    {

                        string key = Projects.InfoKey(project);
                        if (dUser.Equals(Projects.Developer(project, key)))
                        {

                            //USE_WEB_METHODS
                                SecureWeb sWeb = new SecureWeb();

                            //REQUEST_CREDENTIALS
                                string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                                NameValueCollection wParams = new NameValueCollection();
                                wParams.Add("class", "project");
                                wParams.Add("project", project);
                                wParams.Add("call", "guide");
                                wParams.Add("guide", guide);

                            //GET_PHP_RESPONSE
                                sWeb.Post(URI, wParams);

                            MessageBox.Show("Project Thread Updated Successfully", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                            MessageBox.Show("Insufficient Privileges", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Invalid Access Level", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }
                public void Download(string project, string download)
                {

                    if (Validated)
                    {

                        string key = Projects.InfoKey(project);
                        if (dUser.Equals(Projects.Developer(project, key)))
                        {

                            //USE_WEB_METHODS
                                SecureWeb sWeb = new SecureWeb();

                            //REQUEST_CREDENTIALS
                                string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                                NameValueCollection wParams = new NameValueCollection();
                                wParams.Add("class", "project");
                                wParams.Add("project", project);
                                wParams.Add("call", "download");
                                wParams.Add("download", download);

                            //GET_PHP_RESPONSE
                                sWeb.Post(URI, wParams);

                            MessageBox.Show("Project Download Updated Successfully", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                            MessageBox.Show("Insufficient Privileges", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Invalid Access Level", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return;
                }

                public bool Create(string project, string developer, string executable, string download, string thread="http://neetgroup.net", string version = "1.0.0.0", string description = "", string info_key = "root")
                {

                    if (Validated) {

                        if (!Projects.Exists(project))
                        {
                            //USE_WEB_METHODS
                                SecureWeb sWeb = new SecureWeb();

                            //REQUEST_CREDENTIALS
                                string URI = "https://www.guard.neetgroup.name/admin/api/driver.php";
                                NameValueCollection wParams = new NameValueCollection();
                                wParams.Add("class", "project");
                                wParams.Add("project", project);
                                wParams.Add("call", "create");
                                wParams.Add("developer", developer);
                                wParams.Add("executable", executable);
                                wParams.Add("download", download);
                                wParams.Add("version", version);
                                wParams.Add("description", description);
                                wParams.Add("key", info_key);
                                wParams.Add("thread", thread);

                            //GET_PHP_RESPONSE
                                sWeb.Post(URI, wParams);

                            MessageBox.Show("Project Created Successfully", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        else
                        {

                            MessageBox.Show("Project With This Name Already Exists", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return false;
                        }
                    }
                    else
                        MessageBox.Show("Invalid Access Level", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    return false;
                }
            }
        }
    }
}
