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

        // public override SolidColorBrush PrimaryStroke { get; set; } = Brushes.DarkCyan;
        // public override SolidColorBrush SelectedStroke { get; set; } = Brushes.White;

        public Models.AnimationGroup AnimationGroupModel { get; set; }
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

        protected static Random random = new Random();
        public AnimGroupCanvasItem(Models.AnimationGroup model) : base()
        {
            AnimationGroupModel = model;
            TheTextBlock.Text = model.Title;
            TheTextBlock.Visibility = System.Windows.Visibility.Visible;
            
            Rect bounds = model.Frames[0].Rect;

            model.Frames.ForEach(i => bounds.Union(i.Rect));
            Rect = bounds;
        }


        /// <summary>
        /// Creates an animation group item based on a list of canvas items. 
        /// Determines the bounding box around all supplied items by min'ing between the current bounds and the current items bounds.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static AnimGroupCanvasItem AnimGroupFromFrames(Models.AnimationGroup model, double scaleX)
        {
            AnimGroupCanvasItem newItem = new AnimGroupCanvasItem(model)
            {
                StrokeThickness = scaleX,
                PrimaryStroke = new SolidColorBrush(Color.FromRgb((byte)random.Next(100, Byte.MaxValue), (byte)random.Next(100, Byte.MaxValue), (byte)random.Next(100, Byte.MaxValue)))
            };

            return newItem;
        }
    }
}
