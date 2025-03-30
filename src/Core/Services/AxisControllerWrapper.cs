using AI.Interview.Core.Converters;
using AI.Interview.Core.Models;
using SolentimHardwareAccessLayer.Enums;
using SolentimHardwareAccessLayer.Interface;

namespace AI.Interview.Core.Services;

public class AxisControllerWrapper(IAxisController axisController)
{
  // store here so we don't need to keep calling the hardware
  private ePositionUnits? positionUnits = null;
  public ePositionUnits PositionUnits => positionUnits ??= axisController.PositionUnit;

  private eVelocityUnits? velocityUnits = null;
  public eVelocityUnits VelocityUnits => velocityUnits ??= axisController.VelocityUnit;

  public AxisCapabilities GetCapabilities()
  {
    return new AxisCapabilities(
      new MinMaxCounts(
        Converter.ToPositionalCounts(axisController.GetMinPosition(), PositionUnits),
        Converter.ToPositionalCounts(axisController.GetMaxPosition(), PositionUnits)),
      new MinMaxCounts(
        Converter.ToVelocityCounts(axisController.GetMinVelocity(), VelocityUnits),
        Converter.ToVelocityCounts(axisController.GetMaxVelocity(), VelocityUnits)),
      PositionUnits,  VelocityUnits);
  }

  public AxisCurrentState GetCurrentState()
  {
    try
    {
      return new AxisCurrentState(false, null,
        Converter.ToPositionalCounts(axisController.GetCurrentPosition(), PositionUnits),
        Converter.ToVelocityCounts(axisController.GetCurrentVelocity(), VelocityUnits));
    }
    catch (Exception ex)
    {
      // assumption/for time: I only need to capture one error message.
      return new AxisCurrentState(true, ex.Message, null, null);
    }
  }
}
