using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace SpriteTool.UserControls
{
    /// <summary>
    /// Interaction logic for FileExplorer.xaml
    /// </summary>
    public partial class FileExplorer : UserControl
    {
        public event ItemClickedHandler ItemClicked;
        public delegate void ItemClickedHandler(FileExplorerItem item, EventArgs e);
        public event FileExplorerItem.NewTrackHandler SelectionToAnimationTrackEvent;


        public static List<string> SupportedExtensions = new List<string>(){ "jpg", "png", "bmp" };



        public ObservableCollection<FileExplorerItem> SelectedItems { get; set; } = new ObservableCollection<FileExplorerItem>();


        public FileExplorer()
        {
            InitializeComponent();
        }

        public bool IsFolderValid() { return WorkspaceFolder.Length > 0; }

        public string WorkspaceFolder
        {
            get
            {
                return _workspaceFolder;
            }
            set
            {
                _workspaceFolder = value;
                if (IsFolderValid())
                {
                    // Repopulate the file list.
                    BTN_OpenFolder.Visibility = Visibility.Collapsed;
                    B_FileList.Visibility = Visibility.Visible;
                }
                else
                {
                    // Clear the file list and display the button
                    BTN_OpenFolder.Visibility = Visibility.Visible;
                    B_FileList.Visibility = Visibility.Collapsed;
                }
            }
        } private string _workspaceFolder;



        private void BTN_OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Open Folder Dialog");

            System.Windows.Forms.FolderBrowserDialog d = new System.Windows.Forms.FolderBrowserDialog();
            
            System.Windows.Forms.DialogResult result = d.ShowDialog();

            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    string folder = d.SelectedPath;

                    WorkspaceFolder = folder;

                    //
                    System.Threading.Tasks.Task.Run(new Action(PopulateFiles));


                    break;

                default:
                    WorkspaceFolder = "";
                    WP_FolderList.Children.Clear();
                    break;
            }
        }

        
        public void PopulateFiles()
        {
            //WP_FolderList.Children.Clear();
            var files = System.IO.Directory.EnumerateFiles(WorkspaceFolder);

            foreach (string file in files)
            {
                string ext = file.Substring(file.LastIndexOf('.') + 1).ToLower();
                if (SupportedExtensions.Contains(ext))
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(()=>
                    {
                        // Create a new item.
                        FileExplorerItem i = new FileExplorerItem(file, this);

                        // Setup a clicked handler.
                        i.Clicked += new FileExplorerItem.ClickedHandler(EventOnItemClicked);

                        // Pass the add to new animation track event up the chain.
                        i.AddToNewAnimationTrack += (name, e) => { SelectionToAnimationTrackEvent?.Invoke(name, e); };

                        // Add it to the list.
                        WP_FolderList.Children.Add(i);
                    }));
                }
            }
        }


        protected void EventOnItemClicked(FileExplorerItem item, EventArgs e)
        {
            if (item != null)
            {


                // If we clicked while shift is down, multi-select.
                if ((Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) > 0 || (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) > 0)
                {
                    item.IsSelected = !item.IsSelected;
                    if (!item.IsSelected)
                    {
                        SelectedItems.Remove(item);
                    }
                    else
                    {
                        SelectedItems.Add(item);
                    }
                }
                else
                {
                    foreach (var i in SelectedItems)
                    {
                        i.IsSelected = false;
                    }
                    SelectedItems.Clear();

                    item.IsSelected = true;
                    SelectedItems.Add(item);
                }
                



                ItemClicked(item, e);
                
            }
        }
        
    }
}
