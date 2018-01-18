using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Guard.Private
{
    class Heartbeat
    {
        /*
         *  CREATE_POST_KEYS
         *  @function       Creates 'secret_key' used to secure data.
         *  @return         string
         */
            private static string[] post_key()
            {
                //CRYPTOGRAPHY_USE
                    Cryptography cryptgraph = new Cryptography();
                //RANDOM_OBJECT
                    Random rand = new Random();
                //POST_KEY
                    string key = rand.Next(99999).ToString();
                //SECRET_KEY_MD5_CONVERSION
                    string tempKey = (cryptgraph.doHASH("sha1", key.ToString())).ToLower();
                    string secretKey = (cryptgraph.doHASH("md5", tempKey)).ToLower();

                string[] retKeys = new string[] { key, secretKey };
                return retKeys;
            }
        /*
         *  CREATE_SECURE_MESSAGE
         *  @function       ...
         *  @return         string
         */
            private static string secureRet(string str, string key)
            {

                Cryptography cryptgraph = new Cryptography();
                return cryptgraph.doHASH("sha1", str + key); ;
            }

       /*
        *  HEARTBEAT_CONSTRUCTOR 
        *  @function        Set and store heartbeat delay (ms).
        *  @return          void
        */
            public Heartbeat(int seconds)
            {

                Delay = seconds;
            }

        #region HB_INFO

            private bool Active = true;
            public int Delay = 0;
        
        #endregion

        /*
         *  HEARTBEAT_MAIN_RESPONSE
         *  @function       Create & read heartbeat response.
         *  @return         bool
         */
            private bool getBeat()
            {

                //USE_WEB_METHODS
                    SecureWeb sWeb = new SecureWeb();
                //CRYPTOGRAPHY_USE
                    Cryptography cryptgraph = new Cryptography();
                //RESPONSE_DATA
                    string Resp = "";

                //RANDOM_SECRET_KEY<FOR_SERVER_POST>
                    string[] setKeys = post_key();
                    string key = setKeys[0];
                    string secretKey = setKeys[1];

                //REQUEST_CREDENTIALS
                    string URI = "https://www.guard.neetgroup.name/api/driver.php";
                    NameValueCollection wParams = new NameValueCollection();
                    wParams.Add("class", "util");
                    wParams.Add("call", "beat");
                    wParams.Add("key", key);

                //RESPONSE_TYPES
                    string G_HB = secureRet("True", secretKey);
                    string N_HB = secureRet("False", secretKey);

                //SERVER_RESPONSE
                    Resp = sWeb.Post(URI, wParams).Trim();
                    Resp = cryptgraph.qDecode(Resp);

                    if (Resp.Equals(G_HB))
                        return true;
                    else if (Resp.Equals(N_HB))
                        return false;
                    else
                        nullBeat();

                    return false;
            }

        /*
         *  HEARTBEAT_RESPONSE_LOOP
         *  @function       Loop 'getBeat' while module is active.
         *  @return         void
         */
            private void doBeat()
            {
                while (Active)
                {
                    switch (getBeat())
                    {
                        case true:
                            int hbMs = (Delay * 1000);
                            Thread.Sleep(hbMs);
                            break;
                        case false:
                            Active = false;
                            killSwitch eClient = new killSwitch(5);
                            Thread eKick = new Thread(eClient.killClient);
                            eKick.Start();
                            MessageBox.Show("Connection Closed by Server", "Guard . Dialog", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
                            break;
                    }
                }
            }

        /*
         * INVALID_RESPONSE
         * @function        Handle an invalid heartbeat response from the server.
         * @return          void
         */
            private void nullBeat()
            {

                killSwitch eClient = new killSwitch(3);
                Thread eKick = new Thread(eClient.killClient);
                eKick.Start();
                MessageBox.Show("WARNING: Invalid Heartbeat Response", "Guard . Dialog", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
            }

        /*
         *  CREATE_HEARTBEAT
         *  @function       Creates heartbeat thread using 'doBeat'.
         *  @return         void
         */
            public void Beat()
            {
                //HEARTBEAT_THREAD
                    Thread beat = new Thread(doBeat);
                    beat.IsBackground = true;
                    beat.Start();
            }
    }

    class killSwitch
    {

        public int seconds = 0;
        /*
         *  CONSTRUCTOR
         *  @function       Sets timer on kill switch to end host process.
         *  @return         void
         */
            public killSwitch(int seconds)
            {
                this.seconds = seconds;
            }
        
        /*  KILL_SWITCH
         *  @function       Terminates host process using Guard library.
         *  @return         void
         */
            public void killClient()
            {
                if (seconds > 0)
                {
                    int killMs = (seconds * 1000);
                    Thread.Sleep(killMs);
                    Environment.Exit(0);
                }
                else
                {
                    MessageBox.Show("ERR: CODE 10010H", "Guard . Dialog", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
    }
}
