using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace ImagingLibrary.Controls
{
    public class ZoomBorder : Border
    {

        public bool AcceptMouseInput { get; set; } = true;
        public double MaxZoom { get; set; } = 8;
        public double MinZoom { get; set; } = 0.5;



        public UIElement child = null;
        private Point origin;
        public Point start;

        /// <summary>
        /// Gets the TranslateTransform of a given element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private TranslateTransform GetTranslateTransform(UIElement element)
        {
            return (TranslateTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is TranslateTransform);
        }
        /// <summary>
        /// Gets the ScaleTransform of a given element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private ScaleTransform GetScaleTransform(UIElement element)
        {
            return (ScaleTransform)((TransformGroup)element.RenderTransform).Children.First(tr => tr is ScaleTransform);
        }

        public override UIElement Child
        {
            get
            {
                return base.Child;
            }
            set
            {
                if (value != null && value != this.Child)
                    Initialize(value);
                base.Child = value;
            }
        }

        public void Initialize(UIElement element)
        {
            this.child = element;
            if (child != null)
            {
                TransformGroup group = new TransformGroup();
                ScaleTransform st = new ScaleTransform();
                TranslateTransform tt = new TranslateTransform();
                

                group.Children.Add(st);
                group.Children.Add(tt);

                child.RenderTransform = group;
                child.RenderTransformOrigin = new Point(0.0, 0.0);


                // Bind events
                this.MouseWheel += child_MouseWheel;
                this.MouseLeftButtonDown += child_MouseLeftButtonDown;
                this.MouseLeftButtonUp += child_MouseLeftButtonUp;
                this.MouseMove += child_MouseMove;
                this.PreviewMouseRightButtonDown += new MouseButtonEventHandler(
                  child_PreviewMouseRightButtonDown);
            }
        }

        public void Reset()
        {
            if (child != null)
            {
                // reset zoom
                var st = GetScaleTransform(child);
                st.ScaleX = 1.0;
                st.ScaleY = 1.0;

                // reset pan
                var tt = GetTranslateTransform(child);
                tt.X = 0.0;
                tt.Y = 0.0;
            }
        }




        #region Controls
        public void Zoom(double delta, Point relative)
        {
            var st = GetScaleTransform(child);
            var tt = GetTranslateTransform(child);
            
            
            double aX, aY;

            aX = relative.X * st.ScaleX + tt.X;
            aY = relative.Y * st.ScaleY + tt.Y;

            
            st.ScaleX = Math.Max(MinZoom, Math.Min(st.ScaleX + delta, MaxZoom));
            st.ScaleY = Math.Max(MinZoom, Math.Min(st.ScaleY + delta, MaxZoom));

            tt.X = aX - relative.X * st.ScaleX;
            tt.Y = aY - relative.Y * st.ScaleY;
        }
        public void Pan(Vector delta)
        {
            var tt = GetTranslateTransform(child);
            tt.X = origin.X - delta.X;
            tt.Y = origin.Y - delta.Y;
        }
        #endregion Controls




        #region Child Events
        private void child_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (child != null && AcceptMouseInput)
            {
                Zoom(e.Delta > 0 ? .2 : -.2, e.GetPosition(child));
            }
        }

        private void child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (child != null && AcceptMouseInput)
            {
                var tt = GetTranslateTransform(child);
                start = e.GetPosition(this);
                origin = new Point(tt.X, tt.Y);
                this.Cursor = Cursors.Hand;
                child.CaptureMouse();
            }
        }

        private void child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (child != null && AcceptMouseInput)
            {
                child.ReleaseMouseCapture();
                this.Cursor = Cursors.Arrow;
                

                
            }
        }

        private void child_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (AcceptMouseInput)
            {
                //this.Reset();
            }
        }

        private void child_MouseMove(object sender, MouseEventArgs e)
        {
            if (child != null && AcceptMouseInput)
            {
                if (child.IsMouseCaptured)
                {
                    Pan(start - e.GetPosition(this));
                }
            }
        }

        #endregion Child Events
    }
}
