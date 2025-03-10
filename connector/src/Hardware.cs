/*
 * Author: Alessandro de Oliveira Binhara
 * Igloobe Company
 * 
 * This file defines the Hardware abstract class that contains 
 * static configuration properties and utility methods for 
 * hardware device handling in the Igloobe system.
 */
namespace Br.Com.IGloobe.Connector {

    public abstract class Hardware {

        public static bool UseHardlock = false;
        public const string Key = "E84ECEB30334";

        public const string DefaultDeviceName = "Nintendo RVL-CNT-01";
        public static string ReplaceDeviceName = "iGloobe";

        public static bool CheckConnectionHeartbeats = true;
        public static bool ShowConsole;
        public static string CurrentIGlooble;
        //public static List<IDeviceInfo> CachedDevices;

        public static bool IsIgloobeDevice(IDeviceInfo device){
            return device.Name == ReplaceDeviceName;
        }
    }
}
