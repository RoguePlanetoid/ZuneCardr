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

namespace ZuneCardr
{
    /// <summary>Url</summary>
    /// <version>1.0.0</version>
    /// <created>22 May 2011</created>
    /// <modified>22 May 2011</modified>
    public class Url
    {
        #region Private Constants
        private const string Url_Album = "http://catalog.zune.net/v3.2/music/album/{0}";
        private const string Url_Artist = "http://catalog.zune.net/v3.2/en-GB/music/artist/{0}";
        private const string Url_Profile = "http://socialapi.zune.net/en-GB/members/{0}";
        private const string Url_Favs = "http://socialapi.zune.net/en-US/members/{0}/playlists/BuiltIn-FavoriteTracks";
        private const string Url_Recent = "http://socialapi.zune.net/en-US/members/{0}/playlists/BuiltIn-RecentTracks";
        private const string Url_Artists = "http://socialapi.zune.net/en-GB/members/{0}/playlists/BuiltIn-MostPlayedArtists";
        private const string Url_Badges = "http://socialapi.zune.net/en-US/members/{0}/badges";
        private const string Url_Friends = "http://socialapi.zune.net/en-US/members/{0}/friends";
        #endregion

        #region Public Static Methods

        /// <summary>Album</summary>
        /// <param name="id">GUID</param>
        /// <returns>Album</returns>
        public static Uri Album(Guid id) { return new Uri(String.Format(Url_Album, id.ToString())); }

        /// <summary>Artist</summary>
        /// <param name="id">GUID</param>
        /// <returns>Artist</returns>
        public static Uri Artist(Guid id) { return new Uri(String.Format(Url_Artist, id.ToString())); }

        /// <summary>Profile</summary>
        /// <param name="tag">Tag</param>
        /// <returns>Profile</returns>
        public static Uri Profile(string tag) { return new Uri(String.Format(Url_Profile, tag)); }

        /// <summary>Favs</summary>
        /// <param name="tag">Member GUID</param>
        /// <returns>Favs</returns>
        public static Uri Favs(string tag) { return new Uri(String.Format(Url_Favs, tag)); }

        /// <summary>Recent</summary>
        /// <param name="tag">Member GUID</param>
        /// <returns>Recent</returns>
        public static Uri Recent(string tag) { return new Uri(String.Format(Url_Recent, tag)); }

        /// <summary>Artists</summary>
        /// <param name="tag">Member GUID</param>
        /// <returns>Artists</returns>
        public static Uri Artists(string tag) { return new Uri(String.Format(Url_Artists, tag)); }

        /// <summary>Badges</summary>
        /// <param name="tag">Member GUID</param>
        /// <returns>Badges</returns>
        public static Uri Badges(string tag) { return new Uri(String.Format(Url_Badges, tag)); }

        /// <summary>Friends</summary>
        /// <param name="tag">Member GUID</param>
        /// <returns>Friends</returns>
        public static Uri Friends(string tag) { return new Uri(String.Format(Url_Friends, tag)); }
        #endregion
    }
}
