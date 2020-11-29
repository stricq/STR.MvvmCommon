

namespace Str.MvvmCommon.Helpers {

  internal interface IWeakFunc {

    bool IsAlive { get; }

    string? MethodName { get; }

    object? Target { get; }

    void MarkForDeletion();

  }

}
