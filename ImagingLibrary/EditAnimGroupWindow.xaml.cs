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
using System.Windows.Shapes;
using ImagingLibrary.Controls.Canvasing;

namespace ImagingLibrary
{
    /// <summary>
    /// Interaction logic for EditAnimGroupWindow.xaml
    /// </summary>
    public partial class EditAnimGroupWindow : Window
    {
        public AnimGroupListItem ListItem { get; set; }
        public Controls.Canvasing.Models.AnimationGroup NewModel { get; set; }
        private bool WasSaved = false;
        public EditAnimGroupWindow(AnimGroupListItem listItem)
        {
            ListItem = listItem;
            NewModel = listItem.AnimationGroup;
            DataContext = NewModel;
            InitializeComponent();
        }



        public static ImagingLibrary.Controls.Canvasing.Models.AnimationGroup EditListItem(AnimGroupListItem item)
        {
            EditAnimGroupWindow window = new EditAnimGroupWindow(item);
            window.ShowDialog();
            if (window.WasSaved) { return window.NewModel; }
            else { return item.AnimationGroup; }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
            WasSaved = true;
        }
    }
}
