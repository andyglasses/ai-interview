using SolentimHardwareAccessLayer.Enums;

namespace AI.Interview.Core.Converters;

public class Converter
{
  private const double _mmToCounts = 100.0;
  private const double _cmToCounts = 1000.0;
  private const double _inToCounts = 25.4 * 100.0;
  public static double ToPositionalCounts(double value, ePositionUnits units)
  {
    switch (units)
    {
      case ePositionUnits.Counts:
        return value;
      case ePositionUnits.Millimeters:
        return value * _mmToCounts;
      case ePositionUnits.Inches:
        return value * _inToCounts;
      default:
        return value; // if we have not set assume it is in counts, UI will display an error to the user
        //throw new NotImplementedException($"Conversion from `{units.ToString()}` is not supported");
    }
  }

  public static double FromPositionalCounts(double value, ePositionUnits units)
  {
    switch (units)
    {
      case ePositionUnits.Counts:
        return value;
      case ePositionUnits.Millimeters:
        return value / _mmToCounts;
      case ePositionUnits.Inches:
        return value / _inToCounts;
      default:
        return value; // if we have not set assume it is in counts, UI will display an error to the user
        //throw new NotImplementedException($"Conversion to `{units.ToString()}` is not supported");
    }
  }

  public static double ToVelocityCounts(double value, eVelocityUnits units)
  {
    switch (units)
    {
      case eVelocityUnits.CountsPerSecond:
        return value;
      case eVelocityUnits.CentimetersPerSecond:
        return value * _cmToCounts;
      case eVelocityUnits.InchesPerSecond:
        return value * _inToCounts;
      default:
        return value; // if we have not set assume it is in counts, UI will display an error to the user
        //throw new NotImplementedException($"Conversion from `{units.ToString()}` is not supported");
    }
  }

  public static double FromVelocityCounts(double value, eVelocityUnits units)
  {
    switch (units)
    {
      case eVelocityUnits.CountsPerSecond:
        return value;
      case eVelocityUnits.CentimetersPerSecond:
        return value / _cmToCounts;
      case eVelocityUnits.InchesPerSecond:
        return value / _inToCounts;
      default:
        return value; // if we have not set assume it is in counts, UI will display an error to the user
        //throw new NotImplementedException($"Conversion to `{units.ToString()}` is not supported");
    }
  }
}
