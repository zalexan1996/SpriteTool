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

using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SpriteTool
{
    /// <summary>
    /// Interaction logic for TrackPanel.xaml
    /// </summary>
    public partial class TrackPanel : UserControl
    {
        

        


        public TrackPanel()
        {
            InitializeComponent();
        }


        

        protected List<TrackItem> Items = new List<TrackItem>();


        public void AddTrack(TrackItemModel track)
        {
            // If this track name exists, add the frames to the existing track.
            if (DoesTrackExist(track.TrackName))
            {
                TrackItem existingTrack = Items.First(i => i.ItemModel.TrackName == track.TrackName);
                
                foreach (var path in track.FramePaths)
                {
                    existingTrack.ItemModel.FramePaths.Add(path);
                }
            }

            // Else, create a new track.
            else
            {
                TrackItem item = new TrackItem(track);

                Items.Add(item);
                Sp_Tracks.Children.Add(item);
            }

        }

        public void RemoveTrack(TrackItem item)
        {
            Items.Remove(item);
            Sp_Tracks.Children.Remove(item);
        }
        
        
        public bool DoesTrackExist(string name)
        {
            return Items.Where(i => i.ItemModel.TrackName == name).ToList().Count > 0;
        }
    }
}
