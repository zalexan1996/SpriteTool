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



        // Box Select Mode Members
        public bool InSelectBoxMode { get; set; } = false;
        Point _rectSelectStart = new Point(-1, -1);
        Point _rectSelectEnd = new Point(-1, -1);
        Rectangle _selectRectangle;
        public void BeginSelectBoxMode()
        {
            InSelectBoxMode = true;
            zoomBorder.AcceptMouseInput = false;
        }
        public void EndSelectBoxMode()
        {
            InSelectBoxMode = false;
            _rectSelectStart.X = _rectSelectStart.Y = _rectSelectEnd.X = _rectSelectEnd.Y = -1;
            _selectRectangle.Width = _selectRectangle.Height = 0;
            _selectRectangle.Visibility = Visibility.Collapsed;
            zoomBorder.AcceptMouseInput = true;
        }
        public ImageCanvas()
        {
            InitializeComponent();
            _selectRectangle = new Rectangle()
            {
                Stroke = Brushes.LightGray,
                StrokeDashArray = DoubleCollection.Parse("1, 1")
            };
            TheCanvas.Children.Add(_selectRectangle);
            EndSelectBoxMode();
        }

        public void AddCanvasItem(Point position, double width, double height)
        {
            // Get the scaling factors for the current window strech
            double sX = ScaleXRatio;
            double sY = ScaleYRatio;
            Rect r = new Rect(position.X * sX, position.Y * sY, width * sX, height * sY);
            CanvasItem item = CanvasItem.Create(r, ScaleXRatio);
       
            item.OnRightClick += OnCanvasItemRightClicked;
            item.OnLeftClick += OnCanvasItemLeftClicked;

            TheCanvas.Children.Add(item);
            CanvasItems.Add(item);
        }

        public void AddCanvasItem(CanvasItem item)
        {
            item.OnRightClick += OnCanvasItemRightClicked;
            item.OnLeftClick += OnCanvasItemLeftClicked;

            TheCanvas.Children.Add(item);
            CanvasItems.Add(item);
        }
        public void RemoveCanvasItem(CanvasItem item)
        {
            TheCanvas.Children.Remove(item);
            CanvasItems.Remove(item);
            RemoveItemFromSelection(item);
        }



        public void ClearCanvasItems<T>()
        {
            // Remove all Sprite rectangles from the canvas.
            for (int i = TheCanvas.Children.Count - 1; i >= 0; i--)
            {
                if (TheCanvas.Children[i].GetType() == typeof(T))
                {
                    TheCanvas.Children.RemoveAt(i);
                }
            }

            // Clear the local list of sprite rectangles.
            CanvasItems.Clear();
        }

        public void RemoveSelectedItemsFromCanvas()
        {
            for (int i = SelectedItems.Count - 1; i >= 0; --i)
                RemoveCanvasItem(SelectedItems[i]);
        }

        public void SelectAll()
        {
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
            CanvasItem newItem = CanvasItem.Combine(item1, item2, ScaleXRatio, ScaleYRatio);
            AddCanvasItem(newItem);
            AddItemToSelection(newItem);

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

        private void TheCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // If we are in select mode and we have a valid starting point
            if (InSelectBoxMode && _rectSelectStart != new Point(0, 0))
            {
                Point MousePos = Mouse.GetPosition(TheCanvas);
                _rectSelectEnd = new Point(MousePos.X, MousePos.Y);

                _selectRectangle.Visibility = Visibility.Collapsed;
                InSelectBoxMode = false;
                zoomBorder.AcceptMouseInput = true;

                EndSelectBoxMode();
            }
        }

        private void TheCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // If we are in select mode and we are starting the rectangle drag 
            if (InSelectBoxMode)
            {
                Point MousePos = Mouse.GetPosition(TheCanvas);
                _rectSelectStart = new Point(MousePos.X, MousePos.Y);
                _selectRectangle.Visibility = Visibility.Visible;
                _selectRectangle.StrokeThickness = ScaleXRatio;
                zoomBorder.AcceptMouseInput = false;
            }
        }

        private void TheCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (InSelectBoxMode && _rectSelectStart.X != -1 && _rectSelectStart.Y != -1)
            {
                Point MouseLocation = e.GetPosition(TheCanvas);
                Rect r = new Rect(
                    Math.Min(_rectSelectStart.X, MouseLocation.X),
                    Math.Min(_rectSelectStart.Y, MouseLocation.Y),
                    Math.Abs(_rectSelectStart.X - MouseLocation.X),
                    Math.Abs(_rectSelectStart.Y - MouseLocation.Y)
                );

                Canvas.SetLeft(_selectRectangle, r.Left);
                Canvas.SetTop(_selectRectangle, r.Top);
                _selectRectangle.Width = r.Width;
                _selectRectangle.Height = r.Height;


                ClearSelectedItems();
                GetItemsUnderRectangle(_selectRectangle).ForEach(i => AddItemToSelection(i));

            }
        }

        private List<CanvasItem> GetItemsUnderRectangle(Rectangle r)
        {
            Rect selectRect = r.RenderedGeometry.Bounds;
            selectRect.X = Canvas.GetLeft(r);
            selectRect.Y = Canvas.GetTop(r);

            return CanvasItems.Where((CanvasItem i) => {

                Rect otherRect = i.TheRectangle.RenderedGeometry.Bounds;
                otherRect.X = Canvas.GetLeft(i);
                otherRect.Y = Canvas.GetTop(i);
                return otherRect.IntersectsWith(selectRect);
            }).ToList();
        }
    }
}
