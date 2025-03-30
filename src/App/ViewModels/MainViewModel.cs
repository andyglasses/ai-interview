using AI.Interview.App.Models;
using AI.Interview.Core.Services;
using SolentimHardwareAccessLayer;
using SolentimHardwareAccessLayer.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace AI.Interview.App.ViewModels;

public class MainViewModel : BaseViewModel
{
  private HardwareService? HardwareService;
  private BackgroundWorker bootWorker;
  private string title = "AI Interview Task";
  private bool hasError = false;
  private bool canBoot = true;
  private bool canRestart = true;
  private bool isRunning = false;
  private string errorMessage = string.Empty;
  private ePositionUnits positionUnits = ePositionUnits.Counts;
  private eVelocityUnits velocityUnits = eVelocityUnits.CountsPerSecond;
  private bool autoRestartOnEmergencyStop = true;
  private DispatcherTimer? timer;

  public string Title { get => title; private set => SetProperty(ref title, value); }
  public bool HasError { get => hasError; private set => SetProperty(ref hasError, value); }
  public string ErrorMessage { get => errorMessage; private set => SetProperty(ref errorMessage, value); }
  public bool CanBoot { get => canBoot; private set => SetProperty(ref canBoot, value); }
  public bool CanRestart { get => canRestart; private set => SetProperty(ref canRestart, value); }
  public bool IsRunning { get => isRunning; private set => SetProperty(ref isRunning, value); }

  public ePositionUnits PositionUnits { get => positionUnits; set => SetProperty(ref positionUnits, value); }
  public eVelocityUnits VelocityUnits { get => velocityUnits; set => SetProperty(ref velocityUnits, value); }
  public bool AutoRestartOnEmergencyStop { get => autoRestartOnEmergencyStop; set => SetProperty(ref autoRestartOnEmergencyStop, value); }

  public ObservableCollection<AxisViewModel> AxisViewModels { get; } = new();
  public List<KeyValuePair<string, ePositionUnits>> PositionOptions { get; } = new([
      new KeyValuePair<string, ePositionUnits>("Counts", ePositionUnits.Counts),
      new KeyValuePair<string, ePositionUnits>("Inches", ePositionUnits.Inches),
      new KeyValuePair<string, ePositionUnits>("Millimeters", ePositionUnits.Millimeters)
    ]);
  public List<KeyValuePair<string, eVelocityUnits>> VelocityOptions { get; } = new([
      new KeyValuePair<string, eVelocityUnits>("Counts per second", eVelocityUnits.CountsPerSecond),
      new KeyValuePair<string, eVelocityUnits>("Inches per second", eVelocityUnits.InchesPerSecond),
      new KeyValuePair<string, eVelocityUnits>("Centimeters per second", eVelocityUnits.CentimetersPerSecond)
    ]);

  public ICommand BootCommand { get; }
  public ICommand RestartCommand { get; }

  public MainViewModel()
  {
    BootCommand = new RelayCommand(BootWorker_Start, () => CanBoot);
    RestartCommand = new RelayCommand(Restart, () => CanRestart);
    bootWorker = new BackgroundWorker();
    bootWorker.DoWork += BootWorker_DoWork;
    bootWorker.RunWorkerCompleted += BootWorker_Completed;
    timer = new DispatcherTimer();
    timer.Interval = TimeSpan.FromSeconds(1);
    timer.Tick += Timer_Tick;
    UpdateState(State.Idle);
  }

  private void UpdateState(State newState)
  {
    CanBoot = newState == State.Idle;

    IsRunning = newState == State.Running || newState == State.ErrorRun;

    CanRestart = newState == State.ErrorRun;
    HasError = newState == State.ErrorRun || newState == State.ErrorBoot;
  }

  public void Restart()
  {
    UpdateState(State.Running);
    foreach (var axisViewModel in AxisViewModels)
    {
      axisViewModel.ClearState();
    }
    timer?.Start();
  }

  #region Timer

  private void Timer_Tick(object? sender, EventArgs e)
  {
    foreach (var axisViewModel in AxisViewModels)
    {
      axisViewModel.UpdateState();
      if(axisViewModel.State?.HasError ?? false)
      {
        if (!AutoRestartOnEmergencyStop)
        {
          timer?.Stop();
          ErrorMessage = $"{axisViewModel.Name}: {axisViewModel.State?.ErrorMessage ?? "Unknown Error"}";
          UpdateState(State.ErrorRun);
          CommandManager.InvalidateRequerySuggested();
          return;
        }
      }
    }
  }

  #endregion Timer

  #region Boot Worker

  private void BootWorker_Start()
  {
    UpdateState(State.Booting);
    bootWorker.RunWorkerAsync();
  }

  private void BootWorker_DoWork(object? sender, DoWorkEventArgs args)
  {
    HardwareService = new HardwareService(HardwareProviderFactory.GetHardwareProvider());
    var axisControllers = HardwareService.AxisControllers.ToList();
    var returnVal = new List<AxisViewModel>();
    for(int i = 0; i < axisControllers.Count; i++)
    {
      var axisController = axisControllers[i];
      var axisViewModel = new AxisViewModel(axisController, $"Axis {i + 1}");
      returnVal.Add(axisViewModel);
    }
    args.Result = returnVal;
  }

  public void BootWorker_Completed(object? sender, RunWorkerCompletedEventArgs args)
  {
    if (args.Error != null)
    {
      ErrorMessage = args.Error.Message;
      UpdateState(State.ErrorBoot);
      return;
    }
    else if(args.Result == null)
    {
      errorMessage = "No Axis Found";
      UpdateState(State.ErrorBoot);
      return;
    }
    List<AxisViewModel> axisViewModels = ((List<AxisViewModel>)args.Result)??[];
    foreach (var axisViewModel in axisViewModels)
    {
      AxisViewModels.Add(axisViewModel);
    }
    
    UpdateState(State.Running);
    timer?.Start();
  }

  #endregion Boot Worker

}
