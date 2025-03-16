using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Str.MvvmCommon.Contracts;


[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "This is a library.")]
[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "This is a library.")]
public interface IMvvmContainer {

    void Initialize(Action<IServiceCollection, IConfiguration> configure);

    Task InitializeControllersAsync(bool descending = false);

    Task OnStartupAsync();

    Task OnExitAsync();

    object? Get(Type type);

    T Get<T>() where T : notnull;

    IEnumerable<object?> GetAll(Type type);

    IEnumerable<T> GetAll<T>();

}
