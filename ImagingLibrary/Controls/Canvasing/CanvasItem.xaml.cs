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
        


        public CanvasItem()
        {
            InitializeComponent();
        }
    }
}
