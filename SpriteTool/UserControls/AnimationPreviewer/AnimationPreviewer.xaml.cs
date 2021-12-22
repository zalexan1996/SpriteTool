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

namespace SpriteTool
{
    /// <summary>
    /// Interaction logic for AnimationPreviewer.xaml
    /// </summary>
    public partial class AnimationPreviewer : UserControl, IDisposable
    {
        public enum EPlaybackState {
            Reverse = -1, Pause = 0, Play = 1
        };

        protected EPlaybackState PlaybackState;
        protected System.Timers.Timer timer;
        protected int CurrentImageIndex = 0;
        protected double PlaybackSpeed = 500;

        public AnimationPreviewer()
        {
            InitializeComponent();
            timer = new System.Timers.Timer(PlaybackSpeed);
            timer.Elapsed += OnTimerEvent;
        }

        public void Play()
        {
            PlaybackState = EPlaybackState.Play;
            timer.Start();
        }
        public void Play(TrackItemModel NewModel)
        {
            SetPreviewingTrack(NewModel);
            PlaybackState = EPlaybackState.Play;
            timer.Start();
        }
        public void Pause()
        {
            PlaybackState = EPlaybackState.Pause;
            timer.Stop();
        }
        public void Reverse()
        {
            PlaybackState = EPlaybackState.Reverse;
            timer.Start();
        }

        public TrackItemModel PreviewingTrackItemModel { get; private set; }

        public void SetPreviewingTrack(TrackItemModel model)
        {
            PreviewingTrackItemModel = model;

        }

        private void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }

        private void Btn_Pause_Click(object sender, RoutedEventArgs e)
        {
            Pause();
        }

        private void Btn_Reverse_Click(object sender, RoutedEventArgs e)
        {
            Reverse();
        }


        public static void OnTimerEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            AnimationPreviewer p = MainWindow.AnimationPreviewerReference;
            if (p != null)
            {

                p.CurrentImageIndex = (p.CurrentImageIndex + (int)p.PlaybackState + p.PreviewingTrackItemModel.FrameCount) % p.PreviewingTrackItemModel.FrameCount;
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                    
                        if (
                            p != null && 
                            p.I_FramePreview != null &&
                            p.PreviewingTrackItemModel != null &&
                            p.PreviewingTrackItemModel.Images[p.CurrentImageIndex] != null
                        ) {
                            p.I_FramePreview.Source = p.PreviewingTrackItemModel.Images[p.CurrentImageIndex].Source;
                        }
                    });
                }
            }
        }

        private void Btn_Slow_Click(object sender, RoutedEventArgs e)
        {
            PlaybackSpeed *= 1.25;
            timer.Interval = PlaybackSpeed;
        }

        private void Btn_Faster_Click(object sender, RoutedEventArgs e)
        {
            PlaybackSpeed /= 1.25;
            timer.Interval = PlaybackSpeed;
        }

        public void Dispose()
        {
            timer.Stop();
        }
    }
}
