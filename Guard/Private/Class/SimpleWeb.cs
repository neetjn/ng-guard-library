using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Guard.Private
{
    class SimpleWeb
    {

        /*
         *  @function       Returns web request response as a string.
         *  @return         string
         */
            public string sRecv(string url)
            {

                using (var webClient = new WebClient())
                {

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    var response = webClient.DownloadString(url);
                    return response;
                }
            }
        /*
         *  @function       Returns web request response as boolean.
         *  @return         bool
         */
            public bool bRecv(string url)
            {

                using (var webClient = new WebClient())
                {

                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    var response = webClient.DownloadString(url);
                    try
                    {

                        return bool.Parse(response);
                    }
                    catch (Exception) { return false; }
                }
            }
    }
}