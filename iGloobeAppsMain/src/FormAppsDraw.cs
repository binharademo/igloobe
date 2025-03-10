using System;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Br.Com.IGloobe.Apps.Main.Gui {

    public partial class FormAppsDraw : DraggableForm {

        private readonly System.ComponentModel.ComponentResourceManager _resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAppsDraw));

        private static readonly string[] ColorsIcons = new[] { "draw_color_black48", "draw_color_blue48", "draw_color_red48", "draw_color_yellow48", "draw_color_white48" };
        private static readonly Color[] Colors = new[] { Color.Black, Color.Blue, Color.Red, Color.Yellow, Color.White };

        private static readonly string[] SizesIcons = new[] { "draw_size48a", "draw_size48b", "draw_size48c" };
        private static readonly float[] Sizes = new[] { 4f, 12f, 36f };

        private static readonly string[] AlphasIcons = new[] { "draw_alpha48a", "draw_alpha48c" };
        private static readonly int[] Alphas = new[] {255, 60};

        private static readonly string[] Locations = new[] { "draw_pointer48", "draw_desktop48", "draw_witheboard48" };

        private static FormAppsDraw _singleton;

        private int _currentColor;
        private int _currentSize = 1;
        private int _currentAlpha;
        private int _currentLocation;

        private Board _board;

        private FormAppsDraw() {
            InitializeComponent();
            MakeTrasparent();
            MoveToBottomCenter();
            InitializeDragSupport(DrawImage);
            _board = new Board();
            _board.Hide();
        }

        private void BtnCloseWindow_Click(object sender, EventArgs e) {
            Visible = false;
            SetLocationToPointer();
            FormAppsPresentation.Singleton().Enabled = true;
        }

        public static FormAppsDraw Singleton() {
            if (_singleton == null || _singleton.IsDisposed)
                _singleton = new FormAppsDraw();
            return _singleton;
        }

        private Image GetResource(string[] colection, int currentIndex) {
            return ((Image)(_resources.GetObject(colection[currentIndex])));
        }

        public Color CurrentColor { get { return Colors[_currentColor]; } }
        private Image CurrentColorIcon { get { return GetResource(ColorsIcons, _currentColor); } }
        private void BtnColorClick(object sender, EventArgs e) {
            _currentColor++;
            if (_currentColor >= ColorsIcons.Length) _currentColor = 0;
            BtnColor.Image = CurrentColorIcon;
        }

        public float CurrentSize { get { return Sizes[_currentSize]; } }
        private Image CurrentSizeIcon { get { return GetResource(SizesIcons, _currentSize); } }
        private void BtnSizeClick(object sender, EventArgs e) {
            _currentSize++;
            if (_currentSize >= Sizes.Length) _currentSize = 0;
            BtnSize.Image = CurrentSizeIcon;
        }

        public int CurrentLocation { get { return _currentLocation; } }
        private Image CurrentLocationIcon { get { return GetResource(Locations, _currentLocation); } }
        private void BtnLocationClick(object sender, EventArgs e) {

            _currentLocation++;
            FormAppsPresentation.Singleton().Enabled = _currentLocation != 2;

            if (_currentLocation >= Locations.Length) _currentLocation = 0;

            BtnWhere.Image = CurrentLocationIcon;

            bool notPointer = _currentLocation != 0;
            BtnSize.Enabled = notPointer;
            BtnColor.Enabled = notPointer;
            BtnAlpha.Enabled = notPointer;
            BtnErase.Enabled = notPointer;

            if (_currentLocation == 0) {
                _board.Hide();
                return;
            }

            _board.Show();
            Window.SetForegroundWindow(_board.Handle);

            if (_currentLocation == 1) _board.Desktop();
            if (_currentLocation == 2) _board.Whiteboard();

            Window.SetForegroundWindow(Handle);            
        }

        public int CurrentAlpha { get { return Alphas[_currentAlpha]; } }
        private Image CurrentAlphaIcon { get { return GetResource(AlphasIcons, _currentAlpha); } }
        private void BtnAlphaClick(object sender, EventArgs e) {
            _currentAlpha++;
            if (_currentAlpha >= AlphasIcons.Length) _currentAlpha = 0;
            BtnAlpha.Image = CurrentAlphaIcon;
        }

        public bool DrawOnDesktop() {
            return _currentLocation == 1;
        }

        public bool IsPointerSelected() {
            return _currentLocation == 0;
        }

        private void FormAppsDraw_VisibleChanged(object sender, EventArgs e) {
            _board.Visible = Visible;
            if (IsPointerSelected()) _board.Visible = false;
        }

        private void BtnEraseClick(object sender, EventArgs e) {
            _board.Clear();
        }

        public void SetLocationToPointer() {
            _currentLocation = 2;
            BtnLocationClick(null, null);
        }

        private void FormAppsDraw_Leave(object sender, EventArgs e) {
            FormAppsPresentation.Singleton().Enabled = true;
        }
    }

    public partial class Board : Form {

        private Point _lastPoint;
        private Mutex _mutex = new Mutex();

        [DllImport("User32.dll")] public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")] public static extern void ReleaseDC(IntPtr dc);

        public Board() {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e) {
            WindowState = FormWindowState.Maximized;
        }

        private void FormClick(object sender, EventArgs e) {
            if (((MouseEventArgs)e).Button != MouseButtons.Left) return;
            DrawPoint(Cursor.Position);
        }

        private void FormMouseMove(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Left) return;
            DrawPoint(Cursor.Position);
        }

        private void FormMouseUp(object sender, MouseEventArgs e) {
            _lastPoint = Point.Empty;
            Window.SetForegroundWindow(FormAppsDraw.Singleton().Handle);           
        }

        private void DrawPoint(Point p) {
            try {
                _mutex.WaitOne();
                FormAppsDraw gui = FormAppsDraw.Singleton();
                float size = gui.CurrentSize;
                Color color = Color.FromArgb(gui.CurrentAlpha, gui.CurrentColor);

                Pen pen = new Pen(color) {Width = size};
                Brush brush = new SolidBrush(color);

                Graphics formGraphics = gui.DrawOnDesktop() ?
                                                Graphics.FromHdc(GetDC(IntPtr.Zero)) :
                                                CreateGraphics();

                if (_lastPoint.IsEmpty) _lastPoint = p;
                formGraphics.DrawLine(pen, _lastPoint, p);

                if (gui.CurrentAlpha == 255)
                    formGraphics.FillEllipse(brush, p.X - (size/2), p.Y - (size/2), size, size);

                _lastPoint = p;
                pen.Dispose();
                formGraphics.Dispose();
            } finally {
                _mutex.ReleaseMutex();
            }
        }

        public void Clear() {
            CreateGraphics().Clear(Color.White);
        }

        public void Whiteboard() {
            Opacity = 100;
            CreateGraphics().Clear(Color.White);
            Show();
        }

        public void Desktop() {
            CreateGraphics().Clear(Color.White);
            Opacity = 0.01;
        }
    }
}
