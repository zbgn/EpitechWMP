using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Reflection;
using System.Windows.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Media;

namespace MyWindowsMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>  
    /// 

    public partial class MainWindow : MetroWindow
    {

        #region var
        private DispatcherTimer timer = new DispatcherTimer();

        /* Listener */
        private string _CurrentMedia = string.Empty;
        private string _TimeLabelDuration = string.Empty;
        private double _MediaDuration = 0;
        public int _PlaylistPosition = 0;
        public bool _MusicInPlayer = false;
        public bool _VideoInPlayer = false;
        public bool _PlayedFromPlaylist = false;
        private Boolean _isMute;
        private Boolean _isPlaying;
        public bool _isRandom = false;
        public TimeSpan TimeoutToHide { get; private set; }
        public DateTime LastMouseMove { get; private set; }
        public bool IsHidden { get; private set; }

        /* Full Screen */
        bool IsFullScreen_ = false;

        /* PlayList Object */
        public ObservableCollection<ListGrid> MyMP3LIST { get; set; }
        public MediaElement MyMediaPlayer { get; private set; }

        public ObservableCollection<ListGrid> Mp3ListTMP = new ObservableCollection<ListGrid>();
        public ObservableCollection<string> ComboSearch = new ObservableCollection<string>();
        public ObservableCollection<PlaylistList> list_Playlist = new ObservableCollection<PlaylistList>();
        public ObservableCollection<ListVideo> MyVIDLIST = new ObservableCollection<ListVideo>();
        public ObservableCollection<ListVideo> VidListTMP = new ObservableCollection<ListVideo>();
        public ObservableCollection<ListImage> MyIMGLIST = new ObservableCollection<ListImage>();
        public ObservableCollection<ListImage> ImgListTMP = new ObservableCollection<ListImage>();

        #endregion

        #region MainWindow
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1); // L’intervalle entre chaque tick du timer sera d’une seconde
            timer.Tick += new EventHandler(timer_Tick); // A chaque tick, on déclenche l’évènement timer_Tick
            this.MyMP3LIST = new ObservableCollection<ListGrid>();
            initializeDefaultPlaylist();
            GetPlaylistFromFile();
            inizializeComboSearch();
            musiquePlayer.Source = null;
            videoPlayer.Source = null;
            ImageMP3.Source = new BitmapImage(new Uri("../Images/unknown.png", UriKind.Relative));
        }
        #endregion

        System.Windows.Controls.MediaElement getMediaPlayed()
        {
            if (musiquePlayer.Source == null)
                return videoPlayer;
            else
                return musiquePlayer;
        }
        private void Click_OpenLibrary(object sender, RoutedEventArgs args)
        {
            Library lib = new Library(musiquePlayer, imageFile, videoPlayer);
            lib.Show();

        }



        #region GetICON
        private System.Windows.Controls.Image GetIcon(string path)
        {
            Image img = new Image();
            if (Path.GetExtension(path) != ".mkv")
            {
                TagLib.File file = TagLib.File.Create(path);
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    TagLib.IPicture pic = file.Tag.Pictures[0];
                    MemoryStream ms = new MemoryStream(pic.Data.Data);
                    ms.Seek(0, SeekOrigin.Begin);
                    bitmap.BeginInit();
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    img.Source = bitmap;
                }
                catch
                {
                    BitmapImage logo = new BitmapImage(new Uri("pack://application:,,,/Resources/unknown.png", UriKind.RelativeOrAbsolute));
                    img.Source = logo;
                }
                return img;
            }
            return img;
        }
        #endregion

        #region Toggle
        private void ToggleFullscreen()
        {

            if (IsFullScreen_ == false)
            {
                this.ResizeMode = ResizeMode.CanResize;
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
            }
        }
        private void UnToggleFullscreen()
        {

            if (IsFullScreen_ == true)
            {
                this.WindowState = WindowState.Normal;
      
                this.ResizeMode = System.Windows.ResizeMode.CanResize;             
            }
     
        }
        private void ButtonGrid_OnMouseEnter(object sender, MouseEventArgs e)
        {
           if (videoPlayer.Source != null)
                Player_Control.Opacity = 100;
        }
        private void ButtonGrid_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (videoPlayer.Source != null)
                Player_Control.Opacity = 0;
        }
        private void Fullscreen_Toggle(object sender, RoutedEventArgs e)
        {
            ToggleFullscreen();
        }
        private void MPlayer_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ClickCount == 2 && IsFullScreen_ == true)
            {
                UnToggleFullscreen();
                IsFullScreen_ = false;
            }

           else if (e.ClickCount == 2 && IsFullScreen_ == false)
            {
                ToggleFullscreen();
                IsFullScreen_ = true;

            }                 
        }
        #endregion

        #region exit
        public void Exit(object sender, RoutedEventArgs e)
        {
            Save_Playlist();
            Close();
        }
        #endregion

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Save_Playlist();
        }

        private void DL_codecs_for_media(string nameSpace, string outDirectory, string internalFilePath, string ressourceName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            using (Stream s = assembly.GetManifestResourceStream(nameSpace + "." + (internalFilePath == "" ? "" : internalFilePath + ".") + ressourceName))
            using (BinaryReader r = new BinaryReader(s))
            using (FileStream fs = new FileStream(outDirectory + "\\" + ressourceName, FileMode.OpenOrCreate))
            using (BinaryWriter w = new BinaryWriter(fs))
                w.Write(r.ReadBytes((int)s.Length));
        }

        private void DL_codecs(object sender, RoutedEventArgs e)
        {
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = Path.Combine(pathUser, "Downloads");
            string pathDesktop = Path.Combine(pathUser, "Desktop");
            DL_codecs_for_media("MyWindowsMediaPlayer", pathDownload, "Resources", "Media_Player_Codecs.exe");
            MessageBox.Show("Les codecs ont bien été enregistrés dans : " + (pathDownload == "" ? pathDesktop : pathDownload) + Environment.NewLine + "Vous devez les installer pour qu'ils soient fonctionnels.");
        }

        private void Open_Info(object sender, RoutedEventArgs e)
        {
            string info = "";
            info += "Groupe: yezegu_j - frayss_v - rivier_b - beauge_z" + Environment.NewLine + Environment.NewLine;
            info += "Si vous avez des problèmes de lecture vidéos, merci d'installer les codecs fournit (Fichier -> Autres -> Télécharger les codecs...)." + Environment.NewLine + Environment.NewLine;
            info += "Contactez-nous: yezegu_j@epitech.eu (chef de groupe).";
            MessageBox.Show(info, "Informations: My Windows Media Player v2.0", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MetroAnimatedTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl) //if this event fired from TabControl then enter
            {
                recherche.Visibility = Visibility.Visible;
                Filter_Search.Visibility = Visibility.Visible;
                if (TabM.IsSelected)
                {
                    videoPlayer.Source = null;
                    button_stop(null, null);
                }
                if (TabV.IsSelected)
                {
                    musiquePlayer.Source = null;
                    button_stop(null, null);
                    recherche.Visibility = Visibility.Hidden;
                    Filter_Search.Visibility = Visibility.Hidden;
                }
                if (TabI.IsSelected)
                {
                    musiquePlayer.Source = null;
                    button_stop(null, null);
                    recherche.Visibility = Visibility.Hidden;
                    Filter_Search.Visibility = Visibility.Hidden;
                }
            }
        }

        private void videoOpened(object sender, RoutedEventArgs e)
        {
            if (videoPlayer.NaturalDuration.HasTimeSpan)// Musique ou Vid
            {
                timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                timer.Start();
                _MediaDuration = videoPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                Player_Control.Opacity = 0;
                backVideo.Visibility = Visibility.Hidden;
            }
        }

        private void videoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            videoPlayer.Stop();
            videoPlayer.Source = null;
            _VideoInPlayer = false;
            backVideo.Visibility = Visibility.Visible;
            if (_PlayedFromPlaylist == true)
            {
                if (_PlaylistPosition < MyVIDLIST.Count - 1)
                {
                    if (_isRandom == false)
                        _PlaylistPosition += 1;
                    else
                        _PlaylistPosition = random_func(MyVIDLIST.Count);
                    ListV.SelectedItem = MyVIDLIST[_PlaylistPosition];
                    _VideoInPlayer = true;
                }
                else if (_PlaylistPosition == MyVIDLIST.Count - 1)
                {
                    backVideo.Visibility = Visibility.Visible;
                    button_stop(null, null);
                }
            }
        }

        private void musiquePlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            musiquePlayer.Stop();
            musiquePlayer.Source = null;
            _MusicInPlayer = false;

            if (_PlayedFromPlaylist == true)
            {
                if (_PlaylistPosition < MyMP3LIST.Count - 1)
                {
                    if (_isRandom == false)
                        _PlaylistPosition += 1;
                    else
                        _PlaylistPosition = random_func(MyMP3LIST.Count);
                    ListM.SelectedItem = MyMP3LIST[_PlaylistPosition];
                    _MusicInPlayer = true;
                }
                else if (_PlaylistPosition == MyMP3LIST.Count - 1)
                    button_stop(null, null);
            }
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var settingInput = new MetroDialogSettings()
            {
                AffirmativeButtonText = "OK",
                NegativeButtonText = "CANCEL",
                AnimateHide = true,
                AnimateShow = true,
                ColorScheme = MetroDialogColorScheme.Accented,
            };
            var settingOK = new MetroDialogSettings()
            {
                AffirmativeButtonText = "OK",
                AnimateHide = true,
                AnimateShow = true,
                ColorScheme = MetroDialogColorScheme.Accented,
            };
            string toto = "";
            toto = await this.ShowInputAsync("Flux radio", "Entrer l'URL d'un flux Radio", settingInput);
            if (toto != null)
            {
                ListM.Visibility = Visibility.Visible;
                try
                {
                    musiquePlayer.Source = new Uri(toto.ToString());
                    musiquePlayer.Play();
                    await this.ShowMessageAsync("Radio", "Lecture en cours ...", MessageDialogStyle.Affirmative, settingOK);
                }
                catch
                {
                    string erreur = "L'url : " + toto.ToString() + " n'est pas valide.";
                    await this.ShowMessageAsync("Url non valide", erreur.ToString(), MessageDialogStyle.Affirmative, settingOK);
                }
            }
        }
    }
}
