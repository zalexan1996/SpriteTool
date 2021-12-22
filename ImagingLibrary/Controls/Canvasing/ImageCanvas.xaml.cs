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

namespace ImagingLibrary.Controls.Canvasing
{
    /// <summary>
    /// Interaction logic for ImageCanvas.xaml
    /// </summary>
    public partial class ImageCanvas : UserControl
    {



        public List<Rectangle> SpriteRectangles { get; set; } = new List<Rectangle>();

        private Rectangle ClickedRectangle = null;
        private List<Rectangle> SelectedRectangles = new List<Rectangle>();
        private Rectangle CombineWithRectangle = null;
        private bool InCombineMode = false;

        public ImageCanvas()
        {
            InitializeComponent();
        }

        public void AddRectangle(Point position, double width, double height)
        {
            // Get the scaling factors for the current window strech
            double sX = ScaleXRatio;
            double sY = ScaleYRatio;

            /*
            CanvasItem item = new CanvasItem()
            {
                Stroke = Brushes.Cyan,
                StrokeThickness = ScaleXRatio,
                Fill = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                Rect = new Rect(position.X * sX, position.Y * sY, width * sX, height * sY)
            };*/
            
            Rectangle r = new Rectangle();
            r.StrokeThickness = 1 * sX;
            r.Stroke = new SolidColorBrush(Colors.Red);
            r.Fill = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

            TheCanvas.Children.Add(r);



            r.Width = width * sX;
            r.Height = height * sY;
            Canvas.SetLeft(r, position.X * sX);
            Canvas.SetTop(r, position.Y * sY);
            
            SpriteRectangles.Add(r);
        }
        

        public void ClearRectangles()
        {
            // Remove all Sprite rectangles from the canvas.
            SpriteRectangles.ForEach(r => { TheCanvas.Children.Remove(r); });

            // Clear the local list of sprite rectangles.
            SpriteRectangles.Clear();
        }

        public double ScaleXRatio { get { return TheImage.ActualWidth / (TheImage.Source as BitmapImage).PixelWidth; } }
        public double ScaleYRatio { get { return TheImage.ActualHeight / (TheImage.Source as BitmapImage).PixelHeight; } }

        public BitmapImage Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                TheImage.Source = value;
            }
        } private BitmapImage _image = null;


        /// <summary>
        /// Converts the source rectangles to an List of mapped images.
        /// </summary>
        /// <returns></returns>
        public List<Types.MappedBitmapImage> RectanglesToImages()
        {
            // Exit out if we don't have any preview rectangles.
            if (SpriteRectangles.Count <= 0)
                return null;

            // Initialize the output array.
            List<Types.MappedBitmapImage> output = new List<Types.MappedBitmapImage>();
            
            // Convert each rectangle to a MappedBitmapImage
            output = SpriteRectangles.Select(r => new Types.MappedBitmapImage()
            {
                Source = _image,
                X = (int)(Canvas.GetLeft(r) / ScaleXRatio),
                Y = (int)(Canvas.GetTop(r) / ScaleYRatio),
                Width = (int)(r.Width / ScaleXRatio),
                Height = (int)(r.Height / ScaleYRatio)
            }).ToList();
            
            
            return output;
        }

        private void CombineRectangles(Rectangle r1, Rectangle r2)
        {
            // Cache the dimensions of the first rectangle
            double r1X = Canvas.GetLeft(r1);
            double r1Y = Canvas.GetTop(r1);
            double r1W = r1.Width;
            double r1H = r1.Height;

            // Cache the dimensions of the second rectangle
            double r2X = Canvas.GetLeft(r2);
            double r2Y = Canvas.GetTop(r2);
            double r2W = r2.Width;
            double r2H = r2.Height;

            // Create the union of the dimensions
            Rect union = Rect.Union(new Rect(r1X / ScaleXRatio, r1Y / ScaleYRatio, r1W / ScaleXRatio, r1H / ScaleYRatio), new Rect(r2X / ScaleXRatio, r2Y / ScaleYRatio, r2W / ScaleXRatio, r2H / ScaleYRatio));

            Console.WriteLine(union);
            // Add this rectangle to the canvas.
            AddRectangle(union.TopLeft, union.Width, union.Height);

            // Remove the old rectangles.
            SpriteRectangles.Remove(r1);
            TheCanvas.Children.Remove(r1);
            SpriteRectangles.Remove(r2);
            TheCanvas.Children.Remove(r2);


            // Make sure the CombineWith Rectangle is the union Rectangle
            CombineWithRectangle = null;
            ClickedRectangle = null;
            InCombineMode = false;

        }

        public Point GetPoint(int element)
        {
            return new Point(Canvas.GetLeft(SpriteRectangles[element]) / ScaleXRatio, Canvas.GetTop(SpriteRectangles[element]) / ScaleYRatio);
        }


        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (ClickedRectangle != null)
            {
                SpriteRectangles.Remove(ClickedRectangle);
                TheCanvas.Children.Remove(ClickedRectangle);
                ClickedRectangle = null;
            }
        }

        private void CombineWith_Click(object sender, RoutedEventArgs e)
        {
            if (!InCombineMode)
            {
                InCombineMode = true;
                CombineWithRectangle = ClickedRectangle;
            }
        }


        private void TheCanvas_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle r = TheCanvas.InputHitTest(Mouse.GetPosition(TheCanvas)) as Rectangle;

            if (r != null)
            {
                ClickedRectangle = r;
                TheCanvas.ContextMenu = (ContextMenu)Resources["contextMenu"];
            }
            else
            {
                ClickedRectangle = null;
            }
        }

        private void TheCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (InCombineMode && CombineWithRectangle != null)
            {

                Rectangle r = TheCanvas.InputHitTest(Mouse.GetPosition(TheCanvas)) as Rectangle;

                if (r != null)
                {
                    CombineRectangles(r, CombineWithRectangle);
                }
            }
        }
    }
}
