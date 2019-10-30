using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace ZuneCardr
{
    /// <summary>ZuneCard Information</summary>
    /// <version>3.0.0</version>
    /// <created>29 April 2011</created>
    /// <modified>29 April 2011</modified>
    [DataContract]
    public class ZuneCard : INotifyPropertyChanged
    {
        #region Private Constants
        private const int ZERO = 0;
        private const string BLANK = "";
        private const string BY_TEXT = " by ";
        private const string FORMAT_NAME_LOCATION = "{0}, {1}";
        private const string FORMAT_PLAYS = "{0:#,##} plays";
        // Properties
        private const string PROP_ARTISTS = "Artists";
        private const string PROP_PLAYS = "Plays";
        private const string PROP_PLAYS_TEXT = "PlaysText";
        private const string PROP_FAVS = "Favs";
        private const string PROP_RECENT = "Recent";
        private const string PROP_BADGES = "Badges";
        private const string PROP_FRIENDS = "Friends";
        private const string PROP_TILE = "Tile";
        private const string PROP_BACKGROUND = "Background";
        #endregion

        #region Private Members
        private string id = BLANK;
        private string tag = BLANK;
        private string status = BLANK;
        private string name = BLANK; // 15 Chars Max
        private string location = BLANK; // 30 Chars Max
        private string biography = BLANK; // 300 Characters Max
        private string msg = BLANK;
        private int plays = ZERO;
        private Uri url;
        private Uri homeUrl;
        private Uri sendUrl;
        private Uri msgUrl;
        private Uri tile;
        private Uri background;

        private List<Track> favs = new List<Track>();
        private List<Track> recent = new List<Track>();
        private List<Artist> artists = new List<Artist>();
        private List<Badge> badges = new List<Badge>();
        private List<Friend> friends = new List<Friend>();
        private DateTime updated = DateTime.Now;
        #endregion

        #region Private Methods

        /// <summary>ListBadges</summary>
        /// <param name="type">Type</param>
        /// <returns>List of Badge of Type</returns>
        private List<Badge> ListBadges(string type)
        {
            return Badges.Where(item => item.Type == type).ToList();
        }

        /// <summary>ListBadges</summary>
        /// <returns>List of Badge by Unique Type</returns>
        private List<Badge> ListBadges()
        {
            return Badges.Distinct(new BadgeComparer()).ToList();
        }

        /// <summary>GetNameLocation</summary>
        /// <param name="name">Name</param>
        /// <param name="location">Location</param>
        /// <returns>Name and/or Location Text</returns>
        private string GetNameLocation()
        {

            if (Name != BLANK && Location != BLANK)
            {
                return string.Format(FORMAT_NAME_LOCATION, Name, Location);
            }
            else if(Name != BLANK)
            {
                return Name;
            }
            else if (Location != BLANK)
            {
                return Location;
            }
            else
            {
                return BLANK;
            }
        }
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
        /// <summary>ID</summary>
        /// <returns>Member ID</returns>
        [DataMember]
        public string ID { get { return id.Trim(); } set { id = value; } }

        /// <summary>Tag</summary>
        /// <returns>Member Tag</returns>
        [DataMember]
        public string Tag { get { return tag.Trim(); } set { tag = value; } }

        /// <summary>Status</summary>
        /// <returns>Member Status</returns>
        [DataMember]
        public string Status { get { return status.Trim(); } set { status = value; } }

        /// <summary>Location</summary>
        /// <returns>Member Location</returns>
        [DataMember]
        public string Location { get { return location.Trim(); } set { location = value; } }

        /// <summary>Biography</summary>
        /// <returns>Member Biography</returns>
        [DataMember]
        public string Biography { get { return biography.Trim(); } set { biography = value; } }

        /// <summary>Name</summary>
        /// <returns>Member Name</returns>
        [DataMember]
        public string Name { get { return name.Trim(); } set { name = value; } }

        /// <summary>Member Name / Location</summary>
        public string NameLocation { get { return GetNameLocation(); } }
   
        /// <summary>Plays</summary>
        /// <returns>Member Plays</returns>
        [DataMember]
        public int Plays { get { return plays; } set { plays = value; NotifyPropertyChanged(PROP_PLAYS); NotifyPropertyChanged(PROP_PLAYS_TEXT); } }

        /// <summary>Plays</summary>
        /// <returns>Member Plays (Full)</returns>
        public string PlaysText { get { return String.Format(FORMAT_PLAYS,plays); } }

        /// <summary>Message</summary>
        /// <returns>System Message</returns>
        [DataMember]
        public string Message { get { return msg.Trim(); } set { msg = value; } }

        /// <summary>Url</summary>
        /// <returns>Member Link</returns>
        public Uri Url { get { return url; } set { url = value; } }

        /// <summary>URL Serialisable</summary>
        [DataMember]
        public string UrlLink { get { return Url == null ? null : Url.ToString(); } set { Url = value == null ? null : new Uri(value); } }

        /// <summary>HomeUrl</summary>
        /// <returns>Member Home</returns>
        public Uri HomeUrl { get { return homeUrl; } set { homeUrl = value; } }

        /// <summary>HomeURL Serialisable</summary>
        [DataMember]
        public string HomeUrlLink { get { return homeUrl == null ? null : homeUrl.ToString(); } set { homeUrl = value == null ? null : new Uri(value); } }

        /// <summary>SendUrl</summary>
        /// <returns>Member Send Card Link</returns>
        public Uri SendUrl { get { return sendUrl; } set { sendUrl = value; } }

        /// <summary>HomeURL Serialisable</summary>
        [DataMember]
        public string SendUrlLink { get { return sendUrl == null ? null : sendUrl.ToString(); } set { sendUrl = value == null ? null : new Uri(value); } }

        /// <summary>MessageUrl</summary>
        /// <returns>Member Send Message Link</returns>
        public Uri MessageUrl { get { return msgUrl; } set { msgUrl = value; } }

        /// <summary>HomeURL Serialisable</summary>
        [DataMember]
        public string MessageUrlLink { get { return msgUrl == null ? null : msgUrl.ToString(); } set { msgUrl = value == null ? null : new Uri(value); } }

        /// <summary>Tile</summary>
        /// <returns>Tile Visual</returns>
        public Uri Tile { get { return tile; } set { tile = value; NotifyPropertyChanged(PROP_TILE); } }

        /// <summary>Tile (Serialisable)</summary>
        [DataMember]
        public string TileLink { get { return tile == null ? null : tile.ToString(); } set { tile = value == null ? null : new Uri(value); } }

        /// <summary>Background</summary>
        /// <returns>Background Visual</returns>
        public Uri Background { get { return background; } set { background = value; NotifyPropertyChanged(PROP_BACKGROUND); } }

        /// <summary>Background (Serialisable)</summary>
        [DataMember]
        public string BackgroundLink { get { return background == null ? null : background.ToString(); } set { background = value == null ? null : new Uri(value); } }

        /// <summary>Badges</summary>
        /// <returns>List Of Badges</returns>
        [DataMember]
        public List<Badge> Badges { get { return badges; } set { badges = value; NotifyPropertyChanged(PROP_BADGES); } }

        /// <summary>Favs</summary>
        /// <returns>List of Favourite Tracks</returns>
        [DataMember]
        public List<Track> Favs { get { return favs; } set { favs = value; NotifyPropertyChanged(PROP_FAVS); } }

        /// <summary>Recent</summary>
        /// <returns>List of Recent Tracks</returns>
        [DataMember]
        public List<Track> Recent { get { return recent; } set { recent = value; NotifyPropertyChanged(PROP_RECENT); } }

        /// <summary>Artists</summary>
        /// <returns>List of Artists</returns>
        [DataMember]
        public List<Artist> Artists { get { return artists; } set { artists = value; NotifyPropertyChanged(PROP_ARTISTS); } }

        /// <summary>Friends</summary>
        /// <returns>List of Friends</returns>
        [DataMember]
        public List<Friend> Friends { get { return friends; } set { friends = value; NotifyPropertyChanged(PROP_FRIENDS); } }

        /// <summary>Unique Badges</summary>
        public List<Badge> Unique { get { return ListBadges(); } }
        #endregion
    }
    #region Classes
        /// <summary>ArtistComparer</summary>
    public class ArtistComparer : IEqualityComparer<Artist>
    {
        /// <summary>Equals</summary>
        /// <param name="x">Source</param>
        /// <param name="y">Target</param>
        /// <returns>True if Equals, False if Not</returns>
        public bool Equals(Artist x, Artist y)
        {
            return x.ID.Equals(y.ID);
        }
        /// <summary>GetHashCode</summary>
        /// <param name="obj">Object</param>
        /// <returns>HashCode for Comparison</returns>
        public int GetHashCode(Artist obj)
        {
            return obj.ID.ToString().GetHashCode();
        }
    }

    /// <summary>BadgeComparer</summary>
    public class BadgeComparer : IEqualityComparer<Badge>
    {
        /// <summary>Equals</summary>
        /// <param name="x">Source</param>
        /// <param name="y">Target</param>
        /// <returns>True if Equals, False if Not</returns>
        public bool Equals(Badge x, Badge y)
        {
            return x.Type.Equals(y.Type);
        }
        /// <summary>GetHashCode</summary>
        /// <param name="obj">Object</param>
        /// <returns>HashCode for Comparison</returns>
        public int GetHashCode(Badge obj)
        {
            return obj.Type.ToString().GetHashCode();
        }
    }
    #endregion
}