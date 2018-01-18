using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace Guard.Private
{
    class Protection
    {
        internal class Anti
        {

            public static bool IsDebuggerAttached()
            {
                return SecureImports.hIsDebuggerP() || Debugger.IsAttached || Debugger.IsLogging() ? true : false;
            }

            public static bool HostFileCheck(string p_dnsName)
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
                string hostsText = File.ReadAllText(path);
                return hostsText.ToLower().Contains(p_dnsName.ToLower());
            }
        }
        private static String sClosingTitle, sClosingBody;
        public static bool bClosing;

#region Public

        public bool isDebugged()
        {

            return Anti.IsDebuggerAttached();
        }
        public bool isConnected()
        {

            return NetworkInterface.GetIsNetworkAvailable();
        }
        public void ProtectFromExternalProcesses(int Action)
        {

            SecureImports.hProtectFromExternalProcesses(Action);
        }
        public void Terminate(string mBody, string mTitle)
        {
            if (!bClosing)
            {

                sClosingTitle = mTitle;
                sClosingBody = mBody;
                var vTerminateThread = new Thread(TerminateThread) { IsBackground = true };
                vTerminateThread.Start();
            }
        }
        private void TerminateThread()
        {

            bClosing = true;
            var vShowMessageThread = new Thread(ShowMessageThread) { IsBackground = true };
            vShowMessageThread.Start();
            System.Threading.Thread.Sleep(2500);
            ForceClose();
        }

#endregion Public

#region Private

        private void ShowMessageThread()
        {
            int i = 0;
            MessageBox.Show(sClosingBody, sClosingTitle, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            i++; //ONE_MESSAGEBOX
        }
        private void ForceClose()
        {
            try
            {

                Application.Exit();
            }
            catch
            {
                try
                {

                    Environment.Exit(0);
                }
                catch
                {
                    try
                    {

                        Process.GetCurrentProcess().Kill();
                    }
                    catch
                    {

                        Marshal.WriteByte(IntPtr.Zero, 0);
                    }
                }
            }
        }

#endregion Private

    }
}
