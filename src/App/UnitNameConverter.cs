using SolentimHardwareAccessLayer.Enums;
using System.Globalization;
using System.Windows.Data;

namespace AI.Interview.App;

public class UnitNameConverter : IValueConverter
{
  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    if(value == null)
    {
      return string.Empty;
    }
    if (value is ePositionUnits ePositionUnits)
    {
      switch(ePositionUnits)
      {
        case ePositionUnits.Counts:
          return "Counts";
        case ePositionUnits.Millimeters:
          return "Millimeters";
        case ePositionUnits.Inches:
          return "Inches";
        default:
          return "Unknown";
      }
    }
    
    if (value is eVelocityUnits eVelocityUnits)
    {
      switch (eVelocityUnits)
      {
        case eVelocityUnits.CountsPerSecond:
          return "Counts per second";
        case eVelocityUnits.CentimetersPerSecond:
          return "Centimeters per second";
        case eVelocityUnits.InchesPerSecond:
          return "Inches per second";
        default:
          return "Unknown";
      }
    }

    throw new Exception("Value is not a valid unit");
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
