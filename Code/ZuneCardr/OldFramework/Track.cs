using System;
using System.Net;
using System.Windows;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace ZuneCardr
{
    /// <summary>Track Information</summary>
    /// <version>4.0.0</version>
    /// <created>29 April 2011</created>
    /// <modified>29 April 2011</modified>
    [DataContract]
    public class Track : INotifyPropertyChanged
    {
        #region Private Constants
        private const string BLANK = "";
        private const string PROP_PLAYURL = "PlayUrl";
        private const string PROP_SHOWPLAYURL = "ShowPlayUrl";
        private const string PROP_PLAYURLLINK = "PlayUrlLink";
        #endregion

        #region Private Members
        private string id = BLANK;
        private string name = BLANK;
        private string genre = BLANK;
        private Album album;
        private Uri playUrl;
        private Uri buyUrl;
        private Uri sendUrl;
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
        /// <returns>Track ID</returns>
        [DataMember]
        public string ID { get { return id.Trim(); } set { id = value; } }

        /// <summary>Name</summary>
        /// <returns>Track Name</returns>
        [DataMember]
        public string Name { get { return name.Trim(); } set { name = value; } }

        /// <summary>Genre</summary>
        /// <returns>Track Genre</returns>
        [DataMember]
        public string Genre { get { return genre == null ? null : genre.Trim(); } set { genre = value; } }

        /// <summary>Album</summary>
        /// <returns>Track Album</returns>
        [DataMember]
        public Album Album { get { return album; } set { album = value; } }

        /// <summary>Buy URL</summary>
        /// <returns>Track Buy Link</returns>
        public Uri BuyUrl { get { return buyUrl; } set { buyUrl = value; } }

        /// <summary>Buy URL Link</summary>
        [DataMember]
        public string BuyUrlLink { get { return buyUrl == null ? null : buyUrl.ToString(); } set { buyUrl = value == null ? null : new Uri(value); } }

        /// <summary>ShowClipUrl</summary>
        /// <returns>Visible if Url Present, Collapsed if Url not Present</returns>
        public Visibility ShowBuyUrl { get { return buyUrl != null ? Visibility.Visible : Visibility.Collapsed; } }

        /// <summary>Send URL</summary>
        /// <returns>Track Send Link</returns>
        public Uri SendUrl { get { return sendUrl; } set { sendUrl = value; } }

        /// <summary>SendUrlLink</summary>
        [DataMember]
        public string SendUrlLink { get { return sendUrl == null ? null : sendUrl.ToString(); } set { sendUrl = value == null ? null : new Uri(value); } }

        /// <summary>ShowSendUrl</summary>
        /// <returns>Visible if Url Present, Collapsed if Url not Present</returns>
        public Visibility ShowSendUrl { get { return sendUrl != null ? Visibility.Visible : Visibility.Collapsed; } }

        /// <summary>Play URL</summary>
        /// <returns>Track Play Link</returns>
        public Uri PlayUrl { get { return playUrl; } set { playUrl = value; NotifyPropertyChanged(PROP_PLAYURL); NotifyPropertyChanged(PROP_PLAYURLLINK); NotifyPropertyChanged(PROP_SHOWPLAYURL); } }

        /// <summary>PlayUrlLink</summary>
        [DataMember]
        public string PlayUrlLink { get { return playUrl == null ? null : playUrl.ToString(); } set { playUrl = value == null ? null : new Uri(value); } }

        /// <summary>ShowPlayUrl</summary>
        /// <returns>Visible if Url Present, Collapsed if Url not Present</returns>
        public Visibility ShowPlayUrl { get { return playUrl != null ? Visibility.Visible : Visibility.Collapsed; } }

        #endregion
    }
}
