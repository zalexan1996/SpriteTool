using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using System.ComponentModel;

namespace SpriteTool.UserControls
{


    /// <summary>
    /// Interaction logic for FilePreview.xaml
    /// </summary>
    public partial class FileExplorerItem : UserControl, INotifyPropertyChanged
    {

        public delegate void ClickedHandler(FileExplorerItem item, EventArgs e);
        public delegate void NewTrackHandler(string TrackName, EventArgs e);

        public event NewTrackHandler AddToNewAnimationTrack;
        public event ClickedHandler SelectedChanged;
        public event ClickedHandler Clicked;
        public event PropertyChangedEventHandler PropertyChanged;




        protected FileExplorer ExplorerParent;



        public FileExplorerItem(string filePath, FileExplorer explorerParent)
        {
            InitializeComponent();
            FilePath = filePath;
            ExplorerParent = explorerParent;

            ImportPreview();
        }


        public string BaseName
        {
            get
            {
                int slashIndex = FilePath.LastIndexOf("\\");
                int periodIndex = FilePath.LastIndexOf('.');
                return FilePath.Substring(slashIndex + 1,periodIndex - slashIndex - 1);
            }
        }
        public string DirectoryName
        {
            get
            {
                return FilePath.Substring(0, FilePath.LastIndexOf("\\"));
            }
        }
        public string Extension
        {
            get
            {
                return FilePath.Substring(FilePath.LastIndexOf(".") + 1);
            }
        }
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;

                TB_FileName.Text = BaseName;
            }
        } private string _filePath;

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;

                // Alert that the IsSelected property was changed.
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                SelectedChanged?.Invoke(this, new EventArgs());
            }
        } private bool _isSelected = false;


        public bool UsePreview { get; set; }


        public void ImportPreview()
        {
            I_Preview.Source = new BitmapImage(new System.Uri(FilePath));
        }

        private void UserControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Show the right-click context menu.
            (sender as FileExplorerItem).ContextMenu = (ContextMenu)Resources["contextMenu"];

        }

        private void ExtractSprites_Click(object sender, RoutedEventArgs e)
        {
            SheetMaker.MainWindow sheetMakerWindow = new SheetMaker.MainWindow(I_Preview.Source as BitmapImage, DirectoryName, BaseName);
            sheetMakerWindow.ShowDialog();

            if (sheetMakerWindow.Frames.Count > 0)
            {
                MainWindow w = App.Current.MainWindow as MainWindow;
                if (w != null)
                {
                    w.fileExplorer.WP_FolderList.Children.Clear();
                    w.fileExplorer.PopulateFiles();
                }
            }
        }

        
        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }

        private void AddToNewAnimationTrack_Click(object sender, RoutedEventArgs e)
        {
            AddToNewAnimationTrack?.Invoke("Track1", e);
        }
    }
}
