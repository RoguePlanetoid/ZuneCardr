using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ZuneCardr
{
    public class Card : INotifyPropertyChanged 
    {
        #region Private Members
        private string tag;
        private Syndication.Feed favs = new Syndication.Feed();
        private Syndication.Feed recent = new Syndication.Feed();
        private Syndication.Feed profile = new Syndication.Feed();
        private Syndication.Feed played = new Syndication.Feed();
        private Syndication.Feed badges = new Syndication.Feed();
        private Syndication.Feed friends = new Syndication.Feed();
        private ObservableCollection<Syndication.Feed> albums = new ObservableCollection<Syndication.Feed>();
        private ObservableCollection<Syndication.Feed> artists = new ObservableCollection<Syndication.Feed>();
        #endregion

        #region Event Handler
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>NotifyPropertyChanged</summary>
        /// <param name="info">Property Name</param>
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        #region Public Properties

        /// <summary>Tag</summary>
        public string Tag { get { return tag; } set { tag = value; } }

        /// <summary>Favs</summary>
        public Syndication.Feed Favs { get { return favs; } set { favs = value; } }

        /// <summary>Recent</summary>
        public Syndication.Feed Recent { get { return recent; } set { recent = value; } }

        /// <summary>Played</summary>
        public Syndication.Feed Played { get { return played; } set { played = value; } }

        /// <summary>Profile</summary>
        public Syndication.Feed Profile { get { return profile; } set { profile = value; } }

        /// <summary>Artists</summary>
        public Syndication.Feed Badges { get { return badges; } set { badges = value; } }

        /// <summary>Profile</summary>
        public Syndication.Feed Friends { get { return friends; } set { friends = value; } }

        /// <summary>Artists</summary>
        public ObservableCollection<Syndication.Feed> Artists { get { return artists; } set { artists = value; } }

        /// <summary>Albums</summary>
        public ObservableCollection<Syndication.Feed> Albums { get { return albums; } set { albums = value; } }
        #endregion
    }
}
