using System;
using System.Net;
using System.Windows;
using System.Runtime.Serialization;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.IO;
using System.ComponentModel;
using System.Windows.Controls;
using System.IO.IsolatedStorage;

namespace ZuneCardr
{
    /// <summary>Visual Information</summary>
    /// <version>2.0.0</version>
    /// <created>4 July 2010</created>
    /// <modified>15 November 2010</modified>
    [DataContract]
    public class Visual
    {
        #region Private Members
        private BitmapImage largeImage;
        private BitmapImage smallImage;
        private Uri largeImageUri;
        private Uri smallImageUri;
        #endregion

        #region Public Methods
        /// <summary>Constructor</summary>
        /// <param name="large">Large Image</param>
        /// <param name="small">Small Image</param>
        public Visual(Uri large, Uri small)
        {
            largeImageUri = large;
            smallImageUri = small;
            largeImage = new BitmapImage(largeImageUri);
            smallImage = new BitmapImage(smallImageUri);
        }
        #endregion

        #region Public Properties

        /// <summary>Small Image</summary>
        public BitmapImage Small { get { return smallImage; } set { smallImage = value; } }

        /// <summary>Small Image - Serialisable</summary>
        [DataMember]
        public string SmallImage { get { return smallImageUri == null ? null : smallImageUri.ToString(); } 
            set { smallImageUri = value == null ? null : new Uri(value); smallImage = new BitmapImage(smallImageUri); } }

        /// <summary>Small Image</summary>
        public BitmapImage Large { get { return largeImage; } set { largeImage = value; } }

        /// <summary>Small Image - Serialisable</summary>
        [DataMember]
        public string LargeImage
        {
            get { return largeImageUri == null ? null : largeImageUri.ToString(); }
            set { largeImageUri = value == null ? null : new Uri(value); largeImage = new BitmapImage(largeImageUri); }
        }
        #endregion
    }
}
