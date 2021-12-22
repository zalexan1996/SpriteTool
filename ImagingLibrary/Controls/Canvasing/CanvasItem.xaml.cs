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
    /// Interaction logic for CanvasItem.xaml
    /// </summary>
    public partial class CanvasItem : UserControl
    {


        public SolidColorBrush Stroke
        {
            get
            {
                return (SolidColorBrush)TheRectangle.Stroke;
            }
            set
            {
                TheRectangle.Stroke = value;
            }
        }
        public SolidColorBrush Fill
        {
            get
            {
                return (SolidColorBrush)TheRectangle.Fill;
            }
            set
            {
                TheRectangle.Fill = value;
            }
        }

        public double StrokeThickness
        {
            get
            {
                return TheRectangle.StrokeThickness;
            }
            set
            {
                TheRectangle.StrokeThickness = value;
            }
        }
        public Rect Rect
        {
            get
            {
                return _rect; ;
            }
            set
            {
                _rect = value;

                Canvas.SetLeft(this, _rect.X);
                Canvas.SetTop(this, _rect.Y);

                TheRectangle.Width = _rect.Width;
                TheRectangle.Height = _rect.Height;

            }
        } private Rect _rect;
        



        public static CanvasItem Create(Rect rect, double strokeThickness, SolidColorBrush stroke, string optionalTitle = null)
        {
            CanvasItem ci = new CanvasItem()
            {
                Stroke = stroke,
                StrokeThickness = strokeThickness,
                Fill = new SolidColorBrush(Colors.Transparent),
                Rect = rect
            };
            return ci;
        }

        public static CanvasItem Combine(CanvasItem item1, CanvasItem item2, double ScaleXRatio, double ScaleYRatio)
        {
            Rect r1 = item1.Rect;
            Rect r2 = item2.Rect;


            // Create the union of the dimensions
            Rect union = Rect.Union(new Rect(r1.X / ScaleXRatio, r1.Y / ScaleYRatio, r1.Width / ScaleXRatio, r1.Height / ScaleYRatio), new Rect(r2.X / ScaleXRatio, r2.Y / ScaleYRatio, r2.Width / ScaleXRatio, r2.Height / ScaleYRatio));

            // Return the combined item
            return Create(union, item1.StrokeThickness, item1.Stroke);
            
        }

        public CanvasItem()
        {
            InitializeComponent();
            IsHitTestVisible = true;
        }

        public void Selected()
        {
            Stroke = Brushes.White;
        }
        public void Unselected()
        {
            Stroke = Brushes.Cyan;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            Console.WriteLine("Clicked!!");
        }

        private void Button_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            OnRightClick?.Invoke(this, e);
        }
        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            OnLeftClick?.Invoke(this, e);
        }

        public event EventHandler OnRightClick;
        public event EventHandler OnLeftClick;

    }
}
