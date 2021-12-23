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


        public SolidColorBrush PrimaryStroke { get; set; } = Brushes.Cyan;
        public SolidColorBrush SelectedStroke { get; set; } = Brushes.White;


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
                return _rect;
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

        



        public static CanvasItem Create(Rect rect, double strokeThickness, string optionalTitle = null)
        {
            CanvasItem ci = new CanvasItem()
            {
                StrokeThickness = strokeThickness,
                Rect = rect
            };
            ci.Rect = rect;

            return ci;
        }

        public static CanvasItem Combine(CanvasItem item1, CanvasItem item2, double ScaleXRatio, double ScaleYRatio)
        {
            Rect r1 = item1.Rect;
            Rect r2 = item2.Rect;


            // Create the union of the dimensions
            // Rect union = Rect.Union(new Rect(r1.X / ScaleXRatio, r1.Y / ScaleYRatio, r1.Width / ScaleXRatio, r1.Height / ScaleYRatio), new Rect(r2.X / ScaleXRatio, r2.Y / ScaleYRatio, r2.Width / ScaleXRatio, r2.Height / ScaleYRatio));
            Rect union = Rect.Union(new Rect(r1.X, r1.Y, r1.Width, r1.Height), new Rect(r2.X, r2.Y, r2.Width, r2.Height));

            // Return the combined item
            return Create(union, item1.StrokeThickness);
            
        }

        public CanvasItem()
        {
            InitializeComponent();
            IsHitTestVisible = true;

            TheRectangle.Stroke = PrimaryStroke;
        }

        public void Selected()
        {
            TheRectangle.Stroke = SelectedStroke;
        }
        public void Unselected()
        {
            TheRectangle.Stroke = PrimaryStroke;
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
