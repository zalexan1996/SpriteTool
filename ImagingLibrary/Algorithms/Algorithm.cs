using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ImagingLibrary.Algorithms
{
    public interface ImageDetectionAlgorithm
    {
        List<Rect> Execute(BitmapImage image);
    }
}
