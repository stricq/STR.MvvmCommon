using JetBrains.Annotations;

using Str.MvvmCommon.Core;


namespace Str.MvvmCommon.Helpers;


[UsedImplicitly]
public static class RelayCommandHelper {

    [UsedImplicitly]
    public static RelayCommandAsync EmptyRelayCommand { get; } = new(() => Task.CompletedTask);

    [UsedImplicitly]
    public static RelayCommandAsync<T> EmptyRelayCommandT<T>() {
        return new RelayCommandAsync<T>(_ => Task.CompletedTask);
    }

}
