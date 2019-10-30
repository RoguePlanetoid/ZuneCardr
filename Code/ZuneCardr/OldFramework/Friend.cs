using System;
using System.Net;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace ZuneCardr
{
    /// <summary>Zune Social Friend</summary>
    /// <version>1.1.0</version>
    /// <created>30 November 2010</created>
    /// <modified>2 December 2010</modified>
    [DataContract]
    public class Friend
    {
        #region Private Constants
        private const string ZERO = "0";
        private const string BLANK = "";
        private const string FORMAT = "#,#";
        private readonly Regex PLAYS_REGEX = new Regex("[^\\d,]");
        #endregion

        #region Private Members
        private string tag = BLANK;
        private string plays = BLANK;
        private Uri tileUri;
        private BitmapImage tileImage;
        #endregion

        #region Private Members

        /// <summary>FormatNumber</summary>
        /// <param name="source">Source</param>
        /// <returns>Target Formatted</returns>
        private string FormatNumber(string source)
        {
            string result = BLANK;
            try
            {
                source = string.Join(String.Empty, PLAYS_REGEX.Split(source));
                result = int.Parse(source).ToString(FORMAT);
            }
            catch
            {
                result = source;
            }
            return result == BLANK ? ZERO : result;
        }

        #endregion

        #region Public Properties

        /// <summary>Tag</summary>
        /// <returns>Member Tag</returns>
        [DataMember]
        public string Tag { get { return tag.Trim(); } set { tag = value; } }

        /// <summary>Plays</summary>
        [DataMember]
        public string PlaysText { get { return FormatNumber(plays); } set { plays = value; } }

        /// <summary>Tile</summary>
        public BitmapImage Tile { get { return tileImage; } set { tileImage = value; } }

        /// <summary>TileUri</summary>
        public Uri TileUri { get { return tileUri; } set { tileUri = value; tileImage = new BitmapImage(tileUri); } }

        /// <summary>Tile - Serialisable</summary>
        [DataMember]
        public string TileLink
        {
            get { return tileUri == null ? null : tileUri.ToString(); }
            set { tileUri = value == null ? null : new Uri(value); tileImage = new BitmapImage(tileUri); }
        }
        #endregion
    }
}
