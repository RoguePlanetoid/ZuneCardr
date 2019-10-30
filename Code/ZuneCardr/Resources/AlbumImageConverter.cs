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
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace ZuneCardr
{
    /// <summary>Album Image Converter</summary>
    /// <version>1.0.0</version>
    /// <created>30 May 2011</created>
    /// <modified>30 May 2011</modified>
    public class AlbumImageConverter : IValueConverter
    {
        #region Public Methods
        /// <summary>Convert</summary>
        /// <param name="value">Source Value</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">Parameters</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>Object</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (App.Framework.Data.Card.Albums.Count > 0)
            {
                Guid id = new Guid(value.ToString());
                Syndication.Feed feed = (from Syndication.Feed item
                                             in App.Framework.Data.Card.Albums
                                         where item.ID == id
                                         select item).FirstOrDefault();
                if (feed != null)
                {
                    if (feed.Image != null)
                    {
                        return feed.Image.Small;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
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
        #endregion
    }
}
