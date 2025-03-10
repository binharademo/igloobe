using System;
#if MSRS
	using Microsoft.Dss.Core.Attributes;
#else
	sealed class DataContract : Attribute { }
	sealed class DataMember: Attribute { }
#endif

namespace Br.Com.IGloobe.Connector.Mote {

#if MSRS
    [DataContract]
    public struct RumbleRequest {
        [DataMember]
        public bool Rumble;
    }
#endif

	[DataContract]
	public class MotionState {

		[DataMember] public AccelCalibrationInfo AccelCalibrationInfo;
		[DataMember] public ButtonState ButtonState;
		[DataMember] public AccelState AccelState;
		[DataMember] public IrState IrState;
		[DataMember] public byte Battery;
		[DataMember] public bool Rumble;
		[DataMember] public LedState LedState;
	}

    public struct LedState {
        [DataMember] public bool Led1, Led2, Led3, Led4;
    }

	[DataContract]
	public struct IrState {
		[DataMember] public IrMode Mode;
		[DataMember] public int RawX1, RawX2, RawX3, RawX4;
		[DataMember] public int RawY1, RawY2, RawY3, RawY4;
		[DataMember] public int Size1, Size2, Size3, Size4;
		[DataMember] public bool Found1, Found2, Found3, Found4;
		[DataMember] public float X1, X2, X3, X4;
		[DataMember] public float Y1, Y2, Y3, Y4;
		[DataMember] public int RawMidX, RawMidY;
		[DataMember] public float MidX, MidY;
	}

	[DataContract]
	public struct AccelState {
		[DataMember] public byte RawX, RawY, RawZ;
		[DataMember] public float X, Y, Z;
	}

	[DataContract]
	public struct AccelCalibrationInfo {
		[DataMember] public byte X0, Y0, Z0;
		[DataMember] public byte Xg, Yg, Zg;
	}

	[DataContract]
	public struct ButtonState {
		[DataMember] public bool A, B, Plus, Home, Minus, One, Two, Up, Down, Left, Right;
	}

	[DataContract]
	public enum IrMode : byte {
		Off			= 0x00,
		Basic		= 0x01,	// 10 bytes
		Extended	= 0x03,	// 12 bytes
        Full = 0x05,	// 16 bytes * 2 (format unknown - unsupported)
	};
}