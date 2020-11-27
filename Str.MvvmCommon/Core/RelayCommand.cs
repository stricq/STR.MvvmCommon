using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;

using Str.Common.Extensions;
using Str.MvvmCommon.Contracts;


namespace Str.MvvmCommon.Core {

  [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is a library.")]
  public class RelayCommandAsync<T> : ICommandAsync<T> {

    #region Private Fields

    private readonly Func<T, Task> executeAsync;

    private readonly Predicate<T>? canExecute;

    #endregion Private Fields

    #region Constructor

    public RelayCommandAsync(Func<T, Task> ExecuteAsync, Predicate<T>? CanExecute = null) {
      executeAsync = ExecuteAsync ?? throw new ArgumentNullException(nameof(ExecuteAsync));
      canExecute   = CanExecute;
    }

    #endregion Constructor

    #region ICommandAsync<T> Implementation

    public bool CanExecute(T Parameter) {
      return canExecute == null || canExecute(Parameter);
    }

    public event EventHandler? CanExecuteChanged {
      add    => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public Task ExecuteAsync(T Parameter) {
      return executeAsync(Parameter);
    }

    #endregion ICommandAsync<T> Implementation

    #region ICommand Implementation

    bool ICommand.CanExecute(object? parameter) {
      if (parameter is T casted) return CanExecute(casted);

      return false;
    }

    void ICommand.Execute(object? parameter) {
      if (parameter is T casted) ExecuteAsync(casted).FireAndWait();
    }

    #endregion ICommand Implementation

  }


  [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "This is a library.")]
  public class RelayCommandAsync : ICommandAsync {

    #region Private Fields

    private readonly Func<Task> executeAsync;

    private readonly Func<bool>? canExecute;

    #endregion Private Fields

    #region Constructor

    public RelayCommandAsync(Func<Task> ExecuteAsync, Func<bool>? CanExecute = null) {
      executeAsync = ExecuteAsync ?? throw new ArgumentNullException(nameof(ExecuteAsync));
      canExecute   = CanExecute;
    }

    #endregion Constructor

    #region ICommandAsync Implementation

    public bool CanExecute() {
      return canExecute == null || canExecute();
    }

    public event EventHandler? CanExecuteChanged {
      add    => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public Task ExecuteAsync() {
      return executeAsync();
    }

    #endregion ICommandAsync Implementation

    #region ICommand Implementation

    bool ICommand.CanExecute(object? parameter) {
      return CanExecute();
    }

    void ICommand.Execute(object? parameter) {
      ExecuteAsync().FireAndWait();
    }

    #endregion ICommand Implementation

  }

}
