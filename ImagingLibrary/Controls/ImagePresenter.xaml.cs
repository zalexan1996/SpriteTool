using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImagingLibrary.Controls
{
   
    /// <summary>
    /// Interaction logic for ImagePresenter.xaml
    /// </summary>
    public partial class ImagePresenter : UserControl
    {
        public enum Mode
        {
            Move, Select
        }

        public Mode mode { get; set; }

        Point EmptyPoint = new Point(-1, -1);
        Point rectStart;
        Point rectEnd;
        Rectangle previewRectangle;

        public ImagePresenter()
        {
            InitializeComponent();
            rectStart = EmptyPoint;
            rectEnd = EmptyPoint;
            RenderOptions.SetBitmapScalingMode(TheImage, BitmapScalingMode.HighQuality);
        }



        public BitmapSource Subsection(Int32Rect r)
        {
            Console.WriteLine(TheImage.Source.GetType().ToString());
            BitmapSource bi = (BitmapSource)TheImage.Source;
            

            int stride = r.Width * (bi.Format.BitsPerPixel / 8);

            byte[] pixels = new byte[r.Width * r.Height];
            bi.CopyPixels(r, pixels, stride, 0);

            return BitmapSource.Create(r.Width, r.Height, bi.DpiX, bi.DpiY, bi.Format, bi.Palette, pixels, stride);

        }
        


        public string ImagePath { get; set; } = "/Sheets/MinishCap.png";



        private void TheCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // If we're selecting a subsection of the sprite
            if (mode == Mode.Select && e.LeftButton == MouseButtonState.Pressed)
            {
                // Check to see if we're closing a selection
                if (rectEnd != EmptyPoint)
                {
                    

                    Point start = new Point();
                    start.X = Canvas.GetLeft(previewRectangle);
                    start.Y = Canvas.GetTop(previewRectangle);

                    Point end = new Point();
                    end.X = Canvas.GetLeft(previewRectangle) + previewRectangle.Width;
                    end.Y = Canvas.GetTop(previewRectangle) + previewRectangle.Height;

                    start = ImageFactory.ConvertScreenToLocalPixels(start, TheImage);
                    end = ImageFactory.ConvertScreenToLocalPixels(end, TheImage);

                    TheImage.Source = ImageFactory.GetSubsection(TheImage.Source as BitmapSource, start, (int)end.X, (int)end.Y);



                    previewRectangle = null;
                    rectStart = EmptyPoint;
                    rectEnd = EmptyPoint;

                }
                else
                {
                    rectStart = e.GetPosition(TheImage);
                    rectEnd = e.GetPosition(TheImage);

                    previewRectangle = new Rectangle();
                    previewRectangle.Stroke = new SolidColorBrush(Colors.Red);
                    previewRectangle.StrokeThickness = 0.2;


                    Console.WriteLine("Rectangle added");

                    Console.WriteLine(rectStart.ToString());
                    Console.WriteLine(rectEnd.ToString());
                    TheCanvas.Children.Add(previewRectangle);
                }


                
            }
        }

        private void Btn_Subsection_Click(object sender, RoutedEventArgs e)
        {
            mode = Mode.Select;
            zoomBorder.AcceptMouseInput = false;

            rectStart = EmptyPoint;
            rectEnd = EmptyPoint;
        }
        private void Btn_MoveMode_Click(object sender, RoutedEventArgs e)
        {
            mode = Mode.Move;
            zoomBorder.AcceptMouseInput = true;
            
        }

        private void TheCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mode == Mode.Select)
            {
                if (rectStart != EmptyPoint)
                {
                    rectEnd = e.GetPosition(TheCanvas);


                    previewRectangle.Width = Math.Abs(rectEnd.X - rectStart.X);
                    previewRectangle.Height = Math.Abs(rectEnd.Y - rectStart.Y);

                    Canvas.SetLeft(previewRectangle, Math.Min(rectStart.X, rectEnd.X));
                    Canvas.SetRight(previewRectangle, Math.Max(rectStart.X, rectEnd.X));
                    Canvas.SetTop(previewRectangle, Math.Min(rectStart.Y, rectEnd.Y));
                    Canvas.SetBottom(previewRectangle, Math.Max(rectStart.Y, rectEnd.Y));


                    
                }
            }
            Tb_X.Text = $"X: { ((int)e.GetPosition(TheCanvas).X).ToString()}";
            Tb_Y.Text = $"Y: { ((int)e.GetPosition(TheCanvas).Y).ToString()}";
        }

        private void TheCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Key Up");

            if (e.Key == Key.Escape)
            {
                if (mode == Mode.Select)
                {
                    TheCanvas.Children.Remove(previewRectangle);

                    previewRectangle = null;
                    rectStart = EmptyPoint;
                    rectEnd = EmptyPoint;

                    zoomBorder.AcceptMouseInput = true;
                    mode = Mode.Select;


                }
            }
        }

        private void ZoomBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            if (previewRectangle != null)
            {
                Vector a = zoomBorder.start - e.GetPosition(zoomBorder.child);
                Console.WriteLine(a.ToString());
                zoomBorder.Pan(zoomBorder.start - e.GetPosition(zoomBorder.child));
            }
        }
    }
}
