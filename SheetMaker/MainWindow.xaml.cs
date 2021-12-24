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
using ImagingLibrary.Types;

namespace SheetMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<MappedBitmapImage> Frames = new List<MappedBitmapImage>();

        public string WorkspaceDirectory { get; set; } = "C:\\temp";
        public string BaseName { get; set; }


        public MainWindow(BitmapImage bitmapImage, string workspaceDirectory, string baseName)
        {
            InitializeComponent();
            ImageCanvas.Image = bitmapImage;
            WorkspaceDirectory = workspaceDirectory;
            BaseName = baseName;


            CdPropWidth.Width = new GridLength(200);

            tbNumSprites.Text = "0";
            tbOffsetX.Text = "0";
            tbOffsetY.Text = "0";
            tbSpriteWidth.Text = "32";
            tbSpriteHeight.Text = "64";
            tbGap.Text = "5";
        }
        public MainWindow()
        {
            InitializeComponent();
            ImageCanvas.Image = new BitmapImage(new Uri("pack://application:,,,/SheetMaker;component/Sheets/Link3.png"));


            CdPropWidth.Width = new GridLength(200);

            tbNumSprites.Text = "1";
            tbOffsetX.Text = "100";
            tbOffsetY.Text = "100";
            tbSpriteWidth.Text = "50";
            tbSpriteHeight.Text = "50";
            tbGap.Text = "5";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ImagePreviewer.Children.Clear();
            Frames.AddRange(ImageCanvas.RectanglesToImages());

            if (Frames != null)
            {
                foreach (var r in Frames)
                {
                    Image i = new Image();
                    i.Source = r.MappedImage;
                    ImagePreviewer.Children.Add(i);

                    r.X = (int)(r.X);
                    r.Y = (int)(r.Y);
                    r.Width = (int)(r.Width);
                    r.Height = (int)(r.Height);


                }
                btnExport.IsEnabled = true;
                btnClear.IsEnabled = true;
            }
        }
        
        private void RefreshSpritePreviews()
        {

            int offsetX, offsetY, spriteWidth, spriteHeight, numSprites, gap;

            // Verify that all of our values are valid before we try adjusting the previews.
            if (Int32.TryParse(tbOffsetX.Text, out offsetX) &&
                Int32.TryParse(tbOffsetY.Text, out offsetY) &&
                Int32.TryParse(tbSpriteWidth.Text, out spriteWidth) &&
                Int32.TryParse(tbSpriteHeight.Text, out spriteHeight) &&
                Int32.TryParse(tbNumSprites.Text, out numSprites) &&
                Int32.TryParse(tbGap.Text, out gap))
            {


                ImageCanvas.ClearCanvasItems<ImagingLibrary.Controls.Canvasing.CanvasItem>();


                BitmapSource source = ImageCanvas.Image as BitmapSource;

                if (cbMultiline.IsChecked.Value)
                {
                    int maxSpritesWidth = (source.PixelWidth - offsetX + gap) / (spriteWidth + gap);
                    Console.WriteLine(maxSpritesWidth);
                    for (int i = 0; i < numSprites; i++)
                    {
                        int xP = i % maxSpritesWidth;
                        int yP = i / maxSpritesWidth;
                        int x = offsetX + (xP * spriteWidth) + (xP * gap);
                        int y = offsetY + (yP * spriteHeight) + (yP * gap);
                        ImageCanvas.AddCanvasItem(new Point(x, y), spriteWidth, spriteHeight);
                    }
                }
                else
                {
                    for (int i = 0; i < numSprites; i++)
                    {
                        int x = offsetX + (i * spriteWidth) + (i * gap);
                        int y = offsetY;
                        ImageCanvas.AddCanvasItem(new Point(x, y), spriteWidth, spriteHeight);
                    }
                }
            }

        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshSpritePreviews();
        }

        private void CbMultiline_IsCheckedChanged(object sender, RoutedEventArgs e)
        {
            RefreshSpritePreviews();
        }

        private void BtnIslandDetection_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvas.ClearCanvasItems<ImagingLibrary.Controls.Canvasing.CanvasItem>();
            ImagingLibrary.Algorithms.IslandDetectionAlgorithm islandDetection = new ImagingLibrary.Algorithms.IslandDetectionAlgorithm()
            {
                BackgroundColor = new SolidColorBrush(Colors.Black)
            };
            

            List<Rect> rects = islandDetection.Execute(ImageCanvas.Image);

            if (rects.Count > 0)
            {

                rects.ForEach(r =>
                {
                    ImageCanvas.AddCanvasItem(r.TopLeft, r.Width, r.Height);
                    
                });
            }
            
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            // Clear the WrapPanel
            ImagePreviewer.Children.Clear();
            Frames.Clear();
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            // Export all mapped images to files
            for (int i = 0; i < Frames.Count; i++)
            {
                Frames[i].SaveToFile($"{WorkspaceDirectory}\\{BaseName}_{i}.png");
            }
        }




        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvas.SelectAll();
        }
        private void SelectAllAnimations_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvas.SelectAll();
        }
        private void SelectAllFrames_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvas.SelectAll();
        }
        private void SelectNone_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvas.ClearSelectedItems();
        }

        private void RemoveSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvas.RemoveSelectedItemsFromCanvas();
        }

        private void CombineSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvas.CombineSelectedItems();
        }

        private void NewAnimFromSelection_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SelectTool_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvas.BeginSelectBoxMode();
        }
    }
}
