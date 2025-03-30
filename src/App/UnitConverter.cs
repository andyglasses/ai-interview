using AI.Interview.Core.Converters;
using SolentimHardwareAccessLayer.Enums;
using System.Globalization;
using System.Windows.Data;

namespace AI.Interview.App;

/// <summary>
/// To support binding to the parameter, we need to use a MultiValueConverter
/// Value 1 is the value we wish to convert
/// Value 2 is the unit we wish to convert to
/// null will be displayed as an empty string
/// </summary>
public class UnitConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values.Length != 2)
    {
      throw new Exception("Expected two values");
    }
    if(values[0] == null)
    {
      return string.Empty;
    }
    if (values[0] is not double counts)
    {
      throw new Exception("Value is not a double");
    }
    double result = 0;
    if (values[1] is ePositionUnits pUnits)
    {
      result = Converter.FromPositionalCounts(counts, pUnits);
    }
    else if (values[1] is eVelocityUnits vUnits)
    {
      result = Converter.FromVelocityCounts(counts, vUnits);
    }
    else
    {
      throw new Exception("Parameter is not a valid unit");
    }
    return result.ToString("F3", culture);
  }

  public object[] ConvertBack(object values, Type[] targetType, object parameter, CultureInfo culture)
  {
    // This is a one-way converter
    throw new NotImplementedException();
  }
}
