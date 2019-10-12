﻿//#define test
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using 酷安_UWP.Model;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace 酷安_UWP
{
    public sealed partial class MainPage : Page
    {
        //public static TextBlock _User_Name;
        //public static ImageBrush _User_Face;
        //public static ColumnDefinition _dcd, _lcd;

        Color CoolColor = ((SolidColorBrush)Application.Current.Resources["CoolApk_Theme"]).Color;
        Color CoolForeColor = ((SolidColorBrush)Application.Current.Resources["CoolApk_Theme_Fore"]).Color;
        Color CoolForeInactiveColor = Color.FromArgb(255, 50, 50, 50);
        Color CoolBackPressedColor = Color.FromArgb(255, 200, 200, 200);
        Color CoolBackHoverColor = Color.FromArgb(255, 255, 255, 255);
        Color CoolBackColor = Color.FromArgb(255, 230, 230, 230);
        //Color.FromArgb(255, 72, 174, 76);
        public static int seletedItem =
#if test
            5;
#else
        4;
#endif
        public MainPage()
        {
            this.InitializeComponent();

            // 判断是否存在 StatusBar
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundOpacity = 1; // 透明度
                statusBar.BackgroundColor = CoolBackColor;
                statusBar.ForegroundColor = CoolForeColor;
            }
            else
            {
                var view = ApplicationView.GetForCurrentView().TitleBar;
                // active
                view.BackgroundColor = CoolBackColor;
                view.ForegroundColor = CoolForeColor;

                // inactive
                view.InactiveBackgroundColor = CoolBackColor;
                view.InactiveForegroundColor = CoolForeInactiveColor;

                // button
                view.ButtonBackgroundColor = CoolBackColor;
                view.ButtonForegroundColor = CoolForeColor;

                view.ButtonHoverBackgroundColor = CoolBackHoverColor;
                view.ButtonHoverForegroundColor = CoolForeColor;

                view.ButtonPressedBackgroundColor = CoolBackPressedColor;
                view.ButtonPressedForegroundColor = CoolForeColor;

                view.ButtonInactiveBackgroundColor = CoolBackColor;
                view.ButtonInactiveForegroundColor = CoolForeInactiveColor;
            }
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

            //_dcd = dcd;

            hamburgerMenuControl.ItemsSource = MenuItem.GetMainItems();
            hamburgerMenuControl.OptionsItemsSource = MenuItem.GetOptionsItems();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Type pageType = typeof(MyPage);
            switch (seletedItem)
            {
                case 0:
                    pageType = typeof(SettingPage);
                    break;
                case 1:
                    pageType = typeof(AppRecommendPage);
                    break;
                case 2:
                    pageType = typeof(DeveloperPage);
                    break;
                case 3:
                    pageType = typeof(MyPage);
                    break;
                case 4:
                    pageType = typeof(IndexPage);
                    break;
                case 5:
                    pageType = typeof(TestPage);
                    break;
            }
            if (hamburgerMenuControl.SelectedIndex != seletedItem - 1)
            {
                hamburgerMenuControl.SelectedIndex = seletedItem - 1;
                hamburgerMenuControl.SelectedOptionsIndex = seletedItem - 1 < 0 ? 0 : -1;
            }
            //VFrame.Navigate(typeof(FeedPage), new object[] { "536381", this });

            VFrame.Navigate(pageType, this);
        }

        private void OnMenuItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as MenuItem;
            VFrame.Navigate(menuItem.PageType, this);
            seletedItem = menuItem.Index;
            if (hamburgerMenuControl.DisplayMode != SplitViewDisplayMode.CompactInline)
                hamburgerMenuControl.IsPaneOpen = false;
        }

        public static void User_Load()
        {
            /*
            _User_Name.Text = LoginPage.UserName;
            _User_Face.ImageSource = new BitmapImage(new Uri(LoginPage.UserFace, UriKind.RelativeOrAbsolute));

            //Save
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["name"] = LoginPage.UserName;
            localSettings.Values["face"] = LoginPage.UserFace;
            localSettings.Values["login"] = "1";
            */
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        public void ActiveProgressRing() => progressRing.IsActive = true;
        public void DeactiveProgressRing() => progressRing.IsActive = false;
    }

    public class MenuItem
    {
        public Symbol Icon { get; set; }
        public string Name { get; set; }
        public Type PageType { get; set; }
        public int Index;

        public static List<MenuItem> GetMainItems()
        {
            List<MenuItem> items = new List<MenuItem>
            {
                new MenuItem() { Icon = Symbol.View, Name = "应用●游戏", PageType = typeof(AppRecommendPage), Index = 1 },
                new MenuItem() { Icon = Symbol.ContactPresence, Name = "开发者中心", PageType = typeof(DeveloperPage), Index = 2 },
                new MenuItem() { Icon = Symbol.Contact, Name = "我", PageType = typeof(MyPage), Index = 3 },
                new MenuItem() { Icon = Symbol.Home, Name = "头条", PageType = typeof(IndexPage), Index = 4 },
                new MenuItem() { Icon = Symbol.Shuffle, Name = "test page", PageType = typeof(TestPage), Index = 5 }
            };
            return items;
        }

        public static List<MenuItem> GetOptionsItems()
        {
            List<MenuItem> items = new List<MenuItem>
            {
                new MenuItem() { Icon = Symbol.Setting, Name = "设置", PageType = typeof(SettingPage), Index = 0 }
            };
            return items;
        }
    }
}