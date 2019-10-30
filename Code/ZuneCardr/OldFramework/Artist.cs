using System;
using System.Net;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace ZuneCardr
{
    /// <summary>Artist Information</summary>
    /// <version>4.0.0</version>
    /// <created>29 April 2011</created>
    /// <modified>29 April 2011</modified>
    [DataContract]
    public class Artist
    {
        #region Private Constants
        private const int ZERO = 0;
        private const string BLANK = "";
        private const string FORMAT_PLAYS = "{0:#,##} plays";
        #endregion

        #region Private Members
        private string id = BLANK;
        private string name = BLANK;
        private int plays = ZERO;
        private string genre = BLANK;
        private Uri url;
        private Visual image;
        #endregion

        #region Public Properties
        /// <summary>ID</summary>
        /// <returns>Artist ID</returns>
        [DataMember]
        public string ID { get { return id; } set { id = value; } }

        /// <summary>Name</summary>
        /// <returns>Artist Name</returns>
        [DataMember]
        public string Name { get { return name; } set { name = value; } }

        /// <summary>Plays</summary>
        /// <returns>Artist Plays</returns>
        [DataMember]
        public int Plays { get { return plays; } set { plays = value; } }

        /// <summary>Plays</summary>
        /// <returns>Artist Plays (Full)</returns>
        public string PlaysText { get { return String.Format(FORMAT_PLAYS,plays); } }

        /// <summary>Genre</summary>
        /// <returns>Artist Primary Genre</returns>
        [DataMember]
        public string Genre { get { return genre; } set { genre = value; } }

        /// <summary>URL</summary>
        /// <returns>Artist Link</returns>
        public Uri Url { get { return url; } set { url = value; } }

        /// <summary>URL Serialisable</summary>
        [DataMember]
        public string UrlLink { get { return Url == null ? null : Url.ToString(); } set { Url = value == null ? null : new Uri(value); } }

        /// <summary>ShowUrl</summary>
        /// <returns>Visible if Url Present, Collapsed if Url not Present</returns>
        public Visibility ShowUrl { get { return Url != null ? Visibility.Visible : Visibility.Collapsed; } }

        /// <summary>Artist Image</summary>
        [DataMember]
        public Visual Image { get { return image; } set { image = value; } }
        #endregion
    }
}
