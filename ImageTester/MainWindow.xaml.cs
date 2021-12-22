using System.Windows;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            using (Image image = Image.Load("ZoomOut.png"))
            {
                image.Mutate(x => x.Resize(image.Width * 4, image.Height * 4));

                image.Save("output.png");
            }
        }
    }
}