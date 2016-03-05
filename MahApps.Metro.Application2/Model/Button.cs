using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;

namespace MyWindowsMediaPlayer
{
    public partial class MainWindow : MetroWindow
    {
        #region button
        private void button_play(object sender, RoutedEventArgs e)
        {
            getMediaPlayed().Play(); // Savoir si musique ou video
        }
        private void button_pause(object sender, RoutedEventArgs e)
        {
            getMediaPlayed().Pause(); // Savoir si musique ou video
        }
        private void button_stop(object sender, RoutedEventArgs e)
        {
            getMediaPlayed().Stop(); // Savoir si musique ou video
            getMediaPlayed().Source = null;
            ImageMP3.Source = null;
            imageFile.Source = null;
            Label_Titre.Content = "Titre : ";
            Label_Artiste.Content = "Artiste : ";
            Label_Album.Content = "Album : ";
            Label_Duree.Content = "Durée : ";
            Label_Taille.Content = "Taille (ko) : ";
            ListM.SelectedIndex = -1;
            ListV.SelectedIndex = -1;
            ListI.SelectedIndex = -1;
            ListV.Visibility = Visibility.Visible;
            _isPlaying = false;
            _MusicInPlayer = false;
            slider.Value = 0;
            Player_Control.Opacity = 100;
            backVideo.Visibility = Visibility.Visible;
            backImage.Visibility = Visibility.Visible;
        }
        private void changeImageButton(Button button, string path)
        {
            button.Content = new Image
            {
                Source = new BitmapImage(new Uri(path, UriKind.Relative)),
            };
        }
        private void button_mute(object sender, RoutedEventArgs e)
        {
           if (!this._isMute)
            {
                getMediaPlayed().IsMuted = !getMediaPlayed().IsMuted;
                this.changeImageButton(mute, "../Images/appbar.sound.mute.png");
                mute.ToolTip = "Muet";
                this._isMute = true;
            }
            else 
            {
                getMediaPlayed().IsMuted = !getMediaPlayed().IsMuted;
                this.changeImageButton(mute, "../Images/appbar.sound.1.png");
                mute.ToolTip = "Volume";
                this._isMute = false;
            }
        }
        private void PlayNextSong(object sender, RoutedEventArgs e)
        {
            if (_PlayedFromPlaylist == true)
            {
                if (_PlaylistPosition < MyMP3LIST.Count - 1)
                {
                    _PlaylistPosition += 1;
                    ListM.SelectedItem = MyMP3LIST[_PlaylistPosition];
                    PlaylistPlayMusique(MyMP3LIST[_PlaylistPosition].Path);
                }
            }
        }
        private void PlayPreviousSong(object sender, RoutedEventArgs e)
        {
            if (_PlayedFromPlaylist == true)
            {
                if (_PlaylistPosition > 0)
                {
                    _PlaylistPosition -= 1;
                    ListM.SelectedItem = MyMP3LIST[_PlaylistPosition];
                    PlaylistPlayMusique(MyMP3LIST[_PlaylistPosition].Path);
                }
            }
        }
        #endregion
    }
}
