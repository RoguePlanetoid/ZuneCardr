using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace ZuneCardr
{
    /// <summary>Badge Information</summary>
    /// <version>3.0.0</version>
    /// <created>29 April 2011</created>
    /// <modified>29 April 2011</modified>
    [DataContract]
    public class Badge
    {
        #region Private Constants
        private const string BLANK = "";
        #endregion

        #region Private Members
        private string name = BLANK;
        private string desc = BLANK;
        private string type = BLANK;
        private Uri image;
        #endregion

        #region Public Properties

        /// <summary>Name</summary>
        /// <returns>Badge Name</returns>
        [DataMember]
        public string Name { get { return name; } set { name = value; } }

        /// <summary>Description</summary>
        /// <returns>Badge Description</returns>
        [DataMember]
        public string Description { get { return desc; } set { desc = value; } }

        /// <summary>Type</summary>
        /// <returns>Badge Type</returns>
        [DataMember]
        public string Type { get { return type; } set { type = value; } }

        /// <summary>Badge Image</summary>
        public Uri Image { get { return image; } set { image = value; } }

        /// <summary>Badge Image Serialisable</summary>
        [DataMember]
        public string ImageLink { get { return image == null ? null : image.ToString(); } set { image = value == null ? null : new Uri(value); } }
        #endregion
    }
}
