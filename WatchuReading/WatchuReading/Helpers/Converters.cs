using System;
using System.Globalization;
using Xamarin.Forms;

namespace WatchuReading.Helpers
{
    public class Converters
    {
        public Converters()
        {
        }
    }

    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool && value != null)
            {
                bool s = (bool)value;
                switch (s)
                {
                    case false:
                        return Color.Transparent;
                    case true:
                        return Constants.ActiveBookColor;
                    default:
                        return Color.Transparent;
                }

            }
            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }


    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //IsVisible Converter for toggling
    public class IsVisibleBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isFav = (bool)value;
            return value = !isFav;
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



}

