using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Xml;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace ZuneCardr
{
    /// <summary>Main</summary>
    /// <version>3.0.0</version>
    /// <created>4 July 2010</created>
    /// <modified>29 April 2011</modified>
    public class Framework : INotifyPropertyChanged
    {
        #region Private Constants
        private const string ERR_DOWNLOAD = "There was a problem downloading for {0}";
        private const string ERR_PARSE = "There was a problem reading for {0}";
        private const string ERR_VALID = "Zune Tag contains disallow characters or is too long";
        private const string ERR_SAVE = "Information could not be saved";
        private const string ERR_LOAD = "Information could not be opened";
        private const string PROP_ZUNECARD = "ZuneCard";
        private const string PROP_ZUNECARDS = "ZuneCards";
        private const string FILENAME = "ZuneCards.Data";
        #endregion

        #region Static Members
        private static IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
        #endregion

        #region Private Members
        private Data data = new Data();
        private ZuneCard zuneCard = new ZuneCard();
        private ObservableCollection<ZuneCard> zuneCards = new ObservableCollection<ZuneCard>();
        private string message;
        #endregion

        #region Public Members
        public Parser Parser = new Parser();
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

        #region Private Methods

        /// <summary>Delete</summary>
        /// <param name="pattern">File Pattern</param>
        private void Delete()
        {
            foreach(string item in storage.GetFileNames())
            {
                storage.DeleteFile(item);
            }
        }

        /// <summary>Get</summary>
        /// <param name="tag">Zune Tag</param>
        /// <returns>ZuneCard</returns>
        private ZuneCard Get(string tag)
        {
            return (from ZuneCard item in ZuneCards
                    where item.Tag == tag
                    select item).FirstOrDefault();
        }

        /// <summary>Exists</summary>
        /// <param name="tag">Tag</param>
        /// <returns>True if Exists, False if Not</returns>
        private bool Exists(string tag)
        {
            return (from ZuneCard item in ZuneCards
                    where item.Tag == tag
                    select item).Any();
        }

        /// <summary>Parse</summary>
        /// <param name="tag">Zune Tag</param>
        private void Parse(string tag)
        {
            ZuneCard card = Parser.Card(tag);
            if (card != null)
            {
                if (Exists(tag))
                {
                    int position;
                    ZuneCard previous = new ZuneCard();
                    previous = Get(tag);
                    position = ZuneCards.IndexOf(previous);
                    Remove(previous);
                    ZuneCards.Insert(position, card);
                }
                else
                {
                    ZuneCards.Add(card);
                }
                NotifyPropertyChanged(PROP_ZUNECARDS);
                NotifyPropertyChanged(PROP_ZUNECARD);
                Completed(this, EventArgs.Empty);
            }
            else
            {
                Failed(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>Deserialize Card Data</summary>
        public void Deserialize()
        {
            try
            {
                if (storage.FileExists(FILENAME))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<ZuneCard>));
                    using (IsolatedStorageFileStream location = new IsolatedStorageFileStream(FILENAME, System.IO.FileMode.Open, storage))
                    {
                        zuneCards = (ObservableCollection<ZuneCard>)serializer.ReadObject(location);
                        NotifyPropertyChanged(PROP_ZUNECARDS);
                    }
                }
            }
            catch
            {
                MessageBox.Show(ERR_LOAD);
            }
        }

        /// <summary>Serialize Card Data</summary>
        public void Serialize()
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<ZuneCard>));
                using (IsolatedStorageFileStream location = new IsolatedStorageFileStream(FILENAME, System.IO.FileMode.Create, storage))
                {
                    serializer.WriteObject(location, zuneCards);
                }
            }
            catch
            {
                // Do Nothing on Exception
            }
        }

        /// <summary>Add</summary>
        /// <param name="tag">Tag</param>
        public void Add(string tag)
        {
            if (Parser.IsValidTag(tag)) { Parse(tag); }
        }

        /// <summary>Refresh</summary>
        /// <param name="card">Zune Card</param>
        public void Refresh(ZuneCard card)
        {
            Add(card.Tag);
        }

        /// <summary>Remove Tag</summary>
        /// <param name="card">Zune Card</param>
        public void Remove(ZuneCard card)
        {
            ZuneCards.Remove(card);
            NotifyPropertyChanged(PROP_ZUNECARD);
            NotifyPropertyChanged(PROP_ZUNECARDS);
        }

        /// <summary>Clear Tags</summary>
        public void Clear()
        {
            ZuneCard = null;
            ZuneCards.Clear();
            Delete();
            NotifyPropertyChanged(PROP_ZUNECARD);
            NotifyPropertyChanged(PROP_ZUNECARDS);
        }

        /// <summary>Reset Tag</summary>
        public void Reset()
        {
            ZuneCard = null;
            NotifyPropertyChanged(PROP_ZUNECARD);
        }
        #endregion

        #region Public Properties

        /// <summary>Home</summary>
        public Data Data { get { return data; } }

        /// <summary>Zune Card</summary>
        public ZuneCard ZuneCard { get { return zuneCard; } set { zuneCard = value; NotifyPropertyChanged(PROP_ZUNECARD); } }

        /// <summary>Zune Cards</summary>
        public ObservableCollection<ZuneCard> ZuneCards { get { return zuneCards; } set { zuneCards = value; NotifyPropertyChanged(PROP_ZUNECARDS); } }

        /// <summary>Message</summary>
        public string Message { get { return message; } set { message = value; } }
        #endregion

        #region Events
        /// <summary>Completed Event</summary>
        public static event EventHandler Completed = delegate { };

        /// <summary>Failed Event</summary>
        public static event EventHandler Failed = delegate { };
        #endregion
    }
}
