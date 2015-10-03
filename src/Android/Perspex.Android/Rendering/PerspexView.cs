using System;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Android.Views;
using Android.Widget;
using Perspex.Input;
using Perspex.Input.Raw;
using Perspex.Media;
using Perspex.Platform;
using Perspex.Rendering;
using ARect = Android.Graphics.Rect;
using APoint = Android.Graphics.Point;
using AApplication = Android.App.Application;

namespace Perspex.Android.Rendering
{
    public class PerspexView : View, IWindowImpl, IRenderTarget, IPlatformHandle
    {
        private TextPaint _painter;
        public Rect Bounds { get; set; }

        public PerspexView(Context context) : base(context)
        {
            ClientSize = MaxClientSize;
            _painter = new TextPaint();
            Resized += size => Invalidate(new Rect(size).ToAndroidGraphics());
        }

        public PerspexView() : this(PerspexActivity.Instance)
        {
            
        }

        public IInputRoot InputRoot { get; private set; }
        public Size ClientSize { get; set; }
        IntPtr IPlatformHandle.Handle => Handle; 
        IPlatformHandle ITopLevelImpl.Handle => this;
        public string HandleDescriptor => "Perspex View";
        Action ITopLevelImpl.Activated { get; set; }
        public Action Closed { get; set; }
        public Action Deactivated { get; set; }
        public Action<RawInputEventArgs> Input { get; set; }
        public Action<Rect> Paint { get; set; }
        public Action<Size> Resized { get; set; }
        public void Activate()
        {
        }

        public void Invalidate(Rect rect)
        {
            this.Invalidate(rect.ToAndroidGraphics());
        }

        public void SetInputRoot(IInputRoot inputRoot)
        {
            InputRoot = inputRoot;
        }

        public Point PointToScreen(Point point)
        {
            throw new NotImplementedException();
        }

        public void SetCursor(IPlatformHandle cursor)
        {

        }

        public Size MaxClientSize => new Size(Resources.DisplayMetrics.WidthPixels, Resources.DisplayMetrics.HeightPixels);
        public void SetTitle(string title)
        {
            
        }

        public void Show()
        {
            ((ViewGroup) Parent)?.RemoveAllViews();
            Resize((int) MaxClientSize.Width, (int) MaxClientSize.Height);
            PerspexActivity.Instance.View = this;
            PerspexActivity.Instance.SetContentView(this);
        }

        public IDisposable ShowDialog()
        {
            throw new NotImplementedException();
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }

        public IDrawingContext CreateDrawingContext()
        {
            return new DrawingContext();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            PerspexActivity.Instance.Canvas = canvas;
            this.Paint?.Invoke(new Rect(ClientSize));
        }

        public void Resize(int width, int height)
        {
            if (ClientSize == new Size(width, height)) return;
            ClientSize = new Size(width, height);
            Resized(ClientSize);
        }
    }
}