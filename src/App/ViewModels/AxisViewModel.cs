using AI.Interview.Core.Models;
using AI.Interview.Core.Services;
using SolentimHardwareAccessLayer.Enums;

namespace AI.Interview.App.ViewModels;
public class AxisViewModel : BaseViewModel
{
  private readonly AxisControllerWrapper axisController;
  private AxisCurrentState? state = new AxisCurrentState(false, null, null, null);
  private bool showCurrentState = false;

  public AxisViewModel(AxisControllerWrapper axisController, string name)
  {
    Capabilities = axisController.GetCapabilities();
    this.axisController = axisController;
    Name = name;
  }
  public AxisCapabilities Capabilities { get; }
  public bool UnknownPositionUnits => Capabilities.SourcePositionUnits == ePositionUnits.NotSet;
  public bool UnknownVelocityUnits => Capabilities.SourceVelocityUnits == eVelocityUnits.NotSet;
  public string Name { get; }
  public AxisCurrentState? State { get => state; private set => SetProperty(ref state, value); }
  public bool ShowCurrentState { get => showCurrentState; private set => SetProperty(ref showCurrentState, value); }



  public void UpdateState()
  {
    State = axisController.GetCurrentState();
    ShowCurrentState = State?.HasError ?? false || state?.PositionInCounts == null || state?.VelocityInCounts == null;
  }

  public void ClearState()
  {
    State = new AxisCurrentState(false, null, null, null);
  }

}

