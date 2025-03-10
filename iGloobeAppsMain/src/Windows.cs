using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace Br.Com.IGloobe.Apps.Main.Gui {
    public class Windows : IEnumerable, IEnumerator {

        [DllImport("user32.dll")] private static extern int GetWindowText(int hWnd, StringBuilder title, int size);
        [DllImport("user32.dll")] private static extern int GetWindowModuleFileName(int hWnd, StringBuilder title, int size);
        [DllImport("user32.dll")] private static extern int EnumWindows(EnumWindowsProc ewp, int lParam); 
        [DllImport("user32.dll")] private static extern bool IsWindowVisible(int hWnd);

        public delegate bool EnumWindowsProc(int hWnd, int lParam);

        internal readonly ArrayList WindowsList = new ArrayList(); //array of windows
        
        private int _position = -1;
        private readonly bool _invisible;
        private readonly bool _notitle;

        public Windows() {
            _invisible = true;
            _notitle = false;

            EnumWindowsProc ewp = EvalWindow;
            EnumWindows(ewp, 0);
        }

        private bool EvalWindow(int hWnd, int lParam) {

            if (_invisible == false && !IsWindowVisible(hWnd))
                return(true);

            StringBuilder title = new StringBuilder(256);
            StringBuilder module = new StringBuilder(256);

            GetWindowModuleFileName(hWnd, module, 256);
            GetWindowText(hWnd, title, 256);

            if (_notitle == false && title.Length == 0)
                return(true);

            WindowsList.Add(new Window(title.ToString(), (IntPtr)hWnd, module.ToString()));

            return(true);
        }
		
        public bool MoveNext() {
            _position++;
            return _position < WindowsList.Count;
        }

        public void Reset() { _position = -1; }
        public object Current { get { return WindowsList[_position]; } }
        public IEnumerator GetEnumerator() { return this; }
    }
}