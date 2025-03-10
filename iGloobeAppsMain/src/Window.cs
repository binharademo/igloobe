using System;
using System.Runtime.InteropServices;

namespace Br.Com.IGloobe.Apps.Main.Gui {

	public class Window {

        [DllImport("user32.dll")] public static extern int FindWindow(string className, string windowText);
	    [DllImport("user32.dll")] public static extern int ShowWindow(int hwnd, int command);
        [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

		[DllImport("user32.dll")] private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
		[DllImport("user32.dll")] private static extern bool IsIconic(IntPtr hWnd);
		[DllImport("user32.dll")] private static extern bool IsZoomed(IntPtr hWnd);
		[DllImport("user32.dll")] private static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);

		public const int SwHide = 0;
        public const int SwShowNormal = 1;
        public const int SwShowMinimized = 2;
        public const int SwShowMaximized = 3;
        public const int SwShowNoActivate = 4;
        public const int SwRestore = 9;
        public const int SwShowDefault = 10;

		private readonly IntPtr _windowHandle;
		private readonly string _title;
		private bool _visiblr = true;
		private readonly string _process;
		private bool _wasMax;

		public IntPtr Handle{ get{return _windowHandle;} }
		public string Title { get{return _title;} }
		public string Process { get{return _process;} }
		public bool Visible {
			get{return _visiblr;}
			set {
				if(value) {
					if(_wasMax) {
						if(ShowWindowAsync(_windowHandle,SwShowMaximized))
							_visiblr = true;
					} else {
						if(ShowWindowAsync(_windowHandle,SwShowNormal))
							_visiblr = true;
					}
				}

			    if (value) return;

			    _wasMax = IsZoomed(_windowHandle);
			    if(ShowWindowAsync(_windowHandle,SwHide))
			        _visiblr = false;
			}
		}

		public Window(string title, IntPtr hWnd, string process) {
			_title = title;
			_windowHandle = hWnd;
			_process = process;
		}

		public override string ToString() {
		    return _title.Length > 0 ? _title : _process;
		}

		public void Activate() {
			if(_windowHandle == GetForegroundWindow()) return;

			IntPtr threadId1 = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
			IntPtr threadId2 = GetWindowThreadProcessId(_windowHandle, IntPtr.Zero);
			
			if (threadId1 != threadId2) {
				AttachThreadInput(threadId1,threadId2,1);
				SetForegroundWindow(_windowHandle);
				AttachThreadInput(threadId1,threadId2,0);
			} else{
				SetForegroundWindow(_windowHandle);
			}

		    ShowWindowAsync(_windowHandle, IsIconic(_windowHandle) ? SwRestore : SwShowNormal);
		}
	}
}
