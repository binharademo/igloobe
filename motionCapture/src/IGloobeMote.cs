using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Threading;

namespace Br.Com.IGloobe.Connector.Mote {

// ReSharper disable InconsistentNaming
	public class IGloobeMote : IDisposable {
// ReSharper restore InconsistentNaming

		public event StateChangedEventHandler StateChanged;

		private const int Vid = 0x057e;
		private const int Pid = 0x0306;
        private const int ReportLength = 22; 
		
        public enum InputReport : byte {
			Status				= 0x20,
			ReadData			= 0x21,
			Buttons				= 0x30,
			ButtonsAccel		= 0x31,
			IRAccel				= 0x33,
		};

		private enum OutputReport : byte {
			//None			= 0x00,
			Leds		= 0x11,
			Type		= 0x12,
			IR			= 0x13,
			Status	= 0x15,
			WriteMemory	= 0x16,
			ReadMemory		= 0x17,
			IR2				    = 0x1a,
		};

		private const int RegisterIR				        = 0x04b00030;
		private const int RegisterIRSensitivity1	    = 0x04b00000;
		private const int RegisterIRSensitivity2	    = 0x04b0001a;
		private const int RegisterIRMode			    = 0x04b00033;

		private SafeFileHandle _mHandle;
		private FileStream _mStream;
		private readonly byte[] _mBuff = new byte[ReportLength];
		private byte[] _mReadBuff;
		private readonly MotionState _mMotionState = new MotionState();
		private readonly AutoResetEvent _mReadDone = new AutoResetEvent(false);
		private bool _mAltWriteMethod;

		public bool Connect() {
            try {
                int index = 0;
                bool found = false;
                Guid guid;

                // 1. get the GUID of the HID class   
                HidBinder.HidD_GetHidGuid(out guid); 

                // 2. get a handle to all devices that are part of the HID class
                IntPtr hDevInfo = HidBinder.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, HidBinder.DigcfDeviceInterface);// | HidBinder.DIGCF_PRESENT);
                HidBinder.SpDeviceInterfaceData diData = new HidBinder.SpDeviceInterfaceData(); // create a new interface data struct and initialize its size
                diData.cbSize = Marshal.SizeOf(diData);

                // 3. get a device interface to a single device (enumerate all devices)
                while(HidBinder.SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref guid, index, ref diData)) {
                    UInt32 size;
                    HidBinder.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, IntPtr.Zero, 0, out size, IntPtr.Zero);
                    HidBinder.SpDeviceInterfaceDetailData diDetail = new HidBinder.SpDeviceInterfaceDetailData { cbSize =(uint) (IntPtr.Size == 8 ? 8 : 5) };

                    if(HidBinder.SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, IntPtr.Zero)) {

                        _mHandle = HidBinder.CreateFile(diDetail.DevicePath, 
                                                                         FileAccess.ReadWrite, 
                                                                         FileShare.ReadWrite, 
                                                                         IntPtr.Zero, 
                                                                         FileMode.Open, 
                                                                         HidBinder.EFileAttributes.Overlapped, 
                                                                         IntPtr.Zero);

                        // 4. create an attributes struct and initialize the size      
                        HidBinder.HiddAttributes attrib = new HidBinder.HiddAttributes();
                        attrib.Size = Marshal.SizeOf(attrib);
                    
                        if(HidBinder.HidD_GetAttributes(_mHandle.DangerousGetHandle(), ref attrib)) {

                            if(attrib.VendorID == Vid  && attrib.ProductID == Pid) {
                                Console.WriteLine("Found it!");
                                found = true;

                                // 5. create a nice .NET FileStream wrapping the handle above
                                _mStream = new FileStream(_mHandle, FileAccess.ReadWrite, ReportLength, true);
                                BeginAsyncRead();

                                try {
                                    ReadCalibration();
                                } catch {
                                    _mAltWriteMethod = true;
                                    ReadCalibration();
                                }

                                GetStatus();
                                break;
                            }
                             _mHandle.Close();
                        }
                    } else {
                        return false;
                    }
                    index++;
                }
                // 6. clean up our list
                HidBinder.SetupDiDestroyDeviceInfoList(hDevInfo);
                return found;
            } catch {
                return false;
            }
        }

		public void Disconnect() {
			if(_mStream != null)
				_mStream.Close();

			if(_mHandle != null)
				_mHandle.Close();
		}

        private void BeginAsyncRead() {
		    if (!_mStream.CanRead) return;

		    byte[] buff = new byte[ReportLength];
		    _mStream.BeginRead(buff, 0, ReportLength, OnReadData, buff);
        }

        private void OnReadData(IAsyncResult ar){
			byte[] buff = (byte[])ar.AsyncState;
            try {
                _mStream.EndRead(ar);
                if(ParseInput(buff)) 
                    if(StateChanged != null)
                        StateChanged(this, new StateChangedEventArgs(_mMotionState));
                BeginAsyncRead();
// ReSharper disable EmptyGeneralCatchClause
            } catch { }
// ReSharper restore EmptyGeneralCatchClause
		}

		private bool ParseInput(byte[] buff) {
			InputReport type = (InputReport)buff[0];

			switch(type) {
				case InputReport.Buttons:
					ParseButtons(buff);
					break;
				case InputReport.ButtonsAccel:
					ParseButtons(buff);
					ParseAccel(buff);
					break;
				case InputReport.IRAccel:
					ParseButtons(buff);
					ParseAccel(buff);
					ParseIR(buff);
					break;
				case InputReport.Status:
					ParseButtons(buff);
					_mMotionState.Battery = buff[6];

					// get the real LED values in case the values from SetLeds() somehow becomes out of sync, which really shouldn't be possible
					_mMotionState.LedState.Led1 = (buff[3] & 0x10) != 0;
					_mMotionState.LedState.Led2 = (buff[3] & 0x20) != 0;
					_mMotionState.LedState.Led3 = (buff[3] & 0x40) != 0;
					_mMotionState.LedState.Led4 = (buff[3] & 0x80) != 0;
					break;
				case InputReport.ReadData:
					ParseButtons(buff);
					ParseReadData(buff);
					break;
				default:
 					return false;
			}

			return true;
		}

		private void ParseButtons(IList<byte> buff) {

			_mMotionState.ButtonState.A		= (buff[2] & 0x08) != 0;
			_mMotionState.ButtonState.B		= (buff[2] & 0x04) != 0;
			_mMotionState.ButtonState.Minus	= (buff[2] & 0x10) != 0;
			_mMotionState.ButtonState.Home	= (buff[2] & 0x80) != 0;
			_mMotionState.ButtonState.Plus	= (buff[1] & 0x10) != 0;
			_mMotionState.ButtonState.One	= (buff[2] & 0x02) != 0;
			_mMotionState.ButtonState.Two	= (buff[2] & 0x01) != 0;
			_mMotionState.ButtonState.Up	= (buff[1] & 0x08) != 0;
			_mMotionState.ButtonState.Down	= (buff[1] & 0x04) != 0;
			_mMotionState.ButtonState.Left	= (buff[1] & 0x01) != 0;
			_mMotionState.ButtonState.Right	= (buff[1] & 0x02) != 0;
		}

		private void ParseAccel(IList<byte> buff) {

			_mMotionState.AccelState.RawX = buff[3];
			_mMotionState.AccelState.RawY = buff[4];
			_mMotionState.AccelState.RawZ = buff[5];

			_mMotionState.AccelState.X = ((float)_mMotionState.AccelState.RawX - _mMotionState.AccelCalibrationInfo.X0) / 
											((float)_mMotionState.AccelCalibrationInfo.Xg - _mMotionState.AccelCalibrationInfo.X0);
			_mMotionState.AccelState.Y = ((float)_mMotionState.AccelState.RawY - _mMotionState.AccelCalibrationInfo.Y0) /
											((float)_mMotionState.AccelCalibrationInfo.Yg - _mMotionState.AccelCalibrationInfo.Y0);
			_mMotionState.AccelState.Z = ((float)_mMotionState.AccelState.RawZ - _mMotionState.AccelCalibrationInfo.Z0) /
											((float)_mMotionState.AccelCalibrationInfo.Zg - _mMotionState.AccelCalibrationInfo.Z0);
		}

		private void ParseIR(IList<byte> buff) {
			_mMotionState.IrState.RawX1 = buff[6]  | ((buff[8] >> 4) & 0x03) << 8;
			_mMotionState.IrState.RawY1 = buff[7]  | ((buff[8] >> 6) & 0x03) << 8;

			switch(_mMotionState.IrState.Mode) {
				case IrMode.Basic:
					_mMotionState.IrState.RawX2 = buff[9]  | ((buff[8] >> 0) & 0x03) << 8;
					_mMotionState.IrState.RawY2 = buff[10] | ((buff[8] >> 2) & 0x03) << 8;

					_mMotionState.IrState.Size1 = 0x00;
					_mMotionState.IrState.Size2 = 0x00;

					_mMotionState.IrState.Found1 = !(buff[6] == 0xff && buff[7] == 0xff);
					_mMotionState.IrState.Found2 = !(buff[9] == 0xff && buff[10] == 0xff);
					break;
				case IrMode.Extended:
					_mMotionState.IrState.RawX2 = buff[9]  | ((buff[11] >> 4) & 0x03) << 8;
					_mMotionState.IrState.RawY2 = buff[10] | ((buff[11] >> 6) & 0x03) << 8;
					_mMotionState.IrState.RawX3 = buff[12] | ((buff[14] >> 4) & 0x03) << 8;
					_mMotionState.IrState.RawY3 = buff[13] | ((buff[14] >> 6) & 0x03) << 8;
					_mMotionState.IrState.RawX4 = buff[15] | ((buff[17] >> 4) & 0x03) << 8;
					_mMotionState.IrState.RawY4 = buff[16] | ((buff[17] >> 6) & 0x03) << 8;

					_mMotionState.IrState.Size1 = buff[8] & 0x0f;
					_mMotionState.IrState.Size2 = buff[11] & 0x0f;
					_mMotionState.IrState.Size3 = buff[14] & 0x0f;
					_mMotionState.IrState.Size4 = buff[17] & 0x0f;

					_mMotionState.IrState.Found1 = !(buff[6] == 0xff && buff[7] == 0xff && buff[8] == 0xff);
					_mMotionState.IrState.Found2 = !(buff[9] == 0xff && buff[10] == 0xff && buff[11] == 0xff);
					_mMotionState.IrState.Found3 = !(buff[12] == 0xff && buff[13] == 0xff && buff[14] == 0xff);
					_mMotionState.IrState.Found4 = !(buff[15] == 0xff && buff[16] == 0xff && buff[17] == 0xff);
					break;
			}

			_mMotionState.IrState.X1 = (_mMotionState.IrState.RawX1 / 1023.5f);
			_mMotionState.IrState.X2 = (_mMotionState.IrState.RawX2 / 1023.5f);
			_mMotionState.IrState.X3 = (_mMotionState.IrState.RawX3 / 1023.5f);
			_mMotionState.IrState.X4 = (_mMotionState.IrState.RawX4 / 1023.5f);

			_mMotionState.IrState.Y1 = (_mMotionState.IrState.RawY1 / 767.5f);
			_mMotionState.IrState.Y2 = (_mMotionState.IrState.RawY2 / 767.5f);
			_mMotionState.IrState.Y3 = (_mMotionState.IrState.RawY3 / 767.5f);
			_mMotionState.IrState.Y4 = (_mMotionState.IrState.RawY4 / 767.5f);

			if(_mMotionState.IrState.Found1 && _mMotionState.IrState.Found2)
			{
				_mMotionState.IrState.RawMidX = (_mMotionState.IrState.RawX2 + _mMotionState.IrState.RawX1) / 2;
				_mMotionState.IrState.RawMidY = (_mMotionState.IrState.RawY2 + _mMotionState.IrState.RawY1) / 2;
		
				_mMotionState.IrState.MidX = (_mMotionState.IrState.X2 + _mMotionState.IrState.X1) / 2.0f;
				_mMotionState.IrState.MidY = (_mMotionState.IrState.Y2 + _mMotionState.IrState.Y1) / 2.0f;
			}
			else
				_mMotionState.IrState.MidX = _mMotionState.IrState.MidY = 0.0f;
		}

		private void ParseReadData(byte[] buff) {
			if((buff[3] & 0x08) != 0)
				throw new Exception(@"Error reading data from IGloobeMote: Bytes do not exist.");
		    
            if((buff[3] & 0x07) != 0)
		        throw new Exception(@"Error reading data from IGloobeMote: Attempt to read from write-only registers.");
		    
            int size = buff[3] >> 4;
		    Array.Copy(buff, 6, _mReadBuff, 0, size+1);

		    // set the event so the other thread will continue
			_mReadDone.Set();
		}

		private byte GetRumbleBit() {
			return (byte)(_mMotionState.Rumble ? 0x01 : 0x00);
		}

		private void ReadCalibration() {
			// this appears to change the report type to 0x31
			byte[] buff = ReadData(0x0016, 7);

			_mMotionState.AccelCalibrationInfo.X0 = buff[0];
			_mMotionState.AccelCalibrationInfo.Y0 = buff[1];
			_mMotionState.AccelCalibrationInfo.Z0 = buff[2];
			_mMotionState.AccelCalibrationInfo.Xg = buff[4];
			_mMotionState.AccelCalibrationInfo.Yg = buff[5];
			_mMotionState.AccelCalibrationInfo.Zg = buff[6];
		}

		public void SetReportType(InputReport type, bool continuous) {
			switch(type)
			{
				case InputReport.IRAccel:
					EnableIR(IrMode.Extended);
					break;
				default:
					DisableIR();
					break;
			}

			ClearReport();
			_mBuff[0] = (byte)OutputReport.Type;
			_mBuff[1] = (byte)((continuous ? 0x04 : 0x00) | (byte)(_mMotionState.Rumble ? 0x01 : 0x00));
			_mBuff[2] = (byte)type;

			WriteReport();
		}

		public void SetLeds(bool led1, bool led2, bool led3, bool led4) {

			_mMotionState.LedState.Led1 = led1;
			_mMotionState.LedState.Led2 = led2;
			_mMotionState.LedState.Led3 = led3;
			_mMotionState.LedState.Led4 = led4;

			ClearReport();

			_mBuff[0] = (byte)OutputReport.Leds;
			_mBuff[1] =	(byte)(
						(led1 ? 0x10 : 0x00) |
						(led2 ? 0x20 : 0x00) |
						(led3 ? 0x40 : 0x00) |
						(led4 ? 0x80 : 0x00) |
						GetRumbleBit());

			WriteReport();
		}

		public void SetLeds(int leds) {
			_mMotionState.LedState.Led1 = (leds & 0x01) > 0;
			_mMotionState.LedState.Led2 = (leds & 0x02) > 0;
			_mMotionState.LedState.Led3 = (leds & 0x04) > 0;
			_mMotionState.LedState.Led4 = (leds & 0x08) > 0;

			ClearReport();

			_mBuff[0] = (byte)OutputReport.Leds;
			_mBuff[1] =	(byte)(
						((leds & 0x01) > 0 ? 0x10 : 0x00) |
						((leds & 0x02) > 0 ? 0x20 : 0x00) |
						((leds & 0x04) > 0 ? 0x40 : 0x00) |
						((leds & 0x08) > 0 ? 0x80 : 0x00) |
						GetRumbleBit());

			WriteReport();
		}

		public void SetRumble(bool on) {
			_mMotionState.Rumble = on;
			SetLeds(_mMotionState.LedState.Led1, 
					_mMotionState.LedState.Led2,
					_mMotionState.LedState.Led3,
					_mMotionState.LedState.Led4);
		}

		public void GetStatus() {
			ClearReport();

			_mBuff[0] = (byte)OutputReport.Status;
			_mBuff[1] = GetRumbleBit();
			WriteReport();
		}

		private void EnableIR(IrMode mode) {

			_mMotionState.IrState.Mode = mode;

			ClearReport();
			_mBuff[0] = (byte)OutputReport.IR;
			_mBuff[1] = (byte)(0x04 | GetRumbleBit());
			WriteReport();

			ClearReport();
			_mBuff[0] = (byte)OutputReport.IR2;
			_mBuff[1] = (byte)(0x04 | GetRumbleBit());
			WriteReport();

			WriteData(RegisterIR, 0x08);
            WriteData(RegisterIRSensitivity1, 9, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x90, 0x00, 0x41 });
            WriteData(RegisterIRSensitivity2, 2, new byte[] { 0x40, 0x00 });
			WriteData(RegisterIRMode, (byte)mode);
		}

		private void DisableIR() {
			_mMotionState.IrState.Mode = IrMode.Off;

			ClearReport();
			_mBuff[0] = (byte)OutputReport.IR;
			_mBuff[1] = GetRumbleBit();
			WriteReport();

			ClearReport();
			_mBuff[0] = (byte)OutputReport.IR2;
			_mBuff[1] = GetRumbleBit();
			WriteReport();
		}

		private void ClearReport() {
			Array.Clear(_mBuff, 0, ReportLength);
		}

		private void WriteReport() {
			if(_mAltWriteMethod)
				HidBinder.HidD_SetOutputReport(_mHandle.DangerousGetHandle(), _mBuff, (uint)_mBuff.Length);
			else
				_mStream.Write(_mBuff, 0, ReportLength);

			Thread.Sleep(100);
		}

		public byte[] ReadData(int address, short size) {
			ClearReport();

			_mReadBuff = new byte[size];

			_mBuff[0] = (byte)OutputReport.ReadMemory;
			_mBuff[1] = (byte)(((address & 0xff000000) >> 24) | GetRumbleBit());
			_mBuff[2] = (byte)((address & 0x00ff0000)  >> 16);
			_mBuff[3] = (byte)((address & 0x0000ff00)  >>  8);
			_mBuff[4] = (byte)(address & 0x000000ff);

			_mBuff[5] = (byte)((size & 0xff00) >> 8);
			_mBuff[6] = (byte)(size & 0xff);

			WriteReport();

			if(!_mReadDone.WaitOne(1000, false))
                throw new Exception("Error reading data from IGloobeMote...is it connected?");

			return _mReadBuff;
		}

		public void WriteData(int address, byte data) {
			WriteData(address, 1, new[] { data });
		}
		
		public void WriteData(int address, byte size, byte[] buff) {
			ClearReport();

			_mBuff[0] = (byte)OutputReport.WriteMemory;
			_mBuff[1] = (byte)(((address & 0xff000000) >> 24) | GetRumbleBit());
			_mBuff[2] = (byte)((address & 0x00ff0000)  >> 16);
			_mBuff[3] = (byte)((address & 0x0000ff00)  >>  8);
			_mBuff[4] = (byte)(address & 0x000000ff);
			_mBuff[5] = size;
			Array.Copy(buff, 0, _mBuff, 6, size);

			WriteReport();
			Thread.Sleep(100);
		}

		public MotionState MotionState {
			get { return _mMotionState; }
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
		    if (!disposing) return;
		    Disconnect();
		}
	}
}
