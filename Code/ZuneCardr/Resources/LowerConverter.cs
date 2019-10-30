using System;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace ZuneCardr
{
    /// <summary>LowerCase Converter</summary>
    /// <version>1.0.0</version>
    /// <created>13 May 2011</created>
    /// <modified>13 May 2011</modified>
    public class LowerConverter : IValueConverter
    {
        /// <summary>Convert</summary>
        /// <param name="value">Source Value</param>
        /// <param name="targetType">Type</param>
        /// <param name="parameter">Parameters</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns>Object</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).ToLower();
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

