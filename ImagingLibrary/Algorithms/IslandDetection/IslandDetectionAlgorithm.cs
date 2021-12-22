using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagingLibrary.Types;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ImagingLibrary.Algorithms
{
    public class IslandDetectionAlgorithm : ImageDetectionAlgorithm
    {

        protected delegate bool IsPixelBackgroundDelegate(byte r, byte g, byte b, byte a);

        public List<Rect> Execute(BitmapImage image)
        {
            
            // Return null if the image is invalid.
            if (image == null)
                return null;

            // Try to get the BitmapImage source.
            source = image;
            bps = source.Format.BitsPerPixel;
            

            IsPixelBackgroundDelegate isPixelBackground;

            if (BackgroundIsAlpha)
            {
                isPixelBackground = new IsPixelBackgroundDelegate((R,G,B,A) => { return A  > -BackgroundThreshold && A < BackgroundThreshold; });
            }
            else
            {
                isPixelBackground = new IsPixelBackgroundDelegate((R, G, B, A) =>
                {
                    return BackgroundColor.Color.R == R && BackgroundColor.Color.G == G && BackgroundColor.Color.B == B;
                });
            }
           

            // Create the output array.
            List<Rect> output = new List<Rect>();

            // (pixelWidth * 32 + 7 ) / 8;
            stride = (pixelWidth * 32) / 8;//((bps + 7) / bps) * pixelWidth;

            pixelData = new byte[stride * pixelHeight];
            source.CopyPixels(pixelData, stride, 0);
            
            
            for (int i = 0; i < pixelData.Length; i += 4)
            {
                byte b = pixelData[i];
                byte g = pixelData[i + 1];
                byte r = pixelData[i + 2];
                byte a = pixelData[i + 3];


                
                if (!isPixelBackground(r, g, b, a))
                {
                    Point point = PointFromIndex(i);

                    if (output.Any((rect) => { return rect.Contains(point); })) {  }
                    else
                    {

                        // Initialize the containers for this run.
                        Initialize();
                        travelledPoints.Add(point);

                        // Potentially expand the bounds.
                        TopLeftBound.X = Math.Min(TopLeftBound.X, point.X);
                        TopLeftBound.Y = Math.Min(TopLeftBound.Y, point.Y);
                        BottomRightBound.X = Math.Max(BottomRightBound.X, point.X);
                        BottomRightBound.Y = Math.Max(BottomRightBound.Y, point.Y);

                        // Tree search recursively to determine the bounds of this island.
                        TreeSearch(point, isPixelBackground);
                        if (TopLeftBound == new Point() { X = pixelWidth, Y = pixelHeight } && BottomRightBound == new Point())
                        {
                            // Invalid box
                        }
                        else
                        {
                            // Add the bounds to the output array.
                            output.Add(new Rect(TopLeftBound, AddPoints(BottomRightBound, new Point(1,1))));
                            
                            
                        }
                    }

                }
                
            }
            
            return output;
        }

        #region Cached Data
        BitmapImage source = null;
        int stride = 0;
        byte[] pixelData;
        int bps = 0;
        int pixelWidth { get { return source.PixelWidth; } }
        int pixelHeight { get { return source.PixelHeight; } }

        // Clockwise starting at directly to the right. This order allows us to naturally trace clockwise.
        List<Point> QueryOrder = new List<Point>()
        {

            new Point(0, -1),
            new Point(1, -1),
            new Point(1,0),
            new Point(1,1),
            new Point(0,1),
            new Point(-1,1),
            new Point(-1, 0),
            new Point(-1, -1)
        };

        #endregion Cached Data

        #region Helper Functions
        protected void Initialize()
        {
            // Initialize the values for this run.
            TopLeftBound = new Point() { X = pixelWidth, Y = pixelHeight };
            BottomRightBound = new Point();
            travelledPoints = new List<Point>();
            stride = ((source.Format.BitsPerPixel + 7) / 8) * pixelWidth;
        }
        

        Point TopLeftBound;
        Point BottomRightBound;

        List<Point> travelledPoints;
        protected void TreeSearch(Point currentPoint, IsPixelBackgroundDelegate isBackground)
        {
            // Add the current point to the list of points.
            travelledPoints.Add(currentPoint);

            // Iterate through all adjacent points.
            for (int i = 0; i < QueryOrder.Count; i++)
            {
                // Store the next point
                Point nextPoint = AddPoints(currentPoint, QueryOrder[i]);

                // If we haven't visited this point before
                if (!travelledPoints.Contains(nextPoint))
                {
                    // Get the next point's index in the pixelData array
                    int nIndex = IndexFromPoint(nextPoint);


                    if (nIndex + 3 < pixelData.Length && nIndex >= 0)
                    {
                        byte r, g, b, a;
                        r = pixelData[nIndex + 2];
                        g = pixelData[nIndex + 1];
                        b = pixelData[nIndex];
                        a = pixelData[nIndex + 3];



                        //  if the point isn't a background pixel.
                        if (!travelledPoints.Contains(nextPoint) && !isBackground(r, g, b, a))
                        {

                            // Potentially expand the bounds.
                            TopLeftBound.X = Math.Min(TopLeftBound.X, nextPoint.X);
                            TopLeftBound.Y = Math.Min(TopLeftBound.Y, nextPoint.Y);
                            BottomRightBound.X = Math.Max(BottomRightBound.X, nextPoint.X);
                            BottomRightBound.Y = Math.Max(BottomRightBound.Y, nextPoint.Y);


                            // TreeSearch the next point.
                            TreeSearch(nextPoint, isBackground);
                        }
                    }
                }
            }
        }

        protected Point PointFromIndex(int index)
        {
            return new Point((index / 4) % pixelWidth, (index / 4) / pixelWidth);
        }
        protected int IndexFromPoint(Point point)
        {
            return (int)(point.X + point.Y * pixelWidth) * 4;
        }

        protected Point AddPoints(Point p1, Point p2)
        {
            p1.X += p2.X;
            p1.Y += p2.Y;
            return p1;
        }
        protected bool IsPointValid(Point point)
        {
            return point.X >= 0 && point.X < pixelWidth && point.Y >= 0 && point.Y < pixelHeight;
        }
        #endregion Helper Functions


        #region Properties
        /// <summary>
        /// The background color that will be used to distinguish between islands.
        /// </summary>
        public SolidColorBrush BackgroundColor = new SolidColorBrush(Colors.Magenta);

        /// <summary>
        /// Whether we treat the alpha-channel as the background.
        /// </summary>
        public bool BackgroundIsAlpha { get; set; } = true;

        /// <summary>
        /// The threshold decides the background variance. Maybe we want to treat alpha values up to 10 as background.
        /// </summary>
        public double BackgroundThreshold { get; set; } = 20.0;

        /// <summary>
        /// The island threshold determines how many pixels of background around the island we consider to be still the island. 
        /// This is useful for individual sprite frames that consist of multiple islands.
        /// </summary>
        public double IslandThreshold { get; set; } = 0.0;
        #endregion Properties
    }
}
