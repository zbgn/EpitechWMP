using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using System.IO;
using Microsoft.Win32;
using WMPLib;
using System.Windows.Media.Imaging;

namespace MyWindowsMediaPlayer
{
    public partial class MainWindow : MetroWindow
    {
        #region importer
        public void ImportFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Musiques (*.mp3 *.wma *.m4a)|*.mp3;*.wma;*.m4a|Vidéos (*.mp4 *.avi *.wmv *.mkv)|*.mp4;*.avi;*.wmv;*.mkv|Images (*.jpg *.png *.bmp)|*.jpg;*.png;*.bmp";
            if (openFileDialog1.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog1.FileNames)
                {
                    string Ftitle = "";
                    string Falbum = "";
                    string Fcomposer = "";
                    long Fsize = 0;
                    Image imagemp3 = new Image();
                    imagemp3.Source = new BitmapImage(new Uri("../Images/unknown.png", UriKind.Relative));
                    FileInfo file = new FileInfo(filename);
                    var player = new WindowsMediaPlayer();
                    var clip = player.newMedia(filename);
                    var durationFile = TimeSpan.FromSeconds(clip.duration);
                    //------------------------ SECTEUR DES MEDIA MUSIQUES ------------------------ \\
                    if (System.IO.Path.GetExtension(filename) == ".mp3" || System.IO.Path.GetExtension(filename) == ".wma" || System.IO.Path.GetExtension(filename) == ".m4a")
                    {
                        TagLib.File f = TagLib.File.Create(filename);
                        var duration = f.Properties.Duration;
                        try
                        {
                            imagemp3 = GetIcon(filename);
                            Falbum = f.Tag.Album;
                            Ftitle = f.Tag.Title;
                            Fcomposer = f.Tag.AlbumArtists[0];
                            Fsize = file.Length / 1000;
                        }
                        catch
                        {
                            Ftitle = file.Name;
                            Falbum = "Unknow";
                            Fcomposer = "Unknow";
                        }
                       
                            ListGrid l = new ListGrid();
                            l.IconUri = imagemp3.Source;
                            l.Title = Ftitle;
                            l.Length = duration;
                            l.Album = Falbum;
                            l.Composer = Fcomposer;
                            l.Path = filename;
                            l.Size = Fsize;
                            if (MP3Equal(l) == false)
                            {
                                MyMP3LIST.Add(l);
                                ListM.DataContext = MyMP3LIST;
                            }
                        AddToPlaylistM();
                    }
                    //------------------------ SECTEUR DES MEDIA VIDEOS ------------------------ \\
                    else if (System.IO.Path.GetExtension(filename) == ".avi" || System.IO.Path.GetExtension(filename) == ".mp4"
                            || System.IO.Path.GetExtension(filename) == ".wmv" || System.IO.Path.GetExtension(filename) == ".mkv")
                    {
                        string title = "";
                        long size = 0;
                        System.TimeSpan dur = durationFile;
                        try
                        {
                            title = file.Name;
                            dur = durationFile;
                            size = file.Length / 1000;
                        }
                        catch
                        {
                            title = "Unknow";
                            size = 0;
                        }
                        ListVideo l = new ListVideo();
                        l.TitleV = title;
                        l.LengthV = durationFile;
                        l.Path = filename;
                        l.Size = size;
                        if (VIDEqual(l) == false)
                        {
                            MyVIDLIST.Add(l);
                            ListV.DataContext = MyVIDLIST;
                        }
                    }
                    //------------------------ SECTEUR DES MEDIA IMAGES ------------------------ \\
                    else if (System.IO.Path.GetExtension(filename).ToLower() == ".jpg" || System.IO.Path.GetExtension(filename) == ".png"
                            || System.IO.Path.GetExtension(filename) == ".bmp")
                    {
                        string title = "";
                        string format = "";
                        long size = 0;
                        try
                        {
                            title = file.Name;
                            format = System.IO.Path.GetExtension(filename);
                            size = file.Length / 1000;
                            title = title.Replace(format, "");
                        }
                        catch
                        {
                            title = "Unknow";
                            format = "Unknow";
                            size = 0;
                        }
                        ListImage l = new ListImage();
                        l.TitleI = title;
                        l.FormatI = format;
                        l.SizeI = size;
                        l.Path = filename;
                        MyIMGLIST.Add(l);
                        ListI.DataContext = MyIMGLIST;
                    }
                }

            }
        }

        #endregion
        public void ImportMP3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Musiques (*.mp3 *.wma *.m4a)|*.mp3;*.wma;*.m4a";
            openFileDialog1.Title = "Importer des musiques";
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)))
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); ;
            if (openFileDialog1.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog1.FileNames)
                {
                    string Ftitle = "";
                    string Falbum = "";
                    string Fcomposer = "";
                    long Fsize = 0;
                    Image imagemp3 = new Image();
                    FileInfo file = new FileInfo(filename);
                    var player = new WindowsMediaPlayer();
                    var clip = player.newMedia(filename);
                    var durationFile = TimeSpan.FromSeconds(clip.duration);
                    TagLib.File f = TagLib.File.Create(filename);
                    var duration = f.Properties.Duration;
                    try
                    {
                        imagemp3 = GetIcon(filename);
                        Falbum = f.Tag.Album;
                        Ftitle = f.Tag.Title;
                        Fcomposer = f.Tag.AlbumArtists[0];
                        Fsize = file.Length / 1000;
                    }
                    catch
                    {
                        imagemp3 = GetIcon(filename);
                        Ftitle = file.Name;
                        Falbum = "Unknow";
                        Fcomposer = "Unknow";
                    }
                    ListGrid l = new ListGrid();
                    l.IconUri = imagemp3.Source;
                    l.Title = Ftitle;
                    l.Length = duration;
                    l.Album = Falbum;
                    l.Composer = Fcomposer;
                    l.Path = filename;
                    l.Size = Fsize;
                    if (MP3Equal(l) == false)
                    {
                        MyMP3LIST.Add(l);
                        ListM.DataContext = MyMP3LIST;
                    }
                    AddToPlaylistM();
                }
            }
        }
        private void ImportVideo(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Vidéos (*.mp4 *.avi *.wmv *.mkv)|*.mp4;*.avi;*.wmv;*.mkv";
            openFileDialog1.Title = "Importer des vidéos";
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)))
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            if (openFileDialog1.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog1.FileNames)
                {
                    FileInfo file = new FileInfo(filename);
                    var player = new WindowsMediaPlayer();
                    var clip = player.newMedia(filename);
                    var durationFile = TimeSpan.FromSeconds(clip.duration);
                    string title = "";
                    long size = 0;
                    System.TimeSpan dur = durationFile;
                    try
                    {
                        title = file.Name;
                        dur = durationFile;
                        size = file.Length / 1000;
                    }
                    catch
                    {
                        title = "Unknow";
                        size = 0;
                    }
                    ListVideo l = new ListVideo();
                    l.TitleV = title;
                    l.LengthV = durationFile;
                    l.Path = filename;
                    l.Size = size;
                    if (VIDEqual(l) == false)
                    {
                        MyVIDLIST.Add(l);
                        ListV.DataContext = MyVIDLIST;
                    }
                }
            }
        }
        private void ImportImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            openFileDialog1.Filter = "Images (*.jpg *.png *.bmp)|*.jpg;*.png;*.bmp";
            openFileDialog1.Title = "Importer des images";
            ListI.Visibility = Visibility.Visible;
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))
                openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (openFileDialog1.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog1.FileNames)
                {
                    FileInfo file = new FileInfo(filename);
                    string title = "";
                    string format = "";
                    long size = 0;
                    try
                    {
                        title = file.Name;
                        format = System.IO.Path.GetExtension(filename);
                        size = file.Length / 1000;
                        title = title.Replace(format, "");
                    }
                    catch
                    {
                        title = "Unknow";
                        format = "Unknow";
                        size = 0;
                    }
                    ListImage l = new ListImage();
                    l.TitleI = title;
                    l.FormatI = format;
                    l.SizeI = size;
                    l.Path = filename;
                    MyIMGLIST.Add(l);
                    ListI.DataContext = MyIMGLIST;
                }
            }
        }
        #region TestImport
        private bool MP3Equal(ListGrid l)
        {
            foreach (ListGrid i in MyMP3LIST)
            {
                if (i.Path == l.Path)
                    return true;
            }
            return false;
        }
        private bool VIDEqual(ListVideo l)
        {
            foreach (ListVideo i in MyVIDLIST)
            {
                if (i.Path == l.Path)
                    return true;
            }
            return false;
        }
        #endregion

    }
}
