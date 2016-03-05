using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

class MyMedia
{
    public string setTitle { get; set; }
    public string setArtist { get; set; }
    public string setAlbum { get; set; }
    public string path { get; set; }
}

namespace MyWindowsMediaPlayer
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Library : MetroWindow
    {
        private string[] _itemMediaLib;

        private MediaElement _libMusic;
        private Image image;
        private MediaElement _libVideo;

        public Library(MediaElement music, Image img, MediaElement video)
        {
            InitializeComponent();
            _libMusic = music;
            image = img;
            _libVideo = video;
            loadAllData();
        }

        private void loadAllData(string option = "Music")
        {
            try
            {
                string[] mediaFiles = Directory.GetFiles(@Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\" + option, "*.*", SearchOption.AllDirectories);
                _itemMediaLib = mediaFiles;
                foreach (string name in mediaFiles)
                {
                    FileInfo info = new FileInfo(name);
                    libList.Items.Add(new MyMedia() { setTitle = info.Name, setArtist = "", setAlbum = "", path = name });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        private string getItemPath(int index)
        {
            try
            {
                return _itemMediaLib[index];
            }
            catch
            {
                return "";
            }
        }
        private void Select_Element(object sender, SelectionChangedEventArgs args)
        {
            if (libList.SelectedItems.Count > 0)
            {
                MyMedia media = (MyMedia)libList.SelectedItems[0];
                string extention = System.IO.Path.GetExtension(media.path);

                if (string.Equals(extention, ".jpg", StringComparison.CurrentCultureIgnoreCase) ||
                           string.Equals(extention, ".png", StringComparison.CurrentCultureIgnoreCase) ||
                           string.Equals(extention, ".bmp", StringComparison.CurrentCultureIgnoreCase))
                {
                    image.Source = new BitmapImage(new Uri(media.path));
                }
                if (string.Equals(extention, ".mp3", StringComparison.CurrentCultureIgnoreCase) ||
                  string.Equals(extention, ".wma", StringComparison.CurrentCultureIgnoreCase) ||
                  string.Equals(extention, ".m4a", StringComparison.CurrentCultureIgnoreCase))
                {
                    _libMusic.Source = new Uri(media.path);
                    _libMusic.Play();                  
                }

                if (string.Equals(extention, ".mp4", StringComparison.CurrentCultureIgnoreCase) ||
                            string.Equals(extention, ".avi", StringComparison.CurrentCultureIgnoreCase) ||
                            string.Equals(extention, ".wmv", StringComparison.CurrentCultureIgnoreCase) ||
                            string.Equals(extention, ".mkv", StringComparison.CurrentCultureIgnoreCase))
                {
                    _libVideo.Source = new Uri(media.path);
                    _libVideo.Play();
                    
                }

            }
        }

        private void Click_Musics(object sender, RoutedEventArgs args)
        {
            libList.Items.Clear();
            _itemMediaLib = null;
            loadAllData("Music");
        }

        private void Click_Movies(object sender, RoutedEventArgs args)
        {
            libList.Items.Clear();
            _itemMediaLib = null;
            loadAllData("Videos");
        }

        private void Click_Pictures(object sender, RoutedEventArgs args)
        {
            libList.Items.Clear();
            _itemMediaLib = null;
            loadAllData("Pictures");
        }
    }
}
