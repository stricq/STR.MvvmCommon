using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Str.MvvmCommon.Contracts;


[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "This is a library.")]
public interface ICommandAsync : ICommand {

    Task ExecuteAsync();

    bool CanExecute();

}

[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "This is a library.")]
public interface ICommandAsync<in T> : ICommand {

    Task ExecuteAsync(T parameter);

    bool CanExecute(T parameter);

}
