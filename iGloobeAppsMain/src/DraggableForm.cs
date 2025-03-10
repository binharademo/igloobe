using System.Drawing;
using System.Windows.Forms;

namespace Br.Com.IGloobe.Apps.Main.Gui {

    public class DraggableForm : Form {

        protected const int XOffset = 5;
        protected const int YOffset = 40;

        private bool _isMouseRightDown;
        private Point _deltaStart;

        public override sealed Color BackColor {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        protected void MoveToBottomRightCorner() {
            Rectangle size = Screen.GetBounds(this);
            Location = new Point(size.Width - Width - XOffset, size.Height - Height - YOffset);
        }

        protected void MoveToBottomLeftCorner() {
            Rectangle size = Screen.GetBounds(this);
            Location = new Point(0 + XOffset, size.Height - Height - YOffset);
        }
        
        protected void MoveToBottomCenter() {
            Rectangle size = Screen.GetBounds(this);
            Location = new Point((size.Width - Width)/2 , size.Height - Height - YOffset);
        }

        protected void MakeTrasparent() {
            BackColor = Color.BlanchedAlmond;
            TransparencyKey = BackColor;
        }

        protected void OnDragging(object sender, MouseEventArgs e) {
            if (!_isMouseRightDown) return;

            Point mousePosition = Cursor.Position;
            int xDest = mousePosition.X - _deltaStart.X;
            int yDest = mousePosition.Y - _deltaStart.Y;
            Location = new Point(xDest, yDest);
        }

        protected void OnStartDragging(object sender, MouseEventArgs e) {
            _isMouseRightDown = true;
            Point mousePosition = Cursor.Position;
            Point windowPosition = Location;
            _deltaStart = new Point(mousePosition.X - windowPosition.X, mousePosition.Y - windowPosition.Y);
        }

        protected void OnReleaseDragging(object sender, MouseEventArgs e) {
            _isMouseRightDown = false;
        }

        protected void InitializeDragSupport(Control draggableComponent) {
            draggableComponent.MouseDown += OnStartDragging;
            draggableComponent.MouseMove += OnDragging;
            draggableComponent.MouseUp += OnReleaseDragging;
        }
    }
}