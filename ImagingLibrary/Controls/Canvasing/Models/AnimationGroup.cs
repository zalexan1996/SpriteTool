using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ImagingLibrary.Controls.Canvasing.Models
{
    public class AnimationGroup : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        } private string _title = "Animation Group";


        public List<CanvasItem> Frames
        {
            get
            {
                return _frames;
            }
            set
            {
                _frames = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Frames)));
            }
        } private List<CanvasItem> _frames;


        public int NumFrames { get { return Frames.Count; } }


        public List<Types.MappedBitmapImage> MappedBitmaps { get; set; }

    }
}
