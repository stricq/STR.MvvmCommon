using System;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Str.MvvmCommon.Core {

  public class RelayCommand<T> : ICommand {

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

    #region ICommand Implementation

    public bool CanExecute(object Parameter) {
      return canExecute == null || canExecute((T)Parameter);
    }

    public event EventHandler CanExecuteChanged {
      add    => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public void Execute(object Parameter) {
      execute((T)Parameter);
    }

    #endregion ICommand Implementation

  }


  public class RelayCommandAsync<T> : ICommand {

    #region Private Fields

    private readonly Func<T, Task>   execute;
    private readonly Predicate<T> canExecute;

    #endregion Private Fields

    #region Constructor

    public RelayCommandAsync(Func<T, Task> Execute, Predicate<T> CanExecute = null) {
      execute    = Execute ?? throw new ArgumentNullException(nameof(Execute));
      canExecute = CanExecute;
    }

    #endregion Constructor

    #region ICommand Implementation

    public bool CanExecute(object Parameter) {
      return canExecute == null || canExecute((T)Parameter);
    }

    public event EventHandler CanExecuteChanged {
      add    => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public async void Execute(object Parameter) {
      await execute((T)Parameter);
    }

    #endregion ICommand Implementation

  }


  public class RelayCommand : ICommand {

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

    #region ICommand Implementation

    public bool CanExecute(object Parameter) {
      return canExecute == null || canExecute();
    }

    public event EventHandler CanExecuteChanged {
      add    => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public void Execute(object Parameter) {
      execute();
    }

    #endregion ICommand Implementation

  }


  public class RelayCommandAsync : ICommand {

    #region Private Fields

    private readonly Func<Task>    execute;
    private readonly Func<bool> canExecute;

    #endregion Private Fields

    #region Constructor

    public RelayCommandAsync(Func<Task> Execute, Func<bool> CanExecute = null) {
      execute    = Execute ?? throw new ArgumentNullException(nameof(Execute));
      canExecute = CanExecute;
    }

    #endregion Constructor

    #region ICommand Implementation

    public bool CanExecute(object Parameter) {
      return canExecute == null || canExecute();
    }

    public event EventHandler CanExecuteChanged {
      add    => CommandManager.RequerySuggested += value;
      remove => CommandManager.RequerySuggested -= value;
    }

    public async void Execute(object Parameter) {
      await execute();
    }

    #endregion ICommand Implementation

  }

}
