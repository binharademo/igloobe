using System;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Security;

namespace Br.Com.IGloobe.Connector.Mote {
    
    [SuppressUnmanagedCodeSecurity]
	class HidBinder { 

		public const int DigcfDefault          = 0x00000001; 
		public const int DigcfPresent          = 0x00000002;
		public const int DigcfAllclasses       = 0x00000004;
		public const int DigcfProfile          = 0x00000008;
		public const int DigcfDeviceInterface  = 0x00000010;

		[Flags]
		public enum EFileAttributes : uint {
		   Readonly         = 0x00000001,
		   Hidden           = 0x00000002,
		   System           = 0x00000004,
		   Directory        = 0x00000010,
		   Archive          = 0x00000020,
		   Device           = 0x00000040,
		   Normal           = 0x00000080,
		   Temporary        = 0x00000100,
		   SparseFile       = 0x00000200,
		   ReparsePoint     = 0x00000400,
		   Compressed       = 0x00000800,
		   Offline          = 0x00001000,
		   NotContentIndexed= 0x00002000,
		   Encrypted        = 0x00004000,
		   WriteThrough    = 0x80000000,
		   Overlapped       = 0x40000000,
		   NoBuffering      = 0x20000000,
		   RandomAccess     = 0x10000000,
		   SequentialScan   = 0x08000000,
		   DeleteOnClose    = 0x04000000,
		   BackupSemantics  = 0x02000000,
		   PosixSemantics   = 0x01000000,
		   OpenReparsePoint = 0x00200000,
		   OpenNoRecall     = 0x00100000,
		   FirstPipeInstance= 0x00080000
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SpDevinfoData {
			public uint cbSize;
			public Guid ClassGuid;
			public uint DevInst;
			public IntPtr Reserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SpDeviceInterfaceData {
			public int cbSize;
			public Guid InterfaceClassGuid;
			public int Flags;
			public IntPtr RESERVED;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct SpDeviceInterfaceDetailData {
			public UInt32 cbSize;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string DevicePath;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct HiddAttributes {
			public int Size;
			public short VendorID;
			public short ProductID;
			public short VersionNumber;
		}

		[DllImport(@"hid.dll", CharSet=CharSet.Auto, SetLastError = true)]
		public static extern void HidD_GetHidGuid(out Guid gHid);

		[DllImport("hid.dll")]
		public static extern Boolean HidD_GetAttributes(IntPtr hidDeviceObject, ref HiddAttributes attributes);

		[DllImport("hid.dll")]
		internal extern static bool HidD_SetOutputReport( IntPtr hidDeviceObject, byte[] lpReportBuffer, uint reportBufferLength);

		[DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SetupDiGetClassDevs( ref Guid classGuid, [MarshalAs(UnmanagedType.LPTStr)] string enumerator, IntPtr hwndParent, UInt32 flags);

		[DllImport(@"setupapi.dll", CharSet=CharSet.Auto, SetLastError = true)]
		public static extern Boolean SetupDiEnumDeviceInterfaces(
			IntPtr hDevInfo,
			IntPtr devInvo,
			ref Guid interfaceClassGuid,
			Int32 memberIndex,
			ref SpDeviceInterfaceData deviceInterfaceData
		);

		[DllImport(@"setupapi.dll", SetLastError = true)]
		public static extern Boolean SetupDiGetDeviceInterfaceDetail(
			IntPtr hDevInfo,
			ref SpDeviceInterfaceData deviceInterfaceData,
			IntPtr deviceInterfaceDetailData,
			UInt32 deviceInterfaceDetailDataSize,
			out UInt32 requiredSize,
			IntPtr deviceInfoData
		);

		[DllImport(@"setupapi.dll", SetLastError = true)]
		public static extern Boolean SetupDiGetDeviceInterfaceDetail(
			IntPtr hDevInfo,
			ref SpDeviceInterfaceData deviceInterfaceData,
			ref SpDeviceInterfaceDetailData deviceInterfaceDetailData,
			UInt32 deviceInterfaceDetailDataSize,
			out UInt32 requiredSize,
			IntPtr deviceInfoData
		);

		[DllImport(@"setupapi.dll", CharSet=CharSet.Auto, SetLastError = true)]
		public static extern UInt16 SetupDiDestroyDeviceInfoList( IntPtr hDevInfo );

		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern SafeFileHandle CreateFile(
			string fileName,
			[MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
			[MarshalAs(UnmanagedType.U4)] FileShare fileShare,
			IntPtr securityAttributes,
			[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
			[MarshalAs(UnmanagedType.U4)] EFileAttributes flags,
			IntPtr template);

			[DllImport("kernel32.dll", SetLastError=true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool CloseHandle(IntPtr hObject);
	}
}
