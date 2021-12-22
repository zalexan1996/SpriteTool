using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SpriteTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static AnimationPreviewer AnimationPreviewerReference { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            
            fileExplorer.ItemClicked += new UserControls.FileExplorer.ItemClickedHandler(ChangePreviewImage);
            fileExplorer.SelectionToAnimationTrackEvent += FileExplorer_SelectionToAnimationTrackEvent;
            AnimationPreviewerReference = AP_Previewer;
        }

        private void FileExplorer_SelectionToAnimationTrackEvent(string TrackName, EventArgs e)
        {

            trackPanel.AddTrack(new TrackItemModel()
            {
                TrackName = TrackName,
                FramePaths = new ObservableCollection<string>(fileExplorer.SelectedItems.Select(fItem => fItem.FilePath))
            });
        }

        protected void ChangePreviewImage(UserControls.FileExplorerItem item, EventArgs e)
        {
            if (item != null)
            {
                CenterImage.Source = item.I_Preview.Source;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AnimationPreviewer animationPreviewer = MainWindow.AnimationPreviewerReference;
            List<Image> images = animationPreviewer.PreviewingTrackItemModel.Images;
            BitmapSource bs = ImagingLibrary.ImageFactory.CombineImages(images[0].Source as BitmapSource, images[1].Source as BitmapSource);
            ImagingLibrary.ImageFactory.BitmapSourceToFile(bs, "C:\\temp\\Test.png");
        }
    }

}
