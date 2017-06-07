using System.IO.IsolatedStorage;
using System.Windows.Media;

namespace Logic
{
    static class AppHelper
    {
        private static int _pageIndex;
        private static SolidColorBrush WhiteBrush, BlackBrush;


        static AppHelper()
        {
            Navigation = 0;
            ToRateCount = 0;
            Storage = IsolatedStorageSettings.ApplicationSettings;

            WhiteBrush = new SolidColorBrush(Color.FromArgb(255, 241, 240, 221));
            BlackBrush = new SolidColorBrush(Color.FromArgb(255, 25, 25, 25));
        }

        public static IsolatedStorageSettings Storage { get; set; }
        /// <summary>
        /// Текущая страница
        /// </summary>
        public static int Navigation { get; set; }
        /// <summary>
        /// Количество ответов для оценки
        /// </summary>
        public static sbyte ToRateCount { get; set; }

        public static bool Font { get; set; }

        public static bool Theme { get; set; }




        public static SolidColorBrush TextTheme 
        {
            get
            {
                if (Theme) return WhiteBrush;
                else return BlackBrush;
            }
        }
        public static SolidColorBrush ElementsTheme 
        {
            get
            {
                if (Theme) return BlackBrush;
                else return WhiteBrush;
            }
        }



        public static double TextSize
        {
            get
            {
                if (Font) return 24d;
                else return 28d;
            }
        }



        // Page index
        public static int PageIndex
        {
            set { if (value <= App.ViewModel.Items.Count - 1 && value > -1) _pageIndex = value; }
            get { return _pageIndex; }
        }
    }
}