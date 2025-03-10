using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using InTheHand.Net;
using System.Threading;
using System;
using System.Collections.Generic;

namespace Br.Com.IGloobe.Connector{
    public class ConnectorImpl : Connector {

        private static readonly MotionCapture MotionCapture = new MotionCapture();
        private static readonly DateTime StartTime = DateTime.Now;

        private const int MaxDevices = 50;
        private const int ScanTime = 10;

        #region State Pattern

        public abstract class ConnectionStateImpl : ConnectionState {

            internal ConnectionStateImpl(Connector connector) : base(connector) {}

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
            internal NotInitialized(Connector connector) : base(connector) { }
        }
        internal class SearchingBluetooth : ConnectionStateImpl {
            public SearchingBluetooth(Connector connector) : base(connector) { }
            public override void Promote() {
                if (Connector.Connect())
                    Connector.CurrentState = new BluetoothClientFound(Connector);
                Connector.CurrentState = new BluetoothClientNotFound(Connector);
            }
        }
        internal class BluetoothClientFound : ConnectionStateImpl{
            internal BluetoothClientFound(Connector connector) : base(connector) { }
            public override void Promote() {
                Connector.CurrentState = new RemovingPreviousSetupData(Connector);
                Connector.CurrentState = new DiscoveringBluetoothDevices(Connector);
            }
        }
        internal class RemovingPreviousSetupData : ConnectionStateImpl{
            internal RemovingPreviousSetupData(Connector connector) : base(connector) { }
            public override void Promote() {
                List<IDeviceInfo> devices = Connector.DiscoverInstalledDevices();
                foreach (IDeviceInfo device in devices) {
                    if (device.Name != Hardware.DefaultDeviceName)
                        continue;
                    UninstallDevice(device);
                }
            }

            private static void UninstallDevice(IDeviceInfo device) {
                BluetoothSecurity.RemoveDevice((BluetoothAddress)device.Address);
            }
        }
        internal class DiscoveringBluetoothDevices : ConnectionStateImpl {
            internal DiscoveringBluetoothDevices(Connector connector) : base(connector) { }
            public override void Promote() {
                ConnectorImpl connectorImpl = (ConnectorImpl)Connector;
                connectorImpl._devices = connectorImpl.DiscoverDevices();

                if (connectorImpl._devices.Count == 0) {
                    if (!IsBlueToothWorking()) {
                        Connector.CurrentState = new BluetoothClientNotFound(Connector);
                        return;
                    }

                    Connector.CurrentState = new HasNoDevices(Connector);
                    return;
                }

                Connector.CurrentState = new SelectingIGloobeDevice(Connector);
            }
        }
        internal class SelectingIGloobeDevice : ConnectionStateImpl {
            internal SelectingIGloobeDevice(Connector connector) : base(connector) { }

            public override void Promote() {
                bool invalidLicenseFounded = false;
                foreach (IDeviceInfo device in ((ConnectorImpl)Connector)._devices) {

                    if (!Hardware.UseHardlock || CheckLicense(device)) {
                        if (device.Name == Hardware.DefaultDeviceName) {
                            ((ConnectorImpl) Connector)._device = device;
                            Connector.CurrentState = new ConnectingDevice(Connector);
                            return;
                        }

                        if (!Hardware.UseHardlock) continue;

                        if (!IsBlueToothWorking()) {
                            Connector.CurrentState = new BluetoothClientNotFound(Connector);
                            return;
                        }
                        Connector.CurrentState = new BluetoothSoError(Connector);
                        return;
                    }

                    if (device.Name == Hardware.DefaultDeviceName)
                        invalidLicenseFounded = true;
                }

                if (invalidLicenseFounded) {
                    Connector.CurrentState = new InvalidLicense(Connector);
                    return;
                }

                if (!IsBlueToothWorking()) {
                    Connector.CurrentState = new BluetoothClientNotFound(Connector);
                    return;
                }

                Connector.CurrentState = new NotFoundIgloobeDevice(Connector);
            }

            private static bool CheckLicense(IDeviceInfo info) {
                return  Hardware.Key.Equals(info.Address);
            }
        }
        internal class ConnectingDevice : ConnectionStateImpl {
            internal ConnectingDevice(Connector connector) : base(connector) { }

            public override void Promote() {
                ((ConnectorImpl)Connector)._device.Connect(); 
                Connector.CurrentState = new InitializingMotionCapture(Connector);
            }
        }
        internal class InitializingMotionCapture : ConnectionStateImpl {
            
            private const int MaxRetry = 4*20;
            private const int SleepTime = 250;

            internal InitializingMotionCapture(Connector connector) : base(connector) { }

            public override void Promote() {

                int timerCounter = 0;
                while (timerCounter < MaxRetry) {

                    if (MotionCapture.Connect()) {
                        Connector.CurrentState = new ReadyToUse(Connector);
                        return;
                    }

                    if (IsBlueToothWorking()) {
                        Thread.Sleep(SleepTime);
                        timerCounter++;
                        continue;
                    }

                    Connector.CurrentState = new BluetoothClientNotFound(Connector);
                }

                Connector.CurrentState = new MotionCaptureError(Connector);
            }
        }
        internal class ReadyToUse : ConnectionStateImpl {
            internal ReadyToUse(Connector connector) : base(connector) { }
        }

        internal class HasNoDevices : ConnectionStateImpl {
            internal HasNoDevices(Connector connector) : base(connector) { }
        }
        internal class NotFoundIgloobeDevice : ConnectionStateImpl{
            internal NotFoundIgloobeDevice(Connector connector) : base(connector) { }
        }
        internal class InvalidLicense : ConnectionStateImpl{
            internal InvalidLicense(Connector connector) : base(connector) { }
        }
        internal class BluetoothSoError : ConnectionStateImpl{
            internal BluetoothSoError(Connector connector) : base(connector) { }
        }
        internal class BluetoothClientNotFound : ConnectionStateImpl{
            internal BluetoothClientNotFound(Connector connector) : base(connector) { }
        }
        internal class MotionCaptureError : ConnectionStateImpl{
            internal MotionCaptureError(Connector connector) : base(connector) { }
        }
        internal class ConnectionProblem : ConnectionStateImpl {
            internal ConnectionProblem(Connector connector) : base(connector) { }
        }
        #endregion

        public override Type[] ConnectionStateTypes {
             get{  return new[] {
                    typeof (NotInitialized),
                    typeof (SearchingBluetooth),
                    typeof (BluetoothClientFound),
                    typeof (RemovingPreviousSetupData),
                    typeof (DiscoveringBluetoothDevices),
                    typeof (SelectingIGloobeDevice),
                    typeof (ConnectingDevice),
                    typeof (InitializingMotionCapture),
                    typeof (ReadyToUse),

                    typeof (HasNoDevices),
                    typeof (NotFoundIgloobeDevice),
                    typeof (InvalidLicense),
                    typeof (BluetoothSoError),
                    typeof (BluetoothClientNotFound),
                    typeof (MotionCaptureError),
                    typeof (ConnectionProblem)
                };
             }
        }

        private BluetoothClient _client;
        private ConnectionState _currentState;
        private IGloobeStateListener _stateListener;

        private List<IDeviceInfo> _devices;
        private IDeviceInfo _device;

        public override sealed ConnectionState CurrentState {
            get { return _currentState; }
            set {
                _currentState = value;
                new Thread(()=> PromoteTo(value)).Start();

                if (_stateListener == null) return;
                _stateListener.StateChanched(_currentState);
            }
        }

        internal static void PromoteTo(ConnectionState state) {
            if (state == null) return;

            string name = state.GetType().Name;
            Thread.CurrentThread.Name = name;
            Console.WriteLine(@"********************************************");
            Console.WriteLine(DateTime.Now - StartTime);
            Console.WriteLine(name);
            state.Promote();
        }

        public override sealed IGloobeStateListener StateListener {
            set {
                value.StateChanched(_currentState);
                _stateListener = value;
            }
        }    
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
                return true;
            } catch {
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
           BluetoothDeviceInfo[] devices = _client.DiscoverDevices(MaxDevices, ScanTime);
            return Adapt(devices);
        }

        public override List<IDeviceInfo> DiscoverInstalledDevices() {
            BluetoothDeviceInfo[] devices = _client.DiscoverDevices(MaxDevices, false, true, false);
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
            get { return _adaptee.DeviceName; }
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
    }
}