using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using InTheHand.Net;
using System.Threading;
using System;
using System.Collections.Generic;

namespace Br.Com.IGloobe.Connector.Windows{
    public class ConnectorImpl : Connector {

        private static readonly MotionCapture MotionCapture = new MotionCapture();

        private const int MaxDevices = 50;
        private const int ScanTime = 5;
        private bool _invalidLicenseFounded;

    #region State Pattern

        public abstract class ConnectionStateImpl : ConnectionState {

            protected ConnectionStateImpl(Connector connector, int stateId) : base(connector, stateId) { }

            protected bool IsBlueToothWorking() {
                try {
                    BluetoothRadio.PrimaryRadio.Mode = RadioMode.Connectable;
                    new BluetoothClient();
                    return true;
                } catch {
                    return false;
                }
            }
        }

        internal class NotInitialized : ConnectionStateImpl {
            internal NotInitialized(Connector connector) : base(connector, Success.NotInitialized) { }
        }
        internal class SearchingBluetooth : ConnectionStateImpl {
            public SearchingBluetooth(Connector connector) : base(connector, Success.SearchingBluetooth) { }
            public override void Promote() {

                if (Connector.Connect()) {
                    new Thread(() => PromoteTo(Success.BluetoothClientFound)).Start();
                    return;
                }
                PromoteTo(Fail.BluetoothClientNotFound);
            }
        }
        internal class BluetoothClientFound : ConnectionStateImpl {
            internal BluetoothClientFound(Connector connector) : base(connector, Success.BluetoothClientFound)  {}
            public override void Promote() {
                PromoteTo(Working.DiscoveringBluetoothDevices);
            }
        }      
        internal class ConnectingDevice : ConnectionStateImpl {
            internal ConnectingDevice(Connector connector) : base(connector, Success.ConnectingDevice) { }

            public override void Promote() {
                IDeviceInfo device = ((ConnectorImpl)Connector)._device;
                Console.WriteLine(@"Connecting Device: " + device);
                device.Connect();
                PromoteTo(Success.InitializingMotionCapture);
            }
        }
        internal class InitializingMotionCapture : ConnectionStateImpl {
            
            private const int MaxRetry = 10;
            private const int SleepTime = 1000;

            internal InitializingMotionCapture(Connector connector) : base(connector, Success.InitializingMotionCapture) { }

            public override void Promote() {
                IDeviceInfo device = ((ConnectorImpl)Connector)._device;

                int timerCounter = 0;
                while (timerCounter < MaxRetry) {

                    if (MotionCapture.Connect()) {
                        PromoteTo(Success.ReadyToUse);
                        return;
                    }

                    if (IsBlueToothWorking()) {
                        Thread.Sleep(SleepTime);
                        timerCounter++;
                        continue;
                    }
                    PromoteTo(Fail.BluetoothClientNotFound);
                }

                device.Disconnect();
                Thread.Sleep(SleepTime);
                PromoteTo(Fail.MotionCaptureError);
            }
        }
        internal class ReadyToUse : ConnectionStateImpl {
            internal ReadyToUse(Connector connector) : base(connector, Success.ReadyToUse) { }
        }

        internal class RemovingPreviousSetupData : ConnectionStateImpl {
            internal RemovingPreviousSetupData(Connector connector) : base(connector, Working.RemovingPreviousSetupData) { }
            public override void Promote() {
                List<IDeviceInfo> devices = Connector.DiscoverInstalledDevices();
                foreach (IDeviceInfo device in devices) {
                    if (device.Name != Hardware.ReplaceDeviceName)
                        continue;
                    UninstallDevice(device);
                }
            }

            private static void UninstallDevice(IDeviceInfo device) {
                Console.WriteLine(@"Removing Device: " + device);
                BluetoothSecurity.RemoveDevice((BluetoothAddress)device.Address);
            }
        }
        internal class DiscoveringBluetoothDevices : ConnectionStateImpl {
            internal DiscoveringBluetoothDevices(Connector connector) : base(connector, Working.DiscoveringBluetoothDevices) { }
            public override void Promote() {
                ConnectorImpl connectorImpl = (ConnectorImpl)Connector;
                connectorImpl._devices = connectorImpl.DiscoverInstalledDevices();
                if (connectorImpl._devices.Count == 0)  
                    connectorImpl._devices = connectorImpl.DiscoverDevices();               

                if (connectorImpl._devices.Count == 0) {
                    if (!IsBlueToothWorking()) {
                        PromoteTo(Fail.BluetoothClientNotFound);
                        return;
                    }

                    PromoteTo(Fail.HasNoDevices);
                    return;
                }
                PromoteTo(Working.SelectingIGloobeDevice);
            }
        }
        internal class SelectingIGloobeDevice : ConnectionStateImpl {
            internal SelectingIGloobeDevice(Connector connector) : base(connector, Working.SelectingIGloobeDevice) { }

            public override void Promote() {
                bool invalidLicenseFounded = false;
                List<IDeviceInfo> deviceInfos = ((ConnectorImpl)Connector)._devices;
                foreach (IDeviceInfo device in deviceInfos) {

                    Console.WriteLine(@"*** Hardlock Check [" + (Hardware.UseHardlock ? "ON" : "OFF") + @"]: " + device.Name);
                    Console.WriteLine(@"Device Address: " + device.Address);

                    if (!Hardware.UseHardlock || CheckLicense(device)) {
                        Console.WriteLine(@"      Hardlock Ok ");
                        if (device.Name == Hardware.ReplaceDeviceName) {
                            Console.WriteLine(@"      Name Ok ");
                            ((ConnectorImpl)Connector)._device = device;
                            //Hardware.CachedDevices = deviceInfos;
                            PromoteTo(Success.ConnectingDevice);
                            return;
                        }

                        if (!Hardware.UseHardlock) continue;

                        if (!IsBlueToothWorking()) {
                            PromoteTo(Fail.BluetoothClientNotFound);
                            return;
                        }
                        PromoteTo(Fail.NotFoundIgloobeDevice);
                        return;
                    }

                    if (Hardware.IsIgloobeDevice(device))
                        invalidLicenseFounded = true;
                }

                if (invalidLicenseFounded) {
                    ConnectorImpl con = (ConnectorImpl)Connector;
                    if (con._invalidLicenseFounded){
                        PromoteTo(Fail.InvalidLicense);
                        return;
                    }
                    con._invalidLicenseFounded = true;
                    PromoteTo(Working.RemovingPreviousSetupData);
                    PromoteTo(Working.DiscoveringBluetoothDevices);
                    return;
                }

                if (!IsBlueToothWorking()) {
                    PromoteTo(Fail.BluetoothClientNotFound);
                    return;
                }

                PromoteTo(Fail.NotFoundIgloobeDevice);
            }

            private static bool CheckLicense(IDeviceInfo info) {
                return Hardware.Key.Equals(info.Address.ToString());
            }
        }      

        internal class HasNoDevices : ConnectionStateImpl {
            internal HasNoDevices(Connector connector) : base(connector, Fail.HasNoDevices) { }
        }
        internal class NotFoundIgloobeDevice : ConnectionStateImpl {
            internal NotFoundIgloobeDevice(Connector connector) : base(connector, Fail.NotFoundIgloobeDevice) { }
        }
        internal class InvalidLicense : ConnectionStateImpl {
            internal InvalidLicense(Connector connector) : base(connector, Fail.InvalidLicense) { }
        }
        internal class BluetoothClientNotFound : ConnectionStateImpl {
            internal BluetoothClientNotFound(Connector connector) : base(connector, Fail.BluetoothClientNotFound) { }
        }
        internal class MotionCaptureError : ConnectionStateImpl {
            internal MotionCaptureError(Connector connector) : base(connector, Fail.MotionCaptureError) { }
            public override void Promote() {
                Connector.LastSuccessefullState = Connector.States[Success.BluetoothClientFound];
                PromoteTo(Working.RemovingPreviousSetupData);
                PromoteTo(Working.DiscoveringBluetoothDevices);
            }
        }
        internal class ConnectionProblem : ConnectionStateImpl {
            internal ConnectionProblem(Connector connector) : base(connector, Fail.ConnectionProblem) { }
        }
        
        internal class Descalibrated : ConnectionStateImpl {
            internal Descalibrated(Connector connector) : base(connector, Fail.Descalibrated) { }
        }

        public override ConnectionState[] States {
             get{  return new ConnectionState[] {
                    new NotInitialized(this),
                    new SearchingBluetooth(this),
                    new BluetoothClientFound(this),
                    new ConnectingDevice(this),
                    new InitializingMotionCapture(this),
                    new ReadyToUse(this),

                    new RemovingPreviousSetupData(this),
                    new DiscoveringBluetoothDevices(this),
                    new SelectingIGloobeDevice(this),

                    new HasNoDevices(this),
                    new NotFoundIgloobeDevice(this),
                    new InvalidLicense(this),
                    new BluetoothClientNotFound(this),
                    new MotionCaptureError(this),
                    new ConnectionProblem(this),
                    new Descalibrated(this)
                };
             }
        }

        #endregion

        private BluetoothClient _client;
        private List<IDeviceInfo> _devices;
        private IDeviceInfo _device;

        public override IRListener IRListener {
            set { MotionCapture.IRListener(value); }
        }
        public override int SmoothingAmount {
            get { return MotionCapture.SmoothingAmount; }
            set { MotionCapture.SmoothingAmount = value; }
        }
        public override bool CursorControl {
            get { return MotionCapture.CursorControl; }
            set { MotionCapture.CursorControl = value; }
        }
        public override DateTime LastHardwareTickTime {
            get { return MotionCapture.LastHardwareTickTime(); }
        }

        public ConnectorImpl() {
            CurrentState = new NotInitialized(this);
        }

        public override bool Connect() {
            try {
                BluetoothRadio.PrimaryRadio.Mode = RadioMode.Connectable;

                if (_client != null) {
                    _client.Close();
                    _client.Dispose();
                }
                _client = new BluetoothClient();
                Console.WriteLine(@"Connect: new BluetoothClient();");
                return true;
            } catch {
                Console.WriteLine(@"ERROR (Connect): new BluetoothClient();");
                return false;
            }
        }
        public override void ComputeWarp(CalibrationData data) {
            MotionCapture.ScreenSize(data.Width, data.Height);
            MotionCapture.ComputeWarp(data.DstX, data.DstY, data.SrcX, data.SrcY);
        }
        public override void Write(int address, byte data) { MotionCapture.Write(address, data); }
        public override byte[] Read(int address, short size) { return MotionCapture.Read(address, size); }

        public override List<IDeviceInfo> DiscoverDevices() {
            //if (Hardware.CachedDevices != null)
            //    return Hardware.CachedDevices;

            Console.WriteLine(@"Discovering Devices...");
            BluetoothDeviceInfo[] devices = _client.DiscoverDevices(MaxDevices, ScanTime);
            Console.WriteLine(devices.Length + @" Devices Found.");
            return Adapt(devices);
        }

        public override List<IDeviceInfo> DiscoverInstalledDevices() {
            Console.WriteLine(@"Discovering Installed Devices...");
            BluetoothDeviceInfo[] devices = _client.DiscoverDevices(MaxDevices, false, true, false);
            Console.WriteLine(devices.Length + @" Installed Devices Found.");
           return Adapt(devices);
        }

        private static List<IDeviceInfo> Adapt(IEnumerable<BluetoothDeviceInfo> devices) {
            List<IDeviceInfo> result = new List<IDeviceInfo>();
            foreach (BluetoothDeviceInfo info in devices) 
                result.Add(new DeviceInfo(info));          
            return result;
        }
    }

    internal class DeviceInfo: IDeviceInfo {

        private readonly BluetoothDeviceInfo _adaptee;

        internal DeviceInfo(BluetoothDeviceInfo info) {
            _adaptee = info;
        }

        public string Name {
            get {
                if (_adaptee.DeviceName == Hardware.DefaultDeviceName)
                    return Hardware.ReplaceDeviceName;
                return _adaptee.DeviceName;
            }
        }

        public object Address {
            get { return _adaptee.DeviceAddress; }
        }

        public string Id {
            get { return "" + _adaptee.DeviceAddress; }
        }

        public void Connect() {
            _adaptee.SetServiceState(BluetoothService.HumanInterfaceDevice, true);
         }

        public void Disconnect() {
            _adaptee.SetServiceState(BluetoothService.HumanInterfaceDevice, false);
        }

        public override string ToString(){
            return Name + @" [" + Address + @"]";
        }
    }
}