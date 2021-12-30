using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace ImagingLibrary.Controls.Canvasing
{
    public class AnimGroupCanvasItem : CanvasItem
    {

        public override SolidColorBrush PrimaryStroke { get; set; } = Brushes.DarkCyan;
        public override SolidColorBrush SelectedStroke { get; set; } = Brushes.White;

        public string Text
        {
            get
            {
                return TheTextBlock.Text;
            }
            set
            {
                TheTextBlock.Text = value;
            }
        }

        protected List<CanvasItem> Items;

        public AnimGroupCanvasItem(List<CanvasItem> items)
            : base()
        {
            Items = items;
            TheTextBlock.Visibility = System.Windows.Visibility.Visible;
            
            Rect bounds = items[0].Rect;
            Random random = new Random();
            PrimaryStroke = new SolidColorBrush(Color.FromRgb((byte)random.Next(100, Byte.MaxValue), (byte)random.Next(100, Byte.MaxValue), (byte)random.Next(100, Byte.MaxValue)));

            foreach (CanvasItem item in items)
            {
                /*
                bounds.X = Math.Min(bounds.X, item.Rect.X);
                bounds.Y = Math.Min(bounds.Y, item.Rect.Y);
                
                bounds.Width = Math.Max(bounds.Width, (item.Rect.X + item.Rect.Width) - bounds.X);
                bounds.Height = Math.Max(bounds.Height,(item.Rect.Y + item.Rect.Height) - bounds.Y);
                
                Console.WriteLine($"\nOther: {item.Rect}");
                */


                bounds.Union(item.Rect);
            }
            Rect = bounds;
        }


        /// <summary>
        /// Creates an animation group item based on a list of canvas items. 
        /// Determines the bounding box around all supplied items by min'ing between the current bounds and the current items bounds.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static AnimGroupCanvasItem AnimGroupFromFrames(List<CanvasItem> items, double scaleX, double scaleY, string title = "Animation Group")
        {
            AnimGroupCanvasItem newItem = new AnimGroupCanvasItem(items)
            {
                Text = title,
                StrokeThickness = scaleX
            };

            return newItem;
        }
    }
}
