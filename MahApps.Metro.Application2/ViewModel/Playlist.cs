using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using System.IO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace MyWindowsMediaPlayer
{
    public partial class MainWindow : MetroWindow
    {
        #region GetPlaylistFromFile
        public void GetPlaylistFromFile()
        {
            try
            {
                string[] lines = File.ReadLines("Playlist_file.txt").ToArray();
                if (lines.Length > 0)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (File.Exists(lines[i].Split(',')[1]) == true)
                        {
                            list_Playlist.Add(new PlaylistList { path = lines[i].Split(',')[1], Title = lines[i].Split(',')[0] });
                        }
                    }
                    Playlist_list.DataContext = list_Playlist;
                }
            }
            catch { }
        }
        #endregion

        #region SavePlaylist
        public void Save_Playlist()
        {
            List<string> Play = new List<string>();
            int i = 0;
            foreach (PlaylistList item in list_Playlist)
            {
                if (i != 0)
                    Play.Add(item.Title + ',' + item.path);
                i++;
            }
            File.WriteAllLines("Playlist_file.txt", Play);
        }
        #endregion

        #region playlist I/E
        private void Export_XML(string path)
        {
            ObservableCollection<ListGrid> save = new ObservableCollection<ListGrid>();
            foreach (ListGrid mp3 in MyMP3LIST)
            {
                save.Add(new ListGrid { Title = mp3.Title, Album = mp3.Album, Composer = mp3.Composer, Length = mp3.Length, Path = mp3.Path, Size = mp3.Size });
            }
            string filename = path;
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<ListGrid>));

            using (StreamWriter wr = new StreamWriter(filename))
                xs.Serialize(wr, save);
        }
        private void Export_M3U(string path)
        {
            List<string> toto = new List<string>();
            List<string> mp3 = new List<string>();

            foreach (ListGrid Item in MyMP3LIST)
            {
                mp3.Add("#EXTINF:" + Item.Length.ToString() + ',' + Item.Composer + " - " + Item.Album + Environment.NewLine + Item.Path);
            }
            toto.Add("#EXTM3U");
            File.WriteAllLines(path, toto);
            File.AppendAllLines(path, mp3);
        }
        private void Export_playlist(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "m3u playlist | *.m3u | xml playlist | *.xml";
            saveFileDialog1.Title = "Enregistrer la playlist";
            saveFileDialog1.FileName = "";
            Nullable<bool> result = saveFileDialog1.ShowDialog();

            if (result == true && Path.GetExtension(saveFileDialog1.FileName) == ".xml")
                Export_XML(saveFileDialog1.FileName);
            else if (result == true)
                Export_M3U(saveFileDialog1.FileName);
        }
        private void add_to_playlist(string info, string path)
        {
            TimeSpan span;
            ListGrid l = new ListGrid();

            string[] spl;
            if (String.IsNullOrEmpty(info) != true)
            {
                if ((spl = info.Split(',')) != null)
                {
                    System.Windows.Controls.Image img = GetIcon(path);
                    l.IconUri = img.Source;
                    FileInfo t = new FileInfo(path);
                    l.Title = t.Name;
                    spl[0] = spl[0].Replace("#EXTINF:", "");
                    TimeSpan.TryParse(spl[0], out span);
                    l.Length = span;
                    l.Composer = spl[1].Split('-')[0];
                    l.Album = spl[1].Split('-')[1];
                    l.Path = path;
                    MyMP3LIST.Add(l);
                    ListM.DataContext = MyMP3LIST;
                }
            }
        }
        private void Import_playlistFromCombo(object sender, RoutedEventArgs e)
        {
            string path = sender.ToString(); // Get the path of the playlist file
            if (Path.GetExtension(path) == ".xml")
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<ListGrid>));

                using (StreamReader rd = new StreamReader(path))
                {
                    ObservableCollection<ListGrid> p = xs.Deserialize(rd) as ObservableCollection<ListGrid>;
                    foreach (ListGrid m in p)
                        MyMP3LIST.Add(m);
                    ListM.DataContext = MyMP3LIST;
                }
            }
            else
            {
                var lineCount = File.ReadLines(path).Count();
                using (var sr = new StreamReader(path))
                {
                    sr.ReadLine();
                    for (int i = 1; i < lineCount; i++)
                        add_to_playlist(sr.ReadLine(), sr.ReadLine());
                }
            }
        }
        private void Import_playlist(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "m3u playlist (.m3u)|*.m3u|xml playlist (.xml)|*.xml";
            Nullable<bool> result = openFileDialog1.ShowDialog();
            if (result == true)
            {
                list_Playlist.Add(new PlaylistList { path = openFileDialog1.FileName, Title = openFileDialog1.SafeFileName });
                Playlist_list.DataContext = list_Playlist;
            }
        }
        #endregion

        #region Event playlist
        private int getPositionFromPlaylist(string path, int L)
        {
            if (L == 1)
            {
                for (int i = 0; i < MyMP3LIST.Count; i++)
                    if (MyMP3LIST[i].Path == path)
                        return i;
                return -1;
            }
            else if (L == 2)
            {
                for (int i = 0; i < MyVIDLIST.Count; i++)
                    if (MyVIDLIST[i].Path == path)
                        return i;
                return -1;
            }
            else if (L == 3)
            {
                for (int i = 0; i < MyIMGLIST.Count; i++)
                    if (MyIMGLIST[i].Path == path)
                        return i;
                return -1;
            }
            return -1;
        }
        private void PlaylistPlayMusique(string path)
        {
            if (path != "")
            {
                ImageMP3.Source = GetIcon(path).Source;

                musiquePlayer.Source = new Uri(path);
                if (musiquePlayer.NaturalDuration.HasTimeSpan)
                {
                    TimeSpan ts = TimeSpan.FromMilliseconds(musiquePlayer.NaturalDuration.TimeSpan.TotalMilliseconds);
                    slider.Maximum = ts.TotalSeconds;
                }
                musiquePlayer.Stretch = Stretch.Fill;
                musiquePlayer.Play();
                _MusicInPlayer = true;
                _PlayedFromPlaylist = true;
                setInformationsList(MyMP3LIST[_PlaylistPosition]);
            }
            else
            {
                MessageBox.Show("Error while opening file");
            }
        }
        private void PlaylistPlayVideo(string path)
        {
            if (path != "")
            {
                videoPlayer.Source = new Uri(path);
                if (getMediaPlayed().NaturalDuration.HasTimeSpan)
                {
                    TimeSpan ts = TimeSpan.FromMilliseconds(getMediaPlayed().NaturalDuration.TimeSpan.TotalMilliseconds);
                    slider.Maximum = ts.TotalSeconds;
                }
                videoPlayer.Stretch = Stretch.Fill;
                videoPlayer.Play();
                _VideoInPlayer = true;
                _PlayedFromPlaylist = true;
                if (Path.GetExtension(path) == ".avi" || (Path.GetExtension(path) == ".mp4" || (Path.GetExtension(path) == ".wmv" || (Path.GetExtension(path) == ".mkv"))))
                {
                    if (!this._isPlaying)
                    {
                        this._isPlaying = true;
                        ListV.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                MessageBox.Show("Error while opening file");
            }
        }
        private void ListM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListM.SelectedIndex != -1)
            {
                ListGrid media = (ListGrid)ListM.SelectedItems[0];
                string path = media.Path;
                setInformationsList(media);
                _PlaylistPosition = getPositionFromPlaylist(path, 1);
                PlaylistPlayMusique(path);
            }
        }
        private void ListV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListV.SelectedIndex != -1)
            {
                ListVideo media = (ListVideo)ListV.SelectedItems[0];
                string path = media.Path;
                Label_Titre.Content = "Titre : " + media.TitleV;
                string olen = media.LengthV.ToString();
                string len = olen.Split('.')[0];
                Label_Duree.Content = "Durée : " + len;
                Label_Taille.Content = "Taille (ko) : " + media.Size.ToString();
                _PlaylistPosition = getPositionFromPlaylist(media.Path, 2);
                PlaylistPlayVideo(path);
            }
        }
        private void ListI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListI.SelectedIndex != -1)
            {
                ListImage media = (ListImage)ListI.SelectedItems[0];
                string path = media.Path;
                imageFile.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
                _isPlaying = true;
                backImage.Visibility = Visibility.Hidden;
            }

        }
        private void Change_Playlist(object sender, SelectionChangedEventArgs e)
        {
            if (Playlist_list.SelectedIndex != -1)
            {
                MyMP3LIST.Clear();
                PlaylistList media = (PlaylistList)Playlist_list.SelectedItem;
                string path = media.path; // Get the path of the playlist file
                if (File.Exists(media.path))
                {
                    if (Path.GetExtension(path) == ".xml")
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<ListGrid>));

                        using (StreamReader rd = new StreamReader(path))
                        {
                            ObservableCollection<ListGrid> p = xs.Deserialize(rd) as ObservableCollection<ListGrid>;
                            foreach (ListGrid m in p)
                                MyMP3LIST.Add(m);
                            ListM.DataContext = MyMP3LIST;
                        }

                    }
                    else
                    {
                        var lineCount = File.ReadLines(path).Count();
                        using (var sr = new StreamReader(path))
                        {
                            sr.ReadLine();
                            for (int i = 1; i < lineCount; i++)
                                add_to_playlist(sr.ReadLine(), sr.ReadLine());
                        }
                    }
                }
                else
                {
                    list_Playlist.Remove(media);
                    Playlist_list.DataContext = list_Playlist;
                }
            }
        }
        private void Random_Set(object sender, RoutedEventArgs e)
        {

            if (_isRandom == true)
            {
                _isRandom = false;
                RandomIMG.Source = new BitmapImage(new Uri("../Images/appbar.refresh.png", UriKind.Relative));
            }
            else
            {
                _isRandom = true;
                RandomIMG.Source = new BitmapImage(new Uri("../Images/appbar.refresh.lock.png", UriKind.Relative));
            }
        }
        #endregion

        #region Delete From Playlist
        private void ListM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && ListM.SelectedIndex != -1)
            {
                ListGrid media = (ListGrid)ListM.SelectedItems[0];
                string path = media.Path;
                MyMP3LIST.RemoveAt(getPositionFromPlaylist(path, 1));
                if (list_Playlist.Count > 0)
                {
                    PlaylistList Play = (PlaylistList)Playlist_list.SelectedItem;
                    if (Path.GetExtension(Play.path) == ".xml")
                        Export_XML(Play.path);
                    else if (Path.GetExtension(Play.path) == ".m3u")
                        Export_M3U(Play.path);
                }
                button_stop(sender, e);
            }
        }
        private void ListV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && ListV.SelectedIndex != -1)
            {
                ListVideo media = (ListVideo)ListV.SelectedItems[0];
                string path = media.Path;
                MyVIDLIST.RemoveAt(getPositionFromPlaylist(path, 2));
                button_stop(sender, e);
            }
        }
        private void ListI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && ListI.SelectedIndex != -1)
            {
                ListImage media = (ListImage)ListI.SelectedItems[0];
                string path = media.Path;
                MyIMGLIST.RemoveAt(getPositionFromPlaylist(path, 3));
                button_stop(sender, e);
            }
        }
        #endregion

        #region Search in Playlist
        private void Text_Update(object sender, TextChangedEventArgs e)
        {
            if (TabM.IsSelected == true)
            {
                var TMP = MyMP3LIST;
                switch (Filter_Search.SelectedItem.ToString())
                {
                    case "Titre":
                        TMP = new ObservableCollection<ListGrid>(from mp3 in MyMP3LIST where mp3.Title.Contains(search.Text) select mp3);
                        break;
                    case "Artiste":
                        TMP = new ObservableCollection<ListGrid>(from mp3 in MyMP3LIST where mp3.Composer.Contains(search.Text) select mp3);
                        break;
                    case "Album":
                        TMP = new ObservableCollection<ListGrid>(from mp3 in MyMP3LIST where mp3.Album.Contains(search.Text) select mp3);
                        break;
                }
                Mp3ListTMP = TMP;
                ListM.DataContext = TMP;
            }
            else if (TabV.IsSelected == true)
            {
                var TMP = MyVIDLIST;
                TMP = new ObservableCollection<ListVideo>(from vid in MyVIDLIST where vid.TitleV.Contains(search.Text) select vid);
                VidListTMP = TMP;
                ListV.DataContext = TMP;
            }
            else if (TabI.IsSelected == true)
            {
                var TMP = MyIMGLIST;
                TMP = new ObservableCollection<ListImage>(from img in MyIMGLIST where img.TitleI.Contains(search.Text) select img);
                ImgListTMP = TMP;
                ListI.DataContext = TMP;
            }
        }
        private void inizializeComboSearch()
        {
            ComboSearch.Add("Titre");
            ComboSearch.Add("Artiste");
            ComboSearch.Add("Album");
            Filter_Search.DataContext = ComboSearch;
        }
        #endregion

        #region Add From Playlist
        private void AddToPlaylistM()
        {
            PlaylistList media = (PlaylistList)Playlist_list.SelectedItem;
            string path = media.path;
            if (Path.GetExtension(path) == ".m3u")
                Export_M3U(path);
            else if (Path.GetExtension(path) == ".xml")
                Export_XML(path);
        }
        #endregion
        private void initializeDefaultPlaylist()
        {
            list_Playlist.Add(new PlaylistList { path = "temp_default_playlist.m3u", Title = "Default" });
            Playlist_list.DataContext = list_Playlist;
            Playlist_list.SelectedIndex = 0;
            Export_M3U("temp_default_playlist.m3u");
        }
    }
}
