using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;


namespace Str.MvvmCommon.Contracts {

  [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "This is a library.")]
  public interface ICommandSlim : ICommand {

    void Execute();

    bool CanExecute();

  }

  [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "This is a library.")]
  public interface ICommand<in T> : ICommand {

    void Execute(T parameter);

    bool CanExecute(T parameter);

  }

}
