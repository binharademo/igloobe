/*
 * Author: Alessandro de Oliveira Binhara
 * Igloobe Company
 * 
 * This file implements the MotionCapture class that handles motion tracking, 
 * coordinates warping, and smoothing for the Igloobe system. It processes 
 * input from motion capture devices and provides cursor control functionality.
 */
using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using Br.Com.IGloobe.Connector.Mote;
using Br.Com.IGloobe.Connector.Commands;

namespace Br.Com.IGloobe.Connector{

    public class MotionCapture {

        private const int SmoothingBufferSize = 50;

        private readonly Mutex _mutex;
        private readonly Warper _warper = new Warper();
        private readonly PointF[] _smoothingBuffer = new PointF[SmoothingBufferSize];

        private IGloobeMote _wmote;
        private IrState _lastIRState;
        private IrState _irState;

        private bool _warpComputed;
        private int _smoothingBufferIndex;        
        private int _smoothingAmount = 20;
        private int _width = 1440;
        private int _height = 900;
        private bool _lighs;

        private IRListener _irListener;
        private DateTime _lastHardwareTickTime;

        //private Input[] _lastMouseDown;
        //private long _lastMouseDownTime;
        //private const int DoubleClickDeltaError = 5;
        //private const int DoubleClickDeltaTime = 1000000;

        public MotionCapture() {
            _mutex = new Mutex();
        }

        public DateTime LastHardwareTickTime() {
            return _lastHardwareTickTime;
        }

        public void ScreenSize(int width, int height){
        	_width = width;
        	_height = height;
        }
        
        public int SmoothingAmount{
            get {return _smoothingAmount;}
            set {_smoothingAmount = value;}
        }

        public bool CursorControl { get; set; }

        public void IRListener(IRListener listener) {
            listener.StateChanged(_lighs, _lastIRState);
            _irListener = listener;
        }

        private bool LightsOn {
            set {
                if(_lighs == value) return;
                _lighs = value;
                _irListener.StateChanged(_lighs, _irState);
            }      
        }
        
        #region Dll binding ***********************

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        public const int InputMouse = 0;
        public const int MouseMove = 0x01;
        public const int MouseAbsolute = 0x8000;
        public const int MouseLeftdown = 0x02;
        public const int MouseLeftup = 0x04;
        public const int MouseRightdown = 0x08;
        public const int MouseRightup = 0x10;
        public const int MouseMiddledown = 0x20;
        public const int MouseMiddleup = 0x40;


        public struct Input {
            public int Type; 
// ReSharper disable MemberHidesStaticFromOuterClass
            public MouseInput MouseInput; 
// ReSharper restore MemberHidesStaticFromOuterClass
        }

        public struct MouseInput {
            public int Dx;
            public int Dy;
            public uint MouseData;
            public uint DwFlags;
            public uint Time;
            public IntPtr DwExtraInfo;
        }

        #endregion ********************************

        public bool Connect() {
            Console.WriteLine(@"Trying connect IGloobeMote...");
            _wmote = new IGloobeMote();

            if(!_wmote.Connect()) {
                _wmote.Dispose();
                _wmote = null;
                return false;
            }

            _wmote.StateChanged += OnStateChanged;
            _wmote.SetReportType(IGloobeMote.InputReport.IRAccel, true);
            _wmote.SetLeds(true, true, true, true);
            return true;
        }

        #region IGloobeMote Listeners

        //void OnWiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs args) {
        //    _wmote.SetReportType(args.Inserted ? Mote.IGloobeMote.InputReport.IRExtensionAccel : 
        //                                                                Mote.IGloobeMote.InputReport.IRAccel, true);
        //}

        void OnStateChanged(object sender, StateChangedEventArgs args) {
            try {
                _mutex.WaitOne();
                _irState = args.MotionState.IrState;

                if(args.MotionState.ButtonState.One || args.MotionState.ButtonState.Two)
                    _lastHardwareTickTime = DateTime.Now;

                LightsOn = _irState.Found1;

                if(!CursorControl) return;
                if(!_warpComputed) return;

                int x = _irState.RawX1;
                int y = _irState.RawY1;

                float warpedX = x;
                float warpedY = y;
                _warper.Warp(x, y, ref warpedX, ref warpedY);

                _smoothingBuffer[_smoothingBufferIndex % SmoothingBufferSize].X = warpedX;
                _smoothingBuffer[_smoothingBufferIndex % SmoothingBufferSize].Y = warpedY;
                _smoothingBufferIndex++;

                if(!_lastIRState.Found1 && _irState.Found1) {
                    if(RightClick.IsRightClick()) {
                        MouseRightDown(warpedX, warpedY);
                        return;
                    }

                    MouseLeftDown(warpedX, warpedY);
                    return;
                }

                if(!_irState.Found1 && _lastIRState.Found1) {
                    if(RightClick.IsRightClick()) {
                        RightClick.Released();
                        MouseRightUp();
                        return;
                    }

                    MouseLeftUp();
                    return;
                }

                if(!_irState.Found1) return;    
                MouseDrag(warpedX, warpedY);
            } finally {
                _lastIRState = _irState;
                _mutex.ReleaseMutex();
            }
        }

        #endregion;

        private void MouseLeftDown(float warpedX, float warpedY) { MouseDown(warpedX, warpedY, MouseLeftdown);}
        private void MouseRightDown(float warpedX, float warpedY) { MouseDown(warpedX, warpedY, MouseRightdown); }
        private void MouseDown(float warpedX, float warpedY, uint mouseEvent)
        {
            _smoothingBufferIndex = 0;

            Input[] buffer = new Input[1];
            buffer[0].Type = InputMouse;
            buffer[0].MouseInput.Dx = (int) (warpedX*65535.0f/_width);
            buffer[0].MouseInput.Dy = (int) (warpedY*65535.0f/_height);
            buffer[0].MouseInput.MouseData = 0;
            buffer[0].MouseInput.DwFlags = MouseAbsolute | MouseMove;
            buffer[0].MouseInput.Time = 0;
            buffer[0].MouseInput.DwExtraInfo = (IntPtr) 0;
            SendInput((uint) buffer.Length, buffer, Marshal.SizeOf(buffer[0]));

            buffer[0].Type = InputMouse;
            buffer[0].MouseInput.Dx = (int) (warpedX*65535.0f/_width);
            buffer[0].MouseInput.Dy = (int) (warpedY*65535.0f/_height);
            buffer[0].MouseInput.MouseData = 0;
            buffer[0].MouseInput.DwFlags = mouseEvent;
            buffer[0].MouseInput.Time = 1;
            buffer[0].MouseInput.DwExtraInfo = (IntPtr) 0;

            //if (IsValidDoubleClickTime()){
                //new Thread(Log).Start();
                //Console.WriteLine(@"IsValidDoubleClickTime...");
               //if (IsValidDoubleClickMousePosition(buffer)) {
               //    Console.WriteLine(@"IsValidDoubleClickMousePosition...");
                   //buffer = _lastMouseDown;                   
               //}
            //}

            //_lastMouseDown = buffer;
            //_lastMouseDownTime = DateTime.Now.ToFileTimeUtc();
            SendInput((uint)buffer.Length, buffer, Marshal.SizeOf(buffer[0]));
        }

        //private void Log() {
        //    Console.WriteLine(@"IsValidDoubleClickTime...");
        //}

        //private bool IsValidDoubleClickTime() {
       //     long delta = DateTime.Now.ToFileTimeUtc() - _lastMouseDownTime;
        //    return delta <= _lastMouseDownTime + DoubleClickDeltaTime;
        //}

        //private bool IsValidDoubleClickMousePosition(Input[] buffer)  {
        //    return IsDoubleClickAxe(_lastMouseDown[0].MouseInput.Dx, buffer[0].MouseInput.Dx)
         //          && IsDoubleClickAxe(_lastMouseDown[0].MouseInput.Dy, buffer[0].MouseInput.Dy);
        //}

        //private static bool IsDoubleClickAxe(int lastPosition, int currentPosition) {
        //    return lastPosition + DoubleClickDeltaError > currentPosition && lastPosition - DoubleClickDeltaError < currentPosition;
        //}

        private void MouseLeftUp() { MouseUp(MouseLeftup); }
        private void MouseRightUp() { MouseUp(MouseRightup); }
        private void MouseUp(uint mouseEvent) {

            _smoothingBufferIndex = 0;

            Input[] buffer = new Input[1];
            buffer[0].Type = InputMouse;
            buffer[0].MouseInput.Dx = 0;
            buffer[0].MouseInput.Dy = 0;
            buffer[0].MouseInput.MouseData = 0;
            buffer[0].MouseInput.DwFlags = mouseEvent;
            buffer[0].MouseInput.Time = 0;
            buffer[0].MouseInput.DwExtraInfo = (IntPtr)0;
            SendInput((uint)buffer.Length, buffer, Marshal.SizeOf(buffer[0]));

            buffer = new Input[1];
            buffer[0].Type = InputMouse;
            buffer[0].MouseInput.Dx = 0;
            buffer[0].MouseInput.Dy = 0;
            buffer[0].MouseInput.MouseData = 0;
            buffer[0].MouseInput.DwFlags = MouseMove;
            buffer[0].MouseInput.Time = 0;
            buffer[0].MouseInput.DwExtraInfo = (IntPtr)0;
            SendInput((uint)buffer.Length, buffer, Marshal.SizeOf(buffer[0]));
        }

        private void MouseDrag(float warpedX, float warpedY) {
            if(_smoothingAmount > 0) {
                PointF s = SmoothedCursor();
                warpedX = s.X;
                warpedY = s.Y;
            }

            Input[] buffer = new Input[1];
            buffer[0].Type = InputMouse;
            buffer[0].MouseInput.Dx = (int)(warpedX * 65535.0f / _width);
            buffer[0].MouseInput.Dy = (int)(warpedY * 65535.0f / _height);
            buffer[0].MouseInput.MouseData = 0;
            buffer[0].MouseInput.DwFlags = MouseAbsolute | MouseMove;
            buffer[0].MouseInput.Time = 0;
            buffer[0].MouseInput.DwExtraInfo = (IntPtr)0;
            SendInput((uint)buffer.Length, buffer, Marshal.SizeOf(buffer[0]));
        }

        private PointF SmoothedCursor() {
            int start = _smoothingBufferIndex - _smoothingAmount;
            if(start < 0)
                start = 0;
            PointF smoothed = new PointF(0, 0);
            int count = _smoothingBufferIndex - start;
            for(int i = start; i < _smoothingBufferIndex; i++) {
                smoothed.X += _smoothingBuffer[i % SmoothingBufferSize].X;
                smoothed.Y += _smoothingBuffer[i % SmoothingBufferSize].Y;
            }
            smoothed.X /= count;
            smoothed.Y /= count;
            return smoothed;
        }

        public void ComputeWarp(float[] dstX, float[] dstY, float[] srcX, float[] srcY) {
            _warper.Destination(dstX[0], dstY[0], dstX[1], dstY[1], dstX[2], dstY[2], dstX[3], dstY[3]);
            _warper.Source(srcX[0], srcY[0], srcX[1], srcY[1], srcX[2], srcY[2], srcX[3], srcY[3]);
            _warper.ComputeWarp();
            _warpComputed = true;
        }

        public void Write(int address, byte data) { _wmote.WriteData(address, data); }
        public byte[] Read(int address, short size) { return _wmote.ReadData(address, size); }

        public void ReadCameraPosition(ref float x, ref float y, ref float z) {
            AccelState state = _wmote.MotionState.AccelState;
            x = state.X;
            y = state.Y;
            z = state.Z;
        }
    }
}