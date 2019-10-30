using System;
using System.Windows;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZuneCardr
{
    /// <summary>Album Information</summary>
    /// <version>4.0.0</version>
    /// <created>4 July 2010</created>
    /// <modified>29 April 2011</modified>
    [DataContract]
    public class Album
    {
        #region Private Constants
        private const string BLANK = "";
        #endregion

        #region Private Members
        private string id = BLANK;
        private string name = BLANK;
        private Uri url;
        private Visual image;
        private Artist artist;
        #endregion

        #region Public Properties
        /// <summary>ID</summary>
        /// <returns>Album ID</returns>
        [DataMember]
        public string ID { get { return id; } set { id = value; } }

        /// <summary>Name</summary>
        /// <returns>Album Name</returns>
        [DataMember]
        public string Name { get { return name; } set { name = value; } }

        /// <summary>URL</summary>
        /// <returns>Album Link</returns>
        public Uri Url { get { return url; } set { url = value; } }

        /// <summary>URL Serialisable</summary>
        [DataMember]
        public string UrlLink { get { return Url == null ? null : Url.ToString(); } set { Url = value == null ? null : new Uri(value); } }

        /// <summary>ShowUrl</summary>
        /// <returns>Visible if Url Present, Collapsed if Url not Present</returns>
        public Visibility ShowUrl { get { return Url != null ? Visibility.Visible : Visibility.Collapsed; } }

        /// <summary>Album Image</summary>
        [DataMember]
        public Visual Image { get { return image; } set { image = value; } }

        /// <summary>Artist</summary>
        /// <returns>Album Artist</returns>
        [DataMember]
        public Artist Artist { get { return artist; } set { artist = value; } }
        #endregion
    }
}
