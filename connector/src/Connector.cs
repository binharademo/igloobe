/*
 * Author: Alessandro de Oliveira Binhara
 * Igloobe Company
 * 
 * This file defines the abstract Connector class which serves as the base for hardware 
 * connectivity implementations. It provides methods for device discovery, connection management,
 * and data communication with Igloobe hardware devices.
 */
using System;
using System.Collections.Generic;

namespace Br.Com.IGloobe.Connector {

    public abstract class Connector {

        #region abstract templates

        public abstract bool Connect();
        public abstract List<IDeviceInfo> DiscoverDevices();
        public abstract List<IDeviceInfo> DiscoverInstalledDevices();

        public abstract IRListener IRListener { set; }

        public abstract int SmoothingAmount { get; set; }
        public abstract bool CursorControl { get; set; }
        public abstract DateTime LastHardwareTickTime { get; }

        public abstract void ComputeWarp(CalibrationData data);
        public abstract void Write(int address, byte data);
        public abstract byte[] Read(int address, short size);

        public abstract ConnectionState[] States { get; }

        #endregion

        private static readonly DateTime StartTime = DateTime.Now; 
        
        protected ConnectionState _currentState;
        public ConnectionState CurrentState {
            get { return _currentState; }
            set {
                if (value.Id <= ConnectionState.Success.MaxIndex)
                    LastSuccessefullState = value;

                _currentState = value;
                PromoteTo(_currentState);

                if (_stateListener == null) return;
                _stateListener.StateChanched(_currentState);
            }
        }

        internal static void PromoteTo(ConnectionState state) {
            if (state == null) return;
           
            Log(state.GetType().Name);
            state.Promote();
        }

        public static void Log(string msg) {
            if(Hardware.ShowConsole) 
                ConsoleManager.Show();

            Console.WriteLine(@"********************************************");
            Console.WriteLine(DateTime.Now - StartTime);
            Console.WriteLine(msg);
        }

        private IGloobeStateListener _stateListener;
        public IGloobeStateListener StateListener {
            set {
                value.StateChanched(CurrentState);
                _stateListener = value;
            }
        }

        private ConnectionState _lastSuccessefullState;
        public ConnectionState LastSuccessefullState
        {
            set { 
                _lastSuccessefullState = value;
                Console.WriteLine(@"LastSuccessefullState: " + value.GetType().Name);
            }
            get { return _lastSuccessefullState; }
        }

        private static Type _prototype;
        public static Type Prototype {
            get { return _prototype ?? 
                             (_prototype = Type.GetType("Br.Com.IGloobe.Connector.ConnectorImpl")); }
            set { _prototype = value; }
        }

        public static Connector NewInstance() {
            return (Connector)Prototype.GetConstructor(new Type[0]).Invoke(new object[0]);
        }

        public void Retry() {
            CurrentState = LastSuccessefullState;
        }
    }

    public interface IDeviceInfo {
        string Name { get; }
        object Address { get; }
        string Id { get; }
        void Connect();
        void Disconnect();
        string ToString();
    }

    public abstract class ConnectionState {

        public int Id { get; set; }

        protected ConnectionState(Connector connector, int stateId) {
            Connector = connector;
            Id = stateId;
        }
           
        public class Success {
            public const int NotInitialized = 0;
            public const int SearchingBluetooth = 1;
            public const int BluetoothClientFound = 2;
            public const int ConnectingDevice = 3;
            public const int InitializingMotionCapture = 4;
            public const int ReadyToUse = 5;
            public const int MaxIndex = ReadyToUse;
        }

        public class Working {
            public const int RemovingPreviousSetupData = 6;
            public const int DiscoveringBluetoothDevices = 7;
            public const int SelectingIGloobeDevice = 8;
        }

        public class Fail {
            public const int HasNoDevices = 9;
            public const int NotFoundIgloobeDevice = 10;
            public const int InvalidLicense = 11;
            public const int BluetoothClientNotFound = 12;
            public const int MotionCaptureError = 13;
            public const int ConnectionProblem = 14;
            public const int Descalibrated = 15;
        }

        protected readonly Connector Connector;

        public virtual void Promote() { }

        public void PromoteTo(int stateId){
            Connector.CurrentState = Connector.States[stateId];
        }

        public ConnectionState State(int stateId) {
            return Connector.States[stateId];
        }

        public bool Equals(int stateId){
            return GetType().Equals(Connector.States[stateId]);
        }
    }
}