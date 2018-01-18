using System;
using System.IO;
using System.Management;

namespace Guard.Private
{
    internal class NGiD
    {
        static Cryptography cryptgraph = new Cryptography();
        
        public static string clientKey()
        {

            string key = cryptgraph.doHASH("ap", cryptgraph.doHASH("sha1", cryptgraph.StringCleaner(cryptgraph.doHASH("sha512", GetHWID())))); //NKEY_UNMODIFIED
            return cryptgraph.qEncode(key); //NKEY_ENCODED
        }


        private static string GetHWID()
        {

            return cryptgraph.doHASH("ripemd160", cryptgraph.doHASH("md5", cryptgraph.SHAEncrypt(GetProcessorID() + GetMotherBoardID() + GetHardDriveID() + GetBiosAndVideoID()))).ToUpper();
        }

        private static string GetBiosAndVideoID()
        {

            return GetBiosID() + GetVideoID();
        }
        private static string GetProcessorID()
        {
            string processorID = "";
            var searcher = new ManagementObjectSearcher(
                            "Select * FROM WIN32_Processor");

            using (var mObject = searcher.Get())
            {
                foreach (ManagementObject obj in mObject)
                {
                    processorID = obj["ProcessorId"].ToString();
                }
            }

            return processorID;
        }
        private static string GetHardDriveID()
        {
            String MainDrive = string.Empty;
            foreach (var Drive in DriveInfo.GetDrives())
            {
                if (!Drive.IsReady)
                    continue;

                MainDrive = Drive.RootDirectory.ToString();
                break;
            }
            if (MainDrive.EndsWith(":\\"))
                MainDrive = MainDrive.Substring(0, MainDrive.Length - 2);

            var disk = new ManagementObject(String.Format(@"win32_logicaldisk.deviceid=""{0}:""", MainDrive));
            disk.Get();

            string volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();

            return volumeSerial;
        }
        private static string GetMotherBoardID()
        {
            String serial = "";
            try
            {
                var mos = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
                using (var moc = mos.Get())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        serial = mo["SerialNumber"].ToString();
                    }
                }
                return serial;
            }
            catch (Exception) { return ""; }
        }
        private static string GetBiosID()
        {
            return GetID("Win32_BIOS", "SMBIOSBIOSVersion") + GetID("Win32_BIOS", "SerialNumber");
        }
        private static string GetVideoID()
        {
            return GetID("Win32_VideoController", "DriverVersion");
        }
        private static string GetID(string wmiClass, string wmiProperty)
        {
            string result = "";
            var mc = new ManagementClass(wmiClass);
            using (var moc = mc.GetInstances())
            {
                foreach (ManagementObject mo in moc)
                {
                    if (String.Compare(result, "", false) != 0)
                        continue;

                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                        result = "";
                        break;
                    }
                }
            }
            return result;
        }
    
    }
}