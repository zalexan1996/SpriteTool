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

        public enum IInputMode
        {
            PanMode,
            SelectMode
        }
        public IInputMode InputMode
        {
            get
            {
                return _inputMode;
            }
            set
            {
                _inputMode = value;
            }
        }
        private IInputMode _inputMode = IInputMode.SelectMode;

        // A list of all items added to the canvas (frames and animation groups)
        public List<CanvasItem> CanvasItems { get; set; } = new List<CanvasItem>();

        // An array of selected canvas items
        private List<CanvasItem> SelectedItems = new List<CanvasItem>();



        public ImageCanvas()
        {
            InitializeComponent();
        }

        public void AddCanvasItem(Point position, double width, double height)
        {
            // Get the scaling factors for the current window strech
            double sX = ScaleXRatio;
            double sY = ScaleYRatio;
            Rect r = new Rect(position.X * sX, position.Y * sY, width * sX, height * sY);
            CanvasItem item = CanvasItem.Create(r, ScaleXRatio, Brushes.Cyan);
       
            item.OnRightClick += OnCanvasItemRightClicked;
            item.OnLeftClick += OnCanvasItemLeftClicked;

            TheCanvas.Children.Add(item);
            CanvasItems.Add(item);
        }

        public void AddCanvasItem(CanvasItem item)
        {
            TheCanvas.Children.Add(item);
            CanvasItems.Add(item);
        }
        public void RemoveCanvasItem(CanvasItem item)
        {
            TheCanvas.Children.Remove(item);
            CanvasItems.Remove(item);
        }



        public void ClearCanvasItems()
        {
            // Remove all Sprite rectangles from the canvas.
            while (CanvasItems.Count > 0)
            {
                TheCanvas.Children.RemoveAt(0);
            }

            // Clear the local list of sprite rectangles.
            CanvasItems.Clear();
        }

        public void SelectAll()
        {
            Console.WriteLine("Select All");
            ClearSelectedItems();
            CanvasItems.ForEach(i => AddItemToSelection(i));
        }



        public void CombineSelectedItems()
        {
            
            if (SelectedItems.Count <= 1)
            {
                ClearSelectedItems();
                return;
            }
            else
            {
                CombineItems(SelectedItems[0], SelectedItems[1]);
                RemoveItemFromSelection(SelectedItems[1]);
                RemoveItemFromSelection(SelectedItems[0]);
                CombineSelectedItems();
            }
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
            if (CanvasItems.Count <= 0)
                return null;

            // Initialize the output array.
            List<Types.MappedBitmapImage> output = new List<Types.MappedBitmapImage>();
            
            // Convert each rectangle to a MappedBitmapImage
            output = CanvasItems.Select(r => new Types.MappedBitmapImage()
            {
                Source = _image,
                X = (int)(Canvas.GetLeft(r) / ScaleXRatio),
                Y = (int)(Canvas.GetTop(r) / ScaleYRatio),
                Width = (int)(r.Width / ScaleXRatio),
                Height = (int)(r.Height / ScaleYRatio)
            }).ToList();
            
            
            return output;
        }



        protected void AddItemToSelection(CanvasItem item)
        {
            item.Selected();
            SelectedItems.Add(item);
        }
        protected void RemoveItemFromSelection(CanvasItem item)
        {
            item.Unselected();
            SelectedItems.Remove(item);
        }
        public void ClearSelectedItems()
        {
            while (SelectedItems.Count > 0)
            {
                SelectedItems[0].Unselected();
                SelectedItems.Remove(SelectedItems[0]);
            }
        }

        private void CombineItems(CanvasItem item1, CanvasItem item2)
        {
            // Add the new rectangle to the canvas.
            AddCanvasItem(CanvasItem.Combine(item1, item2, ScaleXRatio, ScaleYRatio));


            // Remove the old rectangles.
            RemoveCanvasItem(item1);
            RemoveCanvasItem(item2);
        }


        public Point GetPoint(int element)
        {
            return new Point(Canvas.GetLeft(CanvasItems[element]) / ScaleXRatio, Canvas.GetTop(CanvasItems[element]) / ScaleYRatio);
        }

        private void OnCanvasItemRightClicked(object sender, EventArgs e)
        {
            
            CanvasItem item = sender as CanvasItem;
            if (item == null)
                return;
        }
        private void OnCanvasItemLeftClicked(object sender, EventArgs e)
        {
            
            CanvasItem item = sender as CanvasItem;
            if (item == null)
                return;

            switch (InputMode)
            {
                case IInputMode.PanMode:
                    break;


                case IInputMode.SelectMode:

                    if (!Keyboard.IsKeyDown(Key.LeftShift))
                    {
                        ClearSelectedItems();
                    }
                    AddItemToSelection(item);
                    break;
            }
        }
    }
}
