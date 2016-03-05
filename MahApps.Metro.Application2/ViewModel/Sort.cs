using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;

namespace MyWindowsMediaPlayer
{
    public partial class MainWindow : MetroWindow
    {
        #region SORT MP3
        private void SortMyMP3(object sender, RoutedEventArgs e)
        {
            if (MyMP3LIST.Count != 0)
            {
                ObservableCollection<ListGrid> tmp = new ObservableCollection<ListGrid>();
                tmp = MyMP3LIST;
                var list = new List<string>();
                var list2 = new List<string>();
                var Tmp = (Mp3ListTMP.Count == 0 ? tmp : Mp3ListTMP);
                foreach (ListGrid item in Tmp)
                {
                    if (sender.ToString().Split(' ')[1] == "Titre")
                    {
                        list.Add(item.Title);
                        list2.Add(item.Title);
                    }
                    if (sender.ToString().Split(' ')[1] == "Album")
                    {
                        list.Add(item.Album);
                        list2.Add(item.Album);
                    }
                    if (sender.ToString().Split(' ')[1] == "Artiste")
                    {
                        list.Add(item.Composer);
                        list2.Add(item.Composer);
                    }
                    if (sender.ToString().Split(' ')[1] == "Durée")
                    {
                        list.Add(item.Length.ToString());
                        list2.Add(item.Length.ToString());
                    }
                }
                list2.Sort();
                if (list.SequenceEqual(list2))
                {
                    if (sender.ToString().Split(' ')[1] == "Titre")
                        Mp3ListTMP = new ObservableCollection<ListGrid>(from mp3 in Tmp orderby mp3.Title descending select mp3);
                    if (sender.ToString().Split(' ')[1] == "Album")
                        Mp3ListTMP = new ObservableCollection<ListGrid>(from mp3 in Tmp orderby mp3.Album descending select mp3);
                    if (sender.ToString().Split(' ')[1] == "Artiste")
                        Mp3ListTMP = new ObservableCollection<ListGrid>(from mp3 in Tmp orderby mp3.Composer descending select mp3);
                    if (sender.ToString().Split(' ')[1] == "Durée")
                        Mp3ListTMP = new ObservableCollection<ListGrid>(from mp3 in Tmp orderby mp3.Length descending select mp3);
                }
                else
                {
                    if (sender.ToString().Split(' ')[1] == "Titre")
                        Mp3ListTMP = new ObservableCollection<ListGrid>(from mp3 in Tmp orderby mp3.Title select mp3);
                    if (sender.ToString().Split(' ')[1] == "Album")
                        Mp3ListTMP = new ObservableCollection<ListGrid>(from mp3 in Tmp orderby mp3.Album select mp3);
                    if (sender.ToString().Split(' ')[1] == "Artiste")
                        Mp3ListTMP = new ObservableCollection<ListGrid>(from mp3 in Tmp orderby mp3.Composer select mp3);
                    if (sender.ToString().Split(' ')[1] == "Durée")
                        Mp3ListTMP = new ObservableCollection<ListGrid>(from mp3 in Tmp orderby mp3.Length select mp3);
                }
                ListM.DataContext = Mp3ListTMP;
            }
        }
        #endregion

        #region SORT VIDEO
        private void SortMyVideo(object sender, RoutedEventArgs e)
        {
            if (MyVIDLIST.Count != 0)
            {
                ObservableCollection<ListVideo> tmp = new ObservableCollection<ListVideo>();
                tmp = MyVIDLIST;
                var list = new List<string>();
                var list2 = new List<string>();
                var Tmp = (VidListTMP.Count == 0 ? tmp : VidListTMP);
                foreach (ListVideo item in Tmp)
                {
                    if (sender.ToString().Split(' ')[1] == "Titre")
                    {
                        list.Add(item.TitleV);
                        list2.Add(item.TitleV);
                    }
                    if (sender.ToString().Split(' ')[1] == "Durée")
                    {
                        list.Add(item.LengthV.ToString());
                        list2.Add(item.LengthV.ToString());
                    }
                }
                list2.Sort();

                if (list.SequenceEqual(list2))
                {
                    if (sender.ToString().Split(' ')[1] == "Titre")
                        VidListTMP = new ObservableCollection<ListVideo>(from vid in Tmp orderby vid.TitleV descending select vid);
                    if (sender.ToString().Split(' ')[1] == "Durée")
                        VidListTMP = new ObservableCollection<ListVideo>(from vid in Tmp orderby vid.LengthV descending select vid);
                }
                else
                {
                    if (sender.ToString().Split(' ')[1] == "Titre")
                        VidListTMP = new ObservableCollection<ListVideo>(from vid in Tmp orderby vid.TitleV select vid);
                    if (sender.ToString().Split(' ')[1] == "Durée")
                        VidListTMP = new ObservableCollection<ListVideo>(from vid in Tmp orderby vid.LengthV select vid);
                }
                ListV.DataContext = VidListTMP;
            }
        }
        #endregion

        #region SORT IMAGE
        private void SortMyImage(object sender, RoutedEventArgs e)
        {
            if (MyIMGLIST.Count != 0)
            {
                ObservableCollection<ListImage> tmp = new ObservableCollection<ListImage>();
                tmp = MyIMGLIST;
                var list = new List<string>();
                var list2 = new List<string>();
                var Tmp = (ImgListTMP.Count == 0 ? tmp : ImgListTMP);
                foreach (ListImage item in Tmp)
                {
                    if (sender.ToString().Split(' ')[1] == "Titre")
                    {
                        list.Add(item.TitleI);
                        list2.Add(item.TitleI);
                    }
                    if (sender.ToString().Split(' ')[1] == "Format")
                    {
                        list.Add(item.FormatI.ToString());
                        list2.Add(item.FormatI.ToString());
                    }
                    if (sender.ToString().Split(' ')[1] == "Taille")
                    {
                        list.Add(item.SizeI.ToString());
                        list2.Add(item.SizeI.ToString());
                    }
                }
                list2.Sort();
                if (list.SequenceEqual(list2))
                {
                    if (sender.ToString().Split(' ')[1] == "Titre")
                        ImgListTMP = new ObservableCollection<ListImage>(from img in Tmp orderby img.TitleI descending select img);
                    if (sender.ToString().Split(' ')[1] == "Format")
                        ImgListTMP = new ObservableCollection<ListImage>(from img in Tmp orderby img.FormatI descending select img);
                    if (sender.ToString().Split(' ')[1] == "Taille")
                        ImgListTMP = new ObservableCollection<ListImage>(from img in Tmp orderby img.SizeI descending select img);
                }
                else
                {
                    if (sender.ToString().Split(' ')[1] == "Titre")
                        ImgListTMP = new ObservableCollection<ListImage>(from img in Tmp orderby img.TitleI select img);
                    if (sender.ToString().Split(' ')[1] == "Format")
                        ImgListTMP = new ObservableCollection<ListImage>(from img in Tmp orderby img.FormatI select img);
                    if (sender.ToString().Split(' ')[1] == "Taille")
                        ImgListTMP = new ObservableCollection<ListImage>(from img in Tmp orderby img.SizeI select img);
                }
                ListI.DataContext = ImgListTMP;
            }
        }
        #endregion
    }
}
