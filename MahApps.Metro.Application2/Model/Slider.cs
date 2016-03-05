using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace MyWindowsMediaPlayer
{
    public partial class MainWindow : MetroWindow
    {
        #region slider
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && getMediaPlayed().Source != null && getMediaPlayed().NaturalDuration.HasTimeSpan) // Musique ou Vid
            {
                double TotalSeconds = getMediaPlayed().NaturalDuration.TimeSpan.TotalSeconds;
                double SecPosition = (TotalSeconds / 100) * e.NewValue;
                TimeSpan newPosition = TimeSpan.FromSeconds(SecPosition);
                getMediaPlayed().Position = newPosition;
            }
        }

        private void mediaElement1_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (getMediaPlayed().NaturalDuration.HasTimeSpan)// Musique ou Vid
            {
                timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                timer.Start();
                _MediaDuration = getMediaPlayed().NaturalDuration.TimeSpan.TotalSeconds;
            }

        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            getMediaPlayed().Volume = e.NewValue;// Musique ou Vid
        }

        private int random_func(int number)
        {
            Random r = new Random();
            int num = r.Next(0, number);
            return num;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (getMediaPlayed().NaturalDuration.HasTimeSpan)// Musique ou Vid
            {
                slider.Value = (getMediaPlayed().Position.TotalSeconds / _MediaDuration) * 100;
                if (slider.Value == 100)
                    slider.Value = 0;
            }
            majTimeLabel();
        }

        void majTimeLabel()// Musique ou Vid
        {
            string time = string.Format("{0}:", getMediaPlayed().Position.Hours);
            time += getMediaPlayed().Position.Minutes < 10 ? string.Format("0{0}:", getMediaPlayed().Position.Minutes) : string.Format("{0}:", getMediaPlayed().Position.Minutes);
            time += getMediaPlayed().Position.Seconds < 10 ? string.Format("0{0}", getMediaPlayed().Position.Seconds) : string.Format("{0}", getMediaPlayed().Position.Seconds);
            time += " / ";
            if (getMediaPlayed().NaturalDuration.HasTimeSpan)
            {
                time += string.Format("{0}:", getMediaPlayed().NaturalDuration.TimeSpan.Hours);
                time += getMediaPlayed().NaturalDuration.TimeSpan.Minutes < 10 ? string.Format("0{0}:", getMediaPlayed().NaturalDuration.TimeSpan.Minutes) : string.Format("{0}:", getMediaPlayed().NaturalDuration.TimeSpan.Minutes);
                time += getMediaPlayed().NaturalDuration.TimeSpan.Seconds < 10 ? string.Format("0{0}", getMediaPlayed().NaturalDuration.TimeSpan.Seconds) : string.Format("{0}", getMediaPlayed().NaturalDuration.TimeSpan.Seconds);
            }
            time_LAB.Content = time;
        }
        #endregion
    }
}
