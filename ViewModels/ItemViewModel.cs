using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Logic.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _id, _lineOne, _lineTwo, _lineThree, _lineFour, _lineFive, _imagePatch;

        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }
     
        public string LineOne
        {
            get
            {
                return _lineOne;
            }
            set
            {
                if (value != _lineOne)
                {
                    _lineOne = value;
                    NotifyPropertyChanged("LineOne");
                }
            }
        }
        public string LineTwo
        {
            get
            {
                return _lineTwo;
            }
            set
            {
                if (value != _lineTwo)
                {
                    _lineTwo = value;
                    NotifyPropertyChanged("LineTwo");
                }
            }
        }
        public string LineThree
        {
            get
            {
                return _lineThree;
            }
            set
            {
                if (value != _lineThree)
                {
                    _lineThree = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }
        public string LineFour
        {
            get
            {
                return _lineFour;
            }
            set
            {
                if (value != _lineFour)
                {
                    _lineFour = value;
                    NotifyPropertyChanged("LineFour");
                }
            }
        }
        public string LineFive
        {
            get
            {
                return _lineFive;
            }
            set
            {
                if (value != _lineFive)
                {
                    _lineFive = value;
                    NotifyPropertyChanged("LineFive");
                }
            }
        }

        public string ImagePatch
        {
            get
            {
                return _imagePatch;
            }
            set
            {
                if (value != _imagePatch)
                {
                    _imagePatch = value;
                    NotifyPropertyChanged("ImagePatch");
                }
            }
        }

        public SolidColorBrush TextColor 
        {
            get
            {
                return AppHelper.TextTheme;
            }
          
        }

        public SolidColorBrush ElementsColor
        {
            get
            {
                return AppHelper.ElementsTheme;
            }
        }

        public double TextSize
        {
            get
            {
                return AppHelper.TextSize;
            }
        }

        public string Open { get; set; }

        public Visibility Visibility { get; set; }
   
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}