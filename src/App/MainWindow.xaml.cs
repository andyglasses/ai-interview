using AI.Interview.App.ViewModels;
using System.Windows;

namespace AI.Interview.App;

public partial class MainWindow : Window
{
  public MainWindow()
  {
    var vm = new MainViewModel();
    DataContext = vm;

    InitializeComponent();
  }
}