using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Str.Common.Extensions;
using Str.MvvmCommon.Contracts;


namespace Str.MvvmCommon.Core {

  public class RelayCommand<T> : ICommand<T> {

    #region Private Fields

    private readonly Action<T>    execute;
    private readonly Predicate<T> canExecute;

    #endregion Private Fields

    #region Constructor

    public RelayCommand(Action<T> Execute, Predicate<T> CanExecute = null) {
      execute    = Execute ?? throw new ArgumentNullException(nameof(Execute));
      canExecute = CanExecute;
    }

    #endregion Constructor

    #region ICommand<T> Implementation

    public bool CanExecute(T Parameter) {
      return canExecute == null || canExecute(Parameter);
    }

    public event EventHandler CanExecuteChanged {
      add    => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public void Execute(T Parameter) {
      execute(Parameter);
    }

    #endregion ICommand<T> Implementation

    #region ICommand Implementation

    bool ICommand.CanExecute(object parameter) {
      return CanExecute((T)parameter);
    }

    void ICommand.Execute(object parameter) {
      Execute((T)parameter);
    }

    #endregion ICommand Implementation

  }


  public class RelayCommandAsync<T> : ICommandAsync<T> {

    #region Private Fields

    private readonly Func<T, Task>   executeAsync;
    private readonly Predicate<T> canExecute;

    #endregion Private Fields

    #region Constructor

    public RelayCommandAsync(Func<T, Task> ExecuteAsync, Predicate<T> CanExecute = null) {
      executeAsync = ExecuteAsync ?? throw new ArgumentNullException(nameof(ExecuteAsync));
      canExecute   = CanExecute;
    }

    #endregion Constructor

    #region ICommandAsync<T> Implementation

    public bool CanExecute(T Parameter) {
      return canExecute == null || canExecute(Parameter);
    }

    public event EventHandler CanExecuteChanged {
      add    => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public Task ExecuteAsync(T Parameter) {
      return executeAsync(Parameter);
    }

    #endregion ICommandAsync<T> Implementation

    #region ICommand Implementation

    bool ICommand.CanExecute(object parameter) {
      return CanExecute((T)parameter);
    }

    void ICommand.Execute(object parameter) {
      ExecuteAsync((T)parameter).FireAndWait();
    }

    #endregion ICommand Implementation

  }


  public class RelayCommand : ICommandSlim {

    #region Private Fields

    private readonly Action     execute;
    private readonly Func<bool> canExecute;

    #endregion Private Fields

    #region Constructor

    public RelayCommand(Action Execute, Func<bool> CanExecute = null) {
      execute    = Execute ?? throw new ArgumentNullException(nameof(Execute));
      canExecute = CanExecute;
    }

    #endregion Constructor

    #region ICommandSlim Implementation

    public bool CanExecute() {
      return canExecute == null || canExecute();
    }

    public event EventHandler CanExecuteChanged {
      add    => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public void Execute() {
      execute();
    }

    #endregion ICommandSlim Implementation

    #region ICommand Implementation

    bool ICommand.CanExecute(object parameter) {
      return CanExecute();
    }

    void ICommand.Execute(object parameter) {
      Execute();
    }

    #endregion ICommand Implementation

  }


  public class RelayCommandAsync : ICommandAsync {

    #region Private Fields

    private readonly Func<Task>    executeAsync;
    private readonly Func<bool> canExecute;

    #endregion Private Fields

    #region Constructor

    public RelayCommandAsync(Func<Task> ExecuteAsync, Func<bool> CanExecute = null) {
      executeAsync = ExecuteAsync ?? throw new ArgumentNullException(nameof(ExecuteAsync));
      canExecute   = CanExecute;
    }

    #endregion Constructor

    #region ICommandAsync Implementation

    public bool CanExecute() {
      return canExecute == null || canExecute();
    }

    public event EventHandler CanExecuteChanged {
      add    => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public Task ExecuteAsync() {
      return executeAsync();
    }

    #endregion ICommandAsync Implementation

    #region ICommand Implementation

    bool ICommand.CanExecute(object parameter) {
      return CanExecute();
    }

    void ICommand.Execute(object parameter) {
      ExecuteAsync().FireAndWait();
    }

    #endregion ICommand Implementation

  }

}
