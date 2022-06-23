using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Controls.Primitives;

namespace RedditGalleryAvalonia;

public class BooleanToScrollBarVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return boolValue ? ScrollBarVisibility.Visible : ScrollBarVisibility.Disabled;
        return ScrollBarVisibility.Disabled;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}