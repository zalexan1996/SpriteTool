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
using System.Collections.ObjectModel;

using System.ComponentModel;

namespace SpriteTool
{
    /// <summary>
    /// Interaction logic for TrackItem.xaml
    /// </summary>
    public partial class TrackItem : UserControl
    {
        public TrackItemModel ItemModel { get; set; } = new TrackItemModel();
        

        public TrackItem(TrackItemModel trackItemModel)
        {
            ItemModel = trackItemModel;
            ItemModel.FramePaths.CollectionChanged += FramePaths_CollectionChanged;

            InitializeComponent();

            UpdateFrames();
        }

        private void FramePaths_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateFrames();
        }

        protected void UpdateFrames()
        {
            spFramePanel.Children.Clear();
            foreach(var i in ItemModel.Images)
            {
                spFramePanel.Children.Add(i);
            }
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {

            AnimationPreviewer animationPreviewer = MainWindow.AnimationPreviewerReference;
            animationPreviewer?.Play(ItemModel);
        }
    }
}