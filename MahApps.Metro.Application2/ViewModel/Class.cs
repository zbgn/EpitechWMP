using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWindowsMediaPlayer
{
    public partial class MainWindow : MetroWindow
    {
        #region Class
        public class ListGrid
        {
            public string Path { get; set; }
            public string Title { get; set; }
            public System.TimeSpan Length { get; set; }
            public string Album { get; set; }
            public string Composer { get; set; }
            public System.Windows.Media.ImageSource IconUri { get; set; }
            public string sort { get; set; }
            public long Size { get; set; }
        }
        class general
        {
            public System.Windows.Media.ImageSource imageUri { get; set; }
        }
        public class ListVideo
        {
            public string Path { get; set; }
            public string TitleV { get; set; }
            public System.TimeSpan LengthV { get; set; }
            public long Size { get; set; }
        }
        public class ListImage
        {
            public string Path { get; set; }
            public string TitleI { get; set; }
            public long SizeI { get; set; }
            public string FormatI { get; set; }
        }
        public class PlaylistList
        {
            public string path { get; set; }
            public string Title { get; set; }
        }
        #endregion
    }
}
