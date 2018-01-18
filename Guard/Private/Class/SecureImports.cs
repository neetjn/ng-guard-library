using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Guard.Private
{

    class SecureImports
    {

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr LoadLibraryA([In, MarshalAs(UnmanagedType.LPStr)] string lpFileName);
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        delegate int NTInfo(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);
        delegate bool isDebugger();
        delegate bool ICS(ref ConnectionState lpdwFlags, int dwReserved);
        delegate bool SetForWindow(int hWnd);

        public static T CreateAPI<T>(string name, string method)
        {
            return (T)(object)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LoadLibraryA(name), method), typeof(T));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
        public static void hProtectFromExternalProcesses(int Action) //IMPORT_NtSetInformationProcess
        {
            NTInfo NUVS = CreateAPI<NTInfo>("ntdll", "NtSetInformationProcess");
            NUVS(Process.GetCurrentProcess().Handle, 29, ref Action, sizeof(int));
        }

        //DEBUGGER_DLL
        public static bool hIsDebuggerP() //IMPORT_DEBUGGER_PRESENT
        {
            isDebugger IsD = CreateAPI<isDebugger>("kernel32", "IsDebuggerPresent");
            return IsD();
        }
        public static bool hSetForegroundWindow(int hWnd) //IMPORT_DEBUGGER_PRESENT
        {
            SetForWindow IsD = CreateAPI<SetForWindow>("User32", "SetForegroundWindow");
            return IsD(hWnd);
        }


        [Flags]
        public enum ConnectionState : int
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }

        public static bool hInternetCS() //IMPORT_DEBUGGER_PRESENT
        {
            ICS ICS = CreateAPI<ICS>("wininet", "InternetGetConnectedState");
            ConnectionState Description = 0;
            return ICS(ref Description, 0);
        }

    }
}
