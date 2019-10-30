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
using System.Linq;

namespace ZuneCardr
{
    public class Data : INotifyPropertyChanged
    {
        #region Private Constants
        // Properties
        private const string PropCard = "Card";
        private const string PropFavs = "Favs";
        private const string PropRecent = "Recent";
        private const string PropPlayed = "Played";
        // Delegates
        private delegate void Changed();
        private Changed favsChanged = delegate() { App.Framework.Data.FavsChanged(); };
        private Changed recentChanged = delegate() { App.Framework.Data.RecentChanged(); };
        private Changed profileChanged = delegate() { App.Framework.Data.ProfileChanged(); };
        private Changed playedChanged = delegate() { App.Framework.Data.PlayedChanged(); };
        private Changed badgesChanged = delegate() { App.Framework.Data.BadgesChanged(); };
        private Changed friendsChanged = delegate() { App.Framework.Data.FriendsChanged(); };
        private Changed albumChanged = delegate() { App.Framework.Data.AlbumChanged(); };
        private Changed artistChanged = delegate() { App.Framework.Data.ArtistChanged(); };
        #endregion

        #region Private Members
        private Card card = new Card();
        private ObservableCollection<Card> cards = new ObservableCollection<Card>();
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

        #region Public Methods

        /// <summary>GetData</summary>
        public void GetData(string tag)
        {
            card.Tag = tag;
            card.Profile = new Syndication.Feed(Url.Profile(tag), profileChanged); // Profile
            card.Favs = new Syndication.Feed(Url.Favs(tag), favsChanged); // Favourites
            card.Recent = new Syndication.Feed(Url.Recent(tag), recentChanged); // Recent
            card.Played = new Syndication.Feed(Url.Artists(tag), playedChanged); // Artists
            card.Badges = new Syndication.Feed(Url.Badges(tag), badgesChanged); // Badges
            card.Friends = new Syndication.Feed(Url.Friends(tag), friendsChanged); // Friends
            cards.Add(card); // TODO: Check for Existing & do this when last item downloaded?
        }

        /// <summary>ProfileChanged</summary>
        public void ProfileChanged()
        {
           
        }

        /// <summary>AlbumChanged</summary>
        public void AlbumChanged()
        {
            NotifyPropertyChanged(PropCard);
            NotifyPropertyChanged(PropFavs);
            NotifyPropertyChanged(PropRecent);
        }

        /// <summary>Artist Changed</summary>
        public void ArtistChanged()
        {
            NotifyPropertyChanged(PropPlayed);
        }

        /// <summary>FavsChanged</summary>
        public void FavsChanged()
        {
            foreach (Syndication.Item item in card.Favs.Items) 
            {
                card.Albums.Add(new Syndication.Feed(Url.Album(item.Track.Album.Target), albumChanged)); // Get Album    
            }
        }

        /// <summary>RecentsChanged</summary>
        public void RecentChanged()
        {
            foreach (Syndication.Item item in card.Recent.Items)
            {
                card.Albums.Add(new Syndication.Feed(Url.Album(item.Track.Album.Target), albumChanged)); // Get Album
            }
        }

        /// <summary>PlayedChanged</summary>
        public void PlayedChanged()
        {
            foreach (Syndication.Item item in card.Played.Items)
            {
                card.Artists.Add(new Syndication.Feed(Url.Artist(item.Track.Artist.Target), artistChanged)); // Get Artist             
            }
        }

        /// <summary>BadgesChanged</summary>
        public void BadgesChanged()
        {
            NotifyPropertyChanged(PropRecent);
        }

        /// <summary>FriendsChanged</summary>
        public void FriendsChanged()
        {
            NotifyPropertyChanged(PropRecent);
        }
        #endregion

        #region Public Properties
        /// <summary>Card</summary>
        public Card Card { get { return card; } set { card = value; NotifyPropertyChanged(PropCard); } }

        /// <summary>Cards</summary>
        public ObservableCollection<Card> Cards { get { return cards; } set { cards = value; } }
        #endregion
    }
}
