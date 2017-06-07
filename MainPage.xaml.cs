using Logic.ViewModels;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Linq;
using System.Diagnostics;
using Microsoft.Phone.Marketplace;

namespace Logic
{
    public partial class MainPage
    {
        

        public MainPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
           
            if (!AppHelper.Storage.Contains("Navigation")) AppHelper.Storage.Add("Navigation", 0);
            if (!AppHelper.Storage.Contains("Font")) AppHelper.Storage.Add("Font", true);
            if (!AppHelper.Storage.Contains("Rate")) AppHelper.Storage.Add("Rate", false);
            if (!AppHelper.Storage.Contains("FullVersion")) AppHelper.Storage.Add("FullVersion", false);
            if (!AppHelper.Storage.Contains("Theme")) AppHelper.Storage.Add("Theme", true);
            if (!AppHelper.Storage.Contains("Panel")) AppHelper.Storage.Add("Panel", true);
            if (!AppHelper.Storage.Contains("BacFone")) AppHelper.Storage.Add("BacFone", false);
            if (!AppHelper.Storage.Contains("LastItem")) AppHelper.Storage.Add("LastItem", 0);
            if (!AppHelper.Storage.Contains("Starts")) AppHelper.Storage.Add("Starts", 0);
            else AppHelper.Storage["Starts"] = 1 + (int)AppHelper.Storage["Starts"];

            if (App.isTrial)
            {
                AppHelper.Storage["Navigation"] = 0;
                AppHelper.Storage["Font"] = true;
                AppHelper.Storage["Rate"] = false;
                AppHelper.Storage["FullVersion"] = false;
                AppHelper.Storage["Theme"] = true;
                AppHelper.Storage["Panel"] = true;
                AppHelper.Storage["BacFone"] = false;
                AppHelper.Storage["LastItem"] = 0;
                AppHelper.Storage["Starts"] = 0;
            }

            AppHelper.Font = (bool)AppHelper.Storage["Font"];
            AppHelper.Theme = (bool)AppHelper.Storage["Theme"];

            AppHelper.Storage.Save();          
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {


            MainPivot.IsLocked = false;
            MainPivot.SelectedIndex = 1;

            if (!App.ViewModel.IsDataLoaded) App.ViewModel.LoadData();




            // Помечаем прочитанные
            for (var i = 0; i < App.ViewModel.Items.Count; i++) if (AppHelper.Storage.Contains(i.ToString())) App.ViewModel.Items[i].Open = "ОТКРЫТО";

            ApplicationBar.BackgroundColor = AppHelper.ElementsTheme.Color;
            ApplicationBar.ForegroundColor = AppHelper.TextTheme.Color;


            SwitchTheme.IsChecked = !(bool)AppHelper.Storage["Theme"];
            SwitchTheme.Content = SwitchTheme.IsChecked.Value ? "Светлое" : "Темное";

            SwitchSize.IsChecked = !(bool)AppHelper.Storage["Font"];
            SwitchSize.Content = SwitchSize.IsChecked.Value ? "Большой" : "Маленький";

            SwitchBar.IsChecked = !(bool)AppHelper.Storage["Panel"];
            SwitchBar.Content = SwitchBar.IsChecked.Value ? "Большая" : "Маленькая";

            SwitchFone.IsChecked = (bool)AppHelper.Storage["BacFone"];
            SwitchFone.Content = SwitchFone.IsChecked.Value ? "Включено" : "Выключено";


            // Последняя прочитанная головоломка
            for (var i = 0; i < App.ViewModel.Items.Count; i++) App.ViewModel.Items[i].Visibility = System.Windows.Visibility.Collapsed;
            App.ViewModel.Items[(int)AppHelper.Storage["LastItem"]].Visibility = System.Windows.Visibility.Visible;

            // Фон
            this.FoneLogic();
        }
        

        

        // Обработка выбранных элементов, измененных в LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Если выбранный элемент равен NULL (ничего не выбрано), никакие действия не требуются
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Переход на новую страницу
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as ItemViewModel).ID, UriKind.Relative));

            // Сброс выбранного элемента в NULL (ничего не выбрано)
            MainLongListSelector.SelectedItem = null;

            
        }
        // Обратная связь
        private void AppEmail_Click(object sender, EventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask { Subject = "[#Головоломки]", To = "AMShishkin.vrn@gmail.com" };
            emailComposeTask.Show();
        }
        // Оценить
        private void AppRate_Click(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();

            if ((bool)AppHelper.Storage["Rate"] == false)
            {
                AppHelper.Storage["Rate"] = true;
                AppHelper.ToRateCount = 0;
                AppHelper.Storage.Save();
            }
        }
        // Мои приложения
        private void AppMyApps_Click(object sender, EventArgs e)
        {
            MarketplaceSearchTask marketplaceSearchTaskAll = new MarketplaceSearchTask { SearchTerms = "AMShishkin" };
            marketplaceSearchTaskAll.Show();
        }
        // Закладка
        private void AppMarker_Click(object sender, EventArgs e)
        {
            // Демо версия
            if(App.isTrial) MessageBox.Show("Данная функция доступна в полной версии приложения.", "Быстрый переход к закладке",MessageBoxButton.OK);
            else MainLongListSelector.ScrollTo(MainLongListSelector.ItemsSource[(int)AppHelper.Storage["LastItem"]]);
        }

      

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            // Демо версия
            if (App.isTrial) MessageBox.Show("Данная функция доступна в полной версии приложения.", "Список изменений в последнем обновлении", MessageBoxButton.OK);
            else MessageBox.Show("+Новые головоломки\n+Правки дизайна\n+Новые иконки приложения на рабочем столе", "Обновление v1.4.0", MessageBoxButton.OK);
        }

        // Переключение фонового изображения
        private void FoneLogic()
        {
            if ((bool)AppHelper.Storage["BacFone"])
            {
                if (AppHelper.Theme)
                {
                    FonWhite.Visibility = Visibility.Visible;
                    FonBlack.Visibility = Visibility.Collapsed;
                }
                else
                {
                    FonBlack.Visibility = Visibility.Visible;
                    FonWhite.Visibility = Visibility.Collapsed;
                }
            }
            else FonBlack.Visibility = FonWhite.Visibility = Visibility.Collapsed;
        }



        // Оформление
        private void SwitchTheme_Checked(object sender, RoutedEventArgs e)
        {
            // Демо версия
            if (!App.isTrial)
            {
                if (AppHelper.Theme)
                {
                    MessageBox.Show("Новые настройки вступят в силу после перезагрузки приложения", "Требуется перезагрузка приложения", MessageBoxButton.OK);
                    SwitchTheme.Foreground = new SolidColorBrush(Colors.Red);
                }
                else SwitchTheme.Foreground = AppHelper.TextTheme;

                SwitchTheme.Content = "Светлое";

                AppHelper.Storage["Theme"] = false;
                AppHelper.Storage.Save();
            }
        }
        private void SwitchTheme_Unchecked(object sender, RoutedEventArgs e)
        {
            // Демо версия
            if (!App.isTrial)
            {
                if (!AppHelper.Theme)
                {
                    MessageBox.Show("Новые настройки вступят в силу после перезагрузки приложения", "Требуется перезагрузка приложения", MessageBoxButton.OK);
                    SwitchTheme.Foreground = new SolidColorBrush(Colors.Red);
                }
                else SwitchTheme.Foreground = AppHelper.TextTheme;

                SwitchTheme.Content = "Темное";

                AppHelper.Storage["Theme"] = true;
                AppHelper.Storage.Save();
            }
        }

        // Размер текста
        private void SwitchSize_Checked(object sender, RoutedEventArgs e)
        {
            // Демо версия
            if (!App.isTrial)
            {
                if (AppHelper.Font)
                {
                    MessageBox.Show("Новые настройки вступят в силу после перезагрузки приложения", "Требуется перезагрузка приложения", MessageBoxButton.OK);
                    SwitchSize.Foreground = new SolidColorBrush(Colors.Red);
                }
                else SwitchSize.Foreground = AppHelper.TextTheme;

                SwitchSize.Content = "Большой";

                AppHelper.Storage["Font"] = false;
                AppHelper.Storage.Save();
            }
        }
        private void SwitchSize_Unchecked(object sender, RoutedEventArgs e)
        {
            // Демо версия
            if (!App.isTrial)
            {
                if (!AppHelper.Font)
                {
                    MessageBox.Show("Новые настройки вступят в силу после перезагрузки приложения", "Требуется перезагрузка приложения", MessageBoxButton.OK);
                    SwitchSize.Foreground = new SolidColorBrush(Colors.Red);
                }
                else SwitchSize.Foreground = AppHelper.TextTheme;

                SwitchSize.Content = "Маленький";

                AppHelper.Storage["Font"] = true;
                AppHelper.Storage.Save();
            }
        }

        // Размер панели
        private void SwitchBar_Checked(object sender, RoutedEventArgs e)
        {
            // Демо версия
            if (!App.isTrial)
            {
                SwitchBar.Content = "Большая";

                ApplicationBar.Mode = ApplicationBarMode.Default;

                AppHelper.Storage["Panel"] = false;
                AppHelper.Storage.Save();
            }
        }
        private void SwitchBar_Unchecked(object sender, RoutedEventArgs e)
        {
            // Демо версия
            if (!App.isTrial)
            {
                SwitchBar.Content = "Маленькая";

                ApplicationBar.Mode = ApplicationBarMode.Minimized;

                AppHelper.Storage["Panel"] = true;
                AppHelper.Storage.Save();
            }
        }

        // Фоновое изображение
        private void SwitchFone_Checked(object sender, RoutedEventArgs e)
        {
            // Демо версия
            if (!App.isTrial)
            {
                SwitchFone.Content = "Включено";

                AppHelper.Storage["BacFone"] = true;
                AppHelper.Storage.Save();

                this.FoneLogic();
            }
        }
        private void SwitchFone_Unchecked(object sender, RoutedEventArgs e)
        {
            // Демо версия
            if (!App.isTrial)
            {
                SwitchFone.Content = "Выключено";

                AppHelper.Storage["BacFone"] = false;
                AppHelper.Storage.Save();

                this.FoneLogic();
            }
        }

        
    }
}