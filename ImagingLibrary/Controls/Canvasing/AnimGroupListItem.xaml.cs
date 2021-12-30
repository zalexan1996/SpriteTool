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

namespace ImagingLibrary.Controls.Canvasing
{
    /// <summary>
    /// Interaction logic for AnimGroupListItem.xaml
    /// </summary>
    public partial class AnimGroupListItem : UserControl
    {


        // Model
        public Models.AnimationGroup AnimationGroup
        {
            get
            {
                return _animationGroup;
            }
            set
            {
                _animationGroup = value;
            }
        } private Models.AnimationGroup _animationGroup;




        public AnimGroupListItem(Models.AnimationGroup animationGroup)
        {
            InitializeComponent();
            AnimationGroup = animationGroup;
            DataContext = AnimationGroup;
            tbTitle.Text = AnimationGroup.Title;


            AnimationGroup.MappedBitmaps.ForEach(b =>
                spFrames.Children.Add(new Image() { Source = b.MappedImage})
            );
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
