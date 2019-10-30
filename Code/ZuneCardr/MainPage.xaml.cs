using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using System.Text.RegularExpressions;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;
using Microsoft.Devices;
using System.Windows.Media;

namespace ZuneCardr
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Private Constants
        private const int ZERO = 0;
        private const string BLANK = "";
        private const string CAPTION = "ZuneCardr";
        private const string MSG_CLEAR = "Delete all Zune Cards from list?";
        private const string MSG_LABEL = "Enter Zune Tag";
        private const string MSG_VALID = "Enter Valid Zune Tag";
        private const string MSG_CONNECTION = "Connection unavailable";
        private const string MSG_ERROR = "There was a problem with the Zune Card";
        private const string MSG_PLAY = "Playing music recommended via USB-cable or WiFi, Continue?";
        private const string WEBSITE = "http://www.zunecardr.com";
        private const string CONTACT_EMAIL = "ZuneCardr <contact@zunecardr.com>";
        private const string CONTACT_SUBJECT = "Contact ZuneCardr (2.4.0)";
        private const string ABOUT = "ZuneCardr v2.4.0 by Comentsys";
        // Elements
        private const string SCROLL_VIEWER = "ScrollViewer";
        // Validation
        private readonly Regex VALIDATION = new Regex("^[a-zA-Z0-9_\\s]*$");
        private const int MIN_LENGTH = ZERO;
        private const int MAX_LENGTH = 16;
        // State Information
        private const string TEXTBOX_NAME = "ZuneTag";
        private const string TEXTBOX_FOCUS = "textbox-focus";
        private const string TEXTBOX_TEXT = "textbox-text";
        private const string CARD_SELECTED = "card-selected";
        private const string PIVOT_SELECTED = "pivot-selected";
        // Scroll States
        private const string CARD_SCROLL = "card-scroll";
        private const string FAVS_SCROLL = "favs-scroll";
        private const string RECENT_SCROLL = "recent-scroll";
        private const string ARTIST_SCROLL = "artist-scroll";
        private const string BADGES_SCROLL = "badges-scroll";
        private const string FRIEND_SCROLL = "friend-scroll";
        // Haptic Time
        private const int HAPTIC_TIME = 100;
        #endregion

        #region Private Members
        // State Information
        private bool hasTextBoxFocus = false;
        private bool hasPivotSelected = false;
        private bool hasBeenFocused = false;
        private bool hasLayoutUpdated = true;
        #endregion

        #region Private Methods

        /// <summary>Haptic</summary>
        private void Haptic()
        {
            VibrateController vibrate = VibrateController.Default;
            vibrate.Start(TimeSpan.FromMilliseconds(HAPTIC_TIME));
        }

        /// <summary>GoEmail</summary>
        private void GoEmail(string to, string subject)
        {
            try
            {
                EmailComposeTask task = new EmailComposeTask();
                task.To = to;
                task.Subject = subject;
                task.Body = BLANK;
                task.Show();
            }
            catch
            {
                // Do Nothing on Exception
            }
        }

        /// <summary>GoMarketplace</summary>
        /// <param name="search">Search Term</param>
        private void GoMarketplace(string search)
        {
            try
            {
                if (search != BLANK)
                {
                    App.Framework.Serialize();
                    MarketplaceSearchTask task = new MarketplaceSearchTask();
                    task.ContentType = MarketplaceContentType.Music;
                    task.SearchTerms = search;
                    task.Show();
                }
            }
            catch
            {
                // Do Nothing on Exception
            }
        }

        /// <summary>GoTo</summary>
        /// <param name="url">Website Address</param>
        private void GoTo(string url)
        {
            try
            {
                App.Framework.Serialize();
                WebBrowserTask task = new WebBrowserTask();
                task.URL = HttpUtility.UrlEncode(url);
                task.Show();
            }
            catch
            {
                // Do Nothing on Exception
            }  
        }

        /// <summary>GoPlay</summary>
        /// <param name="url">URL</param>
        private void GoPlay(string url)
        {
            try
            {
                MediaPlayerLauncher task = new MediaPlayerLauncher();
                task.Media = new Uri(url);
                task.Show();
            }
            catch
            {
                // Do Nothing
            }
        }

        /// <summary>Has State Item</summary>
        /// <param name="key">State Key</param>
        /// <returns>True if Present, False if Not</returns>
        private bool hasStateItem(string key)
        {
            try
            {
                return PhoneApplicationService.Current.State.ContainsKey(key);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>RemoveStateItem</summary>
        /// <param name="key">State Key</param>
        private void removeStateItem(string key)
        {
            try
            {
                PhoneApplicationService.Current.State.Remove(key);
            }
            catch
            {
                // Do Nothing
            }
        }

        /// <summary>Add State Item</summary>
        /// <param name="key">Name</param>
        /// <param name="value">Object</param>
        private void addStateItem(string key, object value)
        {
            try
            {
                if (hasStateItem(key))
                {
                    removeStateItem(key);
                }
                PhoneApplicationService.Current.State.Add(key, value);
            }
            catch
            {
                // Do Nothing
            }
        }

        /// <summary>Get State Item</summary>
        /// <param name="key">State Name</param>
        private object getStateItem(string key)
        {
            try
            {
                return PhoneApplicationService.Current.State[key];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Has Setting Item</summary>
        /// <param name="key">Setting Name</param>
        /// <returns>True if Present, False if Not</returns>
        private bool hasSettingItem(string key)
        {
            return IsolatedStorageSettings.ApplicationSettings.Contains(key);
        }

        /// <summary>Remove Setting Item</summary>
        /// <param name="key">Setting Name</param>
        private void removeSettingItem(string key)
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings.Remove(key);
            }
            catch
            {
                // Do Nothing
            }      
        }

        /// <summary>Set Setting Item</summary>
        /// <param name="key">Setting Name</param>
        /// <param name="value">Setting Value</param>
        private void setSettingItem(string key, object value)
        {
            try
            {
                IsolatedStorageSettings.ApplicationSettings[key] = value;
            }
            catch
            {
                // Do Nothing
            }
        }

        /// <summary>Add Setting Item</summary>
        /// <param name="key">Name</param>
        /// <param name="value">Object</param>
        private void addSettingItem(string key, object value)
        {
            try
            {
                if (hasSettingItem(key))
                {
                    setSettingItem(key, value);
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add(key, value);
                }
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch
            {
                // Do Nothing
            }
        }

        /// <summary>Get Setting Item</summary>
        /// <param name="key">Setting Name</param>
        private object getSettingItem(string key)
        {
            try
            {
                return IsolatedStorageSettings.ApplicationSettings[key];
            }
            catch
            {
                return null;   
            }
        }

        /// <summary>Select</summary>
        private void Select()
        {
            try
            {
                if (Cards.Items.Count > ZERO)
                {
                    Cards.SelectedIndex = ZERO;
                }
                else
                {
                    App.Framework.Reset();
                    this.DataContext = App.Framework.ZuneCard;
                }
            }
            catch
            {
                // Do Nothing
            }
        }

        #endregion

        /// <summary>Constructor</summary>
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = App.Framework.Data.Card;

            //Cards.ItemsSource = App.Framework.ZuneCards;
            //this.DataContext = App.Framework.ZuneCard;
            Framework.Completed += (object sender, EventArgs e) =>
            {
                Progress.IsIndeterminate = false;
                if (Cards.Items.Count > ZERO)
                {
                    Cards.SelectedIndex = Cards.Items.Count - 1;
                    ZuneTag.Text = BLANK;
                }
            };
            Framework.Failed += (object sender, EventArgs e) =>
            {
                Progress.IsIndeterminate = false;
                MessageBox.Show(App.Framework.Message);
            };
        }

        /// <summary>SaveScrollState</summary>
        /// <param name="listbox">ListBox</param>
        /// <param name="name">State Name</param>
        private void SaveScrollState(ListBox listbox,string name)
        {
            ScrollViewer viewer = ((VisualTreeHelper.GetChild(listbox, ZERO) as FrameworkElement).FindName(SCROLL_VIEWER) as ScrollViewer);
            addStateItem(name,viewer.VerticalOffset);
        }

        /// <summary>LoadScrollState</summary>
        /// <param name="listbox">ListBox</param>
        /// <param name="name">State Name</param>
        private void LoadScrollState(ListBox listbox, string name)
        {
            if (hasStateItem(name))
            {
                listbox.Loaded += delegate
                {
                    try
                    {
                        ScrollViewer viewer = ((VisualTreeHelper.GetChild(listbox, ZERO) as FrameworkElement).FindName(SCROLL_VIEWER) as ScrollViewer);
                        viewer.ScrollToVerticalOffset((double)getStateItem(name));
                    }
                    catch
                    {
                        // Do Nothing on Exception
                    }
                };
            }
        }

        #region Public Methods

        /// <summary>SaveState</summary>
        public void SaveState()
        {
            try
            {
                addStateItem(TEXTBOX_TEXT, ZuneTag.Text);
                FrameworkElement element = (FrameworkElement)FocusManager.GetFocusedElement();
                if (element != null)
                {
                    if (element.Name == TEXTBOX_NAME)
                    {
                        addStateItem(TEXTBOX_FOCUS, true);
                    }
                }
                if (Cards.SelectedItem != null)
                {
                    addSettingItem(CARD_SELECTED, Cards.SelectedIndex);
                }
                addStateItem(PIVOT_SELECTED, Pivot.SelectedIndex);
                // Lists
                SaveScrollState(Cards, CARD_SCROLL);
                SaveScrollState(Favs, FAVS_SCROLL);
                SaveScrollState(Recent, RECENT_SCROLL);
                SaveScrollState(Artists, ARTIST_SCROLL);
                SaveScrollState(Badges, BADGES_SCROLL);
                SaveScrollState(Friends, FRIEND_SCROLL);
            }
            catch
            {
                // Just state information, don't care if it doesn't save
            }
        }

        /// <summary>LoadState</summary>
        public void LoadState()
        {
            try
            {
                if (hasStateItem(TEXTBOX_TEXT)) { ZuneTag.Text = (string)getStateItem(TEXTBOX_TEXT); }

                if (hasStateItem(TEXTBOX_FOCUS)) { hasTextBoxFocus = true; }

                if (hasSettingItem(CARD_SELECTED)) { Cards.SelectedIndex = (int)getSettingItem(CARD_SELECTED); }

                if (hasStateItem(PIVOT_SELECTED)) { hasPivotSelected = true; }
                // Lists
                LoadScrollState(Cards, CARD_SCROLL);
                LoadScrollState(Favs, FAVS_SCROLL);
                LoadScrollState(Recent, RECENT_SCROLL);
                LoadScrollState(Artists, ARTIST_SCROLL);
                LoadScrollState(Badges, BADGES_SCROLL);
                LoadScrollState(Friends, FRIEND_SCROLL);
            }
            catch
            {
                // Just state information, don't care if it doesn't load
            }
            Cards.ItemsSource = App.Framework.ZuneCards;
            this.DataContext = App.Framework.ZuneCard;
        }

        /// <summary>IsValid</summary>
        /// <param name="source">String</param>
        /// <returns>True if Valid, False if Not</returns>
        public bool IsValid(string source)
        {
            return source.Length > MIN_LENGTH
                && source.Length <= MAX_LENGTH
                && VALIDATION.IsMatch(source);
        }

        /// <summary>Add Tag</summary>
        public void Add()
        {
            try
            {
                if (IsValid(ZuneTag.Text))
                {
                    Label.Text = MSG_LABEL;
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        //Progress.IsIndeterminate = true;
                        //App.Framework.Add(ZuneTag.Text);
                        App.Framework.Data.GetData(ZuneTag.Text);

                        this.Focus();
                    }
                    else
                    {
                        MessageBox.Show(MSG_CONNECTION);
                    }
                }
                else
                {
                    Label.Text = MSG_VALID;
                }
            }
            catch
            {
                MessageBox.Show(MSG_ERROR);
            }
        }

        /// <summary>Add</summary>
        /// <param name="ZuneTag">ZuneTag</param>
        public void Add(string Tag)
        {
            ZuneTag.Text = Tag;
            Pivot.SelectedIndex = ZERO;
            Add();
        }

        /// <summary>Remove</summary>
        public void Remove()
        {
            this.DataContext = App.Framework.Data.Card;
            //MessageBox.Show(App.Framework.Data.Card.Favs.Items.Count.ToString());
            //if (Cards.SelectedItem != null)
            //{
            //    App.Framework.Remove((ZuneCard)Cards.SelectedItem);
            //    Select();
            //}
        }

        /// <summary>Refresh</summary>
        public void Refresh()
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    if (Cards.SelectedItem != null)
                    {
                        App.Framework.Refresh((ZuneCard)Cards.SelectedItem);
                    }
                    else
                    {
                        foreach (ZuneCard item in Cards.Items)
                        {
                            App.Framework.Refresh((ZuneCard)item);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(MSG_CONNECTION);
                }
            }
            catch
            {
                MessageBox.Show(MSG_ERROR);
            }
        }

        /// <summary>Delete</summary>
        public void Delete()
        {
            if (MessageBox.Show(MSG_CLEAR, CAPTION, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                App.Framework.Clear();
                Select();
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>Cards Changed</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Cards_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (Cards.SelectedItem != null)
            {
                App.Framework.ZuneCard = (ZuneCard)Cards.SelectedItem;
                this.DataContext = App.Framework.ZuneCard;
            }
        }

        /// <summary>ZuneTag KeyUp</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void ZuneTag_KeyUp(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) Add(); }

        /// <summary>Add Tag</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Add_Click(object sender, EventArgs e) { Add(); }

        /// <summary>Remove Tag</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Remove_Click(object sender, EventArgs e) { Remove(); }

        /// <summary>Refresh</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Refresh_Click(object sender, EventArgs e) { Refresh(); }

        /// <summary>Delete Tags</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Delete_Click(object sender, EventArgs e) { Delete(); }

        /// <summary>Website</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Website_Click(object sender, EventArgs e) { GoTo(WEBSITE); }

        /// <summary>Contact</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Contact_Click(object sender, EventArgs e) { GoEmail(CONTACT_EMAIL, CONTACT_SUBJECT); }

        /// <summary>About</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void About_Click(object sender, EventArgs e) { MessageBox.Show(ABOUT, CAPTION, MessageBoxButton.OK); }

        /// <summary>Pivot_SelectionChanged</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationBar.IsVisible = Pivot.SelectedIndex == ZERO;
        }

        /// <summary>On Navigated From</summary>
        /// <param name="e">Event</param>
        /// <remarks>Save State</remarks>
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e) { SaveState(); }

        /// <summary>On Navigated To</summary>
        /// <param name="e">Event</param>
        /// <remarks>Load State on on App.IsActivated</remarks>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) { LoadState(); }

        /// <summary>Layout Updated</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        /// <remarks>Triggered once whole Page has Displayed, use for Focus</remarks>
        private void PhoneApplicationPage_LayoutUpdated(object sender, EventArgs e)
        {
            if (hasLayoutUpdated)
            {
                hasLayoutUpdated = false;
                if (hasTextBoxFocus)
                {
                    if (!hasBeenFocused)
                    {
                        ZuneTag.Focus();
                    }
                    else
                    {
                        hasBeenFocused = true;
                    }
                }
                if (hasPivotSelected)
                {
                    Pivot.SelectedIndex = (int)getStateItem(PIVOT_SELECTED);
                }
            } 
        }

        /// <summary>Zune Social</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Social_Page(object sender, RoutedEventArgs e)
        {
            GoTo(App.Framework.ZuneCard.HomeUrl.ToString());
        }

        /// <summary>Track Play</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Track_Play(object sender, RoutedEventArgs e)
        {
            Haptic(); // Vibrate
            GoPlay((String)((Button)sender).Tag);
        }

        /// <summary>Marketplace Menu</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Marketplace_Menu(object sender, RoutedEventArgs e)
        {
            GoMarketplace((String)((MenuItem)sender).Tag);
        }

        /// <summary>Track Menu</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Track_Menu(object sender, RoutedEventArgs e)
        {
            GoTo((String)((MenuItem)sender).Tag);
        }

        /// <summary>Add Friend</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event</param>
        private void Add_Friend(object sender, RoutedEventArgs e)
        {
            Haptic(); // Vibrate
            Add((String)((Button)sender).Tag);
        }
        #endregion
    }
}