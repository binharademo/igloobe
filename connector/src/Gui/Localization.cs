using System.Windows.Forms;
using System.Resources;

namespace Br.Com.IGloobe.Connector.Gui {

    public partial class Localization:Form {

        private static readonly ResourceManager ResourceManager = new ResourceManager(typeof(Localization).FullName,
                    System.Reflection.Assembly.GetExecutingAssembly());
        
        public static string BtnMsgCalibrate;
        public static string BtnMsgPleaseWait;
        public static string BtnMsgSearchBluetoothAgain;
        public static string BtnMsgSearchIgloobeAgain;
        public static string BtnMsgExit;

        public static string MsgNotInitialized;
        public static string MsgBluetoothOkSearchingIgloobe;
        public static string MsgReadyToUse;
        public static string MsgDeviceNotFound;
        public static string MsgInvalidLicense;
        public static string MsgConnectionProblem;

        public static string MsgDlgTitle;
        public static string MsgDlgTxt;

        public static string RightClickMouseActivatedMsg;
        public static string RightClickMouseActivatedTitle;
        public static string RightButtonPressed;
        public static string RightButtonReleased;
        public static string ClickText;

        public static void LoadResources() {

            BtnMsgPleaseWait = ResourceManager.GetString("BtnMsgPleaseWait");
            BtnMsgSearchBluetoothAgain = ResourceManager.GetString("BtnMsgSearchBluetoothAgain");
            BtnMsgSearchIgloobeAgain = ResourceManager.GetString("BtnMsgSearchIgloobeAgain");
            BtnMsgExit = ResourceManager.GetString("BtnMsgExit");
            BtnMsgCalibrate = ResourceManager.GetString("BtnMsgCalibrate");

            ClickText = ResourceManager.GetString("MsgClick");

            MsgNotInitialized = ResourceManager.GetString("MsgNotInitialized");
            MsgBluetoothOkSearchingIgloobe = ResourceManager.GetString("MsgBluetoothOkSearchingIgloobe");
            MsgReadyToUse = ResourceManager.GetString("MsgReadyToUse");
            MsgDeviceNotFound = ResourceManager.GetString("MsgDeviceNotFound");
            MsgInvalidLicense = ResourceManager.GetString("MsgInvalidLicense");
            MsgConnectionProblem = ResourceManager.GetString("MsgConnectionProblem");
            MsgDlgTitle = ResourceManager.GetString("MsgDlgTitle");
            MsgDlgTxt = ResourceManager.GetString("MsgDlgTxt");

            RightClickMouseActivatedTitle = ResourceManager.GetString("RightClickMouseActivatedTitle");
            RightClickMouseActivatedMsg = ResourceManager.GetString("RightClickMouseActivatedMsg");
            RightButtonPressed = ResourceManager.GetString("RightButtonPressed");
            RightButtonReleased = ResourceManager.GetString("RightButtonReleased");


        }
    }
}
