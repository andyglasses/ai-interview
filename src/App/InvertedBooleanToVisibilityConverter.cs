using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AI.Interview.App;

public class InvertedBooleanToVisibilityConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value is bool booleanValue)
    {
      return booleanValue ? Visibility.Collapsed : Visibility.Visible;
    }
    return Visibility.Visible;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if (value is Visibility visibility)
    {
      return visibility == Visibility.Collapsed;
    }
    return false;
  }
}
