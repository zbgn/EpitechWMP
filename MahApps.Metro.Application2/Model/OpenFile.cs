using System;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace MyWindowsMediaPlayer
{
    public partial class MainWindow : MetroWindow
    {
        #region ouvrir
        public void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Musiques (*.mp3 *.wma *.m4a)|*.mp3;*.wma;*.m4a|Vidéos (*.mp4 *.avi *.wmv *.mkv)|*.mp4;*.avi;*.wmv;*.mkv|Images (*.jpg *.png *.bmp)|*.jpg;*.png;*.bmp";
            if (openFileDialog1.ShowDialog() == true)
            {
                string extention = System.IO.Path.GetExtension(openFileDialog1.FileName);

                //------------------------ SECTEUR DES MEDIA MUSIQUES ET VIDEO ------------------------ \\
                if (string.Equals(extention, ".mp3", StringComparison.CurrentCultureIgnoreCase)
                    || string.Equals(extention, ".wma", StringComparison.CurrentCultureIgnoreCase)
                    || string.Equals(extention, ".m4a", StringComparison.CurrentCultureIgnoreCase))
                {
                    TabM.IsSelected = true;
                    ImageMP3.Source = GetIcon(openFileDialog1.FileName).Source;
                    musiquePlayer.Source = new Uri(openFileDialog1.FileName);
                    if (musiquePlayer.NaturalDuration.HasTimeSpan)
                    {
                        TimeSpan ts = TimeSpan.FromMilliseconds(musiquePlayer.NaturalDuration.TimeSpan.TotalMilliseconds);
                        slider.Maximum = ts.TotalSeconds;
                    }
                    musiquePlayer.Play();
                    setInformations(openFileDialog1.FileName);
                }
                else if (string.Equals(extention, ".mp4", StringComparison.CurrentCultureIgnoreCase) ||
                        string.Equals(extention, ".avi", StringComparison.CurrentCultureIgnoreCase) ||
                        string.Equals(extention, ".wmv", StringComparison.CurrentCultureIgnoreCase) ||
                        string.Equals(extention, ".mkv", StringComparison.CurrentCultureIgnoreCase)
                    )
                {          
                    if (videoPlayer.NaturalDuration.HasTimeSpan)
                    {
                        TimeSpan ts = TimeSpan.FromMilliseconds(videoPlayer.NaturalDuration.TimeSpan.TotalMilliseconds);
                        slider.Maximum = ts.TotalSeconds;
                    }
                    ListV.Visibility = Visibility.Collapsed;
                    TabV.IsSelected = true;
                    videoPlayer.Source = new Uri(openFileDialog1.FileName);
                   
                    videoPlayer.Play();
                }

                else if (string.Equals(extention, ".jpg", StringComparison.CurrentCultureIgnoreCase) ||
                       string.Equals(extention, ".png", StringComparison.CurrentCultureIgnoreCase) ||
                       string.Equals(extention, ".bmp", StringComparison.CurrentCultureIgnoreCase))
                {
                    TabI.IsSelected = true;
                    FileInfo file = new FileInfo(openFileDialog1.FileName);
                    imageFile.Source = new BitmapImage(new Uri(openFileDialog1.FileName, UriKind.Absolute));
                    _isPlaying = true;
                }

            }
        }
        #endregion

    }
}
