﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AI.Interview.App.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
  {
    if (EqualityComparer<T>.Default.Equals(storage, value))
      return false;
    storage = value;
    this.OnPropertyChanged(propertyName);
    return true;
  }
}
