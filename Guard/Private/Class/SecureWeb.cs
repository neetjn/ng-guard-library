using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using System.Web;

namespace Guard.Private
{
    class SecureWeb
    {

        /*
         *  VALIDATE_SSL_REQUESTS
         */
            public void InitiateSSLTrust()
            {
                try
                {
                    //Change SSL checks so that all checks pass
                    ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(
                            delegate
                            { return true; }
                        );
                } catch (Exception) { }
            }
            public bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }
        
        /*
         *  @function       Return posted php data.
         *  @return         string
         */
            public string Post(string URI, NameValueCollection Message)
            {
                try
                {
                    string result = null;
                    using (WebClient wc = new WebClient())
                    {
                        wc.Proxy = null;
                        wc.Credentials = CredentialCache.DefaultCredentials;
                        wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                        wc.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.6; Windows NT 6.1; Win64; x64; Trident/5.0; InfoPath.2; SLCC1; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; .NET CLR 2.0.50727; rv:25.0) Gecko/20100101 Firefox/25.0 Chrome/32.0.1667.0 Safari/537.36");
                        ServicePointManager.ServerCertificateValidationCallback += ValidateServerCertificate;
                        InitiateSSLTrust();
                        byte[] bByte = wc.UploadValues(URI, Message);
                        result = Encoding.UTF8.GetString(bByte);
                        wc.Dispose();
                    }
                    return result;
                }
                catch (Exception) { Util.Secure.endClient("Server Connection Unauthorized"); }
                return " "; //EXCEPTION_PRECAUTION
            } 
    }
}
