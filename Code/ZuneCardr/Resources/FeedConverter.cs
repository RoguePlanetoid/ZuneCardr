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
using System.Windows.Data;
using System.Globalization;

namespace ZuneCardr
{
    /// <summary>Feed Converter</summary>
    /// <version>1.0.0</version>
    /// <created>30 May 2011</created>
    /// <modified>30 May 2011</modified>
    public class FeedConverter : IValueConverter
    {
        #region Public Constants
        public const string Album = "Album";
        public const string Artist = "Artist";
        // Delegate
        private delegate void Changed();
        #endregion

        /// <summary>Convert</summary>
        /// <param name="value">GUID</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">Parameters</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>Object</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {           
            Changed changed = delegate() { };
            Guid id = new Guid((string)value);
            Uri url = Url.Album(id); // Default
            switch (parameter.ToString())
            {
                case Artist:
                    break;
                default:
                    url = Url.Album(id);
                    break;
            }
            targetType = typeof(Syndication.Feed);
            return new Syndication.Feed(url,changed);
        }

        /// <summary>ConvertBack</summary>
        /// <param name="value">Target Value</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">Parameter</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>Object</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
