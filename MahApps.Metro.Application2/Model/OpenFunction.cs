using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using WMPLib;

namespace MyWindowsMediaPlayer
{
    public partial class MainWindow : MetroWindow
    {

        public void setInformations(string path)
        {
            if (Path.GetExtension(path) == ".mp3" || Path.GetExtension(path) == ".wav" || Path.GetExtension(path) == ".m4a")
            {
                TagLib.File f = TagLib.File.Create(path);
                FileInfo file = new FileInfo(path);
                try
                {
                    Label_Titre.Content = "Titre : " + f.Tag.Title;
                    Label_Artiste.Content = "Artiste : " + f.Tag.AlbumArtists[0];
                    Label_Album.Content = "Album : " + f.Tag.Album;
                    Label_Duree.Content = "Durée : " + f.Properties.Duration.ToString().Split('.')[0];
                    Label_Taille.Content = "Taille (ko) : " + file.Length / 1000;
                }
                catch
                {
                    var duration = f.Properties.Duration;
                    Label_Titre.Content = "Titre : " + file.Name;
                    Label_Artiste.Content = "Artiste : Unknow";
                    Label_Album.Content = "Album : " + "Unknow";
                    Label_Duree.Content = "Durée : " + duration.ToString().Split('.')[0];
                    Label_Taille.Content = "Taille (ko) : " + file.Length / 1000;
                }
            }

        }

        public void setInformationsList(ListGrid media)
        {
            if (Path.GetExtension(media.Path) == ".mp3" || Path.GetExtension(media.Path) == ".wav" || Path.GetExtension(media.Path) == ".m4a")
            {
                FileInfo f = new FileInfo(media.Path);
                Label_Titre.Content = "Titre : " + media.Title;
                Label_Artiste.Content = "Artiste : " + media.Composer;
                Label_Album.Content = "Album : " + media.Album;
                Label_Duree.Content = "Durée : " + media.Length.ToString().Split('.')[0];
                Label_Taille.Content = "Taille (ko) : " + f.Length / 1000;
                ImageMP3.Source = media.IconUri;
            }
        }
    }
}

