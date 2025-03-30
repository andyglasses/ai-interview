using SolentimHardwareAccessLayer.Interface;

namespace AI.Interview.Core.Services;

public class HardwareService(IHardwareProvider hardwareProvider)
{
  public IEnumerable<AxisControllerWrapper> AxisControllers
  {
    get
    {
      return hardwareProvider.Controllers.Select(ac => new AxisControllerWrapper(ac));
    }
  }
}