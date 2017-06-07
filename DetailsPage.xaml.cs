using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;

namespace Logic
{
    public partial class DetailsPage
    {
        private MessageBoxResult _messageResult;
        private readonly BitmapImage _bitmapImage;
        private int _lastIndexItem;
        private BitmapImage _image;
        private WriteableBitmap _writeableBitmap;
        private readonly TextBlock _textBlockQuestion, _textBlockAnswer, _textBlockTitle, _textBlockPrompt;
        private readonly TranslateTransform _translateTransformTbQuestion, _translateTransformTbAnswer, _translateTransformTbTitle, _translatePromtTb;
        private readonly StreamResourceInfo _streamResourceInfo;
 

        public DetailsPage()
        {
            InitializeComponent();
            _bitmapImage = new BitmapImage();
            _streamResourceInfo = Application.GetResourceStream(new Uri("BackGround.jpg", UriKind.Relative));
            _textBlockTitle = new TextBlock
            {
                FontSize = 40,
                Width = 770,
                Height = 60,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
            };
            _textBlockQuestion = new TextBlock
            {
                FontSize = 22,
                Width = 890,
                Height = 300,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
            };
            _textBlockAnswer = new TextBlock
            {
                FontSize = 22,
                Width = 890,
                Height = 100,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                
            };
            _textBlockPrompt = new TextBlock
            {
                FontSize = 22,
                Width = 890,
                Height = 300,
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
            };
            


            _translateTransformTbTitle = new TranslateTransform
            {
                X = 65,
                Y = 25,
            };
            _translateTransformTbQuestion = new TranslateTransform
            {
                X = 5,
                Y = 75,
            };
            _translateTransformTbAnswer = new TranslateTransform
            {
                X = 5,
                Y = 0,
            };
            _translatePromtTb = new TranslateTransform
            {
                X = 5,
                Y = 0,
            };
        }
        private void DataLoad()
        {
            _bitmapImage.UriSource = new Uri(App.ViewModel.Items[AppHelper.PageIndex].ImagePatch, UriKind.Relative);
            TBTileOne.Text = TBTileTwo.Text = TBTileThree.Text = App.ViewModel.Items[AppHelper.PageIndex].LineOne;
            IBOne.Source = IBTwo.Source = IBThree.Source = _bitmapImage;
            DataContext = App.ViewModel.Items[AppHelper.PageIndex];
        }
        private void CheckingOpen()
        {
            if (AppHelper.Storage.Contains(AppHelper.PageIndex.ToString())) RecIsOpenOne.Visibility = RecIsOpenTwo.Visibility = RecIsOpenThree.Visibility = Visibility.Visible;
            else RecIsOpenOne.Visibility = RecIsOpenTwo.Visibility = RecIsOpenThree.Visibility = Visibility.Collapsed;
        }
        private void RateLogic()
        {
            // Демо версия
            if (App.isTrial) AppHelper.ToRateCount = 0;
            else
            {
                if (AppHelper.ToRateCount > 5 && (bool)AppHelper.Storage["Rate"] == false)
                {
                    _messageResult = MessageBox.Show("Помогите мне сделать программу лучше! Оставьте отзыв и оцените приложение!", "Понравилось?", MessageBoxButton.OKCancel);

                    if (_messageResult == MessageBoxResult.OK)
                    {
                        MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
                        marketplaceReviewTask.Show();

                        AppHelper.Storage["Rate"] = true;
                        AppHelper.ToRateCount = 0;
                    }
                    else AppHelper.ToRateCount = -20;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AnswerPanel.Visibility = QuestionPanel.Visibility = Visibility.Collapsed;
            MainPivot.IsLocked = false;
            ApplicationBar.IsVisible = true;

            // Фоновое изображение
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

            if (DataContext == null)
            {
                string selectedIndex;

                if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
                {
                    AppHelper.PageIndex = int.Parse(selectedIndex);
                    this.DataLoad();
                    this.RateLogic();
                }
            }

            AppHelper.Storage["Navigation"] = AppHelper.PageIndex;

            if ((bool) AppHelper.Storage["Panel"]) ApplicationBar.Mode = ApplicationBarMode.Minimized;
            else ApplicationBar.Mode = ApplicationBarMode.Default;

            if (App.isTrial) AdOne.Visibility = AdTwo.Visibility = AdThree.Visibility = System.Windows.Visibility.Visible;
            else AdOne.Visibility = AdTwo.Visibility = AdThree.Visibility = System.Windows.Visibility.Collapsed;
            

            ApplicationBar.BackgroundColor = AppHelper.ElementsTheme.Color;
            ApplicationBar.ForegroundColor = AppHelper.TextTheme.Color;

            this.CheckingOpen();
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (!App.isTrial) AppHelper.Storage["LastItem"] = AppHelper.PageIndex;
            AppHelper.Storage.Save();
        }
            
        

        // Ответ
        private void AppAnswer_Click(object sender, EventArgs e)
        {
            ApplicationBar.IsVisible = false;
            MainPivot.IsLocked = true;
            AnswerPanel.Visibility = Visibility.Visible;
            TBAnswer.Text = App.ViewModel.Items[AppHelper.PageIndex].LineFour;

            if (!App.isTrial) if (!AppHelper.Storage.Contains(AppHelper.PageIndex.ToString())) AppHelper.Storage.Add(AppHelper.PageIndex.ToString(), AppHelper.PageIndex); 
            if (!(bool)AppHelper.Storage["Rate"]) AppHelper.ToRateCount++;
        }
        // Подсказка
        private void AppQuestion_Click(object sender, EventArgs e)
        {
            ApplicationBar.IsVisible = false;
            MainPivot.IsLocked = true;
            QuestionPanel.Visibility = Visibility.Visible;
            TBQuestion.Text = App.ViewModel.Items[AppHelper.PageIndex].LineFive;
        }

        private void MainPivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            Thread.Sleep(85);

            if (MainPivot.SelectedIndex == 0 && _lastIndexItem == 1) AppHelper.PageIndex--;
            else if (MainPivot.SelectedIndex == 0 && _lastIndexItem == 2) AppHelper.PageIndex++;
            else if (MainPivot.SelectedIndex == 1 && _lastIndexItem == 0) AppHelper.PageIndex++;
            else if (MainPivot.SelectedIndex == 1 && _lastIndexItem == 2) AppHelper.PageIndex--;
            else if (MainPivot.SelectedIndex == 2 && _lastIndexItem == 0) AppHelper.PageIndex--;
            else if (MainPivot.SelectedIndex == 2 && _lastIndexItem == 1) AppHelper.PageIndex++;

            _lastIndexItem = MainPivot.SelectedIndex;

            SVOne.ScrollToVerticalOffset(0.0d);
            SVTwo.ScrollToVerticalOffset(0.0d);
            SVThree.ScrollToVerticalOffset(0.0d);

            this.DataLoad();
            this.RateLogic();
            this.CheckingOpen();
        }

        private void AppShow_Click(object sender, EventArgs e)
        {
            // Демо версия
            if (App.isTrial) MessageBox.Show("Данная функция доступна в полной версии приложения.", "Поделиться головоломкой с другом", MessageBoxButton.OK);
            else
            {
                ShareStatusTask sharedStatusTask = new ShareStatusTask() { Status = "[#Головоломки]\n" + App.ViewModel.Items[AppHelper.PageIndex].LineThree };
                sharedStatusTask.Show();
            }
        }

        // Продолжить
        private void TBOk_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (AppHelper.Theme)
            {
                RecOk.Fill = new SolidColorBrush(Color.FromArgb(255, 241, 240, 221));
                TBOk.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }
            else
            {
                RecOk.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                TBOk.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 240, 221));
            } 
        }
        private void TBOk_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            RecOk.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            TBOk.Foreground = AppHelper.Theme ? new SolidColorBrush(Color.FromArgb(255, 241, 240, 221)) : new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
           
            MainPivot.IsLocked = false;
            ApplicationBar.IsVisible = true;
            AnswerPanel.Visibility = Visibility.Collapsed;

            this.CheckingOpen();
        }

        // Сохранение головоломки в виде изображения
        private void SaveJpeg(object arg)
        {
            using (var mediaLibrary = new MediaLibrary())
            {
                using (var stream = new MemoryStream())
                {
                    _writeableBitmap.SaveJpeg(stream, _writeableBitmap.PixelWidth, _writeableBitmap.PixelHeight, 0, 95);
                    stream.Seek(0, SeekOrigin.Begin);
                    mediaLibrary.SavePicture("Головоломка_" + App.ViewModel.Items[AppHelper.PageIndex].ID + ".jpg", stream);
                }
            }
        }
        private void AppSave_Click(object sender, EventArgs e)
        {
            // Демо версия
            if (App.isTrial) MessageBox.Show("Данная функция доступна в полной версии приложения.", "Сохранить головоломку в виде изображения", MessageBoxButton.OK);
            else
            {
                _image = new BitmapImage();
                _image.SetSource(_streamResourceInfo.Stream);

                _writeableBitmap = new WriteableBitmap(_image);

                _textBlockTitle.Text = App.ViewModel.Items[AppHelper.PageIndex].LineOne;
                _textBlockQuestion.Text = App.ViewModel.Items[AppHelper.PageIndex].LineThree;
                _textBlockAnswer.Text = "ОТВЕТ:\n" + App.ViewModel.Items[AppHelper.PageIndex].LineFour;
                _textBlockPrompt.Text = "ПОДСКАЗКА:\n" + App.ViewModel.Items[AppHelper.PageIndex].LineFive;

                _writeableBitmap.Render(_textBlockTitle, _translateTransformTbTitle);
                _writeableBitmap.Render(_textBlockQuestion, _translateTransformTbQuestion);
                _translateTransformTbAnswer.Y = 640 - _textBlockAnswer.ActualHeight;
                _writeableBitmap.Render(_textBlockAnswer, _translateTransformTbAnswer);
                _translatePromtTb.Y = _translateTransformTbQuestion.Y + _textBlockQuestion.ActualHeight + 30;
                _writeableBitmap.Render(_textBlockPrompt, _translatePromtTb);
                _writeableBitmap.Invalidate();

                ThreadPool.QueueUserWorkItem(this.SaveJpeg);

                MessageBox.Show("Головоломка сохранена в библиотеку фотографий.", "Выполнено", MessageBoxButton.OK);
            }
        }



        private void TBQuesOk_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            if (AppHelper.Theme)
            {
                RecQuesOk.Fill = new SolidColorBrush(Color.FromArgb(255, 241, 240, 221));
                TBQuesOk.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }
            else
            {
                RecQuesOk.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                TBQuesOk.Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 240, 221));
            } 
        }

        private void TBQuesOk_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            RecQuesOk.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            TBQuesOk.Foreground = AppHelper.Theme ? new SolidColorBrush(Color.FromArgb(255, 241, 240, 221)) : new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

            MainPivot.IsLocked = false;
            ApplicationBar.IsVisible = true;
            QuestionPanel.Visibility = Visibility.Collapsed;
        }
    }
}
