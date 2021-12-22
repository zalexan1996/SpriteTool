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

namespace SpriteTool.UserControls.Layout
{
    /// <summary>
    /// Interaction logic for Pane.xaml
    /// </summary>
    public partial class Pane : Decorator
    {
        public Pane()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            PathGeometry g = new PathGeometry();

        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            return base.ArrangeOverride(arrangeSize);
        }
    }
}
