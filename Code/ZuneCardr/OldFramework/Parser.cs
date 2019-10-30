using System;
using System.Net;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Collections;

namespace ZuneCardr
{
    /// <summary>Parser</summary>
    /// <version>10.0.0</version>
    /// <created>29 April 2011</created>
    /// <modified>29 April 2011</modified>
    public class Parser
    {
        #region Private Constants
        private const int ZERO = 0;
        private const string BLANK = "";
        private const int TAG_LENGTH = 16;
        private const string TAG_VALID = "^[a-zA-Z0-9_\\s]$";
        // Parts
        private const int PART_LENGTH = 3;
        private const string ERR_PARTLENGTH_MSG = "Part length has to be positive.";
        private const string ERR_PARTLENGTH_VALUE = "partLength";
        private const string URL_DELIMITER = "/";
        // Tags
        private const string TAG_PLAY = "playCount";
        private const string TAG_PLAYCOUNT = "playcount";
        private const string TAG_DISPLAY_NAME = "displayname";
        private const string TAG_STATUS = "status";
        private const string TAG_BIO = "bio";
        private const string TAG_LOCATION = "location";
        private const string TAG_LINK = "link";
        private const string TAG_IMAGES = "images";
        private const string TAG_ENTRY = "entry";
        private const string TAG_ZUNE = "zunetag";
        private const string TAG_TITLE = "title";
        private const string TAG_CONTENT = "content";
        private const string TAG_TYPE = "type";
        private const string TAG_ARTIST = "artist";
        private const string TAG_ID = "id";
        private const string TAG_NAME = "name";
        private const string TAG_IMAGE = "image";
        private const string TAG_GENRE = "primaryGenre";
        private const string TAG_TRACK = "track";
        private const string TAG_ALBUM = "album";
        private const string TAG_PRIMARY_ARTIST = "primaryArtist";
        private const string TAG_RIGHTS = "rights";
        private const string TAG_RIGHT = "right";
        private const string TAG_CODE = "providerCode";
        // Attributes
        private const string ATTR_HREF = "href";
        private const string ATTR_TITLE = "title";
        // Values
        private const string VALUE_BADGES_LABEL = "{0} badges";
        private const string VALUE_PLAYS_LABEL = "{0} plays";
        private const string VALUE_USER_TILE = "usertile";
        private const string VALUE_BACKGROUND = "background";
        private const string VALUE_ID_PREFIX = "urn:uuid:";
        private const string VALUE_LARGE_IMAGE_SUFFIX = "height=200";
        private const string VALUE_SMALL_IMAGE_SUFFIX = "height=75";
        private const string VALUE_TRACK = "track";
        private const string VALUE_ARTIST = "artist";
        private const string VALUE_ALBUM = "album";
        private const string VALUE_COLON = ":";
        private const string VALUE_STRING = "s";
        // Namespaces
        private readonly XNamespace NS_ATOM = "http://www.w3.org/2005/Atom";
        private readonly XNamespace NS_PROFILE = "http://schemas.zune.net/profiles/2008/01";
        private readonly XNamespace NS_MUSIC = "http://schemas.zune.net/catalog/music/2007/10";
        // URLs
        private const string URL_PROFILE = "http://socialapi.zune.net/en-US/members/{0}";
        private const string URL_ALBUM = "http://catalog.zune.net/v3.2/en-US/music/album/{0}";
        private const string URL_RECENT = "http://socialapi.zune.net/en-US/members/{0}/playlists/BuiltIn-RecentTracks";
        private const string URL_FAVS = "http://socialapi.zune.net/en-US/members/{0}/playlists/BuiltIn-FavoriteTracks";
        private const string URL_ARTISTS = "http://socialapi.zune.net/en-US/members/{0}/playlists/BuiltIn-MostPlayedArtists";
        private const string URL_ARTIST = "http://catalog.zune.net/v3.2/en-US/music/artist/{0}";
        private const string URL_BADGES = "http://socialapi.zune.net/en-US/members/{0}/badges";
        private const string URL_FRIENDS = "http://socialapi.zune.net/en-US/members/{0}/friends";
        private const string URL_TILE = "http://cache-tiles.zune.net/tiles/user/{0}";
        private const string URL_IMAGE = "http://catalog.zune.net/v3.2/image/{0}?{1}";
        private const string URL_LINK = "http://social.zune.net/redirect?type={0}&id={1}&target=web";
        private const string URL_PLAY = "http://stream.zune.net/a4241/e3/{0}at/audio.wma";
        private const string URL_HOME = "http://social.zune.net/member/{0}";
        #endregion

        #region Static Methods
        /// <summary>IsValid</summary>
        /// <param name="tag">Zune Tag</param>
        /// <returns>True if Valid, False if Not</returns>
        public static bool IsValidTag(string tag)
        {
            if (tag.Length > ZERO && tag.Length <= TAG_LENGTH)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Helpers
        /// <summary>ParseID</summary>
        /// <param name="id">e.g. urn:uuid:d7bd0000-0200-11db-89ca-0019b92a3933</param>
        /// <returns>d7bd0000-0200-11db-89ca-0019b92a3933</returns>
        private string ParseID(string id)
        {
            return id.Replace(VALUE_ID_PREFIX, BLANK);
        }

        /// <summary>ParseTrackVisual</summary>
        /// <param name="id">e.g. urn:uuid:4e824a4c-7787-dc11-883e-0019b9ef2915</param>
        /// <returns>Visual</returns>
        private Visual ParseVisual(string id)
        {
            return new Visual(new Uri(String.Format(URL_IMAGE,ParseID(id),VALUE_LARGE_IMAGE_SUFFIX)),
                new Uri(String.Format(URL_IMAGE,ParseID(id), VALUE_SMALL_IMAGE_SUFFIX)));
        }

        /// <summary>HasID</summary>
        /// <param name="id">ID</param>
        /// <returns>True if Has, False If Not</returns>
        private bool HasID(string id)
        {
            try
            {
                Guid guid = new Guid(id);
                return guid != Guid.Empty;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>ParseLink</summary>
        /// <param name="id">GUID</param>
        /// <param name="type">Link Type</param>
        /// <returns>URI</returns>
        private Uri ParseLink(string id, string type)
        {
            return new Uri(string.Format(URL_LINK,type,ParseID(id)));
        }

        /// <summary>SplitInParts</summary>
        /// <param name="s">Source</param>
        /// <param name="partLength">Part Length</param>
        /// <returns>Strings</returns>
        private IEnumerable<String> SplitInParts(String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException(VALUE_STRING);
            if (partLength <= ZERO)
                throw new ArgumentException(ERR_PARTLENGTH_MSG, ERR_PARTLENGTH_VALUE);
            for (var i = ZERO; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }

        /// <summary>ParsePlayLink</summary>
        /// <param name="code">Provider Code</param>
        /// <returns>Play URI</returns>
        private Uri ParsePlayLink(string code)
        {
            string provider = code.Substring(ZERO, code.IndexOf(VALUE_COLON));
            StringBuilder uri = new StringBuilder();
            foreach (string part in SplitInParts(provider, PART_LENGTH))
            {
                uri.Append(part);
                uri.Append(URL_DELIMITER);
            }
            return new Uri(string.Format(URL_PLAY, uri), UriKind.Absolute);
        }
        #endregion

        #region Album
        /// <summary>Parse Album</summary>
        /// <param name="data">Data</param>
        /// <param name="track">Tracks</param>
        private void ParseAlbum(string data, KeyValuePair<Track, List<Track>> item)
        {
            XElement elements = XElement.Parse(data);
            Album album = new Album();
            album.ID = elements.Element(NS_ATOM + TAG_ID).Value;
            album.Name = elements.Element(NS_ATOM + TAG_TITLE).Value;
            album.Image = ParseVisual(elements.Element(NS_MUSIC + TAG_IMAGE)
                .Element(NS_MUSIC + TAG_ID).Value);
            album.Url = ParseLink(album.ID, VALUE_ALBUM);
            // Artist
            Artist artist = new Artist();
            artist.ID = elements.Element(NS_MUSIC + TAG_PRIMARY_ARTIST)
                .Element(NS_MUSIC + TAG_ID).Value;
            artist.Name = elements.Element(NS_MUSIC + TAG_PRIMARY_ARTIST)
                .Element(NS_MUSIC + TAG_NAME).Value;
            artist.Url = ParseLink(artist.ID, VALUE_ARTIST);
            album.Artist = artist; // Add Artist to Album
            ((Track)item.Key).Album = album; // Add Album to Track
            ((List<Track>)item.Value).Add((Track)item.Key); // Add Track to Tracks
        }

        /// <summary>GetArtist</summary>
        /// <param name="tracks">Tracks</param>
        private void GetAlbum(string id, KeyValuePair<Track, List<Track>> item)
        {          
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
            {
                try
                {
                    if (!e.Cancelled && e.Error == null)
                    {
                        ParseAlbum(e.Result, (KeyValuePair<Track, List<Track>>)e.UserState);
                    }
                }
                catch
                {
                    // Do Nothing
                }
            };
            client.DownloadStringAsync(new Uri(string.Format(URL_ALBUM, id)), item);
        }
        #endregion

        #region Tracks
        /// <summary>ParseTracks</summary>
        /// <returns>List of Track</returns>
        private void ParseTracks(string data, List<Track> tracks)
        {
            XElement elements = XElement.Parse(data);
            foreach (XElement element in elements.Elements(NS_ATOM + TAG_ENTRY))
            {
                Track track = new Track();
                track.ID = element.Element(NS_ATOM + TAG_ID).Value;
                track.Name = element.Element(NS_ATOM + TAG_TITLE).Value;
                try
                {
                    track.PlayUrl = ParsePlayLink(element.Element(NS_MUSIC + TAG_TRACK)
                        .Element(NS_MUSIC + TAG_RIGHTS).Element(NS_MUSIC + TAG_RIGHT)
                        .Element(NS_MUSIC + TAG_CODE).Value);
                }
                catch
                {
                    track.PlayUrl = null;
                }
                // Album
                string id = ParseID(element.Element(NS_MUSIC + TAG_TRACK)
                    .Element(NS_MUSIC + TAG_ALBUM)
                    .Element(NS_MUSIC + TAG_ID).Value);
                // Get Album
                if (HasID(id)) { GetAlbum(id, new KeyValuePair<Track, List<Track>>(track,tracks)); } 
            }
        }
        #endregion

        #region Favs
        /// <summary>GetFavs</summary>
        /// <param name="zunecard">Get Favourite Tracks</param>
        private void GetFavs(ref ZuneCard zunecard)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
            {
                try
                {
                    if (!e.Cancelled && e.Error == null)
                    {
                        ParseTracks(e.Result, ((ZuneCard)e.UserState).Favs);
                    }
                }
                catch
                {
                    ((ZuneCard)e.UserState).Favs = null;
                }
            };
            client.DownloadStringAsync(new Uri(string.Format(URL_FAVS, zunecard.Tag)), zunecard);
        }
        #endregion

        #region Recent
        /// <summary>GetRecent</summary>
        /// <param name="zunecard">Get Recent Tracks</param>
        private void GetRecent(ref ZuneCard zunecard)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
            {
                try
                {
                    if (!e.Cancelled && e.Error == null)
                    {
                        ParseTracks(e.Result, ((ZuneCard)e.UserState).Recent);
                    }
                }
                catch
                {
                    ((ZuneCard)e.UserState).Recent = null;
                }
            };
            client.DownloadStringAsync(new Uri(string.Format(URL_RECENT, zunecard.Tag)), zunecard);
        }
        #endregion

        #region Artists

        /// <summary>Parse Artist</summary>
        /// <param name="data">XML Data</param>
        /// <returns>Artist</returns>
        private void ParseArtist(string data, List<Artist> artists)
        {
            XElement elements = XElement.Parse(data);
            Artist artist = new Artist();
            artist.ID = elements.Element(NS_ATOM + TAG_ID).Value;
            artist.Name = elements.Element(NS_ATOM + TAG_TITLE).Value;
            artist.Plays = int.Parse(elements.Element(NS_MUSIC + TAG_PLAY).Value);
            artist.Genre = elements.Element(NS_MUSIC + TAG_GENRE).Element(NS_MUSIC + TAG_TITLE).Value;
            artist.Image = ParseVisual(elements.Element(NS_MUSIC + TAG_IMAGE).Element(NS_MUSIC + TAG_ID).Value);
            artist.Url = ParseLink(artist.ID, VALUE_ARTIST);
            // Enforce Unique
            if ((from item in artists where item.ID == artist.ID select item.ID).Count() == ZERO)
            {
                artists.Add(artist);
            }
        }

        /// <summary>GetArtist</summary>
        /// <param name="artist">Artist</param>
        private void GetArtist(string id, ref List<Artist> artists)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
            {
                try
                {
                    if (!e.Cancelled && e.Error == null)
                    {
                        ParseArtist(e.Result,(List<Artist>)e.UserState);
                    }
                }
                catch
                {
                    // Do Nothing
                }
            };
            client.DownloadStringAsync(new Uri(string.Format(URL_ARTIST, id)), artists);
        }

        /// <summary>Parse Artists</summary>
        /// <param name="data">XML Data</param>
        /// <returns>Artists</returns>
        private void ParseArtists(string data, List<Artist> artists)
        {
            XElement elements = XElement.Parse(data);
            foreach (XElement element in elements.Elements(NS_ATOM + TAG_ENTRY))
            {
                string id = ParseID(element.Element(NS_PROFILE + TAG_ARTIST).Element(NS_PROFILE + TAG_ID).Value);
                if (HasID(id)) { GetArtist(id , ref artists); } // Get Artist
            }
        }

        /// <summary>Get Artists</summary>
        /// <param name="zunecard">ZuneCard</param>
        /// <helper>ParseArtists</helper>
        private void GetArtists(ref ZuneCard zunecard)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
            {
                try
                {
                    if (!e.Cancelled && e.Error == null)
                    {
                        ParseArtists(e.Result,((ZuneCard)e.UserState).Artists);
                    }
                }
                catch
                {
                    ((ZuneCard)e.UserState).Artists = null;
                }
            };
            client.DownloadStringAsync(new Uri(string.Format(URL_ARTISTS, zunecard.Tag)), zunecard);
        }
        #endregion

        #region Badges
        /// <summary>Parse Badges</summary>
        /// <param name="data">XML Data</param>
        /// <returns>Badges</returns>
        private List<Badge> ParseBadges(string data)
        {
            List<Badge> badges = new List<Badge>();
            XElement elements = XElement.Parse(data);
            foreach (XElement element in elements.Elements(NS_ATOM + TAG_ENTRY))
            {
                Badge badge = new Badge();
                badge.Name = element.Element(NS_ATOM + TAG_TITLE).Value;
                badge.Description = element.Element(NS_ATOM + TAG_CONTENT).Value;
                badge.Type = element.Element(NS_PROFILE + TAG_TYPE).Value;
                badge.Image = new Uri(element.Element(NS_ATOM + TAG_LINK).Attribute(ATTR_HREF).Value);
                badges.Add(badge);
            }
            return badges;
        }

        /// <summary>Get Badges</summary>
        /// <param name="zunecard">ZuneCard</param>
        /// <helper>ParseBadges</helper>
        private void GetBadges(ref ZuneCard zunecard)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
            {
                try
                {
                    if (!e.Cancelled && e.Error == null)
                    {
                        ((ZuneCard)e.UserState).Badges = ParseBadges(e.Result);
                    }
                }
                catch
                {
                    ((ZuneCard)e.UserState).Badges = null;
                }
            };
            client.DownloadStringAsync(new Uri(string.Format(URL_BADGES, zunecard.Tag)), zunecard);
        }
        #endregion

        #region Profile
        /// <summary>ParseProfileImage</summary>
        /// <param name="element">XElement</param>
        /// <param name="name">Name</param>
        /// <returns>Parse Profile Image</returns>
        private Uri ParseProfileImage(XElement element, string name)
        {
            return new Uri((from item in element.Elements(NS_PROFILE + TAG_LINK)
                            where (string)item.Attribute(ATTR_TITLE) == name
                            select item).Single().Attribute(ATTR_HREF).Value);
        }

        /// <summary>Parse Profile</summary>
        /// <param name="zunecard">ZuneCard</param>
        /// <param name="data">XML Data</param>
        private void ParseProfile(ZuneCard zunecard, string data)
        {
            XElement elements = XElement.Parse(data);
            zunecard.Plays = int.Parse(elements.Element(NS_PROFILE + TAG_PLAYCOUNT).Value);
            zunecard.Tile = ParseProfileImage(elements.Element(NS_PROFILE + TAG_IMAGES), VALUE_USER_TILE);
            zunecard.Background = ParseProfileImage(elements.Element(NS_PROFILE + TAG_IMAGES), VALUE_BACKGROUND);
            zunecard.Name = elements.Element(NS_PROFILE + TAG_DISPLAY_NAME).Value;
            zunecard.Status = elements.Element(NS_PROFILE + TAG_STATUS).Value;
            zunecard.Biography = elements.Element(NS_PROFILE + TAG_BIO).Value;
            zunecard.Location = elements.Element(NS_PROFILE + TAG_LOCATION).Value;
        }

        /// <summary>Get Profile</summary>
        /// <param name="zunecard">ZuneCard</param>
        /// <helper>ParseProfile</helper>
        private void GetProfile(ref ZuneCard zunecard)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
            {
                try
                {
                    if (!e.Cancelled && e.Error == null)
                    {
                        ParseProfile((ZuneCard)e.UserState, e.Result);
                    }
                }
                catch
                {
                    ProfileFailed(this, EventArgs.Empty);
                }
            };
            client.DownloadStringAsync(new Uri(string.Format(URL_PROFILE, zunecard.Tag)), zunecard);
        }
        #endregion

        #region Friends List
        /// <summary>Parse Friends</summary>
        /// <param name="data">XML Data</param>
        /// <returns>Friends List</returns>
        private List<Friend> ParseFriends(string data)
        {
            List<Friend> friends = new List<Friend>();
            XElement elements = XElement.Parse(data);
            foreach (XElement element in elements.Elements(NS_ATOM + TAG_ENTRY))
            {
                Friend friend = new Friend();
                friend.Tag = element.Element(NS_PROFILE + TAG_ZUNE).Value;
                friend.PlaysText = element.Element(NS_PROFILE + TAG_PLAYCOUNT).Value;
                friend.TileUri = new Uri(String.Format(URL_TILE, friend.Tag));
                friends.Add(friend);
            }
            return friends;
        }

        /// <summary>GetFriends</summary>
        /// <param name="zunecard">Zune Card</param>
        /// <helper>Parse Friends</helper>
        private void GetFriends(ref ZuneCard zunecard)
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
            {
                try
                {
                    if (!e.Cancelled && e.Error == null)
                    {
                        ((ZuneCard)e.UserState).Friends = ParseFriends(e.Result);
                    }
                }
                catch
                {
                    ((ZuneCard)e.UserState).Friends = null;
                }
            };
            client.DownloadStringAsync(new Uri(string.Format(URL_FRIENDS, zunecard.Tag)), zunecard);
        }
        #endregion

        #region Public Methods
        /// <summary>Card</summary>
        /// <param name="tag">Zune Tag</param>
        /// <returns>Zune Card</returns>
        public ZuneCard Card(string tag)
        {
            ZuneCard zunecard = new ZuneCard();
            zunecard.Tag = tag; // 0. Get Zune Tag
            zunecard.HomeUrl = new Uri(String.Format(URL_HOME,tag)); // Get Link
            GetFavs(ref zunecard); // 1. Get Favourites
            GetRecent(ref zunecard); // 2. Get Recent
            GetArtists(ref zunecard); // 3. Get Artists
            GetBadges(ref zunecard); // 4. Get Badges
            GetProfile(ref zunecard); // 5. Get Profile
            GetFriends(ref zunecard); // 6. Get Friends
            return zunecard;
        }
        #endregion

        #region Events
        /// <summary>Failed Event</summary>
        public static event EventHandler ProfileFailed = delegate { };
        #endregion
    }
}