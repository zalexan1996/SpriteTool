using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SpriteTool
{
    /// <summary>
    /// The data behind the TrackItem control.
    /// </summary>
    public class TrackItemModel : INotifyPropertyChanged
    {
        #region Events
        /// <summary>
        /// The EventHandler for when a property in this model changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        /// <summary>
        /// The name of the animation track.
        /// </summary>
        public string TrackName
        {
            get
            {
                return _trackName;
            }
            set
            {
                _trackName = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TrackName)));
            }
        } private string _trackName;

        /// <summary>
        /// Whether this animation track is being played in the animation previewer.
        /// </summary>
        public bool IsPreviewing
        {
            get
            {
                return _isPreviewing;
            }
            set
            {
                _isPreviewing = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPreviewing)));
            }
        } private bool _isPreviewing = false;

        /// <summary>
        /// A collection of frames in the animation
        /// </summary>
        public ObservableCollection<string> FramePaths { get; set; } = new ObservableCollection<string>();
        
        #endregion


        #region Helper Properties
        public int FrameCount { get { return FramePaths.Count; } }


        public List<Image> Images => FramePaths.Select(path => new Image() { Source = new BitmapImage(new Uri(path)) } ).ToList();
        #endregion
    }
}
